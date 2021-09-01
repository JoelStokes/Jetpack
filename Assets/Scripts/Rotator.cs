using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

	public float speed = 5;

	private float currentAngle = 0;
	
	// Update is called once per frame
	void Update () {
		currentAngle += speed;
		if (currentAngle > 360)
			currentAngle -= 360;
		
		transform.localRotation = Quaternion.Euler (0,0,currentAngle);
	}
}
