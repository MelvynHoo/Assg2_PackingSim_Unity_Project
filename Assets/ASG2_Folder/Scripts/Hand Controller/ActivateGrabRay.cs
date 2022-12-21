/*
 * Author: Melvyn Hoo
 * Date: 21 Dec 2022
 * Description: Deactivate the left and right controller rays
 * if the player picks something up
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ActivateGrabRay : MonoBehaviour
{

    public GameObject leftGrabRay;
    public GameObject rightGrabRay;

    public XRDirectInteractor leftDirectGrab;
    public XRDirectInteractor rightDirectGrab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftGrabRay.SetActive(leftDirectGrab.interactablesSelected.Count == 0);
        rightGrabRay.SetActive(rightDirectGrab.interactablesSelected.Count == 0);
    }
}
