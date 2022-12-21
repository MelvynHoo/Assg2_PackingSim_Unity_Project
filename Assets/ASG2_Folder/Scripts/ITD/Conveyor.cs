/*
 * Author: Melvyn Hoo
 * Date: 21 Dec 2022
 * Description: A conveyor script to move the item like a conveyor belt
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float speed = 100;
    public Vector3 direction;
    public List<GameObject> onBelts;
    //Rigidbody rBody;
    //public float spinningSpeed = 50;
    void Start()
    {
        //rBody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        for(int i = 0; i < onBelts.Count ; i++)
        {
            onBelts[i].GetComponent<Rigidbody>().velocity = speed * direction * Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        onBelts.Add(collision.gameObject);
    }

    private void OnCollisionExit(Collision collision)
    {
        onBelts.Remove(collision.gameObject);
    }

    /*
    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 pos = rBody.position;
        rBody.position = -transform.forward * speed * Time.deltaTime;
        rBody.MovePosition(pos);
        //transform.Rotate(0f, spinningSpeed * Time.deltaTime, 0f, Space.Self);
    }
    */
}
