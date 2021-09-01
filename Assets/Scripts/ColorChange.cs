using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChange : MonoBehaviour {

	public Color newColor;

	// Use this for initialization
	void Start () {
		GetComponent<SpriteRenderer> ().color = newColor;
	}
}
