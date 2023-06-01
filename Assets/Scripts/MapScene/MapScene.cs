using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//SKRIPTA JE ZAKACENA NA KEMRI (SCENA: MapScene)
//KORISTI SE ZA POMERANJE KAMIONCICA PO EKRANU, POMERANJE POZADINE I IZBOR NIVOA

public class MapScene : MonoBehaviour {
	 

	public ScrollRect mapScrollRect;
	public RectTransform FirstMap;
	public RectTransform LastMap;

	public GameObject[] mape;
	public int SelectedLevel =  1;
	public int PrevSelectedLevel = 1;

	float scrollPosition = 0;
	int scrollMap = 1; 
	int scrollMapLast = 0; 
	int EndOfMapInterval = 0;
	float prevScrollPosition = 0;
	 


	bool bFirst = true;
	Transform[] LevelsPositions  ;
	 
	bool bEnabelMapClick = true;
	//************************************************************
	//TRUCK
	
	Transform[] Pozicije1;
	bool bChangeDirection = false;
	int TruckMoveBack = 1;
	int MoveFromEndPosition = 0;
	int MoveToEndPosition = 0;
	float pomeraj = 0;
	Animator TruckAnimator;
	public Transform Truck;
	Vector3 LastPos;
	Vector3 LastPos2;
 
	float LastAngle =0;
	
	float pathStart = 0;
	float pathEnd = 0;
 
	float OsetljivostPomeranjaMape = 0.04f;//0.04f;
	public Text[] txtCoins;

 
	bool bMoveTruck = false;
	StatusBar statusBar;
	//*****************************************************************

	void Awake () {
		Time.timeScale = 1;

		DataManager.InitDataManager();

		DataManager.Instance.GetTutorialProgress();
		DataManager.Instance.GetLocalData(); 
		//DataManager.Instance.SaveLocalData();

		if( DataManager.Instance.SelectedLevel  ==1 &&  DataManager.Instance.LastLevel>1 ) 
		{
			PrevSelectedLevel = DataManager.Instance.LastLevel;
			SelectedLevel = DataManager.Instance.LastLevel;
		}
		else 	if( DataManager.Instance.SelectedLevel  >1  ) 
		{
			PrevSelectedLevel = DataManager.Instance.SelectedLevel;
			SelectedLevel = DataManager.Instance.SelectedLevel;
		}

		Shop.InitShop();
		Shop.Instance.txtDispalyCoins = txtCoins;
	 

		/*
		GameObject.Find("Canvas").GetComponent<GraphicRaycaster>().sortOrderPriority = 2;
		GameObject.Find("CanvasMap").GetComponent<GraphicRaycaster>().sortOrderPriority = 1;

		GameObject.Find("Canvas").GetComponent<GraphicRaycaster>().renderOrderPriority = 2;
		GameObject.Find("CanvasMap").GetComponent<GraphicRaycaster>().renderOrderPriority = 1;
		*/
	}



	void Start () {
		//LevelTransition.Instance.ShowScene ( );


		mapScrollRect.horizontalNormalizedPosition = 1;
		LastMap.transform.position =  Vector3.zero;
		mapScrollRect.horizontalNormalizedPosition = 0;
		FirstMap.transform.position =  Vector3.zero;
 
		TruckAnimator = Truck.Find ("AnimationHolder").GetComponent<Animator>();
 

		SetSelectLevelButtons();
		InvokeRepeating("TestEndOfMap",2,0.5f);
 
		SetMapStart("btnLevelSelect_"+ SelectedLevel.ToString().PadLeft(2,'0'));

		statusBar = GameObject.Find("StatusBar").GetComponent<StatusBar>();
		statusBar.Init( DataManager.Instance.LastLevel, 0, 0, 0, Shop.Coins);
		statusBar.SetPosition();


		if(	!SoundManager.Instance.menuMusic.isPlaying)
		{
			SoundManager.Instance.Stop_GameplayMusic();
			SoundManager.Instance.Play_MenuMusic();
		}

		menuManager.EscapeButonFunctionStack.Push("btnHomeClicked");
	}
	
	 
	void Update () {
		TestHideMaps();
		if(bMoveTruck) MoveTruck();
	}

