using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Gameplay3 : MonoBehaviour {

	public Transform Cake;

	public Transform[] handPointerPositions;
	public Transform OvenPointerPosition;

	public Animator animHandPointer;
	public GameObject  HandPointer;
	
	public bool bEnableMove = false;
	public bool bEnableCake = false;
	 

	public MenuManager menuManager;
	

	 
	public GameObject PopUpPause;
	public GameObject PopUpLostGame;
	public GameObject PopUpOvenRepair;

	public SpriteRenderer OvenDoorOpened;
	public SpriteRenderer OvenDoorClosed;
	public SpriteRenderer BtnOn;
	public SpriteRenderer BtnOff;
	public ParticleSystem psSmoke;

	public RectTransform Indikator;

	public Sprite CakeBaked;
	public Sprite CakeBurned;

	 
	 
	bool bStartGame = false;
	float TimeToShowHelp = 10;
	float CurrentTimeToShowHelp = 0;
	bool bShowHelp = false;

	public int NextPositionNo = 0;
	
	float bakingTime = 0;
	public Tepsija tepsija;
	StatusBar statusBar;

	public Text txtOvenPrice;
	public Text[] txtCoins;

	string PrethodniMeni = "";
	public ShopManager shopManager;


	bool bPause = false;
	public Text txtGameOver;

	public Button[] WatchVideoButtons;

	void Awake () {
	try{
			DataManager.InitDataManager();
		//DataManager.Instance.SelectLevel(1);
		DataManager.Instance.GetTutorialProgress();

		DataManager.Instance.PopulateGameplay3Data();

		DataManager.Instance.GetPriceData();
		Shop.Instance.txtDispalyCoins = txtCoins;
		 
		if(   DataManager.Instance.PokvarenaPec != 2  && 
			   (DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.Oven || DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.BowlANdOven || DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.SpoonAndOven)
			&& DataManager.Instance.LastOvenRepairPriceIncreaseLevel < DataManager.Instance.SelectedLevel)  
		{
			int price = Mathf.CeilToInt( DataManager.Instance.OvenRepairPrice *   (1+ DataManager.Instance.SelectedLevelData.PercentPriceIncrease/100f));
			DataManager.Instance.OvenRepairPrice  = price;
			DataManager.Instance.LastOvenRepairPriceIncreaseLevel = DataManager.Instance.SelectedLevel;
			DataManager.Instance.SavePriceData();
			 PlayerPrefs.Save();



			 
		}
		 
		}catch{}
	}

	void Start () {
	 
		menuManager.EscapeButonFunctionStack.Push("btnPauseClicked");

		Input.multiTouchEnabled = false;
		psSmoke.Stop();
		psSmoke.GetComponent<Renderer>().sortingOrder = 7;

		StartCoroutine("StartGame");
		//d
		OvenDoorOpened.enabled = false;
		OvenDoorClosed.enabled = true;

 
		 if(DataManager.Instance.Tutorial >=3)  {
			InvokeRepeating ("TestHelpNeeded",4f,0.5f);
			HidePointer();
		}

		BtnOn.enabled = false;
		BtnOff.enabled = true;


		statusBar = GameObject.Find("StatusBar").GetComponent<StatusBar>();
		statusBar.Init(DataManager.Instance.SelectedLevel,DataManager.Instance.SelectedLevelCoinsEarned, DataManager.Instance.SelectedLevelCustomersServed, 
		               DataManager.Instance.SelectedLevelData.CustomersPerLevel, Shop.Coins);
		statusBar.SetPosition();
		//BRISI
		//Shop.Coins = 0;

		if(	SoundManager.Instance.menuMusic.isPlaying)
		{
			SoundManager.Instance.Stop_MenuMusic();
			SoundManager.Instance.Play_GameplayMusic();
		}

		SetWatchVideoButtons();



		GlobalVariables.OnPauseGame +=FLPauseGame;
 
	}
	
 //*******************************************************************
	public void GoalPointer()
	{
		OpenOven();
		if(DataManager.Instance.Tutorial >=3 && !bShowHelp) 
			StartCoroutine(WaitToHidePointer (2.5f));
		iTween.MoveTo( HandPointer , OvenPointerPosition.position,1f);

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
		 if(DataManager.Instance.Tutorial<3 || bShowHelp) //ako nije prikazan treci tutorijal
		{
			HandPointer.SetActive(true);
			iTween.FadeTo(HandPointer,1,0.3f);

		}
	}
	
	public void NextPointer( )
	{
		if(DataManager.Instance.Tutorial<3 )
		{
			NextPositionNo++;
			iTween.MoveTo( HandPointer , handPointerPositions[1].position,1f);
		}
		else
		{
			NextPositionNo++;
			HidePointer();
			CurrentTimeToShowHelp = 0;
			//iTween.MoveTo( HandPointer , handPointerPositions[1].position,1f);
			iTween.MoveTo( HandPointer    ,   iTween.Hash("position",  handPointerPositions[1].position,   "time", .5f,     "delay", 3 ));
		}
	}

	public void HelpNextPointer( )
	{
		 
		ShowPointer();

		if(NextPositionNo == 0) 
		{
			if(tepsija.bDrag) GoalPointer();
			else iTween.MoveTo( HandPointer , handPointerPositions[0].position,1f);
		}
		//else if(NextPositionNo == 0) 
		//iTween.MoveTo( HandPointer , handPointerPositions[1].position,1f);
	}

	void Update()
	{
	  	if(NextPositionNo ==1  && bEnableMove)  
		{
			if(Input.GetMouseButtonDown(0))
			{
				Vector2 mousePosition   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Collider2D hitCollider  = Physics2D.OverlapPoint(mousePosition);
				 
				if(hitCollider!=null && hitCollider.transform.name == "btnOn" && !BtnOn.enabled)
				{
					if(!bPause) SoundManager.Instance.Play_Sound( SoundManager.Instance.OnOffSound);
					if( TestOven() )
					{
					 
						BtnOff.enabled = false;
						BtnOn.enabled= true;
						NextPositionNo++;
						StartCoroutine(TimeHelp());
						SoundManager.Instance.Play_Sound( SoundManager.Instance.Timer);
					}
				}
			}
		}

		else if(NextPositionNo <=3 &&   bakingTime>0.4f   && bEnableMove)  
		{
			if(Input.GetMouseButton (0))
			{
				Vector2 mousePosition   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				Collider2D hitCollider  = Physics2D.OverlapPoint(mousePosition);

				if(hitCollider!=null && hitCollider.transform.name == "btnOn")
				{
					if(!bPause) SoundManager.Instance.Play_Sound( SoundManager.Instance.OnOffSound);
					BtnOff.enabled = true;
					BtnOn.enabled= false;
					 NextPositionNo++;
					if(bakingTime>2.6f && bakingTime <7.9f)
					{
						StartCoroutine("WinGame");
					}
					else
					{
						StartCoroutine("LostGame");
					}

					SoundManager.Instance.Stop_Sound( SoundManager.Instance.Timer);
				}
				
			}
		}

		if(NextPositionNo ==2 || NextPositionNo ==3 && bEnableMove)  
		{

			if(!bPause)
			{ 
				bakingTime+=Time.deltaTime;
				SoundManager.Instance.Play_Sound(SoundManager.Instance.Timer);
				 // if(!SoundManager.Instance.Timer.isPlaying)   SoundManager.Instance.Timer.Play();
			}
			else
			{
				if(SoundManager.Instance.Timer.isPlaying)   SoundManager.Instance.Timer.Pause();
			}
			Indikator.anchoredPosition = new Vector2(0,-270 + (270+285)*bakingTime/10f);
			if(bakingTime>9.9f)
			{
				StartCoroutine("LostGame");
				NextPositionNo = 4;
			}
		}

		if(bakingTime>7.9f)
		{
			if(!psSmoke.isPlaying) psSmoke.Play();
		}
	}

 
	public void CloseOven()
	{
		OvenDoorOpened.enabled = false;
		OvenDoorClosed.enabled = true;
	}

	public void OpenOven()
	{
		OvenDoorOpened.enabled = true;
		OvenDoorClosed.enabled = false;
	}


	 
	public bool TestOven()
	{
		if(bStartGame && !bPause )
		{
			if( (DataManager.Instance.PokvarenaPec == 0  &&  NextPositionNo ==0 )|| //prilikom starta
			   (     DataManager.Instance.PreostaliBrojUpotrebaPeci == 0)   ) //u toku igre
			{
				if(  DataManager.Instance.PokvarenaPec ==1) 
				{
					DataManager.Instance.PokvarenaPec = 0;
					DataManager.Instance.SetGameplay3Data();
				}
				 
				bEnableCake = false;

				txtOvenPrice.text = DataManager.Instance.OvenRepairPrice.ToString();

				PodesiNovcice();
				BlockAll.blocksRaycasts = true;
				StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
				menuManager.ShowPopUpMenu (PopUpOvenRepair);
				menuManager.EscapeButonFunctionStack.Push("ClosePopUpOwen");
				bPause = true;



			}
			else return true; //NIJE POKVARENA
		}
		return false;
		
	}
	void PodesiNovcice()
	{
		for(int i = 0; i<txtCoins.Length;i++)
		{
			txtCoins[i].text =   Shop.Coins.ToString();
		}
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
		BlockAll.blocksRaycasts = true;
		StartCoroutine(WaitToLoadLevel("MainScene",1));
		SoundManager.Instance.Play_ButtonClick();
        AdsManager.Instance.ShowInterstitial();
	}
	
	public void btnReplyClicked()
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(WaitToLoadLevel("CustomerScene",1));
		SoundManager.Instance.Play_ButtonClick();
        AdsManager.Instance.ShowInterstitial();
	}
	
	
	
	public void btnPauseClicked()
	{
 
		if(bStartGame)
		{
			BlockAll.blocksRaycasts = true;
			StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));

			bPause = true;
		 
			bEnableCake = false;
			menuManager.ShowPopUpMenu (PopUpPause);
			SoundManager.Instance.Play_ButtonClick();
		}
	}
	
	public void btnPlayClicked()
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(1f,false));
		StartCoroutine(SetPause(1f,false));
 
		bEnableCake = true;
		menuManager.ClosePopUpMenu (PopUpPause);
		SoundManager.Instance.Play_ButtonClick();
		 bPause = false;
	}
	
	
	public void btnLoadNextClicked()
	{
		SoundManager.Instance.Timer.Pause();
		SoundManager.Instance.Timer.Stop();
		BlockAll.blocksRaycasts = true;
		StartCoroutine(WaitToLoadLevel("Gameplay4",1));
	}
	

	public void btnRepairYesClicked()
	{
		SoundManager.Instance.Play_ButtonClick();
		if( DataManager.Instance.OvenRepairPrice  > Shop.Coins) 
		{
			Debug.Log("Nema dovoljno novca!");
			SoundManager.Instance.Play_Sound(SoundManager.Instance.NoMoney);
			PrethodniMeni = "Oven";
			menuManager.ClosePopUpMenu (PopUpOvenRepair);
			shopManager.ShowPopUpShop();
		 
			return;
		}
 
		Shop.Instance.txtDispalyCoins = txtCoins;
		Debug.Log( "CENA POPRAVKE " +DataManager.Instance.OvenRepairPrice);
		Shop.Instance.AnimiranjeDodavanjaNovcica(-DataManager.Instance.OvenRepairPrice, null,"");

		tepsija.ResetrPosition();
		NextPositionNo = 0;
		OpenOven();

		bEnableCake = true;
		
		DataManager.Instance.PokvarenaPec = 1;
		DataManager.Instance.PreostaliBrojUpotrebaPeci = 5;
		DataManager.Instance.SetGameplay3Data();

		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(1.5f,false));
		StartCoroutine(SetPause(1f,false));

		menuManager.ClosePopUpMenu (PopUpOvenRepair);
		if(NextPositionNo>0)
		{
			BtnOff.enabled = false;
			BtnOn.enabled= true;
			NextPositionNo++;
			StartCoroutine(TimeHelp(1));
		}
	}

	public void btnRepairNoClicked()
	{
		SoundManager.Instance.Play_ButtonClick();
		//BESPLATNA POPRAVKA - MINIGAME 
	 
		bEnableCake = false;
		int rlevel = Mathf.FloorToInt(Random.Range(1,3));
		BlockAll.blocksRaycasts = true;

		StartCoroutine(WaitToLoadLevel("MiniGame" + rlevel.ToString(),1));
	}

	public void btnRepairWatchVideoClicked()
	{
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;
		Shop.Instance.WatchVideo();
		StartCoroutine(SetBlockAll(2,false));
	}
	
	
	public void EndWatchingVideo( bool bFinishWatching)
	{
        Debug.Log("ODGLEDAN VIDEO");
        
        bEnableCake = true;
        
        DataManager.Instance.PokvarenaPec = 1;
        DataManager.Instance.PreostaliBrojUpotrebaPeci = 1;
        DataManager.Instance.SetGameplay3Data();
        
        
        tepsija.ResetrPosition();
        NextPositionNo = 0;
        OpenOven();
        
        BlockAll.blocksRaycasts = true;
        StartCoroutine(SetBlockAll(1.5f,false));
        
        menuManager.ClosePopUpMenu (PopUpOvenRepair);
         
		StartCoroutine(SetBlockAll(1,false));
	}

	//**************************************************
	
	
	IEnumerator WaitToLoadLevel(string leveName, float timeW = 0)
	{
		SoundManager.Instance.Timer.Stop();
		yield return new WaitForSeconds(timeW);
		SoundManager.Instance.Stop_Sound(SoundManager.Instance.Timer);
		LevelTransition.Instance.HideScene(leveName);
	}

	IEnumerator WinGame()
	{
		BlockAll.blocksRaycasts = true;
		 
		CancelInvoke("TestHelpNeeded");
		if(DataManager.Instance.Tutorial <3) 
		{
			DataManager.Instance.Tutorial = 3;
			DataManager.Instance.SaveTutorialProgress();
		}
		bEnableMove = false;
		HidePointer();
		OpenOven();
		Cake.GetComponent<SpriteRenderer>().sprite = CakeBaked;

		yield return new WaitForSeconds(1);
		 
		btnLoadNextClicked();
	}

	IEnumerator LostGame()
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(1.5f,false));

		CancelInvoke("TestHelpNeeded");

		bEnableMove = false;
		HidePointer();
		OpenOven();
		if(bakingTime>7.8f) 
		{
			Cake.GetComponent<SpriteRenderer>().sprite = CakeBurned;
			SoundManager.Instance.Play_Sound(SoundManager.Instance.Lose);
			txtGameOver.text = "Oh, no! You\nmust be faster!";
		}
		yield return new WaitForSeconds(1);
		menuManager.ShowPopUpMenu(PopUpLostGame);
		
	}
	
	
	IEnumerator StartGame()
	{
		yield return new WaitForSeconds(.1f);

		bEnableMove = true;
		bStartGame = true;
		bEnableCake = true;


		TestOven();
		ShowPointer();
		 
		DataManager.Instance.PreostaliBrojUpotrebaPeci--;
	 
		if( DataManager.Instance.PreostaliBrojUpotrebaPeci<0 ) DataManager.Instance.PreostaliBrojUpotrebaPeci = 0;
 
		DataManager.Instance.SetGameplay3Data();
	}
	
	
	IEnumerator ShowPopUp()
	{
		 
		bEnableMove = false;
		
		iTween.FadeUpdate(HandPointer,0,0);
		yield return new WaitForSeconds(.1f);
		btnStartClick();
	}

	IEnumerator TimeHelp(float twait = 0)
	{


		yield return new WaitForSeconds(twait);

		HandPointer.transform.localScale = new Vector3(-1,1,1);

		iTween.MoveTo( HandPointer , handPointerPositions[2].position,1f);
		yield return new WaitForSeconds(1);
		iTween.MoveTo( HandPointer , handPointerPositions[3].position,1f);
		yield return new WaitForSeconds(1);


		HandPointer.transform.localScale = new Vector3(1,1,1);
		NextPositionNo++;
		iTween.MoveTo( HandPointer , handPointerPositions[1].position,1f);
		if(DataManager.Instance.Tutorial >=3 && !bShowHelp) StartCoroutine(WaitToHidePointer (1.2f));
	}


 
	//AKO JE IGRAC ZABORAVIO STA TREBA DA URADI POKERECE SE KURSOR KOJI POKAZUJE POMOC
	void TestHelpNeeded()
	{
		if(bStartGame)
		{
//			 Debug.Log(bPause + " Vreme do  pomoci: "+ CurrentTimeToShowHelp);
			 if(CurrentTimeToShowHelp ==0) bShowHelp = false;
			if(!bPause && !ShopManager.bShowShop && !MenuManager.bPopUpVisible ) CurrentTimeToShowHelp+=0.5f;
			
			if(TimeToShowHelp  == CurrentTimeToShowHelp)
			{
 		//		 Debug.Log("ShowHelp");
				bShowHelp = true;
				 
				HelpNextPointer();
			}
		}
	}

 
	//*********************************************
	public CanvasGroup BlockAll;
	public GameObject popUpShop;
	
	public void ClosePopUpShop()
	{
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;
		menuManager.ClosePopUpMenu (popUpShop);
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));

		if(PrethodniMeni == "Oven")
		{
			menuManager.ShowPopUpMenu (PopUpOvenRepair);
		}
		else if(!PopUpPause.activeSelf)
		{
			StartCoroutine(SetPause(1f,false));
		}

		PrethodniMeni = "";
		ShopManager.bShowShop = false;
	}

	public void ClosePopUpOwen()
	{
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;
		menuManager.ClosePopUpMenu (PopUpOvenRepair);
		StartCoroutine(SetBlockAll(2.5f,false));
		PrethodniMeni = "";

		if(NextPositionNo == 0) 
		{
			bEnableCake = true;
		}
		StartCoroutine(SetPause(1.5f,false));
		CurrentTimeToShowHelp = 0;
		bPause = false;
	}
	 
	
	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		
	}

	IEnumerator SetPause(float time, bool _bPause)
	{
		yield return new WaitForSeconds(time);
		bPause = _bPause;
		bEnableCake = !_bPause;
	}

	public void SetPauseOn( )
	{
		bEnableCake = false;
		bPause = true;
		
	}

	public void SetWatchVideoButtons()
	{
		for(int i = 0;i<WatchVideoButtons.Length;i++)
		{
			if(AdsManager.Instance.bVideoRewardReady)
			{
				WatchVideoButtons[i].interactable = true;
				WatchVideoButtons[i].GetComponent<Image>().color = new Color(1,1,1,1f);
			}
			else
			{
				WatchVideoButtons[i].interactable = false;
				WatchVideoButtons[i].GetComponent<Image>().color = new Color(1,1,1,0.5f);
			}
		}
	}

	public void btnShowHideBanner()
	{
		GameObject.Find("StatusBar").GetComponent<StatusBar>().SetPosition();
	}


	void FLPauseGame()
	{
 
		if(bStartGame)
		{
			BlockAll.blocksRaycasts = true;
			StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));

			bPause = true;

			bEnableCake = false;
			menuManager.ShowPopUpMenu (PopUpPause);
			 
		}

 
	}

	void OnDestroy()
	{
		//Debug.Log("ClearAllPauseEvents"); 
		GlobalVariables.ClearAllPauseEvents();
	}

	 

}
