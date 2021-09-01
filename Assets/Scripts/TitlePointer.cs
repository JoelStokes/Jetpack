using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitlePointer : MonoBehaviour {

	private int cursorPos = 1;
	private Vector3 StartPos;
	private bool help = false;

	public AudioClip CursorMove;
	public GameObject[] ObjectsToHide;
	public GameObject[] ObjectsToShow;

	public GameObject TopBlack;
	public GameObject BottomBlack;
	public GameObject LoadText;
	public GameObject MainCamera;

	private bool loading = false;

	private float moveSpeed = .009f;
	private float moveAdd = .0035f;
	private bool moveDone = false;

	// Use this for initialization
	void Start () {
		StartPos = transform.position;

		for (int i = 0; i < ObjectsToShow.GetLength (0); i++) {
			ObjectsToShow [i].SetActive (false);
		}

		LoadText.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (!loading) {
			if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Return)) {
				if (!help) {
					if (cursorPos < 3) {
						loading = true;
					} else {
						help = true;
						for (int i = 0; i < ObjectsToHide.GetLength (0); i++) {
							ObjectsToHide [i].SetActive (false);
						}

						for (int i = 0; i < ObjectsToShow.GetLength (0); i++) {
							ObjectsToShow [i].SetActive (true);
						}
					}
				} else {
					if (Input.GetKeyDown (KeyCode.Space) || Input.GetKeyDown (KeyCode.Return)) {
						help = false;
						for (int i = 0; i < ObjectsToHide.GetLength (0); i++) {
							ObjectsToHide [i].SetActive (true);
						}

						for (int i = 0; i < ObjectsToShow.GetLength (0); i++) {
							ObjectsToShow [i].SetActive (false);
						}
					}
				}
			}

			if (!help) {
				if (Input.GetKeyDown (KeyCode.UpArrow) || Input.GetKeyDown (KeyCode.W)) {
					if (cursorPos > 1) {
						cursorPos--;
						GetComponent<AudioSource> ().PlayOneShot (CursorMove, .5f);
					}
				} else if (Input.GetKeyDown (KeyCode.DownArrow) || Input.GetKeyDown (KeyCode.S)) {
					if (cursorPos < 3) {
						cursorPos++;
						GetComponent<AudioSource> ().PlayOneShot (CursorMove, .5f);
					}
				}
			}

			if (cursorPos == 1)
				transform.position = StartPos;
			else if (cursorPos == 2)
				transform.position = new Vector3 (StartPos.x, StartPos.y - 1, StartPos.z);
			else if (cursorPos == 3)
				transform.position = new Vector3 (StartPos.x, StartPos.y - 2, StartPos.z);
		} else {
			if (TopBlack.transform.position.y > 4) {
				moveSpeed += moveAdd;
				TopBlack.transform.position = new Vector3 (TopBlack.transform.position.x, TopBlack.transform.position.y - moveSpeed, TopBlack.transform.position.z);
				BottomBlack.transform.position = new Vector3 (BottomBlack.transform.position.x, BottomBlack.transform.position.y + moveSpeed, BottomBlack.transform.position.z);
			
				MainCamera.GetComponent<AudioSource> ().volume -= .05f;	//Fade out music
			} else {
				LoadText.SetActive (true);

				if (cursorPos == 1)
					SceneManager.LoadScene ("Moon");
				else if (cursorPos == 2)
					SceneManager.LoadScene ("Volcano");
			}
		}
	}
}
