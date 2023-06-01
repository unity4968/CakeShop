using UnityEngine;
using System.Collections;
using System.IO;
using System;
using UnityEngine.UI;

public class CaptureCam : MonoBehaviour {
	string path;
	string mainPath;
	string cName = "cake";
 
	int textureWidth;
	int textureHeight;
	Vector3 TexturePosition;

	public byte[] bytes;

	public Transform CameraTopLeft;
	public Transform CameraBottomRight;
	 
	bool bEnableCapture = true;
	public Image CapturedImage;
	void Start () {

		path = Application.persistentDataPath + "/";
		mainPath = path + "MakeCakeSC";

		if(!Directory.Exists(mainPath))
		{
			Directory.CreateDirectory(mainPath);
		}

 
	}
	
 
 

 

	public void  CaptureScreen()
	{
		if(bEnableCapture)
			StartCoroutine(cCaptureScreen());
	}

	 
 
 
	IEnumerator cCaptureScreen()
	{
		//RACUNANJE DIMEZIJA TEKSTURE U KOJU SE RENDERUJE SNIMAK SOBE
		Vector3 c3 =  Camera.main.WorldToScreenPoint(CameraTopLeft.position);
		Vector3 c4 =  Camera.main.WorldToScreenPoint(CameraBottomRight.position);
		Vector3 c1 = (c3- c4);
		
		textureWidth = (int) (Mathf.Abs( c1.x) );
		textureHeight =(int) Mathf.Abs( c1.y);
		TexturePosition =    new Vector3 ( c3.x , c4.y,0);





		bEnableCapture = false;
		//GameObject bg = GameObject.Find("Background");
		GameObject btnDelete = GameObject.Find("btnDelete");
		GameObject can = GameObject.Find("Canvas");
		//bg.SetActive(false);
		can.SetActive(false);
		btnDelete.SetActive(false);
		yield return new WaitForEndOfFrame();
	 
		Texture2D tex = new Texture2D (textureWidth,textureHeight, TextureFormat.RGB24, false);
 
//		Debug.Log(TexturePosition +  " -  "  +textureWidth+"    "+ textureHeight);
		tex.ReadPixels(new Rect((int)TexturePosition.x, (int)TexturePosition.y,textureWidth,textureHeight),0,0);
		 
 		tex.Apply();
		can.SetActive(true);
		btnDelete.SetActive(true);
		//bg.SetActive(true);
		//SKALIRANJE TEKSTURE  
 		Texture2D newTex = (Texture2D) GameObject.Instantiate (tex);
 		TextureScale.Bilinear (newTex,1024, Mathf.FloorToInt(1024 *textureHeight/(float)textureWidth ));
		
 
		if(!Directory.Exists( path+"CAPTURE"))
		{
			Directory.CreateDirectory( path+"CAPTURE");
		}
		bytes = null;
		bytes = newTex.EncodeToPNG ();// tex.EncodeToPNG ();
		string finalPath = path+"CAPTURE/" + cName +".png";
		File.WriteAllBytes (finalPath, bytes);

//		Debug.Log("SNIMANJE"+finalPath);

		Captured(textureWidth, textureHeight ,newTex);
		yield return new WaitForSeconds(2.1f);	 

		 
		
		bEnableCapture = true; 

		 
	}
 
	public void Captured(int tw,int th, Texture2D texture  )
	{
		 
		//MenuShare.anchoredPosition = Vector2.zero;
	//	string path = Application.persistentDataPath + "/CAPTURE/cake.png";
		//		
	//	if(File.Exists(path))
	 	{
			 
			Sprite sp = Sprite.Create(texture, new Rect(0,0,1024,   Mathf.FloorToInt(1024 *th/(float)tw )   ) , new Vector2(0.5f,0.5f), 1.0f);
			
			//Image CapturedImage = (Image) GameObject.Find("CapturedImage").GetComponent<Image>();
			CapturedImage.sprite = sp;
			CapturedImage.color = Color.white;
			
		}
	}
	

 
}
