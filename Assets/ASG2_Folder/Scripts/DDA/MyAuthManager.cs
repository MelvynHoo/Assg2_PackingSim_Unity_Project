/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Auth Manager for the enable user to login, register or recover from
 * forget password
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// importing directives for authentication
using Firebase;
using Firebase.Auth;
using Firebase.Extensions; // Using Firebase Auth service
using Firebase.Database; // Firebase real time database
using TMPro; // TextMesh Pro
using UnityEngine.UI;
using UnityEngine.SceneManagement; //scene loading
using System.Threading.Tasks;
using System.Text.RegularExpressions;

public class MyAuthManager : MonoBehaviour
{
    //Firebase references
    public FirebaseAuth auth;
    public DatabaseReference dbReference;

    //For register new user
    public TMP_InputField emailRegisterInput;
    public TMP_InputField passwordRegisterInput;
    public TMP_InputField usernameRegisterInput;

    //For existing user to login
    public TMP_InputField emailLoginInput;
    public TMP_InputField passwordLoginInput;

    //setup buttons and UI
    public GameObject signUpBtn;
    public GameObject logInBtn;
    public GameObject forgetPasswordBtn;
    public GameObject signOutBtn;

    public TextMeshProUGUI loginValidation;
    public TextMeshProUGUI registerValidation;

    /// <summary>
    /// Initialize Firebase to start running the function
    /// </summary>
    private void Awake()
    {
        InitializeFirebase();
    }


