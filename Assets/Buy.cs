using UnityEngine;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class Buy : MonoBehaviour {

	int helthPrice = 10;
	int userMoney = 0;
	private Text mone_total;
	private Text user_life;
	private int lifeN = 0;
	private int Life = 0;
	private GameObject btnOk = null;
	// Use this for initialization
	void Start () {
		btnOk= GameObject.Find ("ok");
		btnOk.GetComponent<Button>().interactable = false;
		GameObject mt = GameObject.Find ("mone_total");
		if (mt != null) {

			mone_total = mt.GetComponent<Text> ();
			userMoney = PlayerPrefs.GetInt ("user_money", 10);
			//print ("money NULL NOT money="+money);
			mone_total.text =userMoney.ToString() ;
		} 
		lifeN = PlayerPrefs.GetInt("lifeN",1);
		Life  = PlayerPrefs.GetInt("life",100);
		GameObject ht = GameObject.Find ("user_life");
		if(ht !=null){
			user_life = ht.GetComponent<Text>();
			user_life.text =lifeN+":"+ Life;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void MainScene(){
		Application.LoadLevel("StartScene");
	}
	/*
	 * Покупка жизни
	 */
	public void BtnPlusHelth(){
		//userMoney = PlayerPrefs.GetInt ("user_money", 10);
		if ( (userMoney-helthPrice) < 0) {
			return;
		} else {
			lifeN++;
			userMoney -= helthPrice;
			this.UpdateItems ();
		}

	}
	public void ReInitial(){

		userMoney = PlayerPrefs.GetInt ("user_money", 10);

	}

	public void BtnConfirmBuy(){
		this.SaveItems ();
	}

	public void UpdateItems(){
		btnOk.GetComponent<Button>().interactable = true;
		user_life.text =lifeN+":"+ Life;
		mone_total.text =userMoney.ToString() ;
	}
	public void SaveItems(){
		print ("SaveItems..."+userMoney);
		PlayerPrefs.SetInt("lifeN",lifeN);
		PlayerPrefs.SetInt("life",Life);
		PlayerPrefs.SetInt ("user_money", userMoney);
		print ("SaveItems Updates...");
	}
}
