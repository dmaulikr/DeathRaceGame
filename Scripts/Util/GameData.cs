﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameData : PausableBehaviour
{

    /* params to be set by the user */
    private int numTracks;
    public string[] sceneSequence;


    private int currentTrack = 0;
    private int playersFinished = 0;

    public string[] PlayerNames =
    {
        "Player 1",
        "Player 2"
    };

    /* This will define the main player in this race */
    public string mainPlayer = "Player 1";

    //map of player names to Data objects
    private Dictionary<string, DataDTO> playerData;

    public object async;

    // Use this for initialization
    protected override void _awake () {
        numTracks = sceneSequence.Length;
        if (sceneSequence.Length == 0){
            Debug.LogError("No valid scenes to turn to!");
        }

        playerData = new Dictionary<string, DataDTO>();
        foreach (string player in PlayerNames)
        {
            playerData.Add(player, new DataDTO(numTracks));
        }
    }

    /*
     * validates that this playerName is in the static PlayerNames 
     * array since only those are legal player names.
     */
    public bool validatePlayerName(string playerName)
    {
        return Array.IndexOf(PlayerNames, playerName) >= 0;
    }

    public string getPlacement(string playerName)
    {
        if(validatePlayerName(playerName))
        {
            return playerData[playerName].placements[currentTrack] + "";
        }

        return null;
    }

    public string getTotalTime(string playerName)
    {
        if (validatePlayerName(playerName))
        {
            return "Time: " + String.Format("{0:f} s", playerData[playerName].lapTimes[currentTrack].Sum());
        }

        return null;
    }

    /*
     * Signal a player as finished the race
     */
    public void addPlayerFinish(string playerName, float[] lapTimes)
    {
        playersFinished++;
        playerData[playerName].placements[currentTrack] = playersFinished;
        playerData[playerName].lapTimes[currentTrack] = lapTimes.ToList();

        Debug.Log(playerName + " finished " + playersFinished);

        //The game is over
        if(playersFinished == PlayerNames.Length)
        {
            Debug.Log("GAME IS OVER");
        }
    }

    /*
     * Debugging method to print contents
     */
    public void printContents()
    {
        Debug.Log("current Track:" + 0);
        foreach (string player in PlayerNames)
        {
            string plac = "";
            foreach(int p in playerData[player].placements)
            {
                plac += p + ", ";
            }
            Debug.Log(player + ": " + plac);
        }
    }

    /*
     * Used to get all player data
     */
    public List<string> getPlayersByPlacement()//TODO:also return total lap times
    {
        return playerData
            .OrderBy(e => e.Value.placements.ToList().Sum())
            .Select(e => e.Key)
            .ToList();
    }


    public void loadNextTrack()
    {
        Debug.Log("Race finished!");
        playersFinished = 0;
        currentTrack++;

        //Return to the main menu if the sequence has finished
        if(currentTrack == sceneSequence.Length)
        {
            loadSceneAfterSeconds("CurcuitResults", 0.5f);
            return;
        }

        loadSceneAfterSeconds(sceneSequence[currentTrack], 0.5f);
    }

    public void loadSceneAfterSeconds(string sceneName, float seconds)
    {
        callAfterSeconds(seconds, () =>
        {
            async = SceneManager.LoadSceneAsync(sceneName);
            StartCoroutine(Load(seconds));
        });
    }

    IEnumerator Load(float seconds)
    {
        yield return async;
    }

    public class DataDTO
    {
        public int[] placements;

        public List<float>[] lapTimes;

        public DataDTO(int tracks)
        {
            placements = new int[tracks];
            lapTimes = new List<float>[tracks];
        }
    }

}
