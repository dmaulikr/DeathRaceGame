﻿using UnityEngine;
using System.Collections;

/**
 * This class is used for debugging purposes only.
 * 
 * It is used to track the movement of a given entity by displaying a path of its position over time
 */

 //TODO could add option to also display orientation etc.
public class PathDebug : MonoBehaviour {

    public GameObject target;

    private Vector3 previousSpot;
	
    void Awake()
    {
        previousSpot = target.transform.position;
    }

	// Update is called once per frame
	void Update () {
        AppConfig.DrawLine(previousSpot, target.transform.position, Color.blue);

        //Debug.Log((previousSpot - target.transform.position).magnitude);

        previousSpot = target.transform.position;
    }

}
