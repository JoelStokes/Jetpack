using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : MonoBehaviour {

	public float movementSpeed = 1;

	private int counter = 0;
	private int countLimit = 8;	//Make it so the laser cannot die before a few frames

	// Update is called once per frame
	void Update () {
		counter++;
		transform.position += transform.up * Time.deltaTime * movementSpeed;
	}

	void OnTriggerEnter2D(Collider2D other){	//Die
		if (other.gameObject.tag != "Shooter" && other.gameObject.tag != "Player" && other.gameObject.tag != "Follower" &&
			other.gameObject.tag != "OuterRange" && other.gameObject.tag != "Hurt") {
			if (counter > countLimit)
				Destroy (gameObject);
		}
	}
}
