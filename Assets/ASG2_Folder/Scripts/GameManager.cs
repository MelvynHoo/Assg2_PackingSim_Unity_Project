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

    int noOfboxDelivered;
    int noOfMoneyEarned;
    public bool ClosedBoxBool = false;

    public TextMeshProUGUI boxDelieveredText;
    public TextMeshProUGUI MoneyEarnedText;

    /// <summary>
    /// TIMER
    /// </summary>
    float currentTime = 0f;
    float startingTime = 200f;

    // Start is called before the first frame update
    private void Start()
    {
        currentTime = startingTime;
        isPlayerStatUpdated = false;
        
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
        Debug.Log("Current Time: " + currentTime);

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    public void SocketBox()
    {
        ClosedBoxBool = true;
        UpdateScores();
    }

    public async void UpdateScores()
    {
        noOfMoneyEarned += 10;
        noOfboxDelivered++;
        Debug.Log("noOfMoneyEarned" + noOfMoneyEarned);
    
        await Task.Delay(200);
        ClosedBoxBool = false;
        //Debug.Log("Gamemanager closeboxbool: " + ClosedBoxBool);
    }

    public void GameOver()
    {
        currentTime = 0;
        isGameActive = false;
        if (!isPlayerStatUpdated)
        {
            UpdatePlayerStat(this.noOfMoneyEarned, this.noOfMoneyEarned);
        }
        isPlayerStatUpdated = true;
    }

    public void UpdatePlayerStat(int currentmoney, int boxesdelivered)
    {
        firebaseMgr.UpdatePlayerStats(auth.GetCurrentUser().UserId, noOfMoneyEarned, noOfboxDelivered, auth.GetCurrentUserDisplayName());
    }

}
