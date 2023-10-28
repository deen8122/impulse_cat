using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorFinish : MonoBehaviour {

	// Use this for initialization
	public bool isOpened = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter2D(Collider2D col){

		if(col.gameObject.tag == "supercat" ){
			print ("DOOR FINISH");
			GameObject lighting = GameObject.Find ("GameManager");

			GameObject supercat = GameObject.Find ("supercat");
			if(supercat!=null)
			supercat.SetActive (false);
			lighting.GetComponent<GameManager> ().PlayerWon ();
		}

	}
	void OnCollisionEnter2D(Collision2D col)
	{
		print ("DOOR FINISH  2");
		if(col.gameObject.tag == "supercat" ){
			print ("DOOR FINISH");
			GameObject lighting = GameObject.Find ("GameManager");

			GameObject supercat = GameObject.Find ("supercat");
			if (supercat != null) {
				supercat.SetActive (false);
				supercat.GetComponent<supercat> ().TouchUp ();
			}
			lighting.GetComponent<GameManager> ().PlayerWon ();
		}

	}
}
