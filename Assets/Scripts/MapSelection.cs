using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSelection : MonoBehaviour {

	private float moveSpeed = .06f;
	private bool shiftHeld = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.LeftShift) || Input.GetKey (KeyCode.RightShift))
			shiftHeld = true;
		else
			shiftHeld = false;

		if (Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.UpArrow)) {
			if (shiftHeld)
				transform.position = new Vector3 (transform.position.x, transform.position.y + (moveSpeed*2), transform.position.z);
			else
				transform.position = new Vector3 (transform.position.x, transform.position.y + moveSpeed, transform.position.z);
		} else if (Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.DownArrow)) {
			if (shiftHeld)
				transform.position = new Vector3 (transform.position.x, transform.position.y - (moveSpeed*2), transform.position.z);
			else
				transform.position = new Vector3 (transform.position.x, transform.position.y - moveSpeed, transform.position.z);
		}

		if (Input.GetKey (KeyCode.D) || Input.GetKey (KeyCode.RightArrow)) {
			if (shiftHeld)
				transform.position = new Vector3 (transform.position.x + (moveSpeed*2), transform.position.y, transform.position.z);
			else
				transform.position = new Vector3 (transform.position.x + moveSpeed, transform.position.y, transform.position.z);
		} else if (Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.LeftArrow)) {
			if (shiftHeld)
				transform.position = new Vector3 (transform.position.x - (moveSpeed*2), transform.position.y, transform.position.z);
			else
				transform.position = new Vector3 (transform.position.x - moveSpeed, transform.position.y, transform.position.z);
		}
	}

	void OnTriggerStay2D(Collider2D other){		//Need TriggerStay in instances when inside radius after death
		if (other.gameObject.tag == "Mappoint") {	//Walked into sensor range of Follower
			if (Input.GetKeyDown (KeyCode.Return)) {
				SceneManager.LoadScene (other.GetComponent<MapPoint> ().sceneName);
			}
		}
	}
}
