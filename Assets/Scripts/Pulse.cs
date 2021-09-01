using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {

	private SpriteRenderer Object;

	private bool fadeOut = true;
	private float fadeSpeed = .006f;

	void Start(){
		Object = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (fadeOut) {
			Object.color = new Vector4 (1, 1, 1, Object.color.a - (fadeSpeed*Time.deltaTime*60));
			if (Object.color.a <= 0.005f)
				fadeOut = false;
		} else {
			Object.color = new Vector4 (1,1,1,Object.color.a + (fadeSpeed * Time.deltaTime * 60));
			if (Object.color.a >= .995f)
				fadeOut = true;
		}
	}
}