    /// <summary>
    /// Set auth and the dbreference to instance
    /// </summary>
    void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    /// <summary>
    /// Sign up process that requirement user input of their email and password
    /// </summary>
    public async void SignUpnewUser()
    {
        // Get user input
        string email = emailRegisterInput.text.Trim();
        string password = passwordRegisterInput.text.Trim();
        
        //Checking if email and password are valid
        if(ValidateEmail(email) && ValidatePassword(password))
        {
            Debug.Log("validate success");
            // Creates new user by calling signupnewuseronly
            FirebaseUser newPlayer = await SignUpNewUserOnly(email, password);

            // If there no player with the same name
            if (newPlayer != null)
            {
                registerValidation.gameObject.SetActive(false);
                string username = usernameRegisterInput.text;
                //CreateNewPlayer(newPlayer.UserId, username, username, newPlayer.Email);
                await CreateNewPlayer(newPlayer.UserId, username, newPlayer.Email);
                Debug.Log("Give me username: " + username);
                await UpdatePlayerDisplayName(username); //update user's display name in authenticate service 
                SceneManager.LoadScene(1);
            }
        }
        // Checks to user there an error signing up
        else
        {
            registerValidation.text = "Error in Signing Up. Invalid Email or Passord";
            registerValidation.gameObject.SetActive(true);
        }
    }
    /// <summary>
    /// To be called from SignUpNewuser function, to create new user
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    public async Task<FirebaseUser> SignUpNewUserOnly(string email, string password)
    {
        Debug.Log("Sign Up method...");
        FirebaseUser newPlayer = null;
        // Get value from the function and creates new user
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            //Error handling if sign up process when into a problem
            if (task.IsFaulted || task.IsCanceled)
            {
                if (task.Exception != null)
                {
                    string errorMsg = this.SignUpErrorHandler(task);
                    registerValidation.text = errorMsg;
                    Debug.Log("Error in signing up: " + errorMsg);
                    registerValidation.gameObject.SetActive(true);
                }

                //Debug.LogError("Sorry, there was an error creating your new account, ERROR: " + task.Exception);
            }
            // Sign up is successful
            else if (task.IsCompleted)
            {
                registerValidation.gameObject.SetActive(false);
                newPlayer = task.Result;
                Debug.LogFormat("New Player Details {0} {1}", newPlayer.UserId, newPlayer.Email);
            }
        });
        //Finish creating new user
        return newPlayer;
    }

    /// <summary>
    /// To display name by retrieving from database
    /// </summary>
    /// <param name="displayName"></param>
    /// <returns></returns>
    public async Task UpdatePlayerDisplayName(string userName)
    {
        if(auth.CurrentUser != null) 
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = userName
            };
            await auth.CurrentUser.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was cancelled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    return;
                }
                Debug.Log("User profile updated successfully");
                Debug.LogFormat("Checking current user display name from auth {0}", GetCurrentUserDisplayName());
            });
        }
    }

    /// <summary>
    /// Create a database model of the player using input value when creating new user
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="displayName"></param>
    /// <param name="userName"></param>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task CreateNewPlayer(string uuid, string userName, string email)
    {
        Player newPlayer = new Player(userName, email);
        Debug.LogFormat("Player Details: {0}", newPlayer.PrintPlayer());

        //root/players/$uuid
        await dbReference.Child("players/" + uuid).SetRawJsonValueAsync(newPlayer.GamePlayerToJson());

        //Update auth player with new display name => tagging along the username input field
        await UpdatePlayerDisplayName(userName);
    }
    /// <summary>
    /// Allow other script to call this function to retrieve the user name
    /// </summary>
    /// <returns></returns>
    public string GetCurrentUserDisplayName()
    {
        Debug.Log("Inside getcurrentuserdisplayname: " + auth.CurrentUser.DisplayName);
        return auth.CurrentUser.DisplayName;
    }
       
    /// <summary>
    /// Log in process when existing user wants to log in
    /// </summary>
    public void LogInUser()
    {
        // get user inputs
        Debug.Log("Log In method...");
        string email = emailLoginInput.text.Trim();
        string password = passwordLoginInput.text.Trim();
        // checks the email and password is valid
        if (ValidateEmail(email) && ValidatePassword(password))
        {
            // log in the user
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
            {
                // error when something wrong with the log in process
                if (task.IsFaulted || task.IsCanceled)
                {
                    string errorMsg = this.LogInErrorHandler(task);
                    loginValidation.text = errorMsg;
                    Debug.Log("Error in log in: " + errorMsg);
                    loginValidation.gameObject.SetActive(true);
                    //Debug.LogError("Sorry, there was an error signing in your account, ERROR: " + task.Exception);
                }
                // log in complete
                else if (task.IsCompleted)
                {
                    loginValidation.gameObject.SetActive(false);
                    FirebaseUser currentPlayer = task.Result;
                    Debug.LogFormat("Welcome to PackingSim {0} :: {1}", currentPlayer.UserId, currentPlayer.Email);
                    SceneManager.LoadScene(1);
                }
            });

        }
        // Tell user log had a problem
        else
        {
            loginValidation.text = "Error in Logging in. Invalid email/password";
            loginValidation.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// For the user to sign up
    /// </summary>
    public void SignOutUser()
    {
        Debug.Log("Sign Out method...");
        if (auth.CurrentUser != null)
        {
            Debug.LogFormat("Auth user {0} {1}", auth.CurrentUser.UserId,auth.CurrentUser.Email);

            //get current index of a scene
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            auth.SignOut();

            if (currentSceneIndex != 0)
            {
                SceneManager.LoadScene(0);
            }

        }
    }

    /// <summary>
    /// Forget password that send a email to the user to reset password
    /// </summary>
    public void ForgetPassword()
    {
        string email = emailLoginInput.text.Trim();

        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Sorry, there was an sending a password reset, ERROR: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                Debug.Log("Forget password email sent sucessfully...");
            }
        });
        Debug.Log("Forget password method...");
    }

    /// <summary>
    /// Get Current user
    /// </summary>
    /// <returns></returns>
    public FirebaseUser GetCurrentUser()
    {
        return auth.CurrentUser;
    }
    /// <summary>
    /// Email validation
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public bool ValidateEmail(string email)
    {
        bool isValid = false;
        //for all email have @
        
        const string pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?$";
        const RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;

        if (email != "" && Regex.IsMatch(email, pattern, options))
        {
            isValid = true;
        }

        return isValid;
    }
    /// <summary>
    /// Password validation
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    public bool ValidatePassword(string password)
    {
        bool isValid = false;
        if (password != "" && password.Length >=6)
        {
            isValid = true;
        }
        return isValid;
    }

    /// <summary>
    /// Handles the any form of sign up error processes
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public string SignUpErrorHandler(Task<FirebaseUser> task)
    {
        Debug.Log("Sign up error");
        string errorMsg = "";

        if(task.Exception != null)
        {
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError) firebaseEx.ErrorCode;

            errorMsg = "Sign Up Fail\n";
            switch (errorCode)
            {
                case AuthError.EmailAlreadyInUse:
                    errorMsg += "Email already in use";
                    break;
                case AuthError.WeakPassword:
                    errorMsg += "Password is weak. user at least 6 characters";
                    break;
                case AuthError.MissingPassword:
                    errorMsg += "Password is missing";
                    break;
                case AuthError.InvalidEmail:
                    errorMsg += "Invalid email used";
                    break;
                default:
                    errorMsg += "Issue in authentication: " + errorCode;
                    break;
            }
            Debug.Log("Error message " + errorMsg);
        }

        return errorMsg;
    }

    /// <summary>
    /// Handles the any form of log in error processes
    /// </summary>
    /// <param name="task"></param>
    /// <returns></returns>
    public string LogInErrorHandler(Task<FirebaseUser> task)
    {
        Debug.Log("Log in error");
        string errorMsg = "";

        if (task.Exception != null)
        {
            FirebaseException firebaseEx = task.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            errorMsg = "log In Fail\n";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    errorMsg += "No email...";
                    break;
                case AuthError.MissingPassword:
                    errorMsg += "No password detected...";
                    break;
                case AuthError.WrongPassword:
                    errorMsg += "Incorrect password";
                    break;
                case AuthError.InvalidEmail:
                    errorMsg += "Invalid email used";
                    break;
                case AuthError.UserNotFound:
                    errorMsg += "User is not found";
                    break;
                default:
                    errorMsg += "Issue in authentication: " + errorCode;
                    break;
            }
            Debug.Log("Error message " + errorMsg);
        }

        return errorMsg;
    }

    /// <summary>
    /// Quit the application from login page
    /// </summary>
    public void QuitApplication()
    {
        Application.Quit();
    }
}
