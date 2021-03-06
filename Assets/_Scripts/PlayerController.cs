﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour 
{
	private Rigidbody rb;
	public float tilt;
	public float speed;
	public Boundary boundary;
	public GameObject shot;
    public GameObject shield;
	public float fireRate = 0.5f;
	private float nextFire = 0.0f;
	private AudioSource audioSource;
	private GameController gameController;

	// the code below is for the multi shot power up
	private bool multiShot;
	public Transform shotSpawn1; 
	public Transform shotSpawn2; 
	public Transform shotSpawn3;
    public Transform shieldSpawn1;
    public Transform shieldSpawn2;
    public Transform shieldSpawn3;
    public Transform shieldSpawn4;
    private int multiShotAmmo;
    private bool fireRateTime;


	//private DestroyByContact shield = new DestroyByContact();
	private bool theSwitch;

	//gunSwitch doesn't seem to work when I use it from PickupOnContact.cs
	public void gunSwitch()
	{
		multiShot = true;
	}

	void Start ()
	{
		theSwitch = false;
		multiShot = false;
        fireRateTime = false;
		multiShotAmmo = 0;
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();

		GameObject gameControllerObject = GameObject.FindWithTag("GameController");

		if (gameControllerObject != null){
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		if (gameController == null){
			Debug.Log("cannot find 'GameController' script");
		}
	}

	void Update()
	{
		//if (theSwitch) {
		//	shield.shieldSwitch (true);
		//} 
		//else
		//{
		//	shield.shieldSwitch (false);
		//}


		if(Input.GetButton ("Fire1") && Time.time > nextFire){

			if (multiShotAmmo > 0)
			{
				FireShots ();
				multiShotAmmo--;
				//StartCoroutine ("MultiShotTimer", 0); // use this if you want multiShot to be timed instead of having ammo count
			}
			nextFire = Time.time + fireRate;
			Instantiate (shot, shotSpawn1.position, shotSpawn1.rotation);// as GameObject;
			audioSource.Play();
		}
	}

	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rb.velocity = movement * speed;

		rb.position = new Vector3
		(
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);

		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}


	void OnTriggerEnter (Collider other)
	{
		//the power ups
		if (other.tag == "MultiShot") {
			//multiShot = true; // if you want multi shot to use timer
			multiShotAmmo += 20;
			Destroy (other.gameObject);
		} else if (other.tag == "FireRate") {
			fireRate *= 0.75f;
            if(fireRate < 0.20f)
            {
                fireRate = 0.20f;
            }
			Destroy (other.gameObject);
            if (!fireRateTime)
            {
                fireRateTime = true;
                StartCoroutine("FireRateTimer", 0);
            }
		} else if (other.tag == "SpeedBoost") {
			speed *= 1.25f;
			Destroy (other.gameObject);
			StartCoroutine ("SpeedBoostTimer", 0);
		} else if (other.tag == "ShieldPowerUp") {
			
			Destroy (other.gameObject);
            if (!theSwitch)
            {
                Instantiate(shield, shieldSpawn1.position, shieldSpawn1.rotation);
                Instantiate(shield, shieldSpawn2.position, shieldSpawn2.rotation);
                Instantiate(shield, shieldSpawn3.position, shieldSpawn3.rotation);
                Instantiate(shield, shieldSpawn4.position, shieldSpawn4.rotation);

                theSwitch = true;
                StartCoroutine("ShieldTimer", 0);
            }
		}
		if (other.tag == "bonusPowerUp") {
			Debug.Log ("Picked up Bonus level");
			Destroy (other.gameObject);
			gameController.setBonus(true);
			Debug.Log ("Set the bonus flag to true");
		}
			
	}

	void FireShots()
	{
		// the second bullet for the multiShot power up
		GameObject Bullet2 = (GameObject)Instantiate (shot, shotSpawn2.position, shotSpawn2.rotation);
		Bullet2.transform.position = shotSpawn2.transform.position;

		// the third bullet for the multiShot power up
		GameObject Bullet3 = (GameObject)Instantiate (shot, shotSpawn3.position, shotSpawn3.rotation);
		Bullet3.transform.position = shotSpawn3.transform.position;
	}

	//after fire rate power up is obtained, this code will undo the boost after 10 seconds
	IEnumerator FireRateTimer()
	{
		yield return new WaitForSeconds(10f);
		fireRate = 0.35f;
        fireRateTime = false;
	}

	//after speed boost power up is obtained, this code will undo the boost after 10 seconds
	IEnumerator SpeedBoostTimer()
	{
		yield return new WaitForSeconds(10f);
		speed /= 1.25f;
	}

	//this code will not allow multi shot power up to be stacked
	IEnumerator MultiShotTimer()
	{
		yield return new WaitForSeconds(10f);
		multiShot = false;
	}
	IEnumerator ShieldTimer()
	{
		yield return new WaitForSeconds (10f);
		theSwitch = false;
	}

}
