using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Glow : MonoBehaviour {

	public Color lerpedColor;
	private float speed = 0.8f;
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, lerpedColor, Mathf.PingPong(Time.time * speed, 1.0f));
	}
}
