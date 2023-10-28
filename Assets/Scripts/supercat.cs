using UnityEngine;
using System.Collections;
using Assets.Scripts;
using UnityEngine.UI;
public class supercat : MonoBehaviour {


	Vector3 location;
	Vector3 pos;
	Vector3 posStart;
	Vector3 delta;
	private float distance = 1;
	private float MaxL = 1;

	public GameObject circleSmall = null;
	public int ACTION = 0;
	public float JumpSpeed = 10;
	public int Life = 100;
	private int delayShoot = 0;

	private GameObject pricel = null;
	private GameManager mGameManager = null;
	private Vector3 different;
	private GameObject lighting = null;
	private GameObject lighting2 = null;
	public Sprite dieSprite;
	private Text helthtext;
	private bool flock = false;
	private int lifeN = 1;
	private GameObject FireRotator = null;
	private GameObject CatFace = null;

	private GameObject SelectedDistanceObject = null;
	void Start () {
		CameraFollow.BirdToFollow = transform;
		pricel = GameObject.FindGameObjectWithTag ("sc-pricel");
		pricel.SetActive (false);
		GameObject gm = GameObject.FindGameObjectWithTag ("gm");
		if (gm != null) {
			mGameManager = gm.GetComponent<GameManager>();
		}

		lighting = GameObject.Find ("light");
		lighting.SetActive (false);
		lighting2 = GameObject.Find ("light2");
		lighting2.SetActive (false);
		GameObject ht = GameObject.Find ("helthtext");
		FireRotator= GameObject.Find ("FireRotator");
		CatFace= GameObject.Find ("Cat");
		lifeN = PlayerPrefs.GetInt("lifeN",1);
		Life  = PlayerPrefs.GetInt("life",100);
		if(ht !=null){
			helthtext = ht.GetComponent<Text>();
			helthtext.text =lifeN+":"+ Life;
		}
	}
	public void EndGame(){
		 PlayerPrefs.SetInt("lifeN",lifeN);
		 PlayerPrefs.SetInt("life",Life);
	}


	void Die(){
	//	GetComponent<Animator> ().enabled = false;
	//	this.CatFace.GetComponent<SpriteRenderer>().sprite = dieSprite;
	}
	public void Stope(){
		delayShoot=120;
		flock = true;
		TouchUp();
	}
	
