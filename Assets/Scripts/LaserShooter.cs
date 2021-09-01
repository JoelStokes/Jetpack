using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour {

	public int shootCount = 100;
	public int startTime = 0;
	public GameObject Laser;
	public AudioClip laserSFX;

	private int counter = 0;
	private bool started = false;

	// Update is called once per frame
	void Update () {
		counter++;
		if (!started) {
			if (counter >= startTime) {
				counter = 0;
				started = true;
			}
		} else {
			if (counter >= shootCount) {
				counter = 0;
				GameObject newLaser = Instantiate (Laser, new Vector3 (transform.position.x, 
					transform.position.y, transform.position.z + 1), transform.rotation) as GameObject;
				GetComponent<AudioSource> ().PlayOneShot (laserSFX, 1f);
			}
		}
	}
}
