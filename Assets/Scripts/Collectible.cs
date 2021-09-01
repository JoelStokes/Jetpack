using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	public AudioClip pickupSFX;
	public GameObject collectedParticles;

	public void Collected(){
		GameObject particleObject = Instantiate (collectedParticles, new Vector3 (transform.position.x, 
			transform.position.y, collectedParticles.transform.position.z), Quaternion.identity) as GameObject;
		particleObject.GetComponent<ParticleSystem> ().Play();
		Destroy (gameObject);
	}
}
