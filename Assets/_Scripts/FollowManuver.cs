﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowManuver : MonoBehaviour {

	private Rigidbody rb;
	private Transform target;		//Player ship
	public float chaseSpeed;		//Movement speed
	private GameObject playerShip;

	void Start () {
		rb = GetComponent<Rigidbody> ();
		playerShip = GameObject.FindWithTag ("Player");

		if (playerShip == null) {
			playerShip = GameObject.FindWithTag ("God");
		}

		target = playerShip.transform;


	}

	void FixedUpdate () {
		//while player is alive look at player, rotate enemy unit to face, and move towards player
		if (target) {
			rb.transform.LookAt (target.position);
			transform.Rotate (new Vector3 (0, 180, 0), Space.Self);
			transform.position = Vector3.MoveTowards(transform.position, target.position, chaseSpeed);
		}
	}
}
