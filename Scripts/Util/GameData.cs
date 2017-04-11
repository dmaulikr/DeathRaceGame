﻿using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameData : PausableBehaviour
{

    /* params to be set by the user */
    public int numTracks = 2;
    public string[] sceneSequence;


    private int currentTrack = 0;
    private int playersFinished = 0;

    public string[] PlayerNames =
    {
        "Player 1",
        "AI 1"
    };


    //map of player names to Data objects
    private Dictionary<string, DataDTO> playerData;

    public object async;

    // Use this for initialization
    protected override void _awake () {
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
            return playerData[playerName].lapTimes[currentTrack] + "";
        }

        return null;
    }

    /*
     * Signal a player as finished the race
     */
    public void addPlayerFinish(string playerName, int placement, float totalTime)
    {
        playerData[playerName].placements[currentTrack] = placement;
        playerData[playerName].lapTimes[currentTrack] = totalTime;
        playersFinished++;

        //The game is over
        if(playersFinished == PlayerNames.Length)
        {
            //loadNextTrack();
            Debug.Log("GAMWE IS OVER");
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

    public void loadNextTrack()
    {
        Debug.Log("Race finished!");
        playersFinished = 0;
        currentTrack++;
        loadSceneAfterSeconds(sceneSequence[currentTrack], 3f);
    }

    public void loadSceneAfterSeconds(string sceneName, float seconds)
    {
        if(Array.IndexOf(sceneSequence, sceneName) < 0)
        {
            Debug.LogError(sceneName + " Not found!");
        }
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

}
