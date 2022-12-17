using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using System.Threading.Tasks;

public class BoxManager : MonoBehaviour
{

    bool itemOneCheck;
    bool itemTwoCheck;
    bool itemThreeCheck;
    public bool overallSocketCheck;
    public GameObject openedBox;
    public GameObject closedBox;
    public GameObject theBox;

    public XRGrabInteractable XRGrabBoxInteractable;
    //InteractionLayerMask BoxLayer;

    private void Start()
    {

        itemOneCheck = false;
        itemTwoCheck = false;
        itemThreeCheck = false;
        overallSocketCheck = false;
    }

    // Start is called before the first frame update
    public void Update()
    {
        
        //Debug.Log("Overall Socket Check: " + overallSocketCheck);
        //UpdateCheckforSocket();
    }

    public void SocketItemOne()
    {
        itemOneCheck = true;
        //Debug.Log("Socket Item One is " + itemOneCheck);
        UpdateCheckforSocket();
    }

    public void SocketItemTwo()
    {
        itemTwoCheck = true;
        //Debug.Log("Socket Item Two is " + itemTwoCheck);
        UpdateCheckforSocket();
    }

    public void SocketItemThree()
    {
        itemThreeCheck = true;
        //Debug.Log("Socket Item Three is " + itemThreeCheck);

        UpdateCheckforSocket();
    }

    public async void UpdateCheckforSocket()
    {
        //Debug.Log("Check for all true");
        if(itemOneCheck == true && itemTwoCheck == true && itemThreeCheck == true)
        {
            //Debug.Log("All three item is in places");
            overallSocketCheck = true;
            theBox.tag = "ClosedBoxTag";
            XRGrabBoxInteractable.interactionLayers = InteractionLayerMask.GetMask("ClosedBox");
            //StaticController.boolBoxToSocket = overallSocketCheck;
            await Task.Delay(200);
            Destroy(openedBox);
            closedBox.SetActive(true);
            await Task.Delay(1000);
            overallSocketCheck = false;
            itemOneCheck = false;
            itemTwoCheck = false;
            itemThreeCheck = false;
        }
        
    }
}
