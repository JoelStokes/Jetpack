using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricGate : MonoBehaviour {

	public int onFrames = 60;
	public int offFrames = 100;
	public bool isOn = false;	//Public for player access

	private int counter = 0;

	// Update is called once per frame
	void Update () {
		counter++;

		if (isOn && counter > onFrames) {
			counter = 0;
			isOn = false;
		} else if (!isOn && counter > offFrames) {
			counter = 0;
			isOn = true;
		}
	}
}
