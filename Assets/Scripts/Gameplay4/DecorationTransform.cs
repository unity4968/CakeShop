using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DecorationTransform : MonoBehaviour {

	public Sprite MoveSp;
	public Sprite RotateSp;
	public Sprite ScaleSp;

	public Transform ActiveDecoration;
	public int stateT = 0;
	bool bEnableDrag = true;
	 
	List<int> ActiveTouches = new List<int>();
	float touchTimeOff = 0;
	public bool bActivated = false;

	//SpriteRenderer spr;
//	int touchTapCount = 0;
	bool bEnableFlip = true;

	 
	Vector2 mousePosP =  Vector2.zero;

	bool bTwoTouches = false;
	Vector2 tp1 = Vector2.zero;
	Vector2 tp2 = Vector2.zero;
	Vector2 tpp1 = Vector2.zero;
	Vector2 tpp2 = Vector2.zero;
	Vector2 tpr1 = Vector2.zero;
	Vector2 tpr2 = Vector2.zero;
	Vector3 scaleStart = Vector3.one;

	void Start () {
		//spr = transform.GetComponent<SpriteRenderer>();
		//spr.sprite = MoveSp;
		scaleStart =  Vector3.one;
	}



	void Update(){
 
		if(	bActivated)
		{
			int fingerCount = 0;
			int tapTouchId = -1;
			foreach (Touch touch in Input.touches) 
			{
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				{
					fingerCount++;
					if(!ActiveTouches.Contains(touch.fingerId)) ActiveTouches.Add(touch.fingerId);
					if(tapTouchId == -1) tapTouchId = touch.fingerId;
				}
				else {
					if(ActiveTouches.Contains(touch.fingerId)) ActiveTouches.Remove(touch.fingerId);
				}
			}
//			Debug.Log("FING C" + fingerCount + "  : "  +  ActiveTouches.Count);

			//BROJANJE PRITISKANJA
			if(bEnableFlip &&   tapTouchId != -1 && fingerCount==1 ) 
			{
				foreach (Touch touch1 in Input.touches) 
				{
					if(touch1.fingerId == tapTouchId) 
//					Debug.Log("TT   "+touch1.tapCount);
					if(touch1.tapCount >1) StartCoroutine (FlipDecoration());
				}
			}

			if(fingerCount==1)
			{
				if(bTwoTouches)
				{
					StartCoroutine(ScaleDecorationEnd());
					bTwoTouches = false;
				}

				foreach (Touch touch in Input.touches) 
				{
					if(touch.fingerId == ActiveTouches[0]) 
					{
						if(touch.phase == TouchPhase.Moved)
						{
							if(bEnableDrag)
							{
							   Vector3 p1 = Camera.main.ScreenToWorldPoint(touch.position);

								if(p1.y>3) p1 = new Vector3(p1.x,3,-2);
								transform.position  = new Vector3(p1.x,  p1.y, -2) ;
								if(ActiveDecoration!=null)	ActiveDecoration.position = transform.position;
							}
						}
						else if(touch.phase == TouchPhase.Began)
						{
							//TESTIRANJE KLIKNUTE DEKORACIJE
							Vector2 mousePosition   = Camera.main.ScreenToWorldPoint(touch.position);
							Collider2D hitCollider  = Physics2D.OverlapPoint(mousePosition);

							if(hitCollider!=null && hitCollider.transform.tag == "Decoration"  && (ActiveDecoration== null || hitCollider.transform.name != ActiveDecoration.name) )
							{
//								Debug.Log(hitCollider.transform.name);
								  bActivated = true;
								 ActiveDecoration = hitCollider.transform;
								
								 transform.position =  hitCollider.transform.position;
								bEnableDrag = true;

								Camera.main.SendMessage("PodesiSortingOrder",ActiveDecoration);
							}

							if(hitCollider ==null || hitCollider.transform.tag != "Decoration" && Vector2.Distance(mousePosition, mousePosP) >2)
							{
								 
								bEnableDrag = false;
								
								//transform.position =  new Vector3(-1000,1000,0);//hitCollider.transform.position;
							}
							mousePosP = mousePosition;
 						}
					}	
				}
			}
			else	if(fingerCount==2 &&  ActiveDecoration !=null)
			{
				if(bTwoTouches  )
				{
					foreach (Touch touch in Input.touches) 
					{
						if(touch.fingerId == ActiveTouches[0])
							tp1 = touch.position;
						 
						if(touch.fingerId == ActiveTouches[1])
							tp2 = touch.position;
					}


					//ODREDJIVANJE SKALIRANJA
					float scale = 	Mathf.Sqrt ((tp1.y - tp2.y)*(tp1.y - tp2.y) + (tp1.x - tp2.x)*(tp1.x - tp2.x)) /	 Mathf.Sqrt((tpp1.x - tpp2.x)*(tpp1.x - tpp2.x) + (tpp1.y - tpp2.y)*(tpp1.y - tpp2.y))   ; 
					float scZ =  scale*scaleStart.z ;
					if(scZ >0.5f && scZ<2f)  ActiveDecoration.localScale  =scale*scaleStart;  //if(scZ >0.8f && scZ<2.5f)

					//ODREDJIVANJE ROTACIJE
					float rotation  =  (Mathf.Atan2( (tp1.y - tp2.y)  ,(tp1.x - tp2.x) )   - Mathf.Atan2((tpr1.y - tpr2.y),(tpr1.x - tpr2.x) ) )   * Mathf.Rad2Deg ; 
					ActiveDecoration.Rotate(0,0,rotation);


					 tpr2 =tp2;
					 tpr1 = tp1;

				}
				else
				{
					bTwoTouches = true;
					foreach (Touch touch in Input.touches) 
					{
						if(touch.fingerId == ActiveTouches[0])
							tpp1 = touch.position;

						if(touch.fingerId == ActiveTouches[1])
							tpp2  = touch.position;

					}

					tpr2 =tpp2;
					tpr1 = tpp1;

					scaleStart = ActiveDecoration.localScale;
				}
					
			}
			else //vise od dva prsta 
			{
				bTwoTouches = false;
				bEnableDrag = true;
			}

			//AKO NEMA AKCIJE VISE OD 3SEC. DEAKTIVRA SE
			if(fingerCount>0) touchTimeOff = 0;
			else touchTimeOff+=Time.deltaTime;


			if(touchTimeOff >0.2f && DeleteDecoration.bDelte && DeleteDecoration.sDelteName == ActiveDecoration.name)
			{
				Debug.Log("DeleteDecoration!!");
				Camera.main.SendMessage("DeleteDecoration",ActiveDecoration);
				DeleteDecoration.bDelte = false;
				DeleteDecoration.sDelteName = "";
			}

			if(touchTimeOff>2) 
			{
				bActivated = false;
				bEnableDrag = true;
			}
		}


	

	}

 
	IEnumerator FlipDecoration()
	{
		bEnableFlip = false;
		Vector3 sc = ActiveDecoration.transform.localScale;
		ActiveDecoration.transform.localScale = new Vector3(-sc.x,sc.y,sc.z);
		
		yield return new WaitForSeconds(.5f);
		
		bEnableFlip = true;
	}
	

	IEnumerator ScaleDecorationEnd()
	{
		bEnableDrag = false;
		yield return new WaitForSeconds(1f);
		bEnableDrag = true;
	}
	
 








	/*
	void OnMouseDrag(){
		//if(!bIskoriscen &&  bDrag && bEnableMove   )
		if(stateT == 1)
		{
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x,y,8.0f));
			ActiveDecoration.position =  transform.position;
		}
	}

	void OnMouseDown(){
		if(stateT == 1 &&  !bDrag  )
		{
			bDrag = true;
			//transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//transform.localScale = new Vector3(1.5f,1.5f,1.5f);
			//transform.renderer.sortingOrder = 8;
			//if(gp1.NextIngerdientNo == IngredientNo)
			//{
		//		gp1.GoalPointer();
		//	}
		}
	}

	void OnMouseUp(){
		
		
		if(stateT == 1 &&  bDrag   )
		{ 
			bDrag = false;

		}
		
	}
*/

}