	void TestHideMaps()
	{
		if(mapScrollRect.horizontalNormalizedPosition < 0.166f) scrollMap = 1; 
		else if(mapScrollRect.horizontalNormalizedPosition <0.332f) scrollMap = 2; 
		else if(mapScrollRect.horizontalNormalizedPosition <0.498f) scrollMap = 3; 
		else if(mapScrollRect.horizontalNormalizedPosition <0.664f) scrollMap = 4; 
		else if(mapScrollRect.horizontalNormalizedPosition <0.83f) scrollMap = 5; 
		else scrollMap = 6; 
		 
		if(scrollMapLast !=scrollMap)
		{
			for(int i=0;i<mape.Length;i++)
			{
				if(i <= scrollMap && i+2 >=scrollMap)
					mape[i].SetActive(true);
				else
					mape[i].SetActive(false);
			}
		}
		scrollMapLast =scrollMap;
	}

	//************************************************************
	//AKO JE MAPA POMERENA DO KRAJA I NE POMERA SE DO ISTEKA EndOfMapInterval VRACA SE NA POSLEDNJI OTVORENI NIVO
	void TestEndOfMap()
	{
		if(mapScrollRect.horizontalNormalizedPosition >0.98f &&  scrollPosition<0.82f)
		{
			EndOfMapInterval++;
			if(EndOfMapInterval>8)
			{
				StartCoroutine("ScrollBackMap");
			}
		}
		else EndOfMapInterval=0;
	}

	IEnumerator ScrollBackMap()
	{
		EndOfMapInterval = 0;
		prevScrollPosition = mapScrollRect.horizontalNormalizedPosition ;
		while(mapScrollRect.horizontalNormalizedPosition > scrollPosition)
		{
			if((prevScrollPosition+0.01f) < mapScrollRect.horizontalNormalizedPosition) break; 

			mapScrollRect.horizontalNormalizedPosition -=0.01f;
			prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;

			if(mapScrollRect.horizontalNormalizedPosition < scrollPosition)
			{
				mapScrollRect.horizontalNormalizedPosition = scrollPosition;
				prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;
			}
			yield return new WaitForEndOfFrame();
		}

		mapScrollRect.horizontalNormalizedPosition = scrollPosition;
		prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;
		EndOfMapInterval = 0;
	}
	//*****************************************************************


	//*****************************************************************
	//AKTIVIRANJE DUGMICA ZA IZBOR NIVOA
	//POZIVA SE NA POCETKU I PRILIKOM KLIKA NA DUGME ZA IZBOR NIVOA
	void SetSelectLevelButtons()
	{
 
		if(bFirst) LevelsPositions = new Transform[60];
 		

		for(int i = 1;i<=60;i++)
		{
			GameObject go = GameObject.Find("btnLevelSelect_"+i.ToString().PadLeft(2,'0'));
			if(go!=null)
			{
				if(bFirst) LevelsPositions[i-1] = go.transform;

				if(DataManager.Instance.LevelProgressList.ContainsKey(i))
				{
					go.transform.Find("txtLevelStars").GetComponent<Text>().text = i.ToString();//+" : "+DataManager.Instance.LevelProgressList[i].Stars.ToString();
					Button btn = go.GetComponent<Button>();
					btn.onClick.RemoveAllListeners();
					btn.onClick.AddListener(( ) => btnSelectLevelClicked(go.name));
					btn.interactable=true;
				}
				else
				{
					go.transform.Find("txtLevelStars").GetComponent<Text>().text = i.ToString();//+" : -";
					Button btn = go.GetComponent<Button>();
					btn.onClick.RemoveAllListeners();
					btn.onClick.AddListener(( ) => btnSelectLockedLevelClicked(go.name));
					btn.interactable=true;
					btn.GetComponentInChildren<Image>().sprite = btnOff;
				}
			}
		}
	 
 
	}


	public Sprite btnOff;


