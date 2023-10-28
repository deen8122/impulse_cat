using UnityEngine;
using System.Collections;
using Assets.Scripts;
public class CameraFollow : MonoBehaviour
{
	
	// Use this for initialization
	void Start()
	{
		//leftConstraint = Camera.main.ScreenToWorldPoint( new Vector3(0.0f, 0.0f, 0) ).x;
		//	rightConstraint = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width, 0.0f, 0) ).x;
		
		
		StartingPosition = transform.position;
		minY = Screen.height/2;	
		GameObject g = GameObject.Find("Destroyer_left");
		minCameraX = g.transform.position.x;	
		g = GameObject.Find("Destroyer_right");
		maxCameraX = g.transform.position.x;
		minY= 2f * Camera.main.orthographicSize;
		cameraW = minY * Camera.main.aspect/2;
		minY /= 4;
		transform.position =new Vector3(0,0,0);// BirdToFollow.transform.position;
	}
	
	
	// Update is called once per frame
	void Update()
	{
		if (GameManager.CurrentGameState == GameState.BOMB_FLYING)
		{
			if (BirdToFollow != null) //bird will be destroyed if it goes out of the scene
			{
				birdPosition = BirdToFollow.transform.position;
				
				float x = Mathf.Clamp(birdPosition.x , minCameraX, maxCameraX);
				
//				print ("cat x="+birdPosition.x+" cam x="+transform.position.x+" minCameraX="+minCameraX+" cameraW="+cameraW);
				//camera follows bird's x position
				if(offsetX==0){
					//StartingPosition = transform.position;
					//offsetX = x- StartingPosition.x;
				}
				//if(birdPosition.x> StartingPosition.x){
				
				if(  (birdPosition.x  < minCameraX + cameraW)  || (birdPosition.x > maxCameraX - cameraW) ){
					//	x = minCameraX;
					x = transform.position.x;
				}
				if(birdPosition.y < minY-1){
					birdPosition.y  = minY-1;
				}
				
				transform.position = new Vector3(x , birdPosition.y, StartingPosition.z);
				//}
				
			}
			else {
				////StartingPosition = transform.position;
				//IsFollowing = false;
				//offsetX = 0;
			}
			
		}
		else {
			//mainCamera.GetComponents<CameraMove>().minX;
			//StartingPosition = transform.position;
			///IsFollowing = false;
			//offsetX = 0;
		}
	}
	
	[HideInInspector]
	public Vector3 StartingPosition;
	private Vector3 birdPosition;
	private float cameraW = 1;
	public  float minCameraX = -13;
	public  float maxCameraX = 13;
	private float offsetX=0;
	private float minY = 0;
	//[HideInInspector]
	public bool IsFollowing = true;
	// [HideInInspector]
	public static Transform BirdToFollow;
}
