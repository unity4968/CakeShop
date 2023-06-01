using UnityEngine;
using System.Collections;

public class OvenHeater : MonoBehaviour {

	public int ID = 0;
	public Vector3 ConnectedPosition = Vector3.zero;
	//public Vector3 HolderPosition = Vector3.zero;

	public bool bActive = false;
	bool bIskoriscen = false;
	MiniGame2 mg2;

	bool bDrag = false;
	public Transform Holder;
	public static  bool bEnableMove = true; 

	float x;
	float y;

	bool bFree = false; //da li se nalazi na traci
	public static float timeMove = 2;
	public string triggerName = "*";

	public static bool bAdjustPosition = false;

	void Start () {
		mg2 = Camera.main.GetComponent<MiniGame2>();
	}
 
	void Update () {
		if(  bDrag)
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
		}

		//prilikom izbacivanja komadica sa trake pozicije preostalih se koriguju
		else if(/*bAdjustPosition && */!bFree)
		{
			 
			transform.position = Vector3.Lerp(transform.position,Holder.position,timeMove);

			if(timeMove>1.2f)
			{
				transform.position =  Holder.position;
				timeMove = 2.2f;
			}
		}
	}

		

	void OnMouseDown(){
		if( bActive &&  !bIskoriscen &&  !bDrag && bEnableMove )
		{
			bDrag = true;
			transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.GetComponent<Renderer>().sortingOrder += 50;

			mg2.PartClicked(this);
		}
	}
	
	
	
	
	void OnMouseDrag(){
		if(  bActive && bDrag && bEnableMove   )
		{
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f));
			if(posM.y>3.5f) posM = new Vector3(posM.x,3.5f,posM.z);
			transform.position = posM;

			//transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x,y,10.0f));

			if( !bFree && (transform.position.x - Holder.position.x)<-1.5f)
			{
				transform.localScale = mg2.startPCSScale;
				bFree = true;
				mg2.AdjustSliderPositions(Holder);
				bAdjustPosition = true;

				Holder = null;
			}
		}
	}
	
	void OnMouseUp(){
		
		
		if( bActive &&  !bIskoriscen &&  bDrag  && bEnableMove )
		{
			//transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x,y,10.0f)); 
			StartCoroutine(MoveBack());
			bDrag = false;
		}
		if( bActive &&  bIskoriscen &&  bDrag  && bEnableMove )
		{
			//transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x,y,10.0f)); 
			StartCoroutine(Connect());
			bDrag = false;
			bActive=false;
		}
		
	}
	
	
	
	IEnumerator MoveBack()
	{
		yield return new WaitForSeconds(.1f);
	
		if(!bIskoriscen && !bFree )
		{
			mg2.HidePointer();
			float pom = 0;
			while(pom<1)
			{
				pom +=0.05f;
				transform.position = Vector3.Lerp(transform.position, Holder.position,pom);

				yield return new WaitForEndOfFrame();
			}
			
			transform.position = Holder.position;
			transform.GetComponent<Renderer>().sortingOrder -= 50;
		}
	}

	IEnumerator Connect()
	{
		if(bIskoriscen  )
		{
			mg2.Connected(this);
			float pom = 0;
			while(pom<1)
			{
				pom +=0.05f;
				transform.position = Vector3.Lerp(transform.position, ConnectedPosition,pom);
				
				yield return new WaitForEndOfFrame();
			}
			
			transform.position = ConnectedPosition;
			//transform.renderer.sortingOrder -= 50;
			transform.GetComponent<Renderer>().sortingOrder = 1;
			transform.GetComponent<Collider2D>().enabled = false;
		}
	}
	
	
	
	void OnTriggerStay2D(Collider2D other) 
	{
		 
		if(bDrag &&  other.name == triggerName)
		{
			bIskoriscen = true;
		}

	}

	void OnTriggerExit2D(Collider2D other) 
	{
		
		if(bDrag &&  other.name == triggerName)
		{
			bIskoriscen = false;
		}
		
	}


}
