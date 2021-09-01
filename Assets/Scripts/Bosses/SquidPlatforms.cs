using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidPlatforms : MonoBehaviour {

	public float spawnX;
	public float dieX;
	public float speed;

	// Update is called once per frame
	void Update () {
		if (transform.position.x < dieX) {
			transform.position = new Vector3 (spawnX, transform.position.y, transform.position.z);
		} else
			transform.position = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);
	}
}
