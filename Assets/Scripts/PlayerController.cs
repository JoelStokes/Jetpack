using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;	//FOR ENGLISH BETA TO RETURN TO TITLE SCREEN! Will change or remove later?

public class PlayerController : MonoBehaviour {

	//BOSS VARIABLES, STILL IN EARLY PROGRESS
	public bool lockCameraY;
	public bool lockCameraX;
	public float lockTopY;
	public float lockBottomY;
	public float lockLeftX;
	public float lockRightX;
	private GameObject Camera;

	//Jetpack Variables
	private int fuelMax = 90;
	public int fuel;	//Set to public so that the Fuelbar can access it
	private int fuelAdd = 3;
	private float maxFlightSpeed = 9;

	//Framerate Fix Variables
	private float flyTimer = 0;
	private float flyFuelTime = 0.015f;

	//Death Variables
	public GameObject Explosion;
	public GameObject SpawningLight;
	private float midX = 0;
	private float midY = 0;
	public bool dead = false;	//Used for outside scripts to see if the player is dead
	private float lerpSpeed;
	private const float lerpSpeedCONST = 1.5f;
	private float lerpSpeedMult = 1.035f;
	private float lerpStartTime;
	private float lerpLength;
	private Vector3 lerpStartPos;
	private float lerpTimer = 0;

	//Physics Variables
	private float walkMaxSpeed = 5f;
	private float runMaxSpeed = 9.5f;
	private float speedDecriment = 1.1f;
	private float speedDecrimentGround = 1.15f;
	private float speedAdditive = 1.18f;
	private float flightForce = 29.25f;
	private float gravityScale = 0;	//Original gravity scale, set in Start
	private const float gravityCap = -40;	//So you don't fall too fast that you go through blocks

	private Rigidbody2D rigi;

	bool grounded = false;
	public Transform GroundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;

	//SFX Variables
	private AudioSource audioSource;
	public AudioClip RespawnSFX;
	public AudioClip JetpackSFX;

	//Visual Sprite Variables
	private SpriteRenderer PlayerImage;
	public bool facingRight = true;
	public Sprite StandingPic;
	public Sprite Walk1Pic;
	public Sprite Walk2Pic;
	private float walkTimer = 0;
	private int walkSprite = 0;	//What number currently on. 0 & 2 = stand, 1 walk1pic, 3 walk2pic.

	public GameObject JetParticleObject;
	private ParticleSystem jetParticles;
	private float jetParticleLifetime;
	private bool jetting = false;
	private float jetX = 0;	//Set at start

	//Moving Platform Variables
	private float movingPlatformSpeed = 0;
	private bool movingPlatform = false;