	// Update is called once per frame
	void Update () {

		if (delayShoot > 0) {
			delayShoot--;
			flock = false;

		}
		if (ACTION == 0) {

			if (SelectedDistanceObject != null) {
				Vector3 position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector3 selObjPos = SelectedDistanceObject.transform.position;
				Vector3 delta = position - selObjPos;
				//position.Normalize ();
				print (position);
				print (selObjPos);
				print ("delta => "+delta);
				//Rigidbody2D  body = SelectedDistanceObject.GetComponent<Rigidbody2D> ();
				SelectedDistanceObject.GetComponent<Rigidbody2D> ().WakeUp ();
				SelectedDistanceObject.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (delta.x,delta.y) * 200);
				SelectedDistanceObject = null;
			}
		}
		if(ACTION == 1){
			posStart = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			different = transform.position;
			//posStart = Input.mousePosition;

			ACTION = 2;
		}
		if (ACTION == 2) {

			pos = gameObject.transform.position; 
			//location = Camera.main.ScreenToWorldPoint (Input.mousePosition)+posStart;
			location = Camera.main.ScreenToWorldPoint (Input.mousePosition)-(pos -different);
		//	location = Input.mousePosition;

			//we will let the user pull the bird up to a maximum distance
			distance = Vector3.Distance (location, posStart);
			if (distance > MaxL) {
				//basic vector maths :)
				distance = MaxL;
				delta = (location - posStart).normalized * MaxL;
				//var maxPosition = (location - pos1).normalized * 0.2f;

			} else {
				delta =(location - posStart);
			}

			circleSmall.transform.position = pos+delta ;
			//var dir = pos+circleSmall.transform.position;
		//	var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
			//FireRotator.transform.rotation = Quaternion.AngleAxis(angle, circleSmall.transform.position);

			Vector2 v_diff = (  FireRotator.transform.position - circleSmall.transform.position );    
			float atan2 = Mathf.Atan2 ( v_diff.y, v_diff.x )+1.5f;
			FireRotator.transform.rotation = Quaternion.Euler(0f, 0f, atan2 * Mathf.Rad2Deg );

			//=========================================
			if (SelectedDistanceObject == null) {
				Vector3 position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				Vector2 touchPos = new Vector2 (position.x, position.y);
				RaycastHit2D hit = Physics2D.Raycast (touchPos, Vector2.zero);
				if (hit) { 
					print (hit.transform.gameObject.name);
				
					SelectedDistanceObject = GameObject.Find (hit.transform.gameObject.name);
						if(SelectedDistanceObject.GetComponent<Rigidbody2D> ()==null){
							SelectedDistanceObject = null;
						print ("НЕТ RIGIDBODY");
						}
					//position.z = 0; 
					//tObject.rigidbody2D.transform.position = position;

				}
			}



		}

	}


	void OnCollisionEnter2D(Collision2D col)
	{

		if(col.gameObject.tag == "door"){ print ("CAT DOOR FINISH");;}
		if(col.gameObject.tag == "lift_platform"){ return;}
		if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;	

	

		if(col.gameObject.tag == "tomato" || col.gameObject.tag == "bomb"){
			float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;		
			SpecialEffectsHelper.Instance.CatEffect(transform.position);
			delayShoot=120;
			TouchUp();

		//	col.gameObject. = "player_bomb";
			Life -=(int)damage ;
			if (Life < 0) {
				lifeN--;
				Life = 100;
			}
			helthtext.text =lifeN+":"+ Life;
			if(lifeN<0){
				Die ();
				helthtext.text ="0:0";
				RigidbodyConstraints2D fr = RigidbodyConstraints2D.None;

				this.GetComponent<Rigidbody2D>().constraints = fr;
			//SoundManager.instance.RandomizeSfx(destroySound1);
			mGameManager.PlayerLost();
			}
		}
		if(col.gameObject.tag == "bonus_Life" ){
			Life+=50;
			col.gameObject.GetComponent<BonusBlock>().Done();
			if (Life > 100) {
				Life -= 100;
				lifeN++;
			}
			helthtext.text =lifeN+":"+ Life;
		}

	}


	/*
	 * Вызывается при начале установки прыжка
	 */
	public void TouchDown(Vector3 mousePos){
		//print("TouchDown");
		if (flock)
			return;
		//lighting.Play ("lighting");
		//lighting.SetBool ("run", true);
		//lighting.SetBool ("run", false);
		//posStart = Camera.main.ScreenToWorldPoint (mousePos);
	//	circleSmall.transform.position = gameObject.transform.position;
		lighting2.SetActive (true);

		if (mGameManager.SCLock == true)
			return;
		ACTION = 1;
		pricel.SetActive (true);
		if (delayShoot == 0) {
			Time.timeScale = 0.1F;
			//Time.
			Time.fixedDeltaTime = 0.02F * Time.timeScale;
		}
		
	}
	/*
	 * Вызывается при начале установки прыжка
	 */
	public void TouchUp(){
		if (mGameManager.SCLock == true)
			return;
		if (flock)
			return;
		lighting.SetActive (false);
		lighting.SetActive (true);
		lighting2.SetActive (false);
		//lighting.SetBool ("run", false);
	//	print("TouchUp");
		ACTION = 0;
		Vector3 velocity = delta;
//		print (delta);
	//	print (distance);
		GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x, velocity.y) *distance* JumpSpeed*(-1) ;
		pricel.SetActive (false);
		Time.timeScale = 1;
		Time.fixedDeltaTime = 0.02F * Time.timeScale;
		if(circleSmall.transform.position.x < transform.position.x){
			SetLookRight(true);
		}
		else {
			SetLookRight(false);
		}
		//FireRotator.transform.rotation = Quaternion.Euler(0f, 0f, 0f );

	
	}
	public void SetLookRight(bool f){

		if(f == false){
			CatFace.transform.localScale=new Vector3(-1,1,1);
//			this.GetComponent<Fire>().napravlenie = -1;
		}
		else {
			CatFace.transform.localScale=new Vector3(1,1,1);
		//	this.GetComponent<Fire>().napravlenie = 1;
			
		}
	}

}
