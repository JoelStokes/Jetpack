using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBasic : MonoBehaviour {

	public float fadeSpeed = 0;
	
	// Update is called once per frame
	void Update () {
		GetComponent<SpriteRenderer> ().color = new Vector4 (1,1,1,GetComponent<SpriteRenderer> ().color.a - fadeSpeed);
		if (GetComponent<SpriteRenderer> ().color.a <= .0005f)
			Destroy (gameObject);
	}
}
