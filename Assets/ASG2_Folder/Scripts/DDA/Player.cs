/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Data structure for Players to be use for firebase
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player
{
    /*
        Key: uuid
        displayName;
        email
        active
        lastLoggedIn
        updatedOn
        creaton
    */

    //Properties of our player
    public string userName;
    public string email;
    public bool active;
    public long lastLoggedIn;
    public long createdOn;
    public long updatedOn;

    //simple constructor
    public Player()
    {

    }

    /// <summary>
    /// Constructor to create a new player with active as true
    /// Timestamps are being logged
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="displayName"></param>
    /// <param name="email"></param>
    /// <param name="active"></param>
    public Player(string userName, string email, bool active = true)
    {
        this.userName = userName;
        this.email = email;
        this.active = active;

        //Timestamp properties
        var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
        this.lastLoggedIn = timestamp;
        this.createdOn = timestamp;
        this.updatedOn = timestamp;
    }

    //helper functions
    /// <summary>
    /// Simple helper function convert object to JSON
    /// </summary>
    /// <returns></returns>
    public string GamePlayerToJson()
    {
        return JsonUtility.ToJson(this);
    }

    //simple helper function to print player details
    /// <summary>
    ///return player details
    /// </summary>
    /// <returns></returns>
    public string PrintPlayer()
    {
        return String.Format("User Name: {0}\n Email: {1}\n Active: {2}", this.userName, this.email, this.active);
    }
}