	IEnumerator ScrollMapToTruck( )
	{
 
			yield return new WaitForSeconds(.1f);

			float diffX = LastMap.transform.position.x - FirstMap.transform.position.x;
			float diffX2 = Truck.position.x - FirstMap.transform.position.x;
			scrollPosition = diffX2/diffX;
			
			if(scrollPosition>1) scrollPosition =1;
			if(scrollPosition<0) scrollPosition =0;

			EndOfMapInterval = 0;
			prevScrollPosition = mapScrollRect.horizontalNormalizedPosition ;
			 
//			Debug.Log (prevScrollPosition + "  rr  "+ scrollPosition);
		//	if(prevScrollPosition < scrollPosition )
			if(prevScrollPosition - scrollPosition < -OsetljivostPomeranjaMape) //sa desna na levo desna granica
			{
			 
				while(mapScrollRect.horizontalNormalizedPosition - scrollPosition < - OsetljivostPomeranjaMape*1f)
				{
					mapScrollRect.horizontalNormalizedPosition +=0.001f;
					prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;
					yield return new WaitForSeconds(0.02f);

					diffX = LastMap.transform.position.x - FirstMap.transform.position.x;
					diffX2 = Truck.position.x - FirstMap.transform.position.x;
					scrollPosition = diffX2/diffX;
					
					if(scrollPosition>1) scrollPosition =1;
					if(scrollPosition<0) scrollPosition =0;
				}
			}
			else if(prevScrollPosition - scrollPosition > OsetljivostPomeranjaMape) //sa leva ma desno - leva granica
			{
				 
				while(mapScrollRect.horizontalNormalizedPosition - scrollPosition > OsetljivostPomeranjaMape*1f)
				{
					mapScrollRect.horizontalNormalizedPosition -=0.001f;
					prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;
					yield return new WaitForSeconds(0.02f);

					diffX = LastMap.transform.position.x - FirstMap.transform.position.x;
					diffX2 = Truck.position.x - FirstMap.transform.position.x;
					scrollPosition = diffX2/diffX;
					
					if(scrollPosition>1) scrollPosition =1;
					if(scrollPosition<0) scrollPosition =0;
				}
			}
			
			//mapScrollRect.horizontalNormalizedPosition = scrollPosition;
			EndOfMapInterval = 0;

			StopAllCoroutines();
			StartCoroutine (ScrollMap2( ));

		 
 


	}

	IEnumerator ScrollMap2( )
	{
//		Debug.Log("ScrollMap");
		float diffX = LastMap.transform.position.x - FirstMap.transform.position.x;
		float diffX2   = LevelsPositions[SelectedLevel-1].position.x - FirstMap.transform.position.x;
		
		scrollPosition = diffX2/diffX;
		if(scrollPosition>1) scrollPosition =1;
		if(scrollPosition<0) scrollPosition =0;

		while(Mathf.Abs(mapScrollRect.horizontalNormalizedPosition - scrollPosition) > .005f)
		{
			float diffX3 = Truck.position.x - FirstMap.transform.position.x;
			float  truckPosition = diffX3/diffX;

			if(mapScrollRect.horizontalNormalizedPosition > scrollPosition)
			{


				float speed = truckPosition - mapScrollRect.horizontalNormalizedPosition ;
				mapScrollRect.horizontalNormalizedPosition -=   (0.001f  - speed*0.01f)*.4f;
				prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;

			}

			else
			{
				float speed = truckPosition - mapScrollRect.horizontalNormalizedPosition ;
//				Debug.Log("SP:  "+speed);
				mapScrollRect.horizontalNormalizedPosition +=(0.001f + speed*0.01f)*.4f;
				prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;
			}
			yield return new WaitForSeconds(0.02f);
		}
		if(bLoadScene)
		{
			StartCoroutine(WaitToLoadScene());
		}
		
	}

 









	//*****************************************************
	public void btnSelectLockedLevelClicked(string name)
	{
		SoundManager.Instance.Play_Sound(SoundManager.Instance.WrongIngredient);
	 
	}
	//KLIK NA LEVEL NA MAPI
	public void btnSelectLevelClicked(string name)
	{
		if(!bEnabelMapClick) return;

		bEnabelMapClick = false;
		string[] nameSp = name.Split('_');
		int level = int.Parse(nameSp[1]);
		PrevSelectedLevel = SelectedLevel;
		SelectedLevel = level;
	 
		DataManager.Instance.SelectLevel(level);
		 
		bMoveTruck = true;
		StartMovingTruck(name);

		if(!bFirst)
		{
			bEnabelMapClick = false;
		}
		else 
			bFirst = false;



		if( SelectedLevel == 1 &&  PrevSelectedLevel == 1)	
		{
			DataManager.Instance.SelectLevel(1);
			// Application.LoadLevel("CustomerScene");
			LevelTransition.Instance.HideScene("CustomerScene");
		}

		StartCoroutine( ScrollMapToTruck( ));
		SoundManager.Instance.Play_ButtonClick();
	}




