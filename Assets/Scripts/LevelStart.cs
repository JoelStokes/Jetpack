using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour {

	public GameObject TopBlack;
	public GameObject BottomBlack;
	public GameObject TopName;
	public GameObject BottomName;
	public AudioClip TextSFX;

	public bool useBlackBoxes = true;	//If level will use the top and black bars that move up/down

	public string topText;
	public string bottomText;

	private float moveSpeed = .009f;
	private float moveAdd = .0035f;
	private bool moveDone = false;

	private int counter = 0;
	private int textFadeLim = 120;
	private bool topTextDone = false;
	private bool bottomTextDone = false;
	private float originalY;	//Used to calculate if bars have moved enough

	private string objTopText;
	private string objBottomText;

	private float fadeSpeed = .025f;
	private bool done = false;

	void Start(){
		TopName.GetComponent<TextMesh> ().text = "";
		BottomName.GetComponent<TextMesh> ().text = "";

		topText.ToCharArray();
		bottomText.ToCharArray();

		originalY = TopBlack.transform.position.y;

		if (!useBlackBoxes) {
			Destroy (TopBlack);
			Destroy (BottomBlack);
			moveDone = true;
		}
	}

	// Update is called once per frame
	void Update () {
		if (!done) {
			if (!moveDone) {	//Black Boxes Moving
				moveSpeed += moveAdd;

				if (TopBlack.transform.position.y < 15 + originalY) {
					TopBlack.transform.position = new Vector3 (TopBlack.transform.position.x, TopBlack.transform.position.y + moveSpeed, TopBlack.transform.position.z);
					BottomBlack.transform.position = new Vector3 (BottomBlack.transform.position.x, BottomBlack.transform.position.y - moveSpeed, BottomBlack.transform.position.z);
				} else {
					Destroy (TopBlack);
					Destroy (BottomBlack);
					moveDone = true;
				}
			} else {	//Names Scrolling In
				counter++;

				if (!topTextDone) {
					if (counter > 3) {
						if (TopName.GetComponent<TextMesh> ().text.Length < topText.Length) {
							if (!char.IsWhiteSpace (topText [TopName.GetComponent<TextMesh> ().text.Length])) {
								TopName.GetComponent<AudioSource>().PlayOneShot (TextSFX, 1f);
							}

							TopName.GetComponent<TextMesh> ().text = TopName.GetComponent<TextMesh> ().text + (topText [TopName.GetComponent<TextMesh> ().text.Length]);

							counter = 0;
						} else {
							topTextDone = true;
							counter = -3;
						}
					} 

				} else if (!bottomTextDone) {
					if (counter > 3) {
						if (BottomName.GetComponent<TextMesh> ().text.Length < bottomText.Length) {
							if (!char.IsWhiteSpace (bottomText [BottomName.GetComponent<TextMesh> ().text.Length])) {
								TopName.GetComponent<AudioSource>().PlayOneShot (TextSFX, 1f);
							}

							BottomName.GetComponent<TextMesh> ().text = BottomName.GetComponent<TextMesh> ().text + (bottomText [BottomName.GetComponent<TextMesh> ().text.Length]);
						} else
							bottomTextDone = true;
						counter = 0;
					}

				} else {	//Fade when both done
					if (counter > 50) {
						if (TopName.GetComponent<Renderer> ().material.color.a > 0) {
							TopName.GetComponent<Renderer> ().material.color = new Vector4 (1, 1, 1, TopName.GetComponent<Renderer> ().material.color.a - fadeSpeed);
							BottomName.GetComponent<Renderer> ().material.color = new Vector4 (1, 1, 1, BottomName.GetComponent<Renderer> ().material.color.a - fadeSpeed);
						} else {
							Destroy (TopName);
							Destroy (BottomName);
							done = true;
						}
					}
				}
			}
		}
	}
}
