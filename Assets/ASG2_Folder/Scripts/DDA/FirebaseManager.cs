/*
 * Author: Melvyn Hoo
 * Date: 20 Nov 2022
 * Description: Firebase Manager handle the creation of dataset 
 * and calling of dataset to the game
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;

public class FirebaseManager : MonoBehaviour
{
    // Reference the database
    DatabaseReference dbPlayerStatsReference;
    DatabaseReference dbLeaderboardsReference;
    DatabaseReference dbGameDataReference;

    /// <summary>
    /// Initialize firebase
    /// </summary>
    public void Awake()
    {
        InitializeFirebase();
    }
    /// <summary>
    /// Initialize firebase of the player stat, leaderboard and gamedata
    /// </summary>
    public void InitializeFirebase()
    {
        dbPlayerStatsReference = FirebaseDatabase.DefaultInstance.GetReference("playerStats");
        dbLeaderboardsReference = FirebaseDatabase.DefaultInstance.GetReference("leaderboards");
        dbGameDataReference = FirebaseDatabase.DefaultInstance.GetReference("gameData");
    }

    /// <summary>
    /// Create a new entry ONLY if its the first time playing
    /// Update when there's existing entries
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="totalMoney"></param>
    /// <param name="time"></param>
    /// <param name="displayname"></param>
    public void UpdatePlayerStats(string uuid, float money, float time, string displayName, int tier1Count, int tier2Count, int tier1Cost, 
        int tier2Cost, float tier1Profit, float tier2Profit, int upgradeCost, float hitPower, float rateOfMoney)
    {
        Query playerQuery = dbPlayerStatsReference.Child(uuid);

        //READ the data first and check whether there ahs been an entry based on my uuid
        playerQuery.GetValueAsync().ContinueWithOnMainThread(Task =>
        {
            if(Task.IsCanceled || Task.IsFaulted)
            {
                Debug.LogError("Sorry, there was an error creating your entries, ERROR: " + Task.Exception);
            }
            else if (Task.IsCompleted)
            {
                // Snapshot the database from firebase
                DataSnapshot playerStats = Task.Result;
                DataSnapshot gameData = Task.Result;
                DataSnapshot leaderBoards = Task.Result;
                //check if there is an entry created
                if (playerStats.Exists)
                {
                    //UPDATE player stats
                    //compare existing totalMoney and set new totalMoney

                    //create a temp object sp which stores info from the player stats
                    PlayerStats sp = JsonUtility.FromJson<PlayerStats>(playerStats.GetRawJsonValue());
                    sp.totalTimeSpent += time;
                    sp.updatedOn = sp.GetTimeUnix();

                    // temp lb to store leaderboard
                    LeaderBoard lb = JsonUtility.FromJson<LeaderBoard>(leaderBoards.GetRawJsonValue());
                    lb.totalTimeSpent += time;

                    // temp gd to store gd
                    GameData gd = JsonUtility.FromJson<GameData>(gameData.GetRawJsonValue());
                    gd.updatedOn = gd.GetTimeUnix();
                    gd.totalTimeSpent += time;

                    //continue to update the user total earning throughout the game
                    sp.totalMoney += money;
                    UpdatePlayerLeaderBoardEntry(uuid, sp.userName, sp.totalMoney, lb.totalTimeSpent, sp.updatedOn);

                    // Save the game progress into the database, to be called later to continue the game
                    gd.totalMoney = money;
                    gd.tier1Count = tier1Count;
                    gd.tier2Count = tier2Count;
                    gd.tier1Cost = tier1Cost;
                    gd.tier2Cost = tier2Cost;
                    gd.tier1Profit = tier1Profit;
                    gd.tier2Profit = tier2Profit;
                    gd.upgradeCost = upgradeCost;
                    gd.hitPower = hitPower;
                    gd.rateOfMoney = rateOfMoney;
                    UpdateGameDataEntry(uuid, gd.totalMoney, gd.totalTimeSpent, gd.tier1Count, gd.tier2Count, gd.tier1Cost, gd.tier2Cost, gd.tier1Profit,
                        gd.tier2Profit, gd.upgradeCost, gd.hitPower, gd.rateOfMoney);
                    /*
                    if (money > sp.totalMoney)
                    {
                        sp.totalMoney = money;
                        UpdatePlayerLeaderBoardEntry(uuid, sp.totalMoney, sp.updatedOn);

                    }
                    */

                    //update with entire temp sp object
                    //path: playerstats/$uuid
                    dbPlayerStatsReference.Child(uuid).SetRawJsonValueAsync(sp.PlayerStatsToJson());
               
                }
                else
                {
                    //CREATE player stats
                    //if there's no existing data, it's our first time player
                    PlayerStats sp = new PlayerStats(displayName, money, time);
                    LeaderBoard lb = new LeaderBoard(displayName, money, time);
                    GameData gd = new GameData(displayName, money, time, tier1Count, tier2Count, tier1Cost, tier2Cost, tier1Profit,
                        tier2Profit, upgradeCost, hitPower, rateOfMoney);

                    //create new entries into firebase
                    dbPlayerStatsReference.Child(uuid).SetRawJsonValueAsync(sp.PlayerStatsToJson());
                    dbLeaderboardsReference.Child(uuid).SetRawJsonValueAsync(lb.LeaderBoardToJson());
                    dbGameDataReference.Child(uuid).SetRawJsonValueAsync(gd.GameDataToJson());
                }
            }
        });
    }

    /// <summary>
    /// Update any new entries into firebase
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="userName"></param>
    /// <param name="totalMoney"></param>
    /// <param name="totalTimeSpent"></param>
    /// <param name="updatedOn"></param>
    public void UpdatePlayerLeaderBoardEntry(string uuid, string userName, float totalMoney, float totalTimeSpent, long updatedOn)
    {
        //update only specific properties that we want

        //path: leaderboards/&uuid/username
        //path: leaderboards/$uuid/totalMoney
        //path: leaderboards/$uuid/totalTmeSpent
        //path: leaderboards/$uuid/updatedOn
        dbLeaderboardsReference.Child(uuid).Child("userName").SetValueAsync(userName);
        dbLeaderboardsReference.Child(uuid).Child("totalMoney").SetValueAsync(totalMoney);
        dbLeaderboardsReference.Child(uuid).Child("totalTimeSpent").SetValueAsync(totalTimeSpent);
        dbLeaderboardsReference.Child(uuid).Child("updatedOn").SetValueAsync(updatedOn);
    }

    /// <summary>
    /// Update new entries of the gamedata
    /// </summary>
    /// <param name="uuid"></param>
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
    public void UpdateGameDataEntry(string uuid, float totalMoney, float totalTimeSpent, int tier1Count, int tier2Count, int tier1Cost,
        int tier2Cost, float tier1Profit, float tier2Profit, int upgradeCost, float hitPower, float rateOfMoney)
    {
        //update only specific properties that we want

        //path: gameData/$uuid/totalMoney
        //path: gameData/$uuid/totalTmeSpent
        //path: gameData/$uuid/tier1Count
        //path: gameData/$uuid/tier2Count
        //path: gameData/$uuid/tier1Cost
        //path: gameData/$uuid/tier2Cost
        //path: gameData/$uuid/tier1Profit
        //path: gameData/$uuid/tier2Profit
        //path: gameData/$uuid/upgradeCost
        //path: gameData/$uuid/hitPower
        //path: gameData/$uuid/rateOfMoney
        dbGameDataReference.Child(uuid).Child("totalMoney").SetValueAsync(totalMoney);
        dbGameDataReference.Child(uuid).Child("totalTimeSpent").SetValueAsync(totalTimeSpent);
        dbGameDataReference.Child(uuid).Child("tier1Count").SetValueAsync(tier1Count);
        dbGameDataReference.Child(uuid).Child("tier2Count").SetValueAsync(tier2Count);
        dbGameDataReference.Child(uuid).Child("tier1Cost").SetValueAsync(tier1Cost);
        dbGameDataReference.Child(uuid).Child("tier2Cost").SetValueAsync(tier2Cost);
        dbGameDataReference.Child(uuid).Child("tier1Profit").SetValueAsync(tier1Profit);
        dbGameDataReference.Child(uuid).Child("tier2Profit").SetValueAsync(tier2Profit);
        dbGameDataReference.Child(uuid).Child("upgradeCost").SetValueAsync(upgradeCost);
        dbGameDataReference.Child(uuid).Child("hitPower").SetValueAsync(hitPower);
        dbGameDataReference.Child(uuid).Child("rateOfMoney").SetValueAsync(rateOfMoney);
    }

    /// <summary>
    /// To get the game data from the firebase
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    public async Task<GameData> GetGameData(string uuid)
    {
        Query q = dbGameDataReference.Child(uuid).LimitToFirst(1);
        GameData gameData = null;

        await dbGameDataReference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Sorry, there was an error retrieving player stats : ERROR " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot ds = task.Result;//path -> playerstats/$uuid
                if (ds.Child(uuid).Exists)
                {

                    //path to the datasapshot playerstats/$uuid/<we want this values>
                    gameData = JsonUtility.FromJson<GameData>(ds.Child(uuid).GetRawJsonValue());

                    Debug.Log("ds... : " + ds.GetRawJsonValue());
                    Debug.Log("Player stats values.." + gameData.GameDataToJson());

                }
            }
        });
        return gameData;
    }
    /// <summary>
    /// To get the leaderboard data from the firebase
    /// </summary>
    /// <param name="limit"></param>
    /// <returns></returns>
    public async Task<List<LeaderBoard>> GetLeaderboard(int limit = 5)
    {
        Query q = dbLeaderboardsReference.OrderByChild("totalMoney").LimitToLast(limit);
        List<LeaderBoard> leaderBoardList = new List<LeaderBoard>();

        await q.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsCanceled || task.IsFaulted)
            {
                Debug.LogError("Sorry, there was an error getting leaderboard entries, : ERROR: " + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot ds = task.Result;

                if (ds.Exists)
                {
                    int rankCounter = 1;
                    foreach(DataSnapshot d in ds.Children)
                    {
                        //create temp objects based on the results
                        LeaderBoard lb = JsonUtility.FromJson<LeaderBoard>(d.GetRawJsonValue());

                        //add item to list
                        leaderBoardList.Add(lb);

                        //Debug.LogFormat("Leaderboard: Rank {0} Playername {1} High Score {2}", rankCounter, lb.userName, lb.totalMoney);
                    }

                    leaderBoardList.Reverse();

                    foreach(LeaderBoard lb in leaderBoardList)
                    {
                        Debug.LogFormat("Leaderboard: Rank {0} Playername {1} Money Earned {2}", rankCounter, lb.userName, lb.totalMoney);

                        rankCounter++;
                    }
                }
            }
        });
        return leaderBoardList;

    }
    /// <summary>
    /// To get the playestat from the firebase
    /// </summary>
    /// <param name="uuid"></param>
    /// <returns></returns>
    public async Task<PlayerStats> GetPlayerStats(string uuid)
    {
         Query q = dbPlayerStatsReference.Child(uuid).LimitToFirst(1);
         PlayerStats playerStats = null;

         await dbPlayerStatsReference.GetValueAsync().ContinueWithOnMainThread(task =>
         {
             if (task.IsCanceled || task.IsFaulted)
             {
                 Debug.LogError("Sorry, there was an error retrieving player stats : ERROR " + task.Exception);
             }
             else if (task.IsCompleted)
             {
                 DataSnapshot ds = task.Result;//path -> playerstats/$uuid
                 if (ds.Child(uuid).Exists)
                 {

                     //path to the datasapshot playerstats/$uuid/<we want this values>
                     playerStats = JsonUtility.FromJson<PlayerStats>(ds.Child(uuid).GetRawJsonValue());

                     Debug.Log("ds... : " + ds.GetRawJsonValue());
                     Debug.Log("Player stats values.." + playerStats.PlayerStatsToJson());

                 }
             }
         });
        return playerStats;
    }
    /// <summary>
    /// Delete everything of the player sata, leaderboard and game data
    /// </summary>
    /// <param name="uuid"></param>
    public void DeletePlayerStats(string uuid)
    {
        dbPlayerStatsReference.Child(uuid).RemoveValueAsync();
        dbLeaderboardsReference.Child(uuid).RemoveValueAsync();
        dbGameDataReference.Child(uuid).RemoveValueAsync();
    }
}
