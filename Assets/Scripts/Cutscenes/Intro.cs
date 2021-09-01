using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour {

	private int counter = 0;
	private int insideShip = 590;
	private int shipCrashing = 1350;

	public GameObject LoadSquare;

	public Sprite[] Lightning;

	public AudioClip CrashingSFX;

	void Start(){
		LoadSquare.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		counter++;
		if (counter == insideShip)
			transform.position = new Vector3 (transform.position.x, transform.position.y - 50, transform.position.z);
		else if (counter == shipCrashing)
			transform.position = new Vector3 (transform.position.x, transform.position.y + 50, transform.position.z);

		if (Input.GetKeyDown (KeyCode.Escape)) {
			LoadSquare.SetActive (true);
			SceneManager.LoadScene ("Moon");
		}
	}
}
