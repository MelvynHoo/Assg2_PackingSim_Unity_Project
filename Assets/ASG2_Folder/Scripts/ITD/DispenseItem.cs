using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenseItem : MonoBehaviour
{

    public Transform dispenseOrigin;

    public GameObject ItemOnePrefab;
    
    public void ToDispenseItem()
    {
        // Instantiate a bullet using the bulletPrefab
        //User the position of the bulletOrigin as the start position of the new bullet
        // Use the rotation o f the gun as the start rotation of the new bullet
        GameObject newBullet = Instantiate(ItemOnePrefab, dispenseOrigin.position, transform.rotation);

        newBullet.GetComponent<Rigidbody>().AddForce(dispenseOrigin.forward * 100f);
    }
}
