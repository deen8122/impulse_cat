using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InitLevel : MonoBehaviour {

	public string levelname="test";
	public Sprite passedSprite;
	public GameObject text;
	// Use this for initialization
	void Start () {
	
		int level =PlayerPrefs.GetInt(levelname);
		print (level);
		if( level==0 ){
			Text t = this.GetComponentInChildren<Text> ();
			if(t!=null)t.text="";
			this.GetComponent<Image>().sprite = passedSprite;
			this.GetComponent<Button>().interactable = false;
		}
		else {
			Text text2 = this.GetComponentInChildren<Text> ();
			if (text2 != null) {
				text2.text = levelname;
			}
		}

	}
	public void StartGame(){

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
