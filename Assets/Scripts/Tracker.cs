using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour {

	//MOVE VARIABLES
	public float endX;

	public GameObject GearLeft;
	public GameObject GearRight;
	private GameObject Player;

	public LaserBlast laserBlast;

	public bool vertical = false;	//Does the tracker move horizontal or vertical

	/*private GameObject ArmLeft;
	private GameObject ArmRight;
	private float armMoveSpeed = 1;
	private float armRotateSpeed = 1;*/	//Maybe one day I'll add an arm move effect, too much work to waste on it now

	public GameObject Lense;	//For glowing eye effect
	private float lenseGrowth = .2f;
	private float lenseDiminish = .05f;

	private float differenceX;
	private float gearSize;
	private float startX;

	private bool playerDead = false;
	private float deadSpeed = -.055f;

	private float maxDist = 3.75f;
	private float maxSpeed = .18f;

	private PlayerController pc;

	void Start () {
		gearSize = GearLeft.GetComponent<Renderer> ().bounds.size.x;

		Lense.GetComponent<SpriteRenderer> ().color = new Vector4 (1,1,1,0);

		if (!vertical)
			startX = transform.position.x;
		else
			startX = transform.position.y;

		Player = GameObject.Find ("Player");	//Needed to track X coordinate
		pc = Player.GetComponent<PlayerController>();
	}

	void Update () {
		if (pc.dead)
			playerDead = true;
		else
			playerDead = false;

		if (!vertical) {
			if (transform.position.x > Player.transform.position.x - maxDist && transform.position.x < Player.transform.position.x + maxDist && !playerDead) {
				differenceX = Player.transform.position.x - transform.position.x;
				if (Lense.GetComponent<SpriteRenderer> ().color.a < 1)
					Lense.GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, Lense.GetComponent<SpriteRenderer> ().color.a + lenseGrowth);
				laserBlast.charging = true;
			} else {
				differenceX = deadSpeed;
				if (Lense.GetComponent<SpriteRenderer> ().color.a > 0)
					Lense.GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, Lense.GetComponent<SpriteRenderer> ().color.a - lenseDiminish);
				laserBlast.charging = false;
			}
		} else {
			if (transform.position.y > Player.transform.position.y - maxDist && transform.position.y < Player.transform.position.y + maxDist && !playerDead) {
				differenceX = Player.transform.position.y - transform.position.y;
				if (Lense.GetComponent<SpriteRenderer> ().color.a < 1)
					Lense.GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, Lense.GetComponent<SpriteRenderer> ().color.a + lenseGrowth);
				laserBlast.charging = true;
			} else {
				differenceX = deadSpeed;
				if (Lense.GetComponent<SpriteRenderer> ().color.a > 0)
					Lense.GetComponent<SpriteRenderer> ().color = new Vector4 (1, 1, 1, Lense.GetComponent<SpriteRenderer> ().color.a - lenseDiminish);
				laserBlast.charging = false;
			}
		}

		if (differenceX > maxSpeed)	//Help avoid ugly jerks to player location
			differenceX = maxSpeed;
		else if (differenceX < -maxSpeed)
			differenceX = -maxSpeed;
			
		if (!vertical) {
			if (transform.position.x + differenceX < startX) {
				if (transform.position.x != startX)
					differenceX = transform.position.x - startX;
				else
					differenceX = 0;
				transform.position = new Vector3 (startX, transform.position.y, transform.position.z);
			} else if (transform.position.x + differenceX > endX) {
				if (transform.position.x != endX)
					differenceX = transform.position.x - endX;
				else
					differenceX = 0;
				transform.position = new Vector3 (endX, transform.position.y, transform.position.z);
			} else
				transform.position = new Vector3 (transform.position.x + differenceX, transform.position.y, transform.position.z);	//Move Tracker to Player's X
		} else {
			if (transform.position.y + differenceX < startX) {
				if (transform.position.y != startX)
					differenceX = transform.position.y - startX;
				else
					differenceX = 0;
				transform.position = new Vector3 (transform.position.x, startX, transform.position.z);
			} else if (transform.position.y + differenceX > endX) {
				if (transform.position.y != endX)
					differenceX = transform.position.y - endX;
				else
					differenceX = 0;
				transform.position = new Vector3 (transform.position.x, endX, transform.position.z);
			} else
				transform.position = new Vector3 (transform.position.x, transform.position.y + differenceX, transform.position.z);	//Move Tracker to Player's X
		}


		if (differenceX != 0) {	//Rotate Wheels
			GearLeft.transform.Rotate (0, 0, -(float)(differenceX / (gearSize * 3.14) * 180));
			GearRight.transform.Rotate (0, 0, -(float)(differenceX / (gearSize * 3.14) * 180));
		}
	}
}	
