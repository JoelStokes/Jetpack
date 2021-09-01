using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

	public bool switchState = false;
	public bool playSound = false;
	public int multiSwitchNumber = 0;

	public Sprite On;
	public Sprite Off;
	public AudioClip OnSFX;
	public AudioClip OffSFX;

	private int counter = 0;
	private int countLim = 30;
	public bool ready = true;

	// Use this for initialization
	void Start () {
		if (switchState)
			GetComponent<SpriteRenderer> ().sprite = On;
		else
			GetComponent<SpriteRenderer> ().sprite = Off;
	}

	void Update(){
		if (!ready) {
			counter++;
			if (countLim <= counter) {
				counter = 0;
				ready = true;
			}
		}
	}

	public void Activate(){
		switchState = !switchState;
		if (switchState) {
			GetComponent<SpriteRenderer> ().sprite = On;
			if (playSound)
				GetComponent<AudioSource> ().PlayOneShot (OnSFX, 1f);
		} else {
			GetComponent<SpriteRenderer> ().sprite = Off;
			if (playSound)
				GetComponent<AudioSource> ().PlayOneShot (OffSFX, 1f);
		}
		ready = false;
	}
}
