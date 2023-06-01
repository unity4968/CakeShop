using UnityEngine;
using System.Collections;

public class PCS_Slider : MonoBehaviour {


	//minigame 2
	//na objektu BrokenPartsSlider
	//koristi se za pomeranje trake sa delovima grejaca



	Vector3 startPosition;
	float minY;
	public float maxY;
 
	bool bDrag  =false;
	Vector3 delatPos = Vector3.zero;


	MiniGame2 mg2;

	// Use this for initialization
	void Start () {
		minY = Camera.main.orthographicSize;
		startPosition = transform.localPosition;
		//minY = transform.localPosition.y;

		mg2 = Camera.main.GetComponent<MiniGame2>();
	}

	void OnMouseDown(){
		if(    !bDrag && OvenHeater.bEnableMove )
		{
			bDrag = true;
			delatPos = transform.localPosition -  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}

	void OnMouseDrag(){
		if(  bDrag &&  OvenHeater.bEnableMove   )
		{
		
			Vector3 pom =  Camera.main.ScreenToWorldPoint(Input.mousePosition) + delatPos;
	 
			transform.localPosition = new Vector3(startPosition.x,pom.y,startPosition.z);
		 
			if( transform.position.y >maxY+minY )  transform.localPosition =  startPosition + new Vector3(0,  maxY ,0) ;
			if( transform.position.y < minY  )  transform.localPosition =  startPosition ;

			mg2.CorectStartPosition();
		}
	}

	public void CorrectPosition()
	{
		if( transform.position.y >maxY+minY )  transform.localPosition =  startPosition + new Vector3(0,  maxY ,0) ;
		if( transform.position.y < minY  )  transform.localPosition =  startPosition ;
	}

	void OnMouseUp(){
		if(bDrag) mg2.CorectStartPosition();

		bDrag = false;
	}

}
