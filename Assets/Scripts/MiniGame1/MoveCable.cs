using UnityEngine;
using System.Collections;

public class MoveCable : MonoBehaviour {

 
	public Vector3 StartPosition ;
	 
	public bool bDrag = false;
	 
	
	public bool bLeft = false;
	public Color Boja;
	public int IDBoje=0;
	public static  bool bEnableMove = true;
 
	float x;
	float y;
	
	 
	public SpriteRenderer[] Parts;
	public Vector3 posConnect;
	
	 
 
	public Cable3d cable;
	void Start () {
		bEnableMove = false;

		StartPosition = transform.position;
 
		posConnect = StartPosition;
		StartCoroutine( WaitStart());
	}

	IEnumerator WaitStart()
	{
		yield return new WaitForSeconds(0.1f);
		IDBoje = (int) cable.CableCol;
//		Debug.Log(IDBoje.ToString() + cable.CableCol);
	}
	
 
	void Update()
	{
		if(bDrag)
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			 
		}
	}
	
	
	
	void OnMouseDown(){
		 
		if(!cable.bIskoriscen &&  !bDrag && bEnableMove && !MiniGame1.bPause )
		{
			try{SoundManager.Instance.Play_ButtonClick();} catch{}
			bDrag = true;
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//transform.localScale = new Vector3(1.2f,1.2f,1.2f);
			
//			foreach(SpriteRenderer spr in Parts)
//			{
//				spr.sortingOrder +=50;
//			}
//			transform.renderer.sortingOrder += 50;
//			
//			if(transform.parent.name.Contains("eR") ) {
//				Camera.main.SendMessage("ShowHelpLeftWire2",IDBoje);
//				MiniGame1.bRightClickedFirst = true;
//			}
//			else if(transform.parent.name.Contains("eL") )
//			{
//				Camera.main.SendMessage("ShowHelpRightWire",IDBoje);
//				MiniGame1.bRightClickedFirst = false;
//			}
			Camera.main.SendMessage("ShowHelpRightWire",(int)cable.CableCol);
		}
	}
	
	
	  Vector3 fingerOffset = new Vector3(2,-1,0);
	
	void OnMouseDrag(){
		if(!cable.bIskoriscen &&  bDrag && bEnableMove &&   !MiniGame1.bPause)
		{
			 
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x,y,10.0f)) + fingerOffset;
			
		}
	}
	
	void OnMouseUp(){
 
		if(!cable.bIskoriscen &&  bDrag  && bEnableMove && !MiniGame1.bPause)
		{
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x,y,10.0f)) + fingerOffset;
			TestIfWireIsConnected();
			bDrag = false;
		 
		}
		
	}
	
	void TestIfWireIsConnected()
	{
 
		bool bTestWire = false;
		Collider2D [] allWiresColl =  Physics2D.OverlapCircleAll(transform.position,0.4f);

		for(int i = 0;i<allWiresColl.Length;i++)
		{
			if(allWiresColl[i].tag == "Wire" && allWiresColl[i].transform.parent.GetComponent<Cable3d>().bIskoriscen == false) bTestWire = true;
		}

		Vector3 position  = new Vector3 (1000,1000,1000);

		if(bTestWire)
		{
			for(int i = 0;i<allWiresColl.Length;i++)
			{
//				Debug.Log(allWiresColl[i].transform.name);
				if(allWiresColl[i].tag == "Wire"  &&  allWiresColl[i].transform.parent.GetComponent<Cable3d>().bIskoriscen == false)
				{
					bTestWire = true;
					if(Vector3.Distance(transform.position,allWiresColl[i]. transform.position) <  Vector3.Distance(transform.position, position))
						position  = allWiresColl[i]. transform.position  ;
					Cable3d mc = allWiresColl[i].transform.parent.GetComponent<Cable3d>();
					if(    (int)mc.CableCol == IDBoje)
					{
					 
						OnTriggerStay2D_manual(allWiresColl[i]) ;
						bTestWire = false;
						break;
					}
				}
			}

			if(bTestWire) 
			{
				cable.bIskoriscen = true;
				MiniGame1.ConnectedWires = -1;
				Debug.Log("KRATAK SPOJ!!!");
				//position  = (position + transform.position)*.5f;
				Camera.main.SendMessage("ShortCircut",position);
				Camera.main.SendMessage("TestEndGame");
			}
		}
		else
		{
			StartCoroutine(MoveBack());
		}
	}
	
	IEnumerator MoveBack()
	{
		yield return new WaitForSeconds(.1f);
		 
		if(!cable.bIskoriscen  )
		{
			
		 	Camera.main.SendMessage("ShowLeftHelpAgain" );
			float pom = 0;
			while(pom<1)
			{
				pom +=0.05f;
				transform.position = Vector3.Lerp(transform.position, StartPosition,pom);
				 
				yield return new WaitForEndOfFrame();
			}
			
			transform.position = StartPosition;
			
//			foreach(SpriteRenderer spr in Parts)
//			{
//				spr.sortingOrder -=50;
//			}
//			transform.renderer.sortingOrder -= 50;
//			
			
		}
	}
	
	
	
	void OnTriggerStay2D_manual(Collider2D other) 
	{
		if( other.tag == "Wire")
		{
			Cable3d mc = other.transform.parent.GetComponent<Cable3d>();
			if(    (int)mc.CableCol == IDBoje)
			{
				cable.bIskoriscen = true;
				Debug.Log("POVEZANO");

				string sw = transform.name.Replace("ColliderWireStart","")+other.name.Replace("ColliderWireEnd","-");
				 
				//korekcija pozicije spajanja
				switch(sw)
				{
					case "1-1" : posConnect = new Vector3(3.7f,-.1f,3);break;
					case "1-2" : posConnect = new Vector3(3.6f,-0.84f,3);break;
					case "1-3" : posConnect = new Vector3(3.32f,-1.57f,3);break;
					case "1-4" : posConnect = new Vector3(3.38f,-2.45f,3);break;

					case "2-1" : posConnect = new Vector3(3.76f,-.3f,3);break;
					case "2-2" : posConnect = new Vector3(3.34f,-1.2f,3);break;
					case "2-3" : posConnect = new Vector3(2.98f,-2.05f,3);break;
					case "2-4" : posConnect = new Vector3(2.78f,-3.05f,3);break;

					case "3-1" : posConnect = new Vector3(3.92f,-.02f,3);break;
					case "3-2" : posConnect = new Vector3(3.5f,-0.83f,3);break;
					case "3-3" : posConnect = new Vector3(3.24f,-1.6f,3);break;
					case "3-4" : posConnect = new Vector3(3.12f,-2.49f,3);break;

					case "4-1" : posConnect = new Vector3(3.81f,-.19f,3);break;
					case "4-2" : posConnect = new Vector3(3.39f,-0.91f,3);break;
					case "4-3" : posConnect = new Vector3(3.1f,-1.6f,3);break;
					case "4-4" : posConnect = new Vector3(3.05f,-2.42f,3);break;


				}
				 
				//korekcija skaliranja izolacije
				Transform isolation = null;
				switch(sw)
				{

					case "1-1" : 
					{
						isolation = GameObject.Find("Isolation01").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,180);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.23f,-2.02f,-3.6f);
						break;
					} 
					case "1-2" : 
					{
						isolation = GameObject.Find("Isolation02").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;

						isolation.Rotate (0,0,180);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.34f,-1.12f,-3.6f);
						break;
					} 
					case "1-3" : 
					{
						isolation = GameObject.Find("Isolation03").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,180);
						break;
					} 
					case "1-4" : 
					{
						isolation = GameObject.Find("Isolation04").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						 
						break;
					} 


					case "2-1" : 
					{
						isolation = GameObject.Find("Isolation01").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,181);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						break;
					} 
					case "2-2" :
					{
						isolation = GameObject.Find("Isolation02").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,182);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.55f,-1.15f,-3.6f);
						break;
					} 
					case "2-3" : 
					{
						isolation = GameObject.Find("Isolation03").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,180);
						break;
					} 
					case "2-4" : 
					{
						isolation = GameObject.Find("Isolation04").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,179);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.81f,.8f,-3.6f);
						break;
					} 


					case "3-1" :  
					{
						isolation = GameObject.Find("Isolation01").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,182);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						break;
					} 
					case "3-2" :
					{
						isolation = GameObject.Find("Isolation02").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,185);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.54f,-1.13f,-3.6f);
						break;
					} 
					case "3-3" : 
					{
						isolation = GameObject.Find("Isolation03").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,183);
 
						break;
					} 
					case "3-4" :
					{
						isolation = GameObject.Find("Isolation04").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,181);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.85f,0.81f,-3.6f);
						break;
					} 

				
						
					case "4-1" :  
					{
						isolation = GameObject.Find("Isolation01").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,189);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.17f,-1.94f,-3.6f);
						break;
					} 
					case "4-2" :
					{
						isolation = GameObject.Find("Isolation02").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,188);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.53f,-1.05f,-3.6f);
						break;
					} 
					case "4-3" : 
					{
						isolation = GameObject.Find("Isolation03").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,190);
						isolation.localScale = new Vector3(0.8f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.8f,-0.16f,-3.6f);
						break;
					} 
					case "4-4" : 
					{
						isolation = GameObject.Find("Isolation04").transform;
						//isolation.GetComponent<SpriteRenderer>().enabled = true;
						isolation.localRotation = Quaternion.identity;
						isolation.Rotate (0,0,190);
						isolation.localScale = new Vector3(0.78f,0.8f,0.8f);
						isolation.localPosition = new Vector3(2.85f,0.86f,-3.6f);
						break;
					} 
				}

				mc.bIskoriscen = true;
				StartCoroutine(Connect(isolation));
				MiniGame1.ConnectedWires++;
				MiniGame1.ClickedIdBoje =-1;
				
				Camera.main.SendMessage("TestEndGame");
			}
			 
		}
	}
 
	IEnumerator Connect(Transform isolation)
	{
		float pom = 0;
		while(  pom<1)
		{
			pom +=0.05f;
			transform.position = Vector3.Lerp(transform.position, posConnect,pom);
			yield return new WaitForEndOfFrame();
		}

		if(isolation != null) isolation.GetComponent<SpriteRenderer>().enabled = true;
		transform.position = posConnect;
	}
	
	
	
}
