using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuelbar : MonoBehaviour {

	public GameObject Bar1;
	public GameObject Bar2;
	public GameObject Bar3;

	private Vector3 bar1FullScale;
	private Vector3 bar2FullScale;
	private Vector3 bar3FullScale;
	private float bar1FullX;
	private float bar2FullX;
	private float bar3FullX;

	private int third = 33;

	private PlayerController playerInfo;

	// Use this for initialization
	void Start () {
		playerInfo = GameObject.Find ("Player").GetComponent<PlayerController> ();

		bar1FullScale = Bar1.transform.localScale;
		bar2FullScale = Bar2.transform.localScale;
		bar3FullScale = Bar3.transform.localScale;

		bar1FullX = Bar1.transform.position.x;
		bar2FullX = Bar2.transform.position.x;
		bar3FullX = Bar3.transform.position.x;

		transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.095f, 0.06f, 3));
	}

	// Update is called once per frame
	void Update () {
		if (playerInfo.fuel >= 90 - third) {	//90 to 57
			
			int tempFuel = 90 - playerInfo.fuel;
			float ratio = bar1FullScale.x / third;
			Bar1.transform.localScale = new Vector3 (ratio * tempFuel, Bar1.transform.localScale.y, 1);

			Bar2.transform.localScale = new Vector3 (0, Bar2.transform.localScale.y, 1);
			Bar3.transform.localScale = new Vector3 (0, Bar3.transform.localScale.y, 1);

		} else if (playerInfo.fuel >= 90 - (third*2)) { //56 - 24
			Bar1.transform.localScale = bar1FullScale;
			//Bar1.transform.position = new Vector3 (bar1FullX, Bar1.transform.position.y, Bar1.transform.position.z);

			int tempFuel = 56 - playerInfo.fuel;
			float ratio = bar2FullScale.x / third;
			Bar2.transform.localScale = new Vector3 (ratio * tempFuel, Bar2.transform.localScale.y, 1);

			Bar3.transform.localScale = new Vector3 (0, Bar3.transform.localScale.y, 1);

		} else {	//23 to 0
			Bar1.transform.localScale = bar1FullScale;
			//Bar1.transform.position = new Vector3 (bar1FullX, Bar1.transform.position.y, Bar1.transform.position.z);
			Bar2.transform.localScale = bar2FullScale;
			//Bar2.transform.position = new Vector3 (bar2FullX, Bar2.transform.position.y, Bar2.transform.position.z);

			int tempFuel = 23 - playerInfo.fuel;
			float ratio = bar3FullScale.x / 23;
			Bar3.transform.localScale = new Vector3 (ratio * tempFuel, Bar3.transform.localScale.y, 1);
		}
	}
}
