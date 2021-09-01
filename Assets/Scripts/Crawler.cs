using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crawler : MonoBehaviour
{
    public int waitTime = 0;
	public float distance = 0;
	public float speed = 0;
	public bool horizontal = false;

    private int animationCounter = 0;
    private Animator anim;
	private GameObject parentObj;

    void Start()
    {
        anim = GetComponent<Animator>();
		parentObj = transform.parent.gameObject;
		anim.speed = speed;
    }

	void Update(){
		if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Crawler")){
			if (animationCounter == 0){
				if (horizontal)
					parentObj.transform.position = new Vector3(parentObj.transform.position.x+distance, parentObj.transform.position.y, parentObj.transform.position.z);
				else
					parentObj.transform.position = new Vector3(parentObj.transform.position.x, parentObj.transform.position.y+distance, parentObj.transform.position.z);
			}
			animationCounter++;

			if (animationCounter > waitTime){
				//Raycast checks
				Vector2 forwardPos = new Vector2(transform.position.x+(distance*2.5f), transform.position.y);
				Vector2 lowerPos = new Vector2(forwardPos.x, forwardPos.y+(distance*2.25f*parentObj.transform.localScale.x));
				RaycastHit2D hit = Physics2D.Raycast(forwardPos, -transform.right*parentObj.transform.localScale.x, .75f);

				if (hit.collider != null && hit.collider.gameObject.tag != "Player" && hit.collider.gameObject != gameObject
					&& hit.collider.gameObject != transform.GetChild(0).gameObject && hit.collider.gameObject != transform.GetChild(1).gameObject)
					Flip();
				if (!Physics2D.Raycast(lowerPos, -transform.right*parentObj.transform.localScale.x, .25f))	//No ground ahead
					Flip();
				if (hit.collider != null)
					Debug.Log("Name:" + hit.collider.gameObject.name);

				//Replay animation
				animationCounter = 0;
				anim.Play("Crawler");
			}
		}
	}

	void Flip(){
		parentObj.transform.localScale = new Vector3(-parentObj.transform.localScale.x, parentObj.transform.localScale.y, parentObj.transform.localScale.z);
		distance = -distance;
	}

	void OnDrawGizmosSelected(){
		GameObject parentObj = transform.parent.gameObject;
		Gizmos.color = Color.green;
		Vector2 forwardPos = new Vector2(transform.position.x+distance*2.5f, transform.position.y);
		Gizmos.DrawRay(forwardPos, -transform.right*parentObj.transform.localScale.x);

		Gizmos.color = Color.red;
		Vector2 lowerPos = new Vector2(forwardPos.x, forwardPos.y+(distance*2.25f*parentObj.transform.localScale.x));
		Gizmos.DrawRay(lowerPos, -transform.right*parentObj.transform.localScale.x);
	}
}
