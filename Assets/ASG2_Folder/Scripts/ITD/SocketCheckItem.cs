/*
 * Author: Melvyn Hoo
 * Date: 21 Dec 2022
 * Description: To check if the box have all three item in
 * so that it can destroy the item inside
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketCheckItem : MonoBehaviour
{

    XRSocketInteractor socketItemOne;

    [SerializeField] BoxManager boxManager;

    bool allTrueChecker = false;

    // Start is called before the first frame update
    void Start()
    {
        socketItemOne = GetComponent<XRSocketInteractor>();
    }

    private void Update()
    {
        //allTrueChecker = StaticController.boolBoxToSocket;
        allTrueChecker = boxManager.overallSocketCheck;
    }

    // Update is called once per frame
    public void socketCheckItemOne()
    {
       
        IXRSelectInteractable itemOne = socketItemOne.GetOldestInteractableSelected();
        //Debug.Log(itemOne.transform.name + " in socket of " + transform.name);
        //Debug.Log(" name of itemone " + itemOne.ToString());
    }


   
    private void OnTriggerStay(Collider other)
    {
        

        if (other.gameObject.tag == "ItemOneTag" && allTrueChecker == true)
        {
            Destroy(other.gameObject);
            allTrueChecker = false;
        }
        if (other.gameObject.tag == "ItemTwoTag" && allTrueChecker == true)
        {
            Destroy(other.gameObject);
            allTrueChecker = false;
        }
        if (other.gameObject.tag == "ItemThreeTag" && allTrueChecker == true)
        {
            Destroy(other.gameObject);
            allTrueChecker = false;
        }
    }

   
}
