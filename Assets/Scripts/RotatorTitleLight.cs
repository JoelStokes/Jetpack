using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorTitleLight : MonoBehaviour {

	public float speed = 5;
	public bool negative = false;

	private float currentAngle = 0;

	// Update is called once per frame
	void Update () {
		currentAngle += speed;
		if (currentAngle > 360)
			currentAngle -= 360;

		if (!negative)
			transform.Rotate (Vector3.right * Time.deltaTime * speed);
		else
			transform.Rotate (Vector3.right * Time.deltaTime * -speed);
		//transform.localRotation = Quaternion.Euler (transform.localRotation.x,transform.localRotation.y,transform.localRotation.z);
	}
}
