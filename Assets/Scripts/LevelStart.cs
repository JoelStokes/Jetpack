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

	private float moveSpeed = 0.0001f;
	private float moveAdd = .0015f;
	private bool moveDone = false;

	private float timer = 0;
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
					TopBlack.transform.position = new Vector3 (TopBlack.transform.position.x, TopBlack.transform.position.y + (moveSpeed * Time.deltaTime * 60), TopBlack.transform.position.z);
					BottomBlack.transform.position = new Vector3 (BottomBlack.transform.position.x, BottomBlack.transform.position.y - (moveSpeed * Time.deltaTime * 60), BottomBlack.transform.position.z);
				} else {
					Destroy (TopBlack);
					Destroy (BottomBlack);
					moveDone = true;
				}
			} else {    //Names Scrolling In
				timer += Time.deltaTime;

				if (!topTextDone) {
					if (timer > 0.05f) {
						if (TopName.GetComponent<TextMesh> ().text.Length < topText.Length) {
							if (!char.IsWhiteSpace (topText [TopName.GetComponent<TextMesh> ().text.Length])) {
								TopName.GetComponent<AudioSource>().PlayOneShot (TextSFX, 1f);
							}

							TopName.GetComponent<TextMesh> ().text = TopName.GetComponent<TextMesh> ().text + (topText [TopName.GetComponent<TextMesh> ().text.Length]);

							timer = 0;
						} else {
							topTextDone = true;
							timer = -0.05f;
						}
					} 

				} else if (!bottomTextDone) {
					if (timer > 0.05f) {
						if (BottomName.GetComponent<TextMesh> ().text.Length < bottomText.Length) {
							if (!char.IsWhiteSpace (bottomText [BottomName.GetComponent<TextMesh> ().text.Length])) {
								TopName.GetComponent<AudioSource>().PlayOneShot (TextSFX, 1f);
							}

							BottomName.GetComponent<TextMesh> ().text = BottomName.GetComponent<TextMesh> ().text + (bottomText [BottomName.GetComponent<TextMesh> ().text.Length]);
						} else
							bottomTextDone = true;
						timer = 0;
					}

				} else {	//Fade when both done
					if (timer > 0.84f) {
						if (TopName.GetComponent<Renderer> ().material.color.a > 0) {
							TopName.GetComponent<Renderer> ().material.color = new Vector4 (1, 1, 1, TopName.GetComponent<Renderer> ().material.color.a - (fadeSpeed * Time.deltaTime * 60));
							BottomName.GetComponent<Renderer> ().material.color = new Vector4 (1, 1, 1, BottomName.GetComponent<Renderer> ().material.color.a - (fadeSpeed * Time.deltaTime * 60));
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
