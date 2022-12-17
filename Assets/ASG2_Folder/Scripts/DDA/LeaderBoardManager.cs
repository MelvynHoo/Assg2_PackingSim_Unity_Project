/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Handles most of the leaderboard stuff in the scene
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; //To load back to main menu
using System.Threading.Tasks;

public class LeaderBoardManager : MonoBehaviour
{

    // leaderboard menu
    public GameObject leaderBoardMenu;

    // Get the firebase manager to the code
    public FirebaseManager fbManager;

    // Create new row using the prefab
    public GameObject rowPrefab;

    // A place where the row sits
    public Transform tableContent;

    float totalTimeSpent;

    /// <summary>
    /// Call GetLeaderboard
    /// </summary>
    void Start()
    {
        GetLeaderboard();
    }
    /// <summary>
    /// Called UpdateLeaderBoard UI
    /// </summary>
    public void GetLeaderboard()
    {
        UpdateLeaderboardUI();
    }

    /// <summary>
    ///  Get and update leaderboard UI
    /// </summary>
    public async void UpdateLeaderboardUI()
    {
        var leaderBoardList = await fbManager.GetLeaderboard(5);
        int rankCounter = 1;

        //clear all leaderboard entries in UI
        foreach(Transform item in tableContent)
        {
            Destroy(item.gameObject);
        }
        //create prefabs of our rows
        //assign each value from list to the prefab text content
        foreach(LeaderBoard lb in leaderBoardList)
        {
            // Convert the total time spent from firebase
            //totalTimeSpent = lb.totalTimeSpent;

            // conversion of the time to hour, minute and seconds
            /*
            int seconds = (int)(totalTimeSpent % 60);
            int minutes = (int)(totalTimeSpent / 60) % 60;
            int hours = (int)(totalTimeSpent / 3600) % 24;
            */

            // Write and display the time
            //string timerString = string.Format("{0:0}:{1:00}:{2:00}", hours, minutes, seconds);

            Debug.LogFormat("Leaderboard Manager: Rank {0} Playername {1} Money Earned {2} Time Spent {3}", rankCounter, lb.userName, lb.noOfboxDelivered, lb.noOfMoneyEarned) ;

            //create prefabs in the position of tableContent
            GameObject entry = Instantiate(rowPrefab, tableContent);
            TextMeshProUGUI[] leaderBoardDetails = entry.GetComponentsInChildren<TextMeshProUGUI>();

            leaderBoardDetails[0].text = rankCounter.ToString();
            leaderBoardDetails[1].text = lb.userName;
            leaderBoardDetails[2].text = "$" + lb.noOfMoneyEarned;
            leaderBoardDetails[3].text = lb.noOfboxDelivered.ToString();

            rankCounter++;
        }
    }

    /// <summary>
    /// Go to main Menu
    /// </summary>
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(1);
    }
}
