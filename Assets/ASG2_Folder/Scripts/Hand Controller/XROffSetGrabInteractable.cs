/*
 * Author: Melvyn Hoo
 * Date: 21 Dec 2022
 * Description: The grab offset is to offset the grab, so the player does not
 * really have to actually grab fully on the object
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffSetGrabInteractable : XRGrabInteractable
{

    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;

    // Start is called before the first frame update
    void Start()
    {
        if(!attachTransform)
        {
            GameObject attachPoint = new GameObject("Offset Grab Pivot");
            attachPoint.transform.SetParent(transform, false);
            attachTransform = attachPoint.transform;
        }
        else
        {
            initialLocalPos = attachTransform.localPosition;
            initialLocalRot = attachTransform.localRotation;
        }
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if(args.interactorObject is XRDirectInteractor)
        {
            attachTransform.position = args.interactorObject.transform.position;
            attachTransform.rotation = args.interactorObject.transform.rotation;
        }
        else
        {
            attachTransform.localPosition = initialLocalPos;
            attachTransform.localRotation = initialLocalRot;
        }
        base.OnSelectEntered(args);
    }
}
