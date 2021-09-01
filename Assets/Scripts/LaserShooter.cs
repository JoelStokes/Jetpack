using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShooter : MonoBehaviour {

	public float shootCount = 100;
	public float startTime = 0;
	public GameObject Laser;
	public AudioClip laserSFX;

	private float startTimeConvert;
	private float shootCountConvert;

	private float timer = 0;
	private bool started = false;

    private void Start()
    {
		startTimeConvert = startTime / 60f;
		shootCountConvert = shootCount / 60f;
	}

    void Update () {
		timer += Time.deltaTime;
		if (!started) {
			if (timer >= startTimeConvert) {
				timer = 0;
				started = true;
			}
		} else {
			if (timer >= shootCountConvert) {
				timer = 0;
				GameObject newLaser = Instantiate (Laser, new Vector3 (transform.position.x, 
					transform.position.y, transform.position.z + 1), transform.rotation) as GameObject;
				GetComponent<AudioSource> ().PlayOneShot (laserSFX, 1f);
			}
		}
	}
}
