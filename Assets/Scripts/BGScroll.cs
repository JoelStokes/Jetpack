using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroll : MonoBehaviour {

	public GameObject Player;
	public float scrollSpeed;
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3 (Player.transform.position.x/scrollSpeed, 
			Player.transform.position.y/scrollSpeed, transform.position.z);
	}
}
