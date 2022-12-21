/*
 * Author: Melvyn Hoo
 * Date: 21 Dec 2022
 * Description: Game Menu Manager handle the VR menu that spawn in front of player
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public GameObject menu;
    public GameObject gameOverMenu;
    public InputActionProperty showButton;
    public Transform head;
    public float spawnDistance = 2;
    public float spawnGameOverDistance = 4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    /// <summary>
    /// This update where the player position, once activated, 
    /// the menu will appear in front of them
    /// </summary>
    void Update()
    {
        if(showButton.action.WasPressedThisFrame())
        {
            Debug.Log("hello there");
            menu.SetActive(!menu.activeSelf);
            menu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnDistance;
        }
        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1;

        gameOverMenu.transform.position = head.position + new Vector3(head.forward.x, 0, head.forward.z).normalized * spawnGameOverDistance;
        gameOverMenu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        gameOverMenu.transform.forward *= -1;
    }
}
