using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidTentacle : MonoBehaviour {

	public float lavaSurface;

	private bool rising = true;

	private float startY;
	private float yChange = 10;
	private float endY;

	private float speed = 1.0F;
	private float startTime;
	private float journeyLength;

	// Use this for initialization
	void Start () {
		startY = transform.position.y;
		endY = yChange + startY;

		startTime = Time.time;
		journeyLength = Vector3.Distance(new Vector3(transform.position.x, startY, transform.position.z), 
			new Vector3(transform.position.x, endY, transform.position.z));
	}
	
	// Update is called once per frame
	void Update () {
		if (rising)
			Rise ();
	}

	void Rise(){
		if (transform.position.y < lavaSurface) { //LERP ISTN WORKING FixedJoint!!!!
			float distCovered = (Time.time - startTime) * speed;
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(new Vector3(transform.position.x, startY, transform.position.z), 
				new Vector3(transform.position.x, endY, transform.position.z), fracJourney);
		}
	}
}
