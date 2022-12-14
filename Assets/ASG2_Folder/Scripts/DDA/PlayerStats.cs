/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Data structure for PlayerStats to be use for firebase
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerStats
{
    /*
        Key: uuid
        userName
        totalMoney
        totalTimeSpent
        updatedOn
        creaton
    */

    //Properties of our playerstats
    public string userName;
    public float totalTimeSpent;
    public float totalMoney;
    public long updatedOn;
    public long createdOn;

    //simple constructor
    public PlayerStats()
    {

    }

    /// <summary>
    /// Constructor for the data to be send over to firebase
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="totalMoney"></param>
    /// <param name="totalTimeSpent"></param>
    public PlayerStats(string userName, float totalMoney, float totalTimeSpent)
    {
        this.userName = userName;
        this.totalMoney = totalMoney;
        this.totalTimeSpent = totalTimeSpent;
    }

    /// <summary>
    /// Get the time
    /// </summary>
    /// <returns></returns>
    public long GetTimeUnix()
    {
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(); // 1......
    }

    /// <summary>
    /// Simple helper function convert object to JSON
    /// </summary>
    /// <returns></returns>
    public string PlayerStatsToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
