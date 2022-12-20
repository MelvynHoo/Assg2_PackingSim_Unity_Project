/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Handles most the main menu stuff in the scene
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Firebase.Auth;
using TMPro;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Get the auth manager to the code
    public MyAuthManager authMgr;
    public FirebaseManager firebaseMgr;

    // Redundant: Sign out the user
    public GameObject signOutBtn;
    // Redundant: To play the game
    public GameObject playBtn;
    // Redundant: View leaderboard
    public GameObject LeaderBoardMenu;
    // To display the name
    public TextMeshProUGUI displayName;

    bool status = false;


    public void Start()
    {
        status = true;
    }
    /// <summary>
    /// When user enter the main mneu screen, their name will be display
    /// </summary>
    public async void Awake()
    {
        
        //InitializeFirebase();
        //Debug.Log("Main Menu awake: " + authMgr.GetCurrentUserDisplayName());
        displayName.text = "Welcome, " + authMgr.GetCurrentUserDisplayName();
        //authMgr.GetCurrentUserDisplayName();
        await Task.Delay(1000);
        UpdatePlayersActive("Email", "Password", this.status);
    }
    
    public void InitializeFirebase()
    {
        
    }

    /// <summary>
    /// Sign out the user
    /// </summary>
    public void SignOut()
    {
        status = false;
        UpdatePlayersNotActive("Email", "Password", this.status);
        authMgr.SignOutUser();
    }

    /// <summary>
    /// Start the game
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    /// <summary>
    /// Open the leaderboard
    /// </summary>
    public void OpenLeaderBoard()
    {
        SceneManager.LoadScene(4);
    }
    /// <summary>
    /// open the player profile
    /// </summary>
    public void OpenPlayerProfile()
    {
        SceneManager.LoadScene(3);
    }

    public void UpdatePlayersActive(string username, string email, bool status)
    {
        firebaseMgr.PlayersStatus(authMgr.GetCurrentUser().UserId, username, email, status);
    }

    public void UpdatePlayersNotActive(string username, string email, bool status)
    {
        firebaseMgr.PlayersStatus(authMgr.GetCurrentUser().UserId, username, email, status);
    }
}
