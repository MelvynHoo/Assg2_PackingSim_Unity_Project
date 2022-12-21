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
    DatabaseReference dbPlayersReference;

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
        dbPlayersReference = FirebaseDatabase.DefaultInstance.GetReference("players");
        //dbGameDataReference = FirebaseDatabase.DefaultInstance.GetReference("gameData");
    }
    /// <summary>
    /// To update the player status
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="username"></param>
    /// <param name="email"></param>
    /// <param name="status"></param>
    public void PlayersStatus(string uuid, string username, string email, bool status)
    {
        Debug.Log("What that: " + uuid + " " + status);
        Query playerQuery = dbPlayersReference.Child(uuid);

        //READ the data first and check whether there ahs been an entry based on my uuid
        playerQuery.GetValueAsync().ContinueWithOnMainThread(Task =>
        {
            if (Task.IsCanceled || Task.IsFaulted)
            {
                Debug.LogError("Sorry, there was an error creating your entries, ERROR: " + Task.Exception);
            }
            else if (Task.IsCompleted)
            {
                // Snapshot the database from firebase
                DataSnapshot players = Task.Result;

                //check if there is an entry created
                if (players.Exists)
                {
                    Player p = JsonUtility.FromJson<Player>(players.GetRawJsonValue());
                    //update with entire temp sp object
                    //path: playerstats/$uuid
                    p.active = status;
                    
                    UpdatePlayersEntry(uuid, p.userName, p.email, p.active);
                }
                
                
            }
        });
    }

  /// <summary>
  /// To tell firebase to update the active(Status)
  /// </summary>
  /// <param name="uuid"></param>
  /// <param name="userName"></param>
  /// <param name="email"></param>
  /// <param name="active"></param>
    public void UpdatePlayersEntry(string uuid, string userName, string email, bool active)
    {
        //update only specific properties that we want

        //path: leaderboards/&uuid/username
        //path: leaderboards/$uuid/totalMoney
        //path: leaderboards/$uuid/totalTmeSpent
        //path: leaderboards/$uuid/updatedOn
        dbPlayersReference.Child(uuid).Child("active").SetValueAsync(active);

        dbLeaderboardsReference.Child(uuid).Child("active").SetValueAsync(active);
        //dbLeaderboardsReference.Child(uuid).Child("noOfMoneyEarned").SetValueAsync(0);
        //dbLeaderboardsReference.Child(uuid).Child("noOfboxDelivered").SetValueAsync(0);
       dbLeaderboardsReference.Child(uuid).Child("userName").SetValueAsync(userName);

        dbPlayerStatsReference.Child(uuid).Child("active").SetValueAsync(active);
        //dbPlayerStatsReference.Child(uuid).Child("noOfMoneyEarned").SetValueAsync(0);
        //dbPlayerStatsReference.Child(uuid).Child("noOfboxDelivered").SetValueAsync(0);
        dbPlayerStatsReference.Child(uuid).Child("userName").SetValueAsync(userName);

    }

    /// <summary>
    /// Create a new entry ONLY if its the first time playing
    /// Update when there's existing entries
    /// </summary>
    /// <param name="uuid"></param>
    /// <param name="totalMoney"></param>
    /// <param name="time"></param>
    /// <param name="displayname"></param>
    public void UpdatePlayerStats(string uuid, int money, int boxes, string displayName)
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
                //DataSnapshot gameData = Task.Result;
                DataSnapshot leaderBoards = Task.Result;
                //check if there is an entry created
                if (playerStats.Exists)
                {
                    //UPDATE player stats
                    //compare existing totalMoney and set new totalMoney

                    //create a temp object sp which stores info from the player stats
                    PlayerStats sp = JsonUtility.FromJson<PlayerStats>(playerStats.GetRawJsonValue());
                    //sp.totalTimeSpent += time;
                    sp.updatedOn = sp.GetTimeUnix();

                    // temp lb to store leaderboard
                    LeaderBoard lb = JsonUtility.FromJson<LeaderBoard>(leaderBoards.GetRawJsonValue());
                    //lb.totalTimeSpent += time;

                    // temp gd to store gd
                    //GameData gd = JsonUtility.FromJson<GameData>(gameData.GetRawJsonValue());
                    //gd.updatedOn = gd.GetTimeUnix();
                    //gd.totalTimeSpent += time;

                    //continue to update the user total earning throughout the game
                    sp.noOfMoneyEarned += money;
                    sp.noOfboxDelivered += boxes;
                    UpdatePlayerLeaderBoardEntry(uuid, sp.userName, sp.noOfMoneyEarned, sp.noOfboxDelivered, sp.updatedOn);

                    //update with entire temp sp object
                    //path: playerstats/$uuid
                    dbPlayerStatsReference.Child(uuid).SetRawJsonValueAsync(sp.PlayerStatsToJson());
               
                }
                else
                {
                    //CREATE player stats
                    //if there's no existing data, it's our first time player
                    PlayerStats sp = new PlayerStats(displayName, money, boxes);
                    //Was this PlayerStats sp = new PlayerStats(displayName, money,  time);
                    LeaderBoard lb = new LeaderBoard(displayName, money, boxes);
                    //GameData gd = new GameData(displayName, money, time, tier1Count, tier2Count, tier1Cost, tier2Cost, tier1Profit,
                        //tier2Profit, upgradeCost, hitPower, rateOfMoney);

                    //create new entries into firebase
                    dbPlayerStatsReference.Child(uuid).SetRawJsonValueAsync(sp.PlayerStatsToJson());
                    dbLeaderboardsReference.Child(uuid).SetRawJsonValueAsync(lb.LeaderBoardToJson());
                    //dbGameDataReference.Child(uuid).SetRawJsonValueAsync(gd.GameDataToJson());
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
        dbLeaderboardsReference.Child(uuid).Child("noOfMoneyEarned").SetValueAsync(totalMoney);
        dbLeaderboardsReference.Child(uuid).Child("noOfboxDelivered").SetValueAsync(totalTimeSpent);
        dbLeaderboardsReference.Child(uuid).Child("updatedOn").SetValueAsync(updatedOn);
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
                        Debug.LogFormat("Leaderboard: Rank {0} Playername {1} Money Earned {2}", rankCounter, lb.userName, lb.noOfboxDelivered, lb.noOfMoneyEarned);

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
