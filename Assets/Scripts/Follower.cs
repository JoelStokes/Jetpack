using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

	public AudioClip FollowerSFX;
	public float moveSpeed = 4;
	public int noiseCounter = 120;
	public float colorChange = .05f;
	public float turnSpeed = 3;

	private bool chasing = false;
	private bool returning = false;
	private float timer = 0;

	private bool deadLimit = false;
	private float deadTimer = 0;
	private int deadCounterLimit = 120;

	private GameObject Player;
	private Vector3 StartPos;
	private Rigidbody2D rigi;

	private SpriteRenderer Lense;	//Used for flashing lense effect when chasing
	private bool brighten = false;

	// Use this for initialization
	void Start () {
		rigi = GetComponent<Rigidbody2D> ();
		StartPos = transform.position;
		GameObject LenseObject = transform.GetChild (0).gameObject;
		Lense = LenseObject.GetComponent<SpriteRenderer> ();
		Lense.color = new Vector4 (0,0,0,1);
	}
	
	// Update is called once per frame
	void Update () {
		if (chasing && !returning) {
			timer += Time.deltaTime;
			if (timer >= (noiseCounter/60)) {
				GetComponent<AudioSource> ().PlayOneShot (FollowerSFX, 1);
				timer = 0;
			}

			MoveTowards ();

			if (Player.GetComponent<PlayerController> ().dead == true) {
				Deactivate ();
				deadLimit = true;
			}

			if (brighten) {
				if (Lense.color.r < 1)
					Lense.color = new Vector4 (Lense.color.r + colorChange, Lense.color.g + colorChange, Lense.color.b + colorChange, 1);
				else
					brighten = false;
			}

		} else if (returning) {
			MoveTowards ();

			if (transform.position == StartPos)
				returning = false;
		}

		if (deadLimit) {
			deadTimer += Time.deltaTime;
			if (deadTimer >= (deadCounterLimit/60)) {
				deadTimer = 0;
				deadLimit = false;
			}
		}

		if (!brighten) {
			if (Lense.color.r > 0)
				Lense.color = new Vector4 (Lense.color.r - colorChange, Lense.color.g - colorChange, Lense.color.b - colorChange, 1);
			else
				brighten = true;
		}
	}

	public void Activate(){
		if (!chasing && !deadLimit) {
			chasing = true;
			returning = false;
			Player = GameObject.FindGameObjectWithTag ("Player");
			GetComponent<AudioSource> ().PlayOneShot (FollowerSFX, 1);
		}
	}

	public void Deactivate(){
		if (chasing) {
			chasing = false;
			returning = true;
			timer = 0;
			brighten = false;
		}
	}

	private void MoveTowards(){
		float step = turnSpeed * Time.deltaTime;	//Rotate towards and follow the destination point
		Vector3 vectorToTarget;
		if (chasing)
			vectorToTarget = Player.transform.position - transform.position;
		else
			vectorToTarget = StartPos - transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * turnSpeed);
		transform.position += transform.up * Time.deltaTime * moveSpeed;

		if (returning && (transform.position.x > StartPos.x-.05f && transform.position.x < StartPos.x+.05f && transform.position.y > StartPos.y-.05f && 
			transform.position.y < StartPos.y+.05f))
		{
			returning = false;
			deadLimit = false;
			deadTimer = 0;
			transform.position = StartPos;
		}
	}
}
