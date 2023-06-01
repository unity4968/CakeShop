using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScaleCamera : MonoBehaviour {
 
	float xCorner = 9f; //maximalna desna pozicija tacke koja se vidi 
//	float yCorner = 6.3f; //maximalna gornja pozicija tacke koja se vidi 
	public float ortSize = 5f;
	Vector2 nativeAdPos1 = Vector2.zero;
	Vector2 nativeAdPos2 = Vector2.zero;
	void Awake () {
		ortSize= xCorner * (float) Screen.height/ (float) Screen.width;
		Camera.main.orthographicSize = ortSize; 

		if(Application.loadedLevelName == "Gameplay3" ) 
		{
			nativeAdPos1 =  new Vector2(588, 120-( (float) Screen.width/ (float) Screen.height - 1.33f ) * 35);
			nativeAdPos2 =  new Vector2(588, 148-( (float) Screen.width/ (float) Screen.height - 1.33f ) * 35);
		}
	}

	void Update () {
//		ortSize= xCorner * (float) Screen.height/ (float) Screen.width;
//		//if(ortSize>yCorner) ortSize = yCorner;
//		Camera.main.orthographicSize = ortSize; 

		BannerCorrection();
	}


	public void BannerCorrection()
	{
		if(Application.loadedLevelName == "Gameplay1" || Application.loadedLevelName == "Gameplay2a")
		{

//			if(AdsManager.bannerVisible)
//			{
//
//				xCorner = 9f + ( (float) Screen.width/ (float) Screen.height - constb)*consta ;
//				ortSize= xCorner * (float) Screen.height/ (float) Screen.width;
//				Camera.main.orthographicSize = ortSize;
//				Camera.main.transform.position  = new Vector3(0,1.1f,-10);
//			}
//			else
//			{
				Camera.main.transform.position  = new Vector3(0,( (float) Screen.width/ (float) Screen.height - 1.33f)*1.5f ,-10);
				xCorner = 9f + ( (float) Screen.width/ (float) Screen.height - constb)*consta ;
				//xCorner = 9f;
				ortSize= xCorner * (float) Screen.height/ (float) Screen.width;
				Camera.main.orthographicSize = ortSize; 
//			}
			 
		}

		else if(Application.loadedLevelName == "Gameplay3" ) 
		{
			 
//			if(AdsManager.bannerVisible)
//			{
//				Camera.main.transform.position  = new Vector3(0,.4f,-10);
//				xCorner = 9f + ( (float) Screen.width/ (float) Screen.height - const3b)*const3a ;
//				ortSize= xCorner * (float) Screen.height/ (float) Screen.width;
//				Camera.main.orthographicSize = ortSize; 
//			}
//			else
//			{
				Camera.main.transform.position  = new Vector3(0,0,-10);
				xCorner = 9f + ( (float) Screen.width/ (float) Screen.height - const3b)*const3a ;
				ortSize= xCorner * (float) Screen.height/ (float) Screen.width;
				Camera.main.orthographicSize = ortSize; 
			 
//			}
			
		}
 
	}

	public float consta = 1.25f;
	public float constb = 1.25f;

	public float const3a = 3f;
	public float const3b = 1.51f;
}
