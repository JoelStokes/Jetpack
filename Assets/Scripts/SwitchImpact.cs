using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchImpact : MonoBehaviour {

	public bool switchState = false;

	public bool spriteChange = false;
	public Sprite On;
	public Sprite Off;
	private bool move = false;
	public bool moveVertical = true;
	public float moveDistance;

	public int multiSwitchNumber;

	private int moveCounter = 0;
	private int moveLim = 25;

	// Use this for initialization
	void Start () {
		if (switchState) {
			if (spriteChange)
				GetComponent<SpriteRenderer> ().sprite = On;
		} else {
			if (spriteChange)
				GetComponent<SpriteRenderer> ().sprite = Off;
		}
	}

	void Update(){
		if (move) {
			moveCounter++;
			if (moveCounter < moveLim) {
				if (moveVertical)
					transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y + (moveDistance / moveLim), transform.localPosition.z);
				else
					transform.localPosition = new Vector3 (transform.localPosition.x + (moveDistance / moveLim), transform.localPosition.y, transform.localPosition.z);
			}
			else {
				move = false;
				moveCounter = 0;
			}
		}
	}

	public void Activate(){
		switchState = !switchState;
		if (switchState) {
			if (spriteChange) {	//Used for Squid boss
				GetComponent<SpriteRenderer> ().enabled = true;
				if (GetComponent<BoxCollider2D>() != null)
					GetComponent<BoxCollider2D> ().enabled = true;
				if (On != null)
					GetComponent<SpriteRenderer> ().sprite = On;
				else {
					GetComponent<SpriteRenderer> ().enabled = false;
					GetComponent<BoxCollider2D> ().enabled = false;
				}
			}
		} else {
			GetComponent<SpriteRenderer> ().enabled = true;
			if (GetComponent<BoxCollider2D>() != null)
				GetComponent<BoxCollider2D> ().enabled = true;
			if (spriteChange)
				GetComponent<SpriteRenderer> ().sprite = Off;
		}

		moveDistance = -moveDistance;
		move = true;
	}
}
