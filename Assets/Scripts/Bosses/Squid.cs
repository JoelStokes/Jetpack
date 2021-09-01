using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : MonoBehaviour {

	public GameObject Eyeball;
	public GameObject Tentacle;

	private GameObject Player;

	public float tentacleSpawnY;

	public int spawnTime;
	public int spawnMult;

	private int health = 3;
	private int spawnCounter = 0;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
	}

	// Update is called once per frame
	void Update () {
		/*spawnCounter++;
		if (spawnCounter >= spawnTime + (health * spawnMult))
			Spawn ();*/

		RotateEye ();
	}

	void RotateEye(){
		Vector3 vectorToTarget = Player.transform.position - Eyeball.transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		Eyeball.transform.rotation = Quaternion.Slerp (Eyeball.transform.rotation, q, Time.deltaTime);
	}

	void Spawn(){
		Instantiate (Tentacle, new Vector3(Player.transform.position.x, tentacleSpawnY, Tentacle.transform.position.z), Quaternion.identity);
		spawnCounter = 0;
	}
}
