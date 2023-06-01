using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MiniGame2 : MonoBehaviour {


	public  OvenHeater[] pcs;
	public Transform StartPosPref;
	List< Transform> StartPos = new List<Transform>() ;
	public Transform EndPosPref;
	public Transform[] EndPos ;

	List< OvenHeater> HelpList = new List<OvenHeater>();

	public MenuManager menuManager;
	
	//public GameObject PopUpTask;
	//public GameObject PopUpLevelComplete;
	public GameObject PopUpPause;
 
	float tsc = 1;
	public bool bStartGame = false;

	float TimeToShowHelp = 10;
	float CurrentTimeToShowHelp = 0;
	bool bShowHelp = false;
	 
	OvenHeater ovenHetarPartClilcked = null;

	public Animator animHandPointer;
	public GameObject  HandPointer;
	
//	bool bTutorial = false;

	Vector3 StartCursorPos;
	Vector3 EndCursorPos;

	public Vector3 CursorPosOffset ;

	int PartsLeftToConnect = 8;

	public Transform PCS_Holder;
	public PCS_Slider PCS_slider;

	public Vector3 startPCSScale;
	float yDist;
	float  xPosRight ;

	public CanvasGroup BlockAll;

	void Awake () {
//		if(AdsManager.Instance !=null)  AdsManager.Instance.HideBanner();
		DataManager.InitDataManager();
		DataManager.Instance.GetTutorialProgress();
		DataManager.Instance.PopulateGameplay3Data();
	}

	public Transform ResetkaL;
	public Transform ResetkaR;

	Vector3[] StartPosTmp ;
	// Use this for initialization
	void Start () {
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(1,false));

		ResetkaL.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0,0.5f,10));
		ResetkaR.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(1,0.5f,10));
	 
		PCS_Holder.transform.position = ResetkaR.position +new Vector3(-1.6f,0,0);

		iTween.FadeUpdate(HandPointer,0,0);
		Input.multiTouchEnabled = false;



		EndPos = new Transform[pcs.Length];
		startPCSScale =  pcs[0].transform.localScale;

		//odredjivanje pozicija 
		 StartPosTmp = new Vector3[pcs.Length];
		// yDist =4* Camera.main.orthographicSize/pcs.Length ;
		yDist =3* Camera.main.orthographicSize/pcs.Length ;

		 xPosRight = PCS_slider.transform.position.x;
		PCS_slider.maxY =   ((pcs.Length+1.5f)* yDist) *.8f   -  Camera.main.orthographicSize *2   ; //odredjivanje pomeranja slajdera

		//odredjivanje pozicioniranje na traci
		for(int i = 0; i<pcs.Length; i++)
		{
			StartPosTmp[i] = new Vector3(  xPosRight, (Camera.main.orthographicSize  -.5f*yDist  - i* yDist) *.8f,0);
		}
 
		//premestanje rasporeda da bude random 
		Vector3 iTmp;
		for (int i = 0;i<100;i++)
		{
			int r1 = Mathf.FloorToInt(Random.Range(0,pcs.Length));
			iTmp = StartPosTmp[r1];
			int r2 = Mathf.FloorToInt(Random.Range(0,pcs.Length));
				
			StartPosTmp[r1] = StartPosTmp[r2];
			StartPosTmp[r2] = iTmp;
		}


		for(int i = 0; i<pcs.Length; i++)
		{
		
			OvenHeater oh  = pcs[i]; 	//komadic grejaca
			Transform ep;

			 //END POSITION - pozicija na koju treba da se pozicionira kada se spoji - to je pocetna pozicija komadica pre njegovog pomeranja
			ep = (Transform) GameObject.Instantiate(EndPosPref);
			ep.name = "ep" + oh.name;
			ep.parent = EndPosPref.parent;
		 
			EndPos[i] = ep;

			//START POSITION - pozicija na koju treba da se pozicionira na traci
			Transform sp;
			sp = (Transform) GameObject.Instantiate(StartPosPref);
			sp.name = "sp" + oh.name;

			sp.parent = StartPosPref.parent;

			//DODELJIVANJE POZICIJA NA TRACI
			sp.position =   StartPosTmp[i] ; 
			StartPos.Add(sp);

			//pozicija na koju treba da se namesti kada se spoji
			oh.ConnectedPosition = oh.transform.position;
			oh.ID = i+1;


			 EndPos[i].position = oh.ConnectedPosition;
			oh.triggerName = EndPos[i].name;
			
			oh.bActive = true;
			oh.Holder = StartPos[i];
//			oh.HolderPosition = StartPos[i].position;
			
			
			oh.transform.position = StartPos[i].position;
			//KOMADIC SE SKALIRA SE DA BI SE POSTAIO NA TRAKU
			oh.transform.localScale = 0.37f* startPCSScale;

			HelpList.Add(oh);
		}

	  

		if(DataManager.Instance.TutorialMiniGame2 >0) 
		{
			TimeToShowHelp = 6;
			InvokeRepeating ("TestHelpNeeded",4f,0.5f); 	 
		}
		else 
		{
//			bTutorial = true;
			TimeToShowHelp = 1;
			InvokeRepeating ("TestHelpNeeded",0.5f,0.5f); 	 
		}
		
		CurrentTimeToShowHelp = 0;
		HandPointer.transform.position  = Vector3.zero;

		StartCoroutine("StartGame");

		menuManager.EscapeButonFunctionStack.Push("btnPauseClicked");
	}
 


	 public void Connected(OvenHeater oh)
	{
		ovenHetarPartClilcked=null;
		PartsLeftToConnect--;
		HelpList.Remove(oh);
		HidePointer();
		 
		if(PartsLeftToConnect==0) 
			StartCoroutine(ShowEndMenu());
		else
		{
			bShowHelp = false;
			CurrentTimeToShowHelp = 0;

		}
	}

	
	void Update () {
		//prilikom izbacivanja komadica sa trake pozicije preostalih se koriguju
		if(OvenHeater.timeMove<2f ) OvenHeater.timeMove +=Time.deltaTime*3;
		else 	OvenHeater.bAdjustPosition = false;
	}
	
	public void AdjustSliderPositions(Transform remove)
	{
		//prilikom izbacivanja komadica sa trake pozicije preostalih se koriguju
		StartPos.Remove(remove);
		int pom = 0;
		for( int i =0; i< StartPos.Count; i++)
		{
			if(StartPos[i].position.y<remove.position.y)
			{
				StartPos[i].position += new Vector3(  0,  yDist*.8f ,0);
				pom++;
			}
		}

		PCS_slider.maxY =   ((StartPos.Count +1.5f)* yDist) *.8f   -  Camera.main.orthographicSize *2   ;//odredjivanje granica pomeranja slajdera
		  
		PCS_slider.CorrectPosition();
		//	Debug.Log("POMERENO:"+pom);
 
		GameObject.Destroy(remove.gameObject);
	}



 
	
	//************************************
	//buttons
	//************************************
	
	
	public void btnStartClick()
	{
		//menuManager.ClosePopUpMenu (PopUpTask);
		StartCoroutine("StartGame");
	}
	
	
	
	public void btnHomeClicked()
	{
		DataManager.Instance.SelectLevel(   DataManager.Instance.LastLevel );
		OvenHeater.bEnableMove = false;
		SoundManager.Instance.Play_ButtonClick();
	 
		StartCoroutine(WaitToLoadLevel("MainScene",1));
        AdsManager.Instance.ShowInterstitial();
	}
	
	public void btnReplyClicked()
	{
		OvenHeater.bEnableMove = false;
		SoundManager.Instance.Play_ButtonClick();
	 
		//StartCoroutine(WaitToLoadLevel("MiniGame2",1));
		StartCoroutine(WaitToLoadLevel("CustomerScene",1));
        AdsManager.Instance.ShowInterstitial();
	}
	
	
	
	public void btnPauseClicked()
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(1,false));

		OvenHeater.bEnableMove = false;
		SoundManager.Instance.Play_ButtonClick();
		if(bStartGame)
		{
		 
			menuManager.ShowPopUpMenu (PopUpPause);
			bPause = true;
		}
		
	}
	
	public void btnPlayClicked()
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(1,false));

		OvenHeater.bEnableMove = true;
		SoundManager.Instance.Play_ButtonClick();
	 
		
		menuManager.ClosePopUpMenu (PopUpPause);
		bPause = false;
	}
	
	
	public void btnLoadNextClicked()
	{
		BlockAll.blocksRaycasts = true;
        StartCoroutine(WaitToLoadLevel("Gameplay3",1));
	}
	
	
	//**************************************************
	
	
	IEnumerator WaitToLoadLevel(string leveName, float timeW = 0)
	{
		yield return new WaitForSeconds(timeW);
		//Application.LoadLevel( leveName );
		LevelTransition.Instance.HideScene(leveName);
	}
	
	
	
	public void btnFinishClicked()
	{
		StartCoroutine(ShowEndMenu());
	}
	
	IEnumerator ShowEndMenu()
	{
		CancelInvoke("TestHelpNeeded");
		HidePointer();

		yield return new WaitForSeconds(1.5f);

		DataManager.Instance.TutorialMiniGame2 = 1;
		DataManager.Instance.SaveTutorialProgress();

		DataManager.Instance.PreostaliBrojUpotrebaPeci = 4;
		DataManager.Instance.PokvarenaPec = 1;
		DataManager.Instance.SetGameplay3Data();

		OvenHeater.bEnableMove = false;
		
		yield return new WaitForSeconds(.5f);
		btnLoadNextClicked();
	}
	
	IEnumerator StartGame()
	{
		yield return new WaitForSeconds(0.2f);
		bStartGame = true;
		OvenHeater. bEnableMove = true;
	}

	//tutorila*******************************

	bool bPause = false;
	void TestHelpNeeded()
	{
		if(bStartGame)
		{
			//Debug.Log(bPause + " Vreme do  pomoci: "+ CurrentTimeToShowHelp);

			if(CurrentTimeToShowHelp ==0) bShowHelp = false;
			if(!bPause)  CurrentTimeToShowHelp+=0.5f;
			
			if(TimeToShowHelp  == CurrentTimeToShowHelp)
			{
				if(ovenHetarPartClilcked !=null)
				{
					bShowHelp = true;
					MoveCursorToEndPosition(ovenHetarPartClilcked);
				}
				else
				{
					bShowHelp = true;
					ShowStartPosition();

				}
				//Debug.Log("ShowHelp");
			}
		}
	}

	public void  ShowStartPosition()
	{
		bool bOffScreen = true;
		if(HelpList.Count>0)
		{
			int i =0;
			while(bOffScreen)
			{
				i = Mathf.FloorToInt(Random.Range(0,HelpList.Count));
				if(Mathf.Abs (HelpList[i].transform.position.y) <4)  bOffScreen = false;
			}
			HandPointer.transform.position = HelpList[i].transform.position + CursorPosOffset;
			ShowPointer();
			selectedHelp = i;
		}
	}

	int selectedHelp = 0;
	public void  CorectStartPosition()
	{
		if( HandPointer.activeSelf)
		{
			bool bOffScreen =  (Mathf.Abs (HelpList[selectedHelp].transform.position.y) >4);
			if(bOffScreen)
			{
				if(HelpList.Count>0)
				{
					int i =0;
					while(bOffScreen)
					{
						i = Mathf.FloorToInt(Random.Range(0,HelpList.Count));
						if(Mathf.Abs (HelpList[i].transform.position.y) <4)  bOffScreen = false;
					}
					HandPointer.transform.position = HelpList[i].transform.position + CursorPosOffset;
					selectedHelp = i;
				}
			}
			else
			{
				HandPointer.transform.position = HelpList[selectedHelp].transform.position + CursorPosOffset;
			}

		}
	}

	public void PartClicked(OvenHeater oh)
	{
		 
		SoundManager.Instance.Play_ButtonClick();
		if(bShowHelp) 
		{
			MoveCursorToEndPosition(  oh);
		}
		else 
		{
			bShowHelp = false;
			CurrentTimeToShowHelp = 0;
			ovenHetarPartClilcked = oh;
		}
	}

	public void MoveCursorToEndPosition(OvenHeater oh)
	{
		if(ovenHetarPartClilcked!=null)
		{
			HandPointer.transform.position = oh.transform.position + CursorPosOffset;
			ShowPointer();
			ovenHetarPartClilcked = null;
			EndCursorPos = oh.ConnectedPosition + CursorPosOffset;
			iTween.MoveTo( HandPointer     ,   iTween.Hash("position",   EndCursorPos,   "time", 1f,     "delay", .5f ,    "oncomplete","PointerCompleteMoving" ,     "oncompletetarget",gameObject));
		}
		else
		{
			ovenHetarPartClilcked = null;
			EndCursorPos = oh.ConnectedPosition + CursorPosOffset;
			iTween.MoveTo( HandPointer     ,   iTween.Hash("position",   EndCursorPos,   "time", 1f,     "delay", 0 ,    "oncomplete","PointerCompleteMoving" ,     "oncompletetarget",gameObject));
		}
	}

	public void PointerCompleteMoving()
	{
		//if(!bTutorial) 
		//{
//			bShowHelp = false;
//			CurrentTimeToShowHelp = 0;
//			//StartCoroutine(WaitToHidePointer(1));
//			HidePointer();
		//}
	}



	
	IEnumerator WaitToHidePointer(float timeW)
	{
		yield return new WaitForSeconds(timeW);
		HidePointer();
	}
	
	public void HidePointer()
	{
		CurrentTimeToShowHelp = 0;
		iTween.FadeTo(HandPointer,0,0.3f);
	}
	
	public void ShowPointer()
	{
		HandPointer.SetActive(true);
		iTween.FadeTo(HandPointer,1,0.3f);
	}


	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		
	}

}
