using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquidCrusher : MonoBehaviour {

	public GameObject[] Switches;	//used to get switch state
	private Switch[] switchScript = new Switch[4];

	public GameObject LeftEjector;
	public GameObject RightEjector;

	public float stopPoint;
	public float ejectorStopPoint;

	private float startPoint;
	private float ejectorStartPoint;
	private float downSpeed = .7f;
	private float upSpeed = .08f;

	private bool activating = false;
	private bool movingDown = false;
	private bool ejectorDown = false;

	void Start(){
		for (int i = 0; i < 4; i++) {
			switchScript [i] = Switches [i].GetComponent<Switch> ();
		}

		startPoint = transform.position.y;
		ejectorStartPoint = LeftEjector.transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (switchScript [0].switchState && switchScript [1].switchState && switchScript [2].switchState
		    && switchScript [3].switchState && !activating) {
			activating = true;
			movingDown = true;
			ejectorDown = true;
			GetComponent<AudioSource> ().Play ();
		} else if (activating) {
			if (movingDown) {
				if (transform.position.y > stopPoint)
					transform.position = new Vector3 (transform.position.x, transform.position.y - downSpeed, transform.position.z);
				else {
					movingDown = false;
				}
			} else {
				if (transform.position.y < startPoint) {
					transform.position = new Vector3 (transform.position.x, transform.position.y + upSpeed, transform.position.z);
					LeftEjector.transform.position = new Vector3 (LeftEjector.transform.position.x,
						LeftEjector.transform.position.y + (upSpeed / 10), LeftEjector.transform.position.z);
					RightEjector.transform.position = new Vector3 (RightEjector.transform.position.x,
						RightEjector.transform.position.y + (upSpeed / 10), RightEjector.transform.position.z);
				} else {
					activating = false;
				}
			}
		}

		if (ejectorDown) {
			if (LeftEjector.transform.position.y > ejectorStopPoint) {
				LeftEjector.transform.position = new Vector3 (LeftEjector.transform.position.x,
					LeftEjector.transform.position.y - (upSpeed / 1.5f), LeftEjector.transform.position.z);
				RightEjector.transform.position = new Vector3 (RightEjector.transform.position.x,
					RightEjector.transform.position.y - (upSpeed / 1.5f), RightEjector.transform.position.z);
			} else {
				ejectorDown = false;
				GameObject[] Switches = GameObject.FindGameObjectsWithTag ("Switch");
				for (int i = 0; i < Switches.Length; i++) {
					if (Switches [i].GetComponent<Switch> () != null) {
						Switches [i].GetComponent<Switch> ().Activate ();
					} else {
						Switches [i].GetComponent<SwitchImpact> ().Activate ();
					}
				}
			}
		} else if (LeftEjector.transform.position.y < ejectorStartPoint) {
			LeftEjector.transform.position = new Vector3 (LeftEjector.transform.position.x,
				LeftEjector.transform.position.y + (upSpeed / 1.75f), LeftEjector.transform.position.z);
			RightEjector.transform.position = new Vector3 (RightEjector.transform.position.x,
				RightEjector.transform.position.y + (upSpeed / 1.75f), RightEjector.transform.position.z);
		}
	}
}
