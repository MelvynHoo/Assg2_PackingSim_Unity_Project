using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BoxCheck : MonoBehaviour
{

    XRSocketInteractor socketItemOne;
    // Start is called before the first frame update
    void Start()
    {
        socketItemOne = GetComponent<XRSocketInteractor>();
    }

    // Update is called once per frame
    public void socketCheck()
    {
        IXRSelectInteractable objName = socketItemOne.GetOldestInteractableSelected();
        Debug.Log(objName.transform.name + " in socket of " + transform.name);
    }
}
