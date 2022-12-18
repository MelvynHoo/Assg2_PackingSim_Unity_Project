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
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Get the auth manager to the code
    public MyAuthManager authMgr;

    // Redundant: Sign out the user
    public GameObject signOutBtn;
    // Redundant: To play the game
    public GameObject playBtn;
    // Redundant: View leaderboard
    public GameObject LeaderBoardMenu;
    // To display the name
    public TextMeshProUGUI displayName;

    /// <summary>
    /// When user enter the main mneu screen, their name will be display
    /// </summary>
    public void Awake()
    {
        
        //InitializeFirebase();
        //Debug.Log("Main Menu awake: " + authMgr.GetCurrentUserDisplayName());
        displayName.text = "Welcome, " + authMgr.GetCurrentUserDisplayName();
        //authMgr.GetCurrentUserDisplayName();
    }
    
    public void InitializeFirebase()
    {
        
    }

    /// <summary>
    /// Sign out the user
    /// </summary>
    public void SignOut()
    {
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
}