	// Use this for initialization
	void Start () {
		rigi = GetComponent<Rigidbody2D> ();
		gravityScale = rigi.gravityScale;
		audioSource = GetComponent<AudioSource>();
		PlayerImage = GetComponent<SpriteRenderer> ();
		jetX = JetParticleObject.transform.position.x;
		jetParticles = JetParticleObject.GetComponent<ParticleSystem> ();
		jetParticleLifetime = jetParticles.startLifetime;

			midX = transform.position.x;	//Used to set back to spawn if die before midpoint
			midY = transform.position.y;

		fuel = fuelMax;

		//Camera
		Camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	void FixedUpdate()
	{
		if (!dead) {
			grounded = Physics2D.OverlapCircle (GroundCheck.position, groundRadius, whatIsGround);

			float move = 0;
			move = Input.GetAxis ("Horizontal");

			if (move > 0) {				//Changes which way the player is facing
				if (!facingRight) {
					facingRight = true;
					PlayerImage.flipX = false;
				}
			} else if (move < 0) {
				if (facingRight) {
					facingRight = false;
					PlayerImage.flipX = true;
				}
			}
			
			if (facingRight)
				JetParticleObject.transform.position = new Vector3 (jetX + transform.position.x,
					transform.position.y, JetParticleObject.transform.position.z);
			else
				JetParticleObject.transform.position = new Vector3 (-jetX + transform.position.x,
					transform.position.y, JetParticleObject.transform.position.z);

			float rigiMove = rigi.velocity.x + (move * speedAdditive);
			//if (Input.GetKey (KeyCode.LeftShift)) {
			if (rigiMove > runMaxSpeed)
				rigiMove = runMaxSpeed;
			else if (rigiMove < -runMaxSpeed)
				rigiMove = -runMaxSpeed;
			/*} else {
			if (rigiMove > walkMaxSpeed)
				rigiMove = walkMaxSpeed;		//Walking didn't feel necessary, still here if need be
			else if (rigiMove < -walkMaxSpeed)
				rigiMove = -walkMaxSpeed;
		}*/
			if (grounded && move == 0)
				rigi.velocity = new Vector2 ((rigiMove / speedDecrimentGround), rigi.velocity.y);	//Higher friction on ground
			else
				rigi.velocity = new Vector2 (rigiMove / speedDecriment, rigi.velocity.y);
		}
	}

	
	// Update is called once per frame
	void Update () {
		if (!dead) {
			jetting = false;
			if (Input.GetKey (KeyCode.Space) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) {
				Fly ();
				if (fuel > 0)
					audioSource.PlayOneShot (JetpackSFX, .12f);
			}

			if (grounded) {
				if (fuel < fuelMax) {
					flyTimer += Time.deltaTime;
					if (flyTimer >= flyFuelTime)
                    {
						fuel += fuelAdd;
						flyTimer = 0;
						if (fuel > fuelMax)
							fuel = fuelMax;
					}
				}
			}

			Physics2D.IgnoreLayerCollision (8, 10, (rigi.velocity.y > 0.0f));	//Passable blocks

			if (rigi.velocity.y < gravityCap)	//Gravity Cap & Jetpack Cap
			rigi.velocity = new Vector2 (rigi.velocity.x, gravityCap);
			else if (rigi.velocity.y > maxFlightSpeed)
				rigi.velocity = new Vector2 (rigi.velocity.x, maxFlightSpeed);

			if (jetting) {
				jetParticles.startLifetime = jetParticleLifetime;
			} else {
				jetParticles.startLifetime = 0;
			}
		} else {		//Dead, currently lerping back to last checkpoint
			Respawn();
		}

		if (Input.GetKeyDown (KeyCode.Escape)) {
			SceneManager.LoadScene ("Title");
		}

		if (movingPlatform)
			transform.position = new Vector3 (transform.position.x - (movingPlatformSpeed + (movingPlatformSpeed / 200)), 
				transform.position.y, transform.position.z);

		AnimateWalk ();
		ManageCamera ();	//Control Camera Movement
	}

	void ManageCamera(){
		Vector3 newPosition = Camera.transform.position;
		if (!lockCameraX || (transform.position.x > lockLeftX && transform.position.x < lockRightX)) {
			newPosition = new Vector3 (transform.position.x, newPosition.y, newPosition.z);
		}
		if (!lockCameraY || (transform.position.y > lockBottomY && transform.position.y < lockTopY)) {
			newPosition = new Vector3 (newPosition.x, transform.position.y, newPosition.z);
		}

		Camera.transform.position = newPosition;
	}

	void Fly(){
		if (fuel > 0) {
			flyTimer += Time.deltaTime;
			if (flyTimer >= flyFuelTime)
            {
				fuel--;
				rigi.AddForce(new Vector2(0, flightForce));
				flyTimer = 0;
			}
			jetting = true;
		}
	}

	void AnimateWalk(){ //Manage walkAnimation cycle
		walkTimer += Time.deltaTime;

		if (((rigi.velocity.x > 8f || rigi.velocity.x < -8f) && walkTimer > .067f) || 
			((rigi.velocity.x > 5f || rigi.velocity.x < -5f) && walkTimer > 0.117f) ||
			((rigi.velocity.x > 3.5f || rigi.velocity.x < -3.5f) && walkTimer > 0.15f) ||
			((rigi.velocity.x > 2f || rigi.velocity.x < -2f) && walkTimer > 0.217f) ||
			((rigi.velocity.x > 1f || rigi.velocity.x < -1f) && walkTimer > 0.283f) ||
			((rigi.velocity.x > .5f || rigi.velocity.x < -.5f) && walkTimer > 0.317f) ||
			((rigi.velocity.x > 0.005f && rigi.velocity.x < -0.005f) && walkTimer > 0.35f) && grounded) {
			walkTimer = 0;
			walkSprite++;

			ChangeWalkSprite ();

		} else if (!grounded || ((rigi.velocity.x < 0.005f && rigi.velocity.x > -0.005f) && grounded)) {
			walkTimer = 0;

			if (walkSprite == 1)
				walkSprite = 2;
			else if (walkSprite == 3)
				walkSprite = 0;

			ChangeWalkSprite();
		}
	}

	void ChangeWalkSprite(){
		if (walkSprite > 3)
			walkSprite = 0;

		switch (walkSprite) {
		case 0:
			PlayerImage.sprite = StandingPic;
			break;
		case 1: 
			PlayerImage.sprite = Walk1Pic;
			break;
		case 2:
			PlayerImage.sprite = StandingPic;
			break;
		case 3:
			PlayerImage.sprite = Walk2Pic;
			break;
		}
	}

	void Respawn(){		//Lerp to last checkpoint position, otherwise lerp back to spawn location
		float distCovered = (Time.time - lerpStartTime) * lerpSpeed;
		float fracJourney = distCovered / lerpLength;
		transform.position = Vector3.Lerp (lerpStartPos, new Vector3 (midX, midY, transform.position.z), 
			fracJourney);
		if ((transform.position.x > midX - .15f && transform.position.x < midX + .15f) && 
			(transform.position.y > midY - .15f && transform.position.y < midY + .15f)) {
			dead = false;
			GameObject spawnBeam = Instantiate (SpawningLight, new Vector3 (midX, midY, 
				transform.position.z - 2), Quaternion.identity) as GameObject;
			rigi.isKinematic = false;
			GetComponent<BoxCollider2D> ().enabled = true;
			transform.position = new Vector3 (midX, midY, transform.position.z);
			audioSource.PlayOneShot (RespawnSFX, 1f);
			PlayerImage.color = new Vector4 (1, 1, 1, 1);
			rigi.velocity = new Vector2 (0, 0);
			fuel = fuelMax;
		}
		lerpTimer += Time.deltaTime;
		if (lerpTimer >= flyFuelTime)
        {
			lerpSpeed *= lerpSpeedMult;
			lerpTimer = 0;
		}
	}

	void OnTriggerEnter2D(Collider2D other){	
		if (other.gameObject.tag == "Hurt") {	//Die
			if (!dead) {
				GameObject deathExplosion = Instantiate (Explosion, new Vector3 (transform.position.x, 
					                            transform.position.y, transform.position.z - 2), Quaternion.identity) as GameObject;
				PlayerImage.color = new Vector4 (1, 1, 1, 0);
				dead = true;
				lerpStartTime = Time.time;
				lerpStartPos = transform.position;
				lerpLength = Vector3.Distance (lerpStartPos, new Vector3 (midX, midY, transform.position.z));
				rigi.isKinematic = true;
				GetComponent<BoxCollider2D> ().enabled = false;
				lerpSpeed = lerpSpeedCONST;
			}
		} else if (other.gameObject.tag == "Midpoint") {	//Midpoint found
			midX = other.GetComponent<Midpoint> ().MidGem.transform.position.x;
			midY = other.GetComponent<Midpoint> ().MidGem.transform.position.y;
			other.GetComponent<Midpoint> ().Activate ();
		} else if (other.gameObject.tag == "Orb") {	//Midpoint found
			Collectible collectibleScript = other.GetComponent<Collectible> ();
			audioSource.PlayOneShot (collectibleScript.pickupSFX, .5f);
			collectibleScript.Collected ();
		} else if (other.gameObject.tag == "SceneChanger") {	//USED FOR ENGLISH BETA!
			SceneManager.LoadScene ("Title");
		} else if (other.gameObject.tag == "Switch" && dead != true) {
			if (other.gameObject.GetComponent<Switch> ().ready) {
				GameObject[] Switches = GameObject.FindGameObjectsWithTag ("Switch");
				for (int i = 0; i < Switches.Length; i++) {
					if (Switches [i].GetComponent<Switch> () != null) {
						if (Switches [i].GetComponent<Switch> ().multiSwitchNumber == other.gameObject.GetComponent<Switch> ().multiSwitchNumber)
							Switches [i].GetComponent<Switch> ().Activate ();
					} else {
						if (Switches[i].GetComponent<SwitchImpact>().multiSwitchNumber == other.gameObject.GetComponent<Switch> ().multiSwitchNumber)
							Switches [i].GetComponent<SwitchImpact> ().Activate ();
					}
				}
			}
		} else if (other.gameObject.tag == "MovingPlatform") {/*
			movingPlatformSpeed = other.gameObject.transform.parent.GetComponent<SquidPlatforms> ().speed;
			movingPlatform = true;
			other.gameObject.GetComponent<Animator> ().Play ("SquidPlatform", -1, 0);*/
		}
	}

	void OnTriggerStay2D(Collider2D other){		//Need TriggerStay in instances when inside radius after death
		if (other.gameObject.tag == "Follower") {	//Walked into sensor range of Follower
			GameObject Follower = other.transform.parent.gameObject;
			Follower.GetComponent<Follower> ().Activate ();
		}
	}

	void OnTriggerExit2D(Collider2D other){		//Leave outer range of Follower
		if (other.gameObject.tag == "OuterRange") {
			GameObject Follower = other.transform.parent.gameObject;
			Follower.GetComponent<Follower> ().Deactivate ();
		} else if (other.gameObject.tag == "Hurt") {	//Added to on trigger stay for Tracker laser
			if (!dead) {
				GameObject deathExplosion = Instantiate (Explosion, new Vector3 (transform.position.x, 
					transform.position.y, transform.position.z - 2), Quaternion.identity) as GameObject;
				PlayerImage.color = new Vector4 (1, 1, 1, 0);
				dead = true;
				lerpStartTime = Time.time;
				lerpStartPos = transform.position;
				lerpLength = Vector3.Distance (lerpStartPos, new Vector3 (midX, midY, transform.position.z));
				rigi.isKinematic = true;
				GetComponent<BoxCollider2D> ().enabled = false;
				lerpSpeed = lerpSpeedCONST;
			}
		} else if (other.gameObject.tag == "MovingPlatform") {
			movingPlatform = false;
			movingPlatformSpeed = 0;
		}
	}
}
