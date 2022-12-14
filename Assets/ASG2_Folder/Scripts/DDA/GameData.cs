/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Data structure for PlayerStats to be use for firebase
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameData
{
    /*
        Key: uuid
        userName
        totalMoney
        tier1Count
        tier2Count
        tier1Cost
        tier2Cost
        tier1Profit
        tier2Profit
        upgradeCost
        hitPower
        rateOfMoney
        
   */

    //Properties of our gameData
    public string userName;
    public float totalMoney;
    public float totalTimeSpent;
    public int tier1Count;
    public int tier2Count;
    public int tier1Cost;
    public int tier2Cost;
    public float tier1Profit;
    public float tier2Profit;
    public int upgradeCost;
    public float hitPower;
    public float rateOfMoney;
    public long updatedOn;

    //simple constructor
    public GameData()
    {

    }

    /// <summary>
    /// Constructor of the game data to be send over to firebase
    /// </summary>
    /// <param name="userName"></param>
    /// <param name="totalMoney"></param>
    /// <param name="totalTimeSpent"></param>
    /// <param name="tier1Count"></param>
    /// <param name="tier2Count"></param>
    /// <param name="tier1Cost"></param>
    /// <param name="tier2Cost"></param>
    /// <param name="tier1Profit"></param>
    /// <param name="tier2Profit"></param>
    /// <param name="upgradeCost"></param>
    /// <param name="hitPower"></param>
    /// <param name="rateOfMoney"></param>
    public GameData(string userName, float totalMoney, float totalTimeSpent, int tier1Count, int tier2Count, int tier1Cost, int tier2Cost,
        float tier1Profit, float tier2Profit, int upgradeCost, float hitPower, float rateOfMoney)
    {
        this.userName = userName;
        this.totalMoney = totalMoney;
        this.totalTimeSpent = totalTimeSpent;
        this.tier1Count = tier1Count;
        this.tier2Count = tier2Count;
        this.tier1Cost = tier1Cost;
        this.tier2Cost = tier2Cost;
        this.tier1Profit = tier1Profit;
        this.tier2Profit = tier2Profit;
        this.upgradeCost = upgradeCost;
        this.hitPower = hitPower;
        this.rateOfMoney = rateOfMoney;
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
    public string GameDataToJson()
    {
        return JsonUtility.ToJson(this);
    }
}