	public void SetMapStart(string name)
	{
		//		Debug.Log(name);
		if(bEnabelMapClick   )
		  
		{
			string[] nameSp = name.Split('_');
			int level = int.Parse(nameSp[1]);
			PrevSelectedLevel = SelectedLevel;
			SelectedLevel = level;
			
			 
			bMoveTruck = false;
			StartMovingTruck(name);

			if(!bFirst)
			{
				bEnabelMapClick = false;
				
			}
			else bFirst = false;
			
		}

		//*************************************
//		Debug.Log("StartScrollMap");
		
		float diffX = LastMap.transform.position.x - FirstMap.transform.position.x;
		float diffX2   = LevelsPositions[SelectedLevel-1].position.x - FirstMap.transform.position.x;
		
		scrollPosition = diffX2/diffX;
		if(scrollPosition>1) scrollPosition =1;
		if(scrollPosition<0) scrollPosition =0;
		
		
		mapScrollRect.horizontalNormalizedPosition =  scrollPosition;
		prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;
		

			
		
	}


	//************************************************************
	//TRUCK


	void StartMovingTruck(string name)
	{
	 

 
		bChangeDirection = false;
		MoveFromEndPosition = 0;
		MoveToEndPosition = 0;
		if(Mathf.Abs(SelectedLevel - PrevSelectedLevel) <3)
			{
			if(SelectedLevel < PrevSelectedLevel)
			{
				if(TruckMoveBack == 1) bChangeDirection = true;
				TruckMoveBack = -1;
				Truck.localScale =    new Vector3(15,15,15);//       new Vector3(-TruckStartScale.x,TruckStartScale.y,TruckStartScale.z);
				if(SelectedLevel == 1) MoveToEndPosition = 1;
				if(PrevSelectedLevel == DataManager.Instance.LevelsList.Count )  MoveFromEndPosition = 1;
			}
			else
			{
				if(TruckMoveBack == -1) bChangeDirection = true;
				TruckMoveBack = 1;
				Truck.localScale = new Vector3(-15,15,15);
				if(PrevSelectedLevel == 1) MoveFromEndPosition = 1;
				if(SelectedLevel ==    DataManager.Instance.LevelsList.Count )  MoveToEndPosition = 1;
			}

			int pointsCount =  Mathf.Abs (SelectedLevel - PrevSelectedLevel) - MoveToEndPosition - MoveFromEndPosition + 3;
	 

			string PozList=" ";
			Pozicije1 = new Transform[ pointsCount];

			for(int i = 0; i <pointsCount;i++)
			{
				if(TruckMoveBack == 1)
				{
					int ind = PrevSelectedLevel  -1 +     MoveFromEndPosition + i  ; 
					Pozicije1[i] = LevelsPositions[ind-1];

				//	Debug.Log("NAPRED - "+ ind);
					PozList +=ind.ToString()+":";
				}
				else
				{
					int ind = PrevSelectedLevel + 1 -    MoveFromEndPosition - i; 
					Pozicije1[i] = LevelsPositions[ind-1];

					//Debug.Log("NAZAD - "+ ind);
					PozList +=ind.ToString()+":";
				}

	//			Debug.Log( "Razlika " +pointsCount  + " :  " +PrevSelectedLevel + " - "  + SelectedLevel + PozList  );
			}



			
			
			if(PrevSelectedLevel!=SelectedLevel)
			{
				pathStart = (1-MoveFromEndPosition)/(float)( pointsCount-1);
				pathEnd = 1 - (1-MoveToEndPosition)/(float)( pointsCount-1);

				pomeraj = pathStart;

	//			Debug.Log( pathStart + " - "+pathEnd  + " :  "  + pomeraj);
			

				if( MoveFromEndPosition ==0 &&  Pozicije1[0] !=null && Pozicije1[1]!=null) 
				{
	 
					LastAngle =    Mathf.Atan2(Pozicije1[1].position.y- Pozicije1[0].position.y, Pozicije1[1].position.x- Pozicije1[0].position.x) * Mathf.Rad2Deg;

				 if(TruckMoveBack == -1) LastAngle +=180;

					Truck.rotation =  Quaternion.identity;
					Truck.Rotate(new Vector3(0,0,LastAngle ));
						//		Debug.Log(LastAngle);
				}
				else
					LastAngle = 0;
	 
				//LastAngle =Truck.rotation.z;
				LastPos=   Pozicije1[0].localPosition;
				LastPos2 = LastPos;
	 

				TruckAnimator.SetBool("bMove",true);
			}
			else
			{
				Truck.rotation =  Quaternion.identity;
				Truck.position =   LevelsPositions[SelectedLevel-1].position ;
			}
		}
		else
		{
			Truck.rotation =  Quaternion.identity;
			Truck.position =   LevelsPositions[SelectedLevel-1].position ;

		}

		MoveTruck();
	}









