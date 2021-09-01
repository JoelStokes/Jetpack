using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationAutoDestroy : MonoBehaviour {

	public float delay = -.05f;

	void Start(){
		float count = this.GetComponent<Animator> ().GetCurrentAnimatorStateInfo (0).length + delay;
		Destroy (gameObject, count);
		Destroy (transform.parent.gameObject, count);
	}
}
