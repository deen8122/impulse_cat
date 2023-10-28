using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour {
	//public int test = 0;
	public static GameState CurrentGameState = GameState.Playing;

	[HideInInspector]
	public bool isEnemy = false;
	Vector2 pointTo;
	private GameObject[] enemyGO;
	private GameObject MainCamera;
	private float SpeedMovingCamera = 2;
	private bool CameraToLeft = true;
	public string nextleve = "test";
	public static bool f_bonusPricel = true;
	public int count_bonusPricel =0;
	[HideInInspector]
	public int ENEMY_CAT_COL = 1;
	[HideInInspector]
	public int PLAYER_CAT_COL = 1;
	[HideInInspector]
	public int PLAYER_SHOOT =1;
	[HideInInspector]
	public int ENEMY_SHOOT = 1;
	[HideInInspector]
	private int CURRENT_SHOTER = 0;
	private GameObject Sel = null;
	public LineRenderer TrajectoryLineRenderer;
	private int DiedCatsCol = 0;
	private int DiedCatsCol2 = 0;
	[HideInInspector]
	public bool SCLock = false;
	[HideInInspector]
	public float playerCatPositionX;
	private Text moneytext;
	private Text enemytext;
	private int colEnemy = 0;
	private int money = 0;
	/*
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 */


	//------ SUPERCAT----------------
	private GameObject superCat;
	private supercat msupercat;
	/*
	 * 
	 * 
	 * 
	 * 
	 * 
	 * 
	 */
	void Start () {
		MainCamera = GameObject.FindGameObjectWithTag ("MainCamera");
	//	GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMove>().enabled = true;
		Time.timeScale = 1;
		superCat = GameObject.FindGameObjectWithTag ("supercat");
		msupercat = superCat.GetComponent<supercat>();
		GameManager.CurrentGameState = GameState.BOMB_FLYING;
		GameObject mt = GameObject.Find ("moneytext");
		if (mt != null) {
			
			moneytext = mt.GetComponent<Text> ();
			money = PlayerPrefs.GetInt ("user_money", 10);
			//print ("money NULL NOT money="+money);
			moneytext.text =money.ToString() ;
		} 
		//подсчет врагов
		GameObject[] enemy;
		enemy = GameObject.FindGameObjectsWithTag("enemy_cats");
	    mt = GameObject.Find ("enemytext");
		if (mt != null) {

			enemytext = mt.GetComponent<Text> ();
			 colEnemy = enemy.Length;
			print (colEnemy);
			enemytext.text =colEnemy.ToString() ;
		} 
	}



	public void CatFireEvent(){
		if (!isEnemy){
		    minusBinokl();
		}
	}
	//---------------------------------------
	public void setBinoklActive(bool f){
		f_bonusPricel = f;
	}
	/*
	 * добавлнеи жизни	
	 */
	public void plusLife(){
		
	}
	/*
	 * добавлнеи жизни	
	 */
	public void plusMoney(){
		money++;
		moneytext.text =money.ToString() ;
	}

	public void plusBinokl(int col){
		
	}
	public void minusBinokl(){
		
	}
	public bool OnMouseDown(Vector3 mousePos){
		if(GameManager.CurrentGameState == GameState.PressedObj){
			print ("Обьект выбран, нет необходимтости искать...");
			return false;
		}
		float R = 1;
		float distance = 0;
		GameObject[] playeryGO = GameObject.FindGameObjectsWithTag("player_cats");

		for (int i = 0; i < playeryGO.Length; i++) {
			if(playeryGO[i].gameObject == null) continue;
			if(playeryGO[i].gameObject.GetComponent<Fire>().isFired == false){

				distance = Vector2.Distance(mousePos, playeryGO[i].gameObject.transform.position);
//				print ("mousePos="+mousePos.ToString()+" playeryGO="+playeryGO[i].gameObject.transform.position.ToString()+ " distance="+distance);
				if(distance < R){
					R = distance;
					Sel = playeryGO[i];
				}
			}
		
		}
		if(Sel !=null) {

			Sel.GetComponent<Fire>().TouchDown();
			return true;
		}else return false;
	}
	public void onMouseUp(){
		if(Sel!=null){
		Sel.GetComponent<Fire>().TouchUp();
	    Sel = null;
		}
	}

	public void EnemyStartShoot(){

	}
	
	private void EnemyShoot(){


	}





	private Vector3 selected_cat_pos = new Vector3(0,0,0);
	private void  PlayerShoot(){
		DiedCatsCol=0;
		
		print ("PlayerShoot ----PLAYER_CAT_COL="+PLAYER_SHOOT);
		GameObject[] playeryGO = GameObject.FindGameObjectsWithTag("player_cats");
		if (playeryGO.Length == 0)
			return;
		int i2 = 0;
		for (int i = 0; i < playeryGO.Length; i++) {
			if(playeryGO[i].gameObject == null) continue;
			if(playeryGO[i].gameObject.GetComponent<Fire>().isFired == false){
				i2=1;
				playeryGO[i].GetComponent<cat2>().isLock=true;
				//SpecialEffectsHelper.Instance.ArrowShow(playeryGO[i].transform.position);
				//StartCoroutine (ArrowShow( playeryGO[i].transform.position ));
				selected_cat_pos = playeryGO[i].transform.position;
				playerCatPositionX = playeryGO[i].transform.position.x;
				if(GameManager.CurrentGameState != GameState.PressedObj)
				StartCoroutine (AnimateCameraToStartPosition(playeryGO[i],1));

				break;
			}


		}
		if(i2 == 0){
			//BombStoped();
			
		}


	}

	public void CatDie(Vector3 diePos){
		if ( true){
			DiedCatsCol++;
			if(DiedCatsCol>=2){
				DiedCatsCol = 0;
				this.GetComponent<BonusGenerator>().CreateBonusBinokl(diePos);
				int rand =  Random.Range(1,3);
				if(rand==1){
					this.GetComponent<BonusGenerator>().CreateBonusLife(diePos);

				}


			}
			DiedCatsCol2++;
			if(DiedCatsCol2 > 5){
				DiedCatsCol2=0;
				this.GetComponent<BonusGenerator>().CreateBonusLife(diePos);
			}


		}
		colEnemy--;
		enemytext.text =colEnemy.ToString() ;
		//		
		print ("CatDie");
		GameObject[] playeryGO;
		playeryGO = GameObject.FindGameObjectsWithTag("enemy_cats");
		if (playeryGO.Length == 0){
			GameManager.CurrentGameState = GameState.Won;
			PlayerWon();
			return;
		}
	}


	// Use this for initialization

	Vector3	position;

	public void CreateObject(GameObject ob){
		print ("CreateObject");
		//position  = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
		GameObject newobj = Instantiate (ob);
		newobj.AddComponent <ClickImage>();
		//newobj.transform.position = position;

	}
	void BuildGameMode(){
		CurrentGameState = GameState.CREATE_OBJ;
		Time.timeScale = 0;
	}
	public void PlayGameMode(){
		PLAYER_CAT_COL=unlockBombs ("player_cats");
		ENEMY_CAT_COL =unlockBombs ("enemy_cats");
		StartGame ();
		//PlayerStartShoot ();
	}

	void StartGame(){	
		GameObject[] enemyGO2 = GameObject.FindGameObjectsWithTag("draggable_objects");
		for (int i = 0; i < enemyGO2.Length; i++) {
			if(enemyGO2[i].gameObject == null) continue;
			Destroy(enemyGO2[i].gameObject.GetComponent<Dragabble>());

		}
		 enemyGO2 = GameObject.FindGameObjectsWithTag("player_cats");
		for (int i = 0; i < enemyGO2.Length; i++) {
			if(enemyGO2[i].gameObject == null) continue;
			Destroy(enemyGO2[i].gameObject.GetComponent<Dragabble>());
			
		}
	}




	void EventStopAnimToSartPos(){
	//	if(selected_cat_pos!=null)
	//	StartCoroutine (ArrowShow( selected_cat_pos ));

	}
	// Update is called once per frame
	void Update () {
		switch (CurrentGameState)
		{
			//case GameState.CameraMoveToPlayer:
			//	break;
			
		case GameState.CAMERA_MOVING:


			MainCamera.transform.position=
				new Vector3(MainCamera.transform.position.x+SpeedMovingCamera ,MainCamera.transform.position.y,MainCamera.transform.position.z) ;
			if(CameraToLeft == true && ( (MainCamera.transform.position.x ) < pointTo.x )){
				CurrentGameState = GameState.Playing;
			//	EventStopAnimToSartPos();
			}
			if(CameraToLeft == false && ( (MainCamera.transform.position.x) > pointTo.x )){
				CurrentGameState = GameState.Playing;
				//EventStopAnimToSartPos();
			}
			break;

		}
	}




	private IEnumerator AnimateCameraToStartPosition(GameObject cat , float waitsecond)
	{

		yield return new WaitForSeconds(waitsecond);
		float duration = Vector2.Distance(MainCamera.transform.position, cat.transform.position) / 10f;
		if (duration == 0.0f) duration = 0.1f;
		pointTo = cat.transform.position;
//		print ("AnimateCameraToStartPosition pointTo="+pointTo.ToString()+" MainCamera="+MainCamera.transform.position.ToString());
		int direction = 1;
		if (pointTo.x < MainCamera.transform.position.x) {
			CameraToLeft = true;
			if(isEnemy == false ){
				pointTo.x+=3;
			}else {
				pointTo.x-=3;	
			}
			//pointTo.x+=3;
			SpeedMovingCamera = -duration;
			direction = -1;
		} else {
			CameraToLeft = false;
			if(isEnemy == false ){
				pointTo.x+=3;
			}else {
				pointTo.x-=3;	
			}
			direction = 1;
			SpeedMovingCamera = duration;
		}

		CurrentGameState = GameState.CAMERA_MOVING;



	}
	private IEnumerator CameraZoom( float waitsecond)
	{
		yield return new WaitForSeconds(waitsecond);

		CurrentGameState = GameState.CAMERA_MOVING;
		
		
	}


	//
	private int unlockBombs(string tag){
		return 0;
	}





	public void NextGameStart(){

	}

	public void PlayerWon(){
		msupercat.EndGame ();
        GameGUI	mGameGUI = GameObject.FindGameObjectWithTag ("GameCanvas").GetComponent<GameGUI>();
		mGameGUI.PlayerWon ();
		PlayerPrefs.SetInt("bonus_binokl_col",count_bonusPricel);
		PlayerPrefs.SetInt(nextleve,1);
		PlayerPrefs.SetInt("user_money",money);


	}
	public void PlayerLost(){
		SCLock = true;
		//print ("PlayerLost");
		GameGUI	mGameGUI = GameObject.FindGameObjectWithTag ("GameCanvas").GetComponent<GameGUI>();
		mGameGUI.PlayerLost ();
		//wonDialog.SetActive(true);
		//lostDialog.SetActive (true);
	}



	

	//-----------------------------------------------


}
