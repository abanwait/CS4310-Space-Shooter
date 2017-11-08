﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {
	
	private GameController gameController;
	private int currentLevel;
	public GameObject[] hazards;
//	public Vector3 spawnValues;
//	public int hazardCount;
//	public int waveCount;
//	public float spawnWait;
//	public float startWait;
//	public float waveWait;

	void Start () {
		GameObject gameControllerObject = GameObject.FindWithTag("GameController");
		if(gameControllerObject != null){
			gameController = gameControllerObject.GetComponent<GameController>();
			//gameController.toggleReadyForLevel();
			StartCoroutine (SpawnWaves());
			//SpawnWaves();
		}
		if (gameController == null)
		{
			Debug.Log("cannot find 'GameController' script");
		}

	}


	void Update () {
		
	}

	IEnumerator SpawnWaves()
	{	
		for(int j = 0;j < gameController.waveCount; j++){
			//System.Threading.Thread.Sleep((int)startWait*1000);
			yield return new WaitForSeconds (gameController.startWait);
			for(int i = 0;i < gameController.hazardCount; i++)
			{
				GameObject hazard = hazards[Random.Range(0,hazards.Length)];
				Vector3 spawnPosition = new Vector3(Random.Range(-gameController.spawnValues.x,gameController.spawnValues.x), gameController.spawnValues.y, gameController.spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity;
				Instantiate (hazard, spawnPosition, spawnRotation);
				//System.Threading.Thread.Sleep((int)spawnWait*1000);
				yield return new WaitForSeconds (gameController.spawnWait);
			}
			yield return new WaitForSeconds (gameController.waveWait);
			//System.Threading.Thread.Sleep((int)waveWait*1000);

		}
		gameController.toggleReadyForLevel();
		Destroy(gameObject);
	}
}
