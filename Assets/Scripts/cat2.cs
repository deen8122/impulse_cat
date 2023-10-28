using UnityEngine;
using System.Collections;
using Assets.Scripts;


public class cat2 : MonoBehaviour {
	public float Health = 70f;
	public Sprite spr2 ;
	public Sprite spr3 ;
	private float initHealth = 0;

	private GameManager mGameManager=null;
	public bool isEnemy = true;
	public bool isDie = false;
	public bool  lookRight =true;
	public GameObject rot = null;

	public GameObject health1 = null;
	public GameObject health2 = null;
	public GameObject health3 = null;
	public GameObject Strelka = null;
	public GameObject Hat = null;
	public bool isLock = false;
	private AudioSource asMeow = null;
	private bool f_in_hat = false;
	public int catType = 0;
	private bool f_one_check= false;

	public GameObject boolet  = null;
	
	public int  Nbullet = 2;
	private int ibullet = 0;
	private GameObject[] bulletArr;
	private int shootnaprav = 0;
	private int passedShoot = 0;
	public float shootDelay = 1f;
	public float ThrowSpeed = 1;
	private string booletName = "";
	//----------------------------------------------
	void Start(){
		StartCoroutine (WaitANDshoor());
		initHealth = Health;
		if(GameObject.FindGameObjectWithTag ("gm")!=null)
			mGameManager = GameObject.FindGameObjectWithTag ("gm").GetComponent<GameManager>();
		
		asMeow = GetComponent<AudioSource>();
		if(asMeow!=null){
			float randomPitch = Random.Range(0.8f, 1.8f);
			asMeow.pitch = randomPitch;
		}
		if(Strelka!=null){
			Strelka.SetActive(false);
			GetComponent<Fire>().strleka = Strelka;
			
		}
		
		bulletArr = new GameObject[Nbullet];
		for (int i = 0; i < Nbullet; i++)
		{
			//print(boolet.transform.name);
		
		//	booletName = boolet.transform.name;
			GameObject go = Instantiate(boolet, new Vector3((float)i, 1, 0), Quaternion.identity) as GameObject;
			go.SetActive(false);
			go.transform.localScale = Vector3.one;
			bulletArr[i] = go;
		}
		
		if(this.Hat !=null){
			this.f_in_hat = true;
			
		}
		if(this.lookRight == false){
			SetLookRight(lookRight);
		}
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("enemy_bomb"), LayerMask.NameToLayer("enemy"), true);
		
	}
	IEnumerator WaitANDshoor() {
		yield return new WaitForSeconds(shootDelay);
		if(Health > 0){
			StartCoroutine (WaitANDshoor());
		}
		if (passedShoot > 0) {
			passedShoot--;
			//return ;
			
		} else {
			Shoot ();
		}





	}

	private void Shoot(){
		float dist = Vector3.Distance(CameraManager.superCat.transform.position, transform.position);
		//	print("Distance to other: " + dist);
		Vector3 delta =(-transform.position + CameraManager.superCat.transform.position );
		RaycastHit[] hits;
		hits = Physics.RaycastAll(transform.position, delta,dist);
		Debug.DrawLine (transform.position, CameraManager.superCat.transform.position, Color.red, Mathf.Infinity);
		Debug.Log(hits.Length);

		if(dist > 10) {
			return ;
			//break;
		}
		if (this.catType == 0) {



		}
		if (hits.Length > 0) {
			//float distance = Mathf.Abs(hit.point.y - transform.position.y);
			//float heightError = floatHeight - distance;
			//float force = liftForce * heightError - rb2D.velocity.y * damping;
			//return false;
		}
		//;'return false;
		if (ibullet == Nbullet)
			ibullet = 0;
		//Debug.Log(ibullet);
		Vector3 pos = transform.position;
		float randX = Random.Range (-0.2f, 0.2f);
		float randY = Random.Range (-0.2f, 0.2f);

		if (bulletArr[ibullet] != null) {
			bulletArr[ibullet].SetActive(true);
			bulletArr[ibullet].transform.position = pos;
			//pos.x+=randX;
			//pos.y-=randY;
			//	print (CameraManager.superCat.transform.position.x+"|"+transform.position.x);
			delta = delta.normalized;
			delta.x+=randX;
			delta.y-=randY;
			if(delta.x < 0){
				SetLookRight(true);
			}
			else {
				SetLookRight(false);

			}	
			//Тут сложнее
			if(boolet.transform.name == "bullet2x2"){
				bulletArr[ibullet].GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(delta.x, delta.y) * ThrowSpeed;

			}else {
				bulletArr[ibullet].GetComponent<Rigidbody2D>().velocity = new Vector2(delta.x, delta.y) * ThrowSpeed;
			}
			//	bulletArr[ibullet].gameObject.tag = "enemy_bomb";
			ibullet++;

			if(catType == 101){

				if (ibullet == Nbullet)
					ibullet = 0;
				bulletArr[ibullet].SetActive(true);
				bulletArr[ibullet].transform.position = pos;

				if(boolet.transform.name == "bullet2x2"){
					bulletArr[ibullet].GetComponentInChildren<Rigidbody2D>().velocity = new Vector2(delta.x, delta.y) * ThrowSpeed;

				}else {
					bulletArr[ibullet].GetComponent<Rigidbody2D>().velocity = new Vector2(delta.x+0.1f, delta.y-0.1f) * ThrowSpeed;
				}

				ibullet++;
				Jump();
			}
			if(catType == 1||catType == 102){
				Jump();
			}
			if(catType == 2){
				Jump();
			}
			if(catType == 102){
				HideCat();
			}
		}
	}
	private bool isHidden = false;
	private void HideCat(){
		if (isHidden) {
			isHidden = false;
			this.GetComponent<SpriteRenderer> ().color =new Color(1f,1f,1f,1f) ;
		} else {
			isHidden = true;
			this.GetComponent<SpriteRenderer> ().color = new Color(1f,1f,1f,0f) ;;
		}
	}
	//-------------------------------------------------------------------------------------
	//вызывается при начале выстрела.
	public void Jump(){
		//print ("StartShoot catType="+catType);
		if(catType == 1){
			f_one_check= true;
			if(GetComponent<Rigidbody2D>().velocity.magnitude ==0){
			GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range (-5, 5), Random.Range (0, 6) + 6);
			this.gameObject.layer = LayerMask.NameToLayer("enemy_bomb");
			}
			//GetComponent<CircleCollider2D> ().isTrigger = true;
		}
		if(catType == 101 ||catType == 2||catType == 102){
			f_one_check= true;
		
			//GetComponent<Rigidbody2D>().velocity.x
			float x = Random.Range (-7,7);

			float y = Random.Range (-7,7);
			if(Mathf.Abs(x)<4){
				x*=2;
			}

			if(Mathf.Abs(y)<4){
				y*=2;
			}
			GetComponent<Rigidbody2D>().velocity = new Vector2(x, y);


			//GetComponent<CircleCollider2D> ().isTrigger = true;
		}
	}
	/*
	 * Анимация кота который должен стрелять.
	 */




	public void AnimateSelected(){

	}
	public void AnimateBombCollision(){
		//GameObject rot = transform.FindChild("rot").gameObject;
		if(rot !=null){
			int deen_rot_noize = Animator.StringToHash("deen_rot_noize");
			//int runStateHash = Animator.StringToHash("Base Layer.Run");
			//rot.GetComponent<Animator>().Play(deen_rot_noize);
			//rot.GetComponent<Animator>().SetTrigger(deen_cat_mouth_ouu);
		}

		//this.GetComponent<AnimationClip>()
		//this.gameObject
	}
	//----------------
	public void AnimateStartShoot(){
		
	}
	//-------------------
	public void AnimateEndShoot(){

	}
	//---------------
	public void AnimateHappy(){

	}
	//-------------------
	public void AnimateDie(){

	}
	//---------------------------------------------------------------------------------


    void Update(){

		if(f_one_check){
			if(GetComponent<Rigidbody2D>().velocity.y<0){
				this.gameObject.layer = LayerMask.NameToLayer("enemy");
				//GetComponent<CircleCollider2D> ().isTrigger = false;
			}
		}
	}


    public void SetLookRight(bool f){
		this.lookRight = f;
		if(f == false){
			this.transform.localScale=new Vector3(-1,1,1);
			this.GetComponent<Fire>().napravlenie = -1;
		}
		else {
			this.transform.localScale=new Vector3(1,1,1);
			this.GetComponent<Fire>().napravlenie = 1;

		}
	}
	//----------------------------------------------------
	void OnCollisionEnter2D(Collision2D col)
	{
		if(isLock)return;
		if(isDie)return;
		if (GameManager.CurrentGameState == GameState.CREATE_OBJ) {
			return;
		}
		print ("------->"+col.gameObject.tag);
		if(catType == 1 && f_one_check){
			f_one_check = false;
			if(this.transform.position.x < mGameManager.playerCatPositionX){
				SetLookRight(false);
			}else {
				SetLookRight(true);
			}
		}
		if(col.gameObject.tag == "lift_platform"){ return;}
		if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;
		if (mGameManager == null)
			return;
	


		float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;

		if (col.gameObject.tag == "ground") {
			damage = 10;
		}

		if(col.gameObject.tag == "supercat"){
			if (this.catType == 2) {
				
				
				
			}else {

				passedShoot = 2;
			}

		}
		if(col.gameObject.tag == "supercat"){
			SpecialEffectsHelper.Instance.CatEffect(transform.position);
			//SoundManager.instance.RandomizeSfx(destroySound1);
			if(asMeow!=null& SoundManager.instance.soundOn){
			if(!asMeow.isPlaying){

				asMeow.Play();
			}
			}
			if(this.f_in_hat == true){
				this.f_in_hat = false;
				Hat.GetComponent<Rigidbody2D>().isKinematic = false;
				Hat.GetComponent<Rigidbody2D> ().bodyType = RigidbodyType2D.Dynamic;
				Hat.GetComponent<PolygonCollider2D>().isTrigger = false;
				return;
			}

		}

		AnimateBombCollision();
		Health -= damage;
		print ("damage=>"+damage);
		print ("Health=>"+Health);
		if (Health < initHealth / 1.5) {
			if(health1!=null)health1.SetActive(true);
		}
		if (Health < initHealth / 2) {
			if(health2!=null)health2.SetActive(true);
		}
		if (Health < initHealth / 2.2) {
			if(health3!=null)health3.SetActive(true);
		}


		if (Health <= 0) {

			SpecialEffectsHelper.Instance.CatDieEffect(transform.position);

			this.isDie = true;
			this.GetComponent<Fire>().isFired = true;
			if(isEnemy) {
				mGameManager.ENEMY_CAT_COL -- ;
			}
			else mGameManager.PLAYER_CAT_COL--;
			Vector3 pos = transform.position;
			this.transform.position  = new Vector3(-1000,-1000,0);
			this.tag="Untagged";
			for(int i = 0; i < bulletArr.Length;i++){
				Destroy(bulletArr[i]);
			}
			if(asMeow!=null& SoundManager.instance.soundOn){
				Destroy(this.gameObject , asMeow.clip.length);
			}
			else Destroy(this.gameObject , 1f);
			mGameManager.CatDie(pos);


			//Destroy (this.gameObject);
		}

		/*
		if (mGameManager.ENEMY_CAT_COL <= 0) {

			GameManager.CurrentGameState = GameState.Won;
			mGameManager.PlayerWon();
		}
		if (mGameManager.PLAYER_CAT_COL <= 0) {
			
			GameManager.CurrentGameState = GameState.Lost;
			mGameManager.PlayerLost();
		}
		*/
	}
	

}
