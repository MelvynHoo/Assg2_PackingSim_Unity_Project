using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketCheckBox : MonoBehaviour
{

    [SerializeField] GameManager gameManager;

    bool checkBoxTrue = false;

    private void Update()
    {
        //allTrueChecker = StaticController.boolBoxToSocket;
        checkBoxTrue = gameManager.ClosedBoxBool;
        Debug.Log("Socket Check Box bool: " + checkBoxTrue);
    }
    // Start is called before the first frame update
    private void OnTriggerStay(Collider other)
    {


        if (other.gameObject.tag == "ClosedBoxTag" && checkBoxTrue == true)
        {
            Destroy(other.gameObject);
            
        }
        
    }
}
