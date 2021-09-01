using UnityEngine;
using System.Collections;

public class Disintegrating : MonoBehaviour {

	private bool breaking = false;
	private int counter = 0;
	private int countLimit = 5;
	private AudioSource audioSource;
	private Rigidbody2D rigi;
	private float startY;

	public float breakTime = 1;	//Set individually per block in Unity

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		rigi = GetComponent<Rigidbody2D> ();
		startY = transform.position.y;
	}
	
	// Update is called once per frame
	void Update () {
		if (breaking) {
			counter++;
			if (counter>countLimit)
				rigi.gravityScale = .05f;
		}

		Physics2D.IgnoreLayerCollision (0, 11, true);	//Passable blocks

		if (transform.position.y < startY - 50)
			Destroy (gameObject);
	}

	void OnCollisionEnter2D(Collision2D other) {	//Sets the block to begin breaking
		if (other.gameObject.tag == "Player") {
			breaking = true;
			audioSource.Play ();
		}
	}
}
