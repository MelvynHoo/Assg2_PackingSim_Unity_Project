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


    int noOfboxDelivered;
    int noOfMoneyEarned;
    public bool ClosedBoxBool = false;

    public TextMeshProUGUI boxDelieveredText;
    public TextMeshProUGUI MoneyEarnedText;

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    public void Update()
    {
        //itemOne = StaticController.itemOne;
        //Debug.Log(itemOne);
        Debug.Log("Overall Socket box Check: " + ClosedBoxBool);
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


}
