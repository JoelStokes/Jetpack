using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour {

	public int countLimit = 20;

	private float timeLimit;
	private float timeVariation = 9;
	private float timer = 0;

	private void Start()
    {
		timeLimit = (countLimit - timeVariation) / 60;
    }

    void Update () {
		timer += Time.deltaTime;
		if (timer >= timeLimit) {
			SpriteRenderer colorChanger = GetComponent<SpriteRenderer> ();
			colorChanger.color = new Vector4 (1,1,1,colorChanger.color.a-(Time.deltaTime*timeVariation));
		}

		if (timer >= timeLimit + (timeVariation/60))
			Destroy (gameObject);
	}
}
