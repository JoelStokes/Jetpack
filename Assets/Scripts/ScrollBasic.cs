using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBasic : MonoBehaviour {

	public float moveValue = 0;
	public bool horizontal = true;
	
	// Update is called once per frame
	void Update () {
		if (horizontal)
			transform.position = new Vector3 (transform.position.x + moveValue, transform.position.y, transform.position.z);
		else
			transform.position = new Vector3 (transform.position.x, transform.position.y + moveValue, transform.position.z);
	}
}
