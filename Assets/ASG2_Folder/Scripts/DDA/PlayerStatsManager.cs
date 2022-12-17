/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Auth Manager for the enable user to login, register or recover from
 * forget password
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class PlayerStatsManager : MonoBehaviour
{
    // Main Menu button
    public GameObject goToMenuBtn;

    // display total money earned
    public TextMeshProUGUI playerMoneyEarned;

    // display player time spent
    public TextMeshProUGUI playerTimespent;

    //display player last played
    public TextMeshProUGUI playerLastPlayed;

    // display playern name
    public TextMeshProUGUI playerName;

    //Get firebase and auth manager to the code
    public FirebaseManager fbMgr;
    public MyAuthManager auth;

    // float for the total time spent
    float totalTimeSpent;

    // the player money
    int playerMoney;
    int boxesDelivered;

    /// <summary>
    /// Called the update player stat function
    /// </summary>
    void Start()
    {
        UpdatePlayerStats(auth.GetCurrentUser().UserId);
    }

    /// <summary>
    /// To update the parameters of the player 
    /// </summary>
    /// <param name="uuid"></param>
    public async void UpdatePlayerStats(string uuid)
    {
        // Called firebase to get player stats
        PlayerStats playerStats = await fbMgr.GetPlayerStats(uuid);

        //Debug.Log("playerstat....: " + playerStats.PlayerStatsToJson());

        // Convert the total time spent from firebase
        //totalTimeSpent = playerStats.totalTimeSpent;

        // conversion of the time to hour, minute and seconds
        //int seconds = (int)(totalTimeSpent % 60);
        //int minutes = (int)(totalTimeSpent / 60) % 60;
        //int hours = (int)(totalTimeSpent / 3600) % 24;

        // Write and display the time
        //string timerString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);

        // If there existing player, display the following player money, time spent and last played
        if (playerStats != null)
        {

            playerMoney = playerStats.noOfMoneyEarned;
            boxesDelivered = playerStats.noOfboxDelivered;
            playerMoneyEarned.text = "$" + playerMoney.ToString();
            //playerTimespent.text = timerString;
            playerLastPlayed.text = UnixToDateTime(playerStats.updatedOn);
        }

        else
        {
            //reset our UI to 0 and NA
            ResetStatsUI();
        }
        playerName.text = "Your Portfolio, " + auth.GetCurrentUserDisplayName() + ".";
    }

    /// <summary>
    /// Reset the UI
    /// </summary>
    public void ResetStatsUI()
    {
        playerMoneyEarned.text = "0";
        playerTimespent.text = "0";
        playerLastPlayed.text = "0";
    }

    /// <summary>
    /// Called the firebase to delete player stats
    /// </summary>
    public void DeletePlayerStats()
    {
        fbMgr.DeletePlayerStats(auth.GetCurrentUser().UserId);
        Debug.Log("Delete Playe Stats");
        //refresh my player sats on screen
        UpdatePlayerStats(auth.GetCurrentUser().UserId);
    }

    /// <summary>
    /// Get the time
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public string UnixToDateTime(long timestamp)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(timestamp); // number of secs from 1/1/1970
        DateTime dateTime = dateTimeOffset.LocalDateTime; //convert to current time format UTC+0 .. But in Singapore +8
        return dateTime.ToString("dd MMM yyyy");
    }

    /// <summary>
    /// Return to main menu
    /// </summary>
    public void ToMainMenu()
    {
        Debug.Log("To Main menu");
        SceneManager.LoadScene(1);
    }
}
