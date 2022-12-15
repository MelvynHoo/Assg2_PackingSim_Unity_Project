using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    string itemOne;
    int countItemOne;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    public void Update()
    {
        //itemOne = StaticController.itemOne;
        //Debug.Log(itemOne);
        ItemOneScoring();
    }

    public void ItemOneScoring()
    {
        //Debug.Log("Inside item one scoring:" + itemOne);
        
        if (itemOne == "Gaming_Keyboard")
        {
            countItemOne += 10;
            //StaticController.itemOne = "";
        }
        if (itemOne == "Gaming_Keyboard")
        {
            countItemOne += 10;
            //StaticController.itemOne = "";
        }
        //Debug.Log("Count item one" + countItemOne);
        
    }
}
