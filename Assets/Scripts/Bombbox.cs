using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Bombbox : MonoBehaviour {
	public float Health = 100f;
	public Sprite spr2 ;
	public Sprite spr3 ;
	private float initHealth = 0;
	private AudioSource mAudioSource;
	public int objType = 0;
	private GameObject smoke = null;

	void Start(){
		initHealth = Health;
		mAudioSource = GetComponent<AudioSource>();
		//smoke = getChildGameObject (gameObject,"smoke");
		Transform[] ts = gameObject.GetComponentsInChildren<Transform>();
		foreach (Transform t in ts) {
			if (t.gameObject.name == "smoke" || t.gameObject.name == "iskra") {
				smoke = t.gameObject;
			}
		}
		if(smoke!=null) smoke.SetActive (false);
	}







	
	void OnCollisionEnter2D(Collision2D col)
	{
		
		
		if (col.gameObject.GetComponent<Rigidbody2D>() == null) return;
		if(col.gameObject.tag == "lift_platform")return;
		float damage = col.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude * 10;
		if (damage >= 10) {
			//GetComponent<AudioSource>().clip = destroySound1;
			
			
		}
		
		
		Health -= damage;
		//print (Health);
		if (Health < initHealth / 2) {

			if(spr2!=null)this.gameObject.GetComponent<SpriteRenderer>().sprite = spr2;
		}
		if (Health < initHealth / 3) {
			if(objType == 10){
				smoke.SetActive (true);
			}else {
				smoke.SetActive (true);
			}

			if(spr3!=null)this.gameObject.GetComponent<SpriteRenderer>().sprite = spr3;
		}
		/*
		 * 
		 * gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load("img1", typeof(Sprite)) as Sprite;
		 */
		if(col.gameObject.tag == "tomato"){
			//SpecialEffectsHelper.Instance.CatEffect(transform.position);
			
			//SoundManager.instance.RandomizeSfx(destroySound1);
			if(!mAudioSource.isPlaying && SoundManager.instance.soundOn){
				float randomPitch = Random.Range(0.8f, 1.8f);
				mAudioSource.pitch = randomPitch;
				mAudioSource.volume = damage/10;
				mAudioSource.Play();
			}
			
		}else {
			if( SoundManager.instance.soundOn && ( damage > 10) ){
				float randomPitch = Random.Range(0.8f, 1.8f);
				mAudioSource.pitch = randomPitch;
				mAudioSource.volume = damage/(2*10);
				mAudioSource.Play();
			}
			
		}
		//print ("briks col isBall="+isBall);
		
		if(!mAudioSource.isPlaying && SoundManager.instance.soundOn ){
			//float randomPitch = Random.Range(0.8f, 1.8f);
			//mAudioSource.pitch = randomPitch;
			//print ("briks col isBall="+isBall);
			mAudioSource.volume = damage+0.1f;
			mAudioSource.Play();
			
		}
		
		//if health is 0, destroy the block
		if (Health <= 0) {
		

			Collider2D[] Colliders =Physics2D.OverlapCircleAll(transform.position, 2);// Physics.OverlapSphere(transform.position, 100, 0);
			//Debug.LogError("Colliders.Length="+Colliders.Length);
			float distance = 0;
			Vector3 direction;
			for(int i = 0; i<Colliders.Length; i++)
			{
				if(Colliders[i].gameObject.GetComponent<Rigidbody2D>()!=null){
				
					distance = Vector2.Distance (transform.position, Colliders[i].gameObject.transform.position)+0.1f;
					print(Colliders[i].gameObject.transform.name+" distance="+distance);
					direction= transform.position- Colliders[i].gameObject.transform.position;
					direction = Vector3.Normalize(direction);
					Colliders[i].gameObject.GetComponent<Rigidbody2D>().AddForceAtPosition( 
					    direction*10/distance,new Vector2(0,0) );
					if(Colliders[i].gameObject.transform.name == "supercat"){
						print ("superca...................t");
						CameraManager.superCat.GetComponent<supercat>().Stope();
					}
				}
			}


			SpecialEffectsHelper.Instance.Effect("explosion",transform.position);
	
			//SoundManager.instance.RandomizeSfx(destroySound2);
			//this.gameObject.SetActive(false);
			this.transform.position  = new Vector3(-1000,-1000,0);
			Destroy(this.gameObject , mAudioSource.clip.length);
			return;
		}
		
		
		
		
	}
	

}
