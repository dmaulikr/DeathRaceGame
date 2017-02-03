﻿using UnityEngine;
using System.Collections;

public class GameContext : MonoBehaviour {

    public StartingSequence staringSequence;

    public RacePlayer player;

    public RacePlayer[] allPlayers;

    public BackgroundScript pauseMenu;

    //will only read button once every second
    private const float buttonPressTime = 0.25f;

    private float pauseLastPressed;

    void Awake () {

        pauseLastPressed = 0f;

        /**
         * set player.behaviorBlocked/staringSequence.behaviorBlocked to true/false 
         * in an actual game and false/true when testing to skip sequence
         */

        staringSequence.behaviorBlocked = true;

        pauseMenu.paused = false;

        allPlayers = FindObjectsOfType<RacePlayer>();
        Debug.Log(allPlayers.Length);
    }
	
	// TODO: Handle inputs through this class only
	void Update () {
        handleInputs();

        if (staringSequence.finished && player.behaviorBlocked && !pauseMenu.paused)//Only change if player blocked (only should call once)
        {
            foreach (RacePlayer p in allPlayers)
            {
                p.behaviorBlocked = false;
            }
        }
	}

    private void handleInputs()
    {
        //pause game
        if (!pauseMenu.paused && Input.GetKey(KeyCode.Q) && (Time.fixedTime - pauseLastPressed > buttonPressTime))
        {
            pauseGame();
        }
        //un pause game
        else if(pauseMenu.paused && Input.GetKey(KeyCode.Q) && (Time.fixedTime - pauseLastPressed > buttonPressTime))
        {
            unpauseGame();
        }

    }

    private void pauseGame()
    {
        Debug.Log("paused game");
        pauseLastPressed = Time.fixedTime;
        pauseMenu.paused = true;

        foreach(RacePlayer p in allPlayers)
        {
            p.behaviorBlocked = true;
        }

        staringSequence.behaviorBlocked = true;

        blockPrefabBehavior();
    }

    private void unpauseGame()
    {
        Debug.Log("un-paused game");
        pauseLastPressed = Time.fixedTime;
        pauseMenu.paused = false;

        unblockPrefabBehavior();

        staringSequence.behaviorBlocked = false;
        if (staringSequence.finished)
        {
            foreach (RacePlayer p in allPlayers)
            {
                p.behaviorBlocked = false;
            }
        }
    }

    private void blockPrefabBehavior()
    {
        var bannerPrefabs = FindObjectsOfType<BannerScroll>();

        foreach(var banner in bannerPrefabs)
        {
            banner.behaviorBlocked = true;
        }
    }

    private void unblockPrefabBehavior()
    {
        var bannerPrefabs = FindObjectsOfType<BannerScroll>();

        foreach (var banner in bannerPrefabs)
        {
            banner.behaviorBlocked = false;
        }
    }
}
