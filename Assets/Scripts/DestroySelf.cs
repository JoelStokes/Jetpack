using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

	public int countLimit = 20;
	private int counter = 0;
	
	// Update is called once per frame
	void Update () {
		counter++;
		if (counter >= countLimit - 9) {
			SpriteRenderer colorChanger = GetComponent<SpriteRenderer> ();
			colorChanger.color = new Vector4 (1,1,1,colorChanger.color.a-.1f);
		}

		if (counter >= countLimit)
			Destroy (gameObject);
	}
}
