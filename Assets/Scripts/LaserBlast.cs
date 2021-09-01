using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBlast : MonoBehaviour {

	public bool charging = false;	//Set with Tracker script

	public GameObject ChargeCloud;
	public GameObject ChargeParticles;
	public GameObject LaserBase;
	public GameObject[] LaserMid;

	public LayerMask raycastMask;

	public float direction;	//Angle at which the laser is being shot: z0 = down, 180 up, etc.

	public AudioClip ChargeSFX;
	public AudioClip FireSFX;
	private bool chargeSFXStarted = false;

	public int fireTime = 360;
	private int counter = 0;
	private int particleStart;
	private int particleChargeStop;
	private float spawnY;

	private int coreStart;
	private float coreFadeSpeed = .011f;
	private float coreGrowthSpeed = .00078f;
	private float coreStartSize;

	private bool firing = false;
	private float laserFadeSpeed = .07f;
	private int lasersShown = 0;

	public bool dynamicAngle = false;	//used to get angle from parent at time of fire

	// Use this for initialization
	void Start () {
		LaserBase.SetActive (false);
		coreStartSize = ChargeCloud.transform.localScale.y;
		LaserBase.GetComponent<SpriteRenderer> ().color = new Vector4 (1,1,1,0);

		for (int i = 0; i < LaserMid.Length; i++)
			LaserMid [i].SetActive (false);

		Reset ();

		particleStart = fireTime - (fireTime - 10);
		particleChargeStop = fireTime - 120;
		coreStart = fireTime - 200;

		if (direction < 45 && direction > -45)
			spawnY = LaserBase.GetComponent<SpriteRenderer> ().bounds.min.y;
		else if(direction > 135 && direction < 225)
			spawnY = LaserBase.GetComponent<SpriteRenderer> ().bounds.max.y;
		else if (direction > 45 && direction < 135)
			spawnY = LaserBase.GetComponent<SpriteRenderer> ().bounds.max.x;
		else
			spawnY = LaserBase.GetComponent<SpriteRenderer> ().bounds.min.x;
	}

	// Update is called once per frame
	void Update () {
		if (firing)
			Fire ();

		if (charging || counter >= particleStart) {	//If Particles started, beam will finish
			counter++;

			if (!chargeSFXStarted && counter >= 5) {
				GetComponent<AudioSource> ().PlayOneShot (ChargeSFX, 1);
				chargeSFXStarted = true;
			}

			if (counter >= coreStart) {
				ChargeCloud.SetActive (true);
				ChargeCloud.GetComponent<SpriteRenderer> ().color = new Vector4 (1,.25f,.25f,ChargeCloud.GetComponent<SpriteRenderer> ().color.a+coreFadeSpeed);
				ChargeCloud.transform.localScale = new Vector3 (ChargeCloud.transform.localScale.x - coreGrowthSpeed,
					ChargeCloud.transform.localScale.y + coreGrowthSpeed, ChargeCloud.transform.localScale.z);
			}

			if (counter >= particleStart) {
				ChargeParticles.SetActive (true);

				if (counter >= particleChargeStop) {
					SetParticleEmission (0);
				} else {
					SetParticleEmission((counter - particleStart + 80) / 8);
				}
			}

			if (counter > fireTime) {

				if (dynamicAngle) {
					direction = transform.parent.gameObject.transform.rotation.eulerAngles.z;
					if (direction < 0)
						direction += 360;
				}
				
				Vector3 Angle = Quaternion.Euler(0,0,direction) * Vector3.down;
				RaycastHit2D hit = Physics2D.Raycast (LaserBase.transform.position, Angle, 20, raycastMask, -2, 2);

				Debug.DrawRay(LaserBase.transform.position, Angle, Color.green, 10);	//FOR TESTING RAYCAST DIRECTION

				float newSpawnY = spawnY;

				//print ("Location = " + hit.transform.position.y + ", Name = " + hit.transform.tag); //FOR DISPLAYING RAYCAST HIT POSITION

				lasersShown = 0;
				if ((direction <= 45 && direction >= -45) || direction > 315) {
					while (hit.point.y < newSpawnY && lasersShown != LaserMid.Length) {
						LaserMid [lasersShown].SetActive (true);
						LaserMid [lasersShown].GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, 1);
						newSpawnY -= LaserMid [0].GetComponent<SpriteRenderer> ().bounds.size.y;
						lasersShown++;
						//print ("Current Count: " + lasersShown + ", newSpawnY: " + newSpawnY + ", hitpoint: " + hit.point.y);
					}
				} else if (direction >= 135 && direction <= 225) {
					while (hit.point.y > newSpawnY && lasersShown != LaserMid.Length) {
						LaserMid [lasersShown].SetActive (true);
						LaserMid [lasersShown].GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, 1);
						newSpawnY += LaserMid [0].GetComponent<SpriteRenderer> ().bounds.size.y;
						lasersShown++;
						//print ("Current Count: " + lasersShown + ", newSpawnY: " + newSpawnY + ", hitpoint: " + hit.point.y);
					}
				} else if (direction >= 45 && direction <= 135) {
					while (hit.point.x > newSpawnY && lasersShown != LaserMid.Length) {
						LaserMid [lasersShown].SetActive (true);
						LaserMid [lasersShown].GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, 1);
						newSpawnY -= LaserMid [0].GetComponent<SpriteRenderer> ().bounds.size.x;
						lasersShown++;
						//print ("Current Count: " + lasersShown + ", newSpawnY: " + newSpawnY + ", hitpoint: " + hit.point.y);
					}
				} else if (direction >= 225 && direction <= 315) {
					while (hit.point.x < newSpawnY && lasersShown != LaserMid.Length) {
						LaserMid [lasersShown].SetActive (true);
						LaserMid [lasersShown].GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, 1);
						newSpawnY -= LaserMid [0].GetComponent<SpriteRenderer> ().bounds.size.x;
						lasersShown++;
						//print ("Current Count: " + lasersShown + ", newSpawnY: " + newSpawnY + ", hitpoint: " + hit.point.y);
					}
				}

				Reset ();
				GetComponent<AudioSource> ().PlayOneShot (FireSFX, 1);
				counter = -60;
				LaserBase.GetComponent<SpriteRenderer> ().color = new Vector4 (1,1,1,1);
				LaserBase.SetActive (true);
				firing = true;
			}

		} else if (!charging && counter != 0)
			counter = 0;
	}

	void Fire(){
		LaserBase.GetComponent<SpriteRenderer> ().color = new Vector4 (1,1,1,LaserBase.GetComponent<SpriteRenderer> ().color.a-laserFadeSpeed);

		for (int i = 0; i < lasersShown; i++) {
			LaserMid[i].GetComponent<SpriteRenderer> ().color = new Vector4 (1,1,1,LaserMid[i].GetComponent<SpriteRenderer> ().color.a-laserFadeSpeed);
		}

		if (LaserBase.GetComponent<SpriteRenderer> ().color.a <= 0) {
			firing = false;
			LaserBase.SetActive (false);
			for (int i = 0; i < lasersShown; i++) {
				LaserMid [i].SetActive (false);
			}
		}
	}

	void Reset(){
		ChargeParticles.SetActive (false);
		ChargeCloud.GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, 0);
		ChargeCloud.transform.localScale = new Vector3 (-coreStartSize, coreStartSize, ChargeCloud.transform.localScale.z);
		ChargeCloud.SetActive (false);
		chargeSFXStarted = false;
	}

	void SetParticleEmission(int newRate){
		var emission = ChargeParticles.GetComponent<ParticleSystem> ().emission;
		var rate = emission.rate;
		rate.constantMax = newRate;
		emission.rate = rate;
	}
}
