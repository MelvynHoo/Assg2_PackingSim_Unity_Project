/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Data structure for Leaderboards to be use for firebase
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LeaderBoard 
{

    /*
        Key: uuid
        username
        highScore
        updatedOn
        timeSpent
    */

    //Properties of our leaderboards
    public string userName;
    public float totalMoney;
    public float totalTimeSpent;
    public long updatedOn;

    //simple constructor
    public LeaderBoard()
    {

    }

    /// <summary>
    /// Constructor of the leadeboard to be send over to firebase
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="totalMoney"></param>
    /// <param name="totalTimeSpent"></param>
    public LeaderBoard(string userName, float totalMoney, float totalTimeSpent)
    {
        this.userName = userName;
        this.totalMoney = totalMoney;
        this.totalTimeSpent = totalTimeSpent;
        this.updatedOn = GetTimeUnix();
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
    /// Helper function to convert object to JSON
    /// </summary>
    /// <returns></returns>
    public string LeaderBoardToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
