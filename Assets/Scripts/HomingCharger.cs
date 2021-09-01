using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingCharger : MonoBehaviour {

	private GameObject Player;
	private Vector3 vectorToTarget;
	private float turnSpeed = 3;

	public LaserBlast laserBlast;

	// Use this for initialization
	void Start () {
		Player = GameObject.FindGameObjectWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		laserBlast.charging = true;

		vectorToTarget = Player.transform.position - transform.position;
		float angle = (Mathf.Atan2 (vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90;
		Quaternion q = Quaternion.AngleAxis (angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp (transform.rotation, q, Time.deltaTime * turnSpeed);
	}
}