	void MoveTruck()
	{

		if(DataManager.Instance.LastLevel > 1)
		{


			if(pomeraj< pathEnd)
			{
				LastPos = LastPos2;
				LastPos2  =Truck.localPosition; ;

 
				float incTime = Time.deltaTime*2 /(float)( Pozicije1.Length);
				pomeraj+= incTime;

				iTween.PutOnPath(Truck.gameObject,Pozicije1,pomeraj);
				float angle =   Mathf.Atan2(Truck.localPosition.y- LastPos.y, Truck.localPosition.x- LastPos.x) * Mathf.Rad2Deg;

				if( pomeraj > ( pathStart+ 1* incTime))
				{
//					float psa = pathStart/2f;
//					if(pomeraj<psa)
//					{
//						angle =  LastAngle *(psa-pomeraj)/psa + angle*( pomeraj) /psa;
//					}


			 

					if(TruckMoveBack == -1) //vracanje
					{

						angle +=180;

						if( angle<325  && angle>270 )  angle = 325;
						 if( angle<270  && angle>215)  angle = 215;

						if(LastPos.x>Truck.localPosition.x) 
						{
							//Debug.Log(angle);
							Truck.localScale = new Vector3(15,15,15);
							if( angle<145  && angle>35 )  angle = 35;
						}
						else
						{
							Truck.localScale = new Vector3(15,-15,15);
							if( angle<145  && angle>35 )  angle = 145;
						}


					}
					else //smer napred
					{
						//Debug.Log(angle);

						if(angle<-35)  angle = -35;
						if(angle<135 && angle>45)  angle = 45;
						if(LastPos.x>Truck.localPosition.x) 
						{
							//Debug.Log(angle);
							Truck.localScale = new Vector3(-15,-15,15);
							  angle = 180-angle;
						}
						else
						{
							Truck.localScale = new Vector3(-15,15,15);
						}

					}

				 	Truck.rotation =  Quaternion.identity;
				    Truck.Rotate(new Vector3(0,0,angle));
				}
				if(bChangeDirection ) Truck.Rotate(new Vector3(0,0,180));
				bChangeDirection = false;
				mapScrollRect.enabled = false;
			}
			else
			{
				 
				TruckAnimator.SetBool("bMove",false);
				 mapScrollRect.enabled = true;
				//bEnabelMapClick = true;
//				Debug.Log("Skok" + bMoveTruck);
				if( bMoveTruck)
				{
					if(!bLoadScene ) StartCoroutine(WaitToLoadScene());
				}
				 
			}
		}
	}

	bool bLoadScene = false;
	IEnumerator WaitToLoadScene ()
	{
		BlockAll.blocksRaycasts = true;
		bLoadScene = true;
 
		Time.timeScale = 1;
 
		yield return new WaitForSeconds(.5f);
	 
		LevelTransition.Instance.HideScene("CustomerScene");
	}


	//BUTTONS **********************************************
 
	public void btnHomeClicked()
	{
		DataManager.Instance.SelectLevel(   DataManager.Instance.LastLevel );
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(2,false));
		LevelTransition.Instance.HideScene("MainScene");
	 
		SoundManager.Instance.Play_ButtonClick();
	}


	//*********************************************
	public CanvasGroup BlockAll;
	public GameObject popUpShop;
	public MenuManager menuManager;
	
	public void ClosePopUpShop()
	{
        Debug.Log("gasimo shop");
		BlockAll.blocksRaycasts = true;
		menuManager.ClosePopUpMenu (popUpShop);
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
		SoundManager.Instance.Play_ButtonClick();
		if( menuManager.EscapeButonFunctionStack.Count >0 &&  menuManager.EscapeButonFunctionStack.Peek()== "ClosePopUpShop" ) menuManager.EscapeButonFunctionStack.Pop ();
	}
	
	
	
	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		
	}

	public void btnClicked_PlaySound()
	{
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
	}

	public void ClosePopUpByName(string popUpName)
	{
		GameObject popUp  = GameObject.Find(popUpName);
		if(popUp!=null) menuManager.ClosePopUpMenu(popUp);
		if( menuManager.EscapeButonFunctionStack.Count >0 &&  menuManager.EscapeButonFunctionStack.Peek().Contains("*"+popUpName) ) menuManager.EscapeButonFunctionStack.Pop ();
	}


	public void btnShowHideBanner()
	{
		GameObject.Find("StatusBar").GetComponent<StatusBar>().SetPosition();
	}
 
}
