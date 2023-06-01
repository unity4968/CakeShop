using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//SKRIPTA JE ZAKACENA NA KEMRI (SCENA: MapScene)
//KORISTI SE ZA POMERANJE KAMIONCICA PO EKRANU, POMERANJE POZADINE I IZBOR NIVOA

public class MapScene1 : MonoBehaviour {
	 

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
 

	//Animator TruckAnimator;
	public Transform Truck;
	Vector3 LastPos;
	Vector3 LastPos2;
 
 
	public Text[] txtCoins;
 
	StatusBar statusBar;
	//*****************************************************************

	void Awake () {
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
	 
	}



	void Start () {
		//LevelTransition.Instance.ShowScene ( );


		mapScrollRect.horizontalNormalizedPosition = 1;
		LastMap.transform.position =  Vector3.zero;
		mapScrollRect.horizontalNormalizedPosition = 0;
		FirstMap.transform.position =  Vector3.zero;
 
		//TruckAnimator = Truck.FindChild ("AnimationHolder").GetComponent<Animator>();
 

		SetSelectLevelButtons();
		InvokeRepeating("TestEndOfMap",2,0.5f);
 
		SetMapStart("btnLevelSelect_"+ SelectedLevel.ToString().PadLeft(2,'0'));

		statusBar = GameObject.Find("StatusBar").GetComponent<StatusBar>();
		statusBar.Init( DataManager.Instance.LastLevel, 0, 0, 0, Shop.Coins);

		if(	!SoundManager.Instance.menuMusic.isPlaying)
		{
			SoundManager.Instance.Stop_GameplayMusic();
			SoundManager.Instance.Play_MenuMusic();
		}
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
			
			Truck.position = LevelsPositions[SelectedLevel-1].position;
			
			if(!bFirst)
			{
				bEnabelMapClick = false;
				
			}
			else bFirst = false;
		}
		
		float diffX = LastMap.transform.position.x - FirstMap.transform.position.x;
		float diffX2   = LevelsPositions[SelectedLevel-1].position.x - FirstMap.transform.position.x;
		
		scrollPosition = diffX2/diffX;
		if(scrollPosition>1) scrollPosition =1;
		if(scrollPosition<0) scrollPosition =0;
		
		
		mapScrollRect.horizontalNormalizedPosition =  scrollPosition;
		prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;
		
		
	}




	void Update () {
		TestHideMaps();
//		if(bMoveTruck) MoveTruck();
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
			if((prevScrollPosition+0.001f) < mapScrollRect.horizontalNormalizedPosition) break; 

			mapScrollRect.horizontalNormalizedPosition -=0.001f;
			prevScrollPosition = mapScrollRect.horizontalNormalizedPosition;
			yield return new WaitForEndOfFrame();
		}
		EndOfMapInterval = 0;
	}
	//*****************************************************************


	//*****************************************************************
	//AKTIVIRANJE DUGMICA ZA IZBOR NIVOA
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
					btn.interactable=false;
				}
			}
		}
	 
 
	}


 

	//*****************************************************
	
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
	 
		Truck.position = LevelsPositions[SelectedLevel-1].position;

 
		
		if( SelectedLevel == 1 &&  PrevSelectedLevel == 1)	
		{
			DataManager.Instance.SelectLevel(1);
			// Application.LoadLevel("CustomerScene");
			LevelTransition.Instance.HideScene("CustomerScene");
		}
		
		 StartCoroutine( WaitToLoadLevel( ));
		SoundManager.Instance.Play_ButtonClick();
	}
	
	

	IEnumerator WaitToLoadLevel( )
	{

		yield return new WaitForSeconds(1.5f);
		LevelTransition.Instance.HideScene("CustomerScene");

	}





	//BUTTONS **********************************************
 
	public void btnHomeClicked()
	{
		LevelTransition.Instance.HideScene("MainScene");
 
		SoundManager.Instance.Play_ButtonClick();
	}


	//*********************************************
	public CanvasGroup BlockAll;
	public GameObject popUpShop;
	public MenuManager menuManager;
	
	public void ClosePopUpShop()
	{
		BlockAll.blocksRaycasts = true;
		menuManager.ClosePopUpMenu (popUpShop);
		StartCoroutine(SetBlockAll(1.5f,false));
		SoundManager.Instance.Play_ButtonClick();
	}
	
	
	
	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		
	}

 
 
}
