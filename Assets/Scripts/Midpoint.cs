using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Midpoint : MonoBehaviour {

	public GameObject MidGem;
	private bool activated = false;
	private float addAmount = .01f;
	private float scaleCounter = .03f;

	// Use this for initialization
	void Start () {
		MidGem.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (activated && scaleCounter < .3f) {
			MidGem.transform.localScale = new Vector3 (MidGem.transform.localScale.x, scaleCounter, MidGem.transform.localScale.z);
			scaleCounter += addAmount;
		}
	}

	public void Activate(){
		activated = true;
		MidGem.SetActive (true);
		GetComponent<AudioSource> ().Play ();
		GetComponent<BoxCollider2D> ().enabled = false;	//To avoid double midpoint checks
	}
}
