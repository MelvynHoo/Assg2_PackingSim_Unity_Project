/*
 * Author: Melvyn Hoo
 * Date: 21 Dec 2022
 * Description: Game Manager handle everything such as count the score and sending it
 * over to the firebase, opening game over menu and tracking scores.
 * and calling of dataset to the game
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.XR.Interaction.Toolkit;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Firebase and auth manager inputs
    /// </summary>
    public MyAuthManager auth;
    public FirebaseManager firebaseMgr;

    /// <summary>
    /// Check if game is active
    /// </summary>
    public bool isGameActive;

    /// <summary>
    /// Check if player is updated
    /// </summary>
    public bool isPlayerStatUpdated;

    /// <summary>
    /// Store player game progress
    /// </summary>
    int noOfboxDelivered;
    int noOfMoneyEarned;
    public bool ClosedBoxBool = false;

    public TextMeshProUGUI boxDeliveredText;
    public TextMeshProUGUI MoneyEarnedText;
    public TextMeshProUGUI gameOverboxDeliveredText;
    public TextMeshProUGUI gameOverMoneyEarnedText;
    public TextMeshProUGUI timerText;

    public TextMeshProUGUI redirectTimerText;
    public GameObject gameOverCanvas;
    /// <summary>
    /// Timer for game
    /// </summary>
    float currentTime = 0f;
    float startingTime = 300f;

    /// <summary>
    /// Timer for gameover
    /// </summary>
    float redirectCurrentTime = 0f;
    float redirectStartingTime = 6f;

    // Start is called before the first frame update
    private void Start()
    {
        currentTime = startingTime;
        redirectCurrentTime = redirectStartingTime;


        isPlayerStatUpdated = false;
        /*
        if (noOfboxDelivered == 0 && noOfMoneyEarned == 0)
        {
            UpdatePlayerStat(this.noOfMoneyEarned, this.noOfboxDelivered);
        }
        */
    }

    // Update is called once per frame
    public void Update()
    {
        //itemOne = StaticController.itemOne;
        //Debug.Log(itemOne);
        //Debug.Log("Overall Socket box Check: " + ClosedBoxBool);
        ///TIMER///
        currentTime -= 1 * Time.deltaTime;
        //countdownText.text = currentTime.ToString("0");
        //Debug.Log("Current Time: " + currentTime);

        // conversion of the time to hour, minute and seconds

        int currentseconds = (int)(currentTime % 60);
        int currentminutes = (int)(currentTime / 60) % 60;
        int currenthours = (int)(currentTime / 3600) % 24;

        // Write and display the time
        timerText.text = string.Format("{0:0}:{1:00}:{2:00}", currenthours, currentminutes, currentseconds);

        //Debug.Log(hours + ":" + minutes + ":" + seconds);
        boxDeliveredText.text = "Box Delivered: " + noOfboxDelivered;
        MoneyEarnedText.text = "Money Earned: $" + noOfMoneyEarned;
        gameOverboxDeliveredText.text = "Box Delivered: " + noOfboxDelivered;
        gameOverMoneyEarnedText.text = "Money Earned: $" + noOfMoneyEarned;

        // When game time hit over, the game over will start counting
        if (currentTime <= 0)
        {
            currentTime = 0;
            redirectCurrentTime -= 1 * Time.deltaTime;
            int redirectseconds = (int)(redirectCurrentTime % 60);
            Debug.Log(redirectseconds);
            gameOverCanvas.SetActive(true);
            redirectTimerText.text = string.Format("You will be redirected to Main Menu in {0:0}", redirectseconds);
            if (redirectCurrentTime <= 0)
            {
                redirectCurrentTime = 0;
                GameOver();
            }
        }
    }

    /// <summary>
    /// To call update scores
    /// </summary>
    public void SocketBox()
    {
        ClosedBoxBool = true;
        UpdateScores();
    }

    /// <summary>
    /// To update scores
    /// </summary>
    public async void UpdateScores()
    {
        noOfMoneyEarned += 10;
        noOfboxDelivered++;
        Debug.Log("noOfMoneyEarned" + noOfMoneyEarned);
    
        await Task.Delay(200);
        ClosedBoxBool = false;
        //Debug.Log("Gamemanager closeboxbool: " + ClosedBoxBool);
    }

    /// <summary>
    /// To be call when timer hits 0, game over.
    /// </summary>
    public void GameOver()
    {
        isGameActive = false;
        if (!isPlayerStatUpdated)
        {
            UpdatePlayerStat(this.noOfMoneyEarned, this.noOfboxDelivered);
            SceneManager.LoadScene(1);
        }
        isPlayerStatUpdated = true;
    }

    /// <summary>
    /// Leave button update whatever the player progress is currently on
    /// and send it over to firebase
    /// </summary>
    public async void LeaveButton()
    {
        UpdatePlayerStat(this.noOfMoneyEarned, this.noOfboxDelivered);
        await Task.Delay(200);
        SceneManager.LoadScene(1);
    }

    public void UpdatePlayerStat(int currentmoney, int boxesdelivered)
    {
        firebaseMgr.UpdatePlayerStats(auth.GetCurrentUser().UserId, noOfMoneyEarned, noOfboxDelivered, auth.GetCurrentUserDisplayName());
    }

}
