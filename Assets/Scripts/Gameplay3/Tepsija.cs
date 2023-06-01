using UnityEngine;
using System.Collections;

public class Tepsija : MonoBehaviour {
	//koristi se na sceni Gameplay3 na objektu TEPSIJA

	Vector3 StartPosition ;
	bool bIskoriscen = false;
	public bool bDrag = false;
	Gameplay3 gp3;
	Vector3 StartScale ;
	public Transform CakePosition;

	float x;
	float y;
	Vector3 diffPos = new Vector3(0,0,0);

	void Start () {
		StartPosition = transform.position;
		gp3 = Camera.main.GetComponent<Gameplay3>();
		StartScale = transform.localScale;
	}

	void Update () {
		if(bDrag)
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
		}
	}

	void OnMouseDown(){
		if( gp3.bEnableMove &&  gp3.bEnableCake &&   !bIskoriscen &&  !bDrag    )
		{
			bDrag = true;
			diffPos =transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)   ;
			diffPos = new Vector3(diffPos.x,diffPos.y,0);
			 
			gp3.GoalPointer();
		}
	}

	void OnMouseDrag(){
		if( gp3.bEnableMove &&  gp3.bEnableCake && !bIskoriscen &&  bDrag  )
		{
			
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f)) + diffPos;
			if(posM.y>3.0f) posM = new Vector3(posM.x,3.0f,posM.z);
			transform.position = posM;
		}
	}
	
	void OnMouseUp(){
		if( gp3.bEnableMove &&  gp3.bEnableCake && !bIskoriscen &&  bDrag  )
		{
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f)) + diffPos;
			transform.localScale = StartScale;
			StartCoroutine("MoveBack");
			bDrag = false;
		}
	}
	
	
	IEnumerator MoveBack( )
	{
		yield return new WaitForEndOfFrame( );
		float pom = 0;
		while(pom<1 && !bIskoriscen)
		{ 
			pom +=Time.deltaTime*5;
			transform.position = Vector3.Lerp(transform.position, StartPosition,pom);
			yield return new WaitForEndOfFrame();
		}
		if( !bIskoriscen ) transform.position = StartPosition;
	}

	IEnumerator MoveToOven(float timeWait=  0)
	{
		StopCoroutine("MoveBack");
		float pom = 0;
		while(pom<1)
		{
			pom +=Time.deltaTime*5;
			transform.position = Vector3.Lerp(transform.position, CakePosition.position,pom);
			yield return new WaitForEndOfFrame();
		}

		transform.position = CakePosition.position;
		gp3.CloseOven();
		yield return new WaitForSeconds(0.1f);
		gp3.NextPointer( );

		this.enabled = false;
	}


	void OnTriggerEnter2D(Collider2D other) 
	{
//		Debug.Log(other.name);
		if(other.name == "t0" && bDrag   )  
		{
			StartCoroutine(MoveToOven(1));
			bIskoriscen = true;
		}
	}

	public void ResetrPosition()
	{
		transform.position = StartPosition;
		this.enabled = true;
		bIskoriscen = false;
	}
	
}

