﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ocillator : MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    float movementFactor; // 0 for not moved, 1 for fully moved

    Vector3 startingPos;

	// Use this for initialization
	void Start () {
        startingPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		// Set movement factor
        if(period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period; // Grows continually from 0

        const float tau = Mathf.PI * 2; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to 1

        movementFactor = (rawSinWave + 1f) / 2f; // goes from 0 to 1
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
	}
}
