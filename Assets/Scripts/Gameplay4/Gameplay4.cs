using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Gameplay4 : MonoBehaviour {

	public GameObject btnMenuItem;
	public RectTransform ScrollContent;
	public RectTransform HiddenButtons;
	public ScrollRect scrollRect;
	MenuItems menuItems;
	Dictionary <int,MenuItemData> menuItemsDict = new Dictionary<int, MenuItemData>();
	public Sprite[] ItemiSprites ;


	DecorationType CakeBaseShape = DecorationType.Empty; //oblik baze kolaca - utice na oblik krema za fill koji ce da se pokaze

	public SpriteRenderer  CakeBase;
	public SpriteRenderer  CakeFill;
	public Vector3 BaseCorrection;
	Vector3 BaseStartPos;
	Vector3 FillStartPos;

	public DecorationTransform decTransform;
	 

	List<Transform> itemButtons = new List<Transform>();
	List<Transform> decorationsList = new List<Transform>();

	int btnMenuCount =0;


	int SelectedMenu = 0;
	string SelectedItem = "";
	string SelectedItemToBuy = "";
	 

	int IDcounter =0;

	public Image imgBuyDecoration;
	public Text[] txtCoins;

	public Text txtDecorationPrice;

	public Transform btnDelete;
	public GameObject btnDeletePos;

	TutorialGP4 tutorialGP4;


	public MenuManager menuManager;
	
 
	 
	public GameObject PopUpPause;
	public GameObject PopUpBuyDecoration;

	float tsc = 1;
	bool bEnableMove = false;
	bool bStartGame = false;
	StatusBar statusBar;

    public static bool bTutorial = true;

	string PrethodniMeni = "";
	public ShopManager shopManager;

	public Button[] WatchVideoButtons;

	void Awake () {
//		AdsManager.Instance.HideBanner();
		//DataManager.InitDataManager();
		//DataManager.Instance.SelectLevel(1);
		//DataManager.Instance.GetTutorialProgress();
		//DataManager.Instance.PopulateGameplay4Data();

		Shop.InitShop();
		// Text txtCoins = GameObject.Find("txtCoins").GetComponent<Text>();
		//Text txtCoins1 = GameObject.Find("txtCoins1").GetComponent<Text>();
		//Text txtCoins2 = GameObject.Find("txtCoins2").GetComponent<Text>();
		
		Shop.Instance.txtDispalyCoins = txtCoins;
	}



	void Start () {

		//BRISI
		//Shop.Coins = 0;

		 if(DataManager.Instance.Tutorial <4)
		{
			Debug.Log("TUTORIAL");
			//tutorial
			bTutorial = true;
			DataManager.Instance.CustomerOrder = "m1_01;m2_02;m4_04";
			TutorialGP4.TutorialPhase =0;

		}
		else
		{
			bTutorial = false;
			transform.GetComponent<TutorialGP4>().enabled = false;
			TutorialGP4.TutorialPhase =0;
		}

		Input.multiTouchEnabled = true;
		StartCoroutine("ShowPopUp");
		menuItems = new MenuItems();

		CakeBase.sprite = null;
		CakeFill.sprite = null;
	 

		BaseStartPos = CakeBase.transform.position;
		FillStartPos = CakeFill.transform.position;


		//btnDelete.position = new Vector3(btnDeletePos.transform.position.x,btnDeletePos.transform.position.y,-1);

		statusBar = GameObject.Find("StatusBar").GetComponent<StatusBar>();
		statusBar.Init(DataManager.Instance.SelectedLevel,DataManager.Instance.SelectedLevelCoinsEarned, DataManager.Instance.SelectedLevelCustomersServed, 
		               DataManager.Instance.SelectedLevelData.CustomersPerLevel, Shop.Coins);

		if(	SoundManager.Instance.menuMusic.isPlaying)
		{
			SoundManager.Instance.Stop_MenuMusic();
			SoundManager.Instance.Play_GameplayMusic();
		}

		SetWatchVideoButtons();
		menuManager.EscapeButonFunctionStack.Push("btnPauseClicked");
	}
	
	 
	void Update () {

		btnDelete.position = new Vector3(btnDeletePos.transform.position.x,btnDeletePos.transform.position.y,-1);
		if(bEnableMove)
		{


		//	if(SelectedMenu!=2)
		//	{
				if(Input.GetMouseButton(0)  && !decTransform.bActivated)
				{
					Vector2 mousePosition   = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Collider2D hitCollider  = Physics2D.OverlapPoint(mousePosition);

					if(hitCollider!=null && hitCollider.transform.tag == "Decoration")
					{
			//			Debug.Log(hitCollider.transform.name);
						decTransform.bActivated = true;
						decTransform.ActiveDecoration = hitCollider.transform;

						decTransform.transform.position =  hitCollider.transform.position;
					}
				}
//			}
//			else
//			{
//				decTransform.bActivated = false;
//			}
		}
	}

	//kada se odabere neka dekoracija iz menija
	public void ClickItem(string item)
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(0.5f,false));

		SoundManager.Instance.Play_ButtonClick();
		if(bTutorial  &&  ((TutorialGP4.TutorialPhase ==4 && item == "m1_01") ||  ( TutorialGP4.TutorialPhase ==8 && item == "m2_02")  || (TutorialGP4.TutorialPhase ==12 && item == "m4_04")  )       )
		{
			Camera.main.SendMessage("NextPhase");
			 
		}	
		else if(bTutorial   ) return;

		SelectedItem = item;
		if(temporaryUnlockedDecorations.Contains(item + ";"))
		{
			temporaryUnlockedDecorations = temporaryUnlockedDecorations.Replace(item + ";", "");

			//zamena slike i eventa
		 
			Transform btn = GameObject.Find(SelectedItem).transform;
			Button btnC = btn.GetComponent<Button>();
			btn.Find("ImageLock").GetComponent<Image>().enabled =true;			 
			btn.GetComponent<Image>().color = new Color(1,1,1,1);
			AddEvtListenerBuyDecoration(  btnC,  SelectedItem);
		}

		if(SelectedMenu == 1)
		{
				CakeBase.sprite =   GameObject.Find(item).GetComponent<Image>().sprite;
				CakeBaseShape =  MenuItems.menuItemsDictionary[item].decorationType;
 
				//TODO:PROVERA DA LI DA OBRISE SVE
				DeleteAllDecoration();

			//korekcija pozicija
			if(CakeBaseShape == DecorationType.CakeShapeCylinder)
			{
				CakeBase.transform.position = BaseStartPos;
				CakeFill.transform.position = FillStartPos;
			}
			else
			{
				CakeBase.transform.position = BaseStartPos + BaseCorrection;
				CakeFill.transform.position = FillStartPos + BaseCorrection;
			}
 
		}

		else if(SelectedMenu ==2 && CakeBaseShape != DecorationType.Empty)
		{
					CakeFill.sprite = GameObject.Find(item).GetComponent<Image>().sprite;
		}


		else if(SelectedMenu >=3 && CakeBaseShape != DecorationType.Empty)
		{
			CreateDecoration();
		}
		 
	}

	void PodesiNovcice()
	{
		for(int i = 0; i<txtCoins.Length;i++)
		{
			txtCoins[i].text =   Shop.Coins.ToString();
		}
	}
	
	public void ShowBuyDecorationMenu(string name)
	{
		if(bTutorial) return;

		SoundManager.Instance.Play_Sound(SoundManager.Instance.LockedItem);

		txtDecorationPrice.text  = DataManager.Instance.UnlockDecorationPrice.ToString();
		PodesiNovcice();


		Debug.Log("BUY "+name);

		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
		menuManager.ShowPopUpMenu (PopUpBuyDecoration);
		menuManager.EscapeButonFunctionStack.Push("btnExitBuyDecorationMenu");

		SelectedItemToBuy = name;
		imgBuyDecoration.sprite = GameObject.Find(SelectedItemToBuy).GetComponent<Image>().sprite;

	}



	//******************************************
	//izbor menija 
	public void ClickMenuButton(int _SelectedMenu)
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(0.5f,false));

		if(bTutorial  &&  (  (TutorialGP4.TutorialPhase ==6   && _SelectedMenu == 2   ) ||   (TutorialGP4.TutorialPhase ==10    && _SelectedMenu == 4  )  ) )
		{

			Camera.main.SendMessage("NextPhase");
 
		}	
		else if(bTutorial   ) return;
		SoundManager.Instance.Play_ButtonClick();
		StartCoroutine( PodesiMeni(_SelectedMenu));
	}

	IEnumerator PodesiMeni(int _SelectedMenu)
	{
		SelectedMenu = _SelectedMenu;
 
 
		//animacija skuplajnaja dugmica
		for(int i = 6; i>=0;i--)
		{
 
			yield return new WaitForEndOfFrame();
		}

 
		yield return new WaitForEndOfFrame();
 
		foreach(Transform btn in itemButtons)
		{
			btn.SetParent(HiddenButtons.transform);
			btn.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		}



		//ucitavanje liste dugmica
		menuItemsDict.Clear();
		//Debug.Log(SelectedMenu+ "  :  " +CakeBaseShape);
		menuItemsDict = menuItems.ReturmMenu(SelectedMenu,CakeBaseShape);
		string atlas = "";
		string tmp =  "m" + SelectedMenu.ToString() + "_";
		if(menuItemsDict.Count>0) atlas = menuItemsDict[1].Atlas;

		 
		ItemiSprites =(Sprite[]) Resources.LoadAll<Sprite>( "Decorations/"+atlas);

		btnMenuCount = menuItemsDict.Count;
		 
		ScrollContent.sizeDelta = new Vector2(110,120*btnMenuCount+10 );
		scrollRect.verticalNormalizedPosition = 1;

		//POPUNA ITEMA
		
		for(int i = 1; i<=menuItemsDict.Count;i++)
		{
			MenuItemData d = menuItemsDict[i];

		


			//ponovo se koristi neko dugme
			if(i<=itemButtons.Count)
			{
				bool bNadjeno = false;
				foreach( Sprite sp in  ItemiSprites)
				{
					if(bNadjeno) break;
					if(sp.name == d.Name)
					{
						Transform btn =itemButtons[i-1];
						btn.SetParent( ScrollContent.transform);
						 
						btn.GetComponent<Image>().sprite = sp;
						btn.GetComponent<RectTransform>().localScale = Vector3.one;
						btn.name =  d.Name;//  tmp + i.ToString().PadLeft(2,'0');      // "btn"+(i+1).ToString().PadLeft(2,'0');

						bNadjeno = true;
						Button btnC = 	btn.GetComponent<Button>();

						 
						if(d.Locked &&  !temporaryUnlockedDecorations.Contains(btn.name)) 
						{
							//btnC.onClick.RemoveAllListeners();
						 
							btn.Find("ImageLock").GetComponent<Image>().enabled =true;
							btn.GetComponent<Image>().color = new Color (1,1,1,1);
							AddEvtListenerBuyDecoration(btnC,btn.name);
						}
						else
						{
							btn.Find("ImageLock").GetComponent<Image>().enabled =false;
							AddEvtListenerClick(btnC,btn.name);
							btn.GetComponent<Image>().color = new Color (1,1,1,1);
						}




					}
				}
				if(!bNadjeno) Debug.Log ("GRESKA: "+ tmp + i.ToString().PadLeft(2,'0'));
			}
			else
			{
				bool bNadjeno = false;
				foreach( Sprite sp in  ItemiSprites)
				{	
					if(bNadjeno) break;
					if(sp.name.Trim() == d.Name.Trim())
					{
						//						Debug.Log("* " + sp.name + "  "+d.ButtonImgName);
						Transform btn = ((GameObject) GameObject.Instantiate(btnMenuItem)).transform;
						btn.SetParent( ScrollContent.transform,false);
						 
						btn.GetComponent<Image>().sprite = sp;
						btn.GetComponent<RectTransform>().localScale = Vector3.one;
						btn.name =   tmp + i.ToString().PadLeft(2,'0');  
						bNadjeno = true;

						Button btnC = 	btn.GetComponent<Button>();
					
						if(d.Locked &&  !temporaryUnlockedDecorations.Contains(btn.name))  
						{
							//btnC.onClick.RemoveAllListeners();
							 
							btn.Find("ImageLock").GetComponent<Image>().enabled =true;
							btn.GetComponent<Image>().color = new Color(1,1,1,1);
							AddEvtListenerBuyDecoration(btnC,btn.name);
							 
						}
						else
						{
							btn.Find("ImageLock").GetComponent<Image>().enabled =false;
							AddEvtListenerClick(btnC,btn.name);
							btn.GetComponent<Image>().color = new Color (1,1,1,1);
						}

						itemButtons.Add(btn);
					}
				}
				if(!bNadjeno) Debug.Log ("GRESKA: "+i);
			}
		}

//		 Vector2 pV2 = ScrollContent.sizeDelta ;
//		
//		for(int i = 1; i<=6;i++)
//		{
//			ScrollContent.sizeDelta =  new Vector2(pV2.x, pV2.y/6f*i);
//			yield return new WaitForEndOfFrame();
//		}
		 
	}

	 
	void AddEvtListenerClick(Button btnC,string name)
	{
		btnC.onClick.RemoveAllListeners();
		btnC.onClick.AddListener(() => ClickItem(name));  

//		btnC.GetComponent<DecorationMenuButton>().bDecorationUnlocked =  true;
	}

	void AddEvtListenerBuyDecoration(Button btnC,string name)
	{
		btnC.onClick.RemoveAllListeners();
		btnC.onClick.AddListener(() => ShowBuyDecorationMenu(name));  
//		btnC.GetComponent<DecorationMenuButton>().bDecorationUnlocked =  false;

	}




	public void CreateDecoration()
	{

	
		MenuItemData m = MenuItems.menuItemsDictionary[SelectedItem];
		Sprite sprite = GameObject.Find(SelectedItem).GetComponent<Image>().sprite;
		GameObject pref =  GameObject.Find(m.PrefabName);
		if(pref !=null)
		{
			//Transform item = (Transform)Instantiate(pref);
			Transform item = ((GameObject)Instantiate(pref)).transform;
			item.position = new Vector3(2.33f+ Random.Range(-3,3),Random.Range(-1,1),-2);
			
			item.name = IDcounter.ToString() +"*"+SelectedItem;
			IDcounter++;
			Debug.Log(SelectedItem);
				decTransform.bActivated = true;
				decTransform.ActiveDecoration =item;
			
			decTransform.transform.position =  item.position;

			decorationsList.Add(item);
			item.GetComponent<Renderer>().sortingOrder = decorationsList.Count;
	 
			item.GetComponent<SpriteRenderer>().sprite = sprite;
		}
		
		
	 
	}

	public void DeleteDecoration(Transform delTransform)
	{
		if(bTutorial) return;
		if(decorationsList.Contains(delTransform))
	   {
			//Debug.Log("DeleteDecoration");
			int sortingOrder = delTransform.GetComponent<Renderer>().sortingOrder;

			foreach(Transform dec in decorationsList)
			{
				if(dec.GetComponent<Renderer>().sortingOrder >sortingOrder) dec.GetComponent<Renderer>().sortingOrder--;
			}

			decorationsList.Remove(delTransform);
			GameObject.Destroy(delTransform.gameObject);
			SoundManager.Instance.Play_ButtonClick();
		}
	}

	public void DeleteAllDecoration( )
	{
		CakeFill.sprite = null;
		for (int i = (decorationsList.Count-1); i>=0;i--)
		{
			GameObject go = decorationsList[i].gameObject;
			decorationsList.RemoveAt(i);
			GameObject.Destroy(go);
		}
	}


	public void PodesiSortingOrder(Transform delTransform)
	{
		Debug.Log("PodesiSortingOrder");
		int sortingOrder = delTransform.GetComponent<Renderer>().sortingOrder;

		foreach(Transform dec in decorationsList)
		{
			if(dec.GetComponent<Renderer>().sortingOrder >sortingOrder) dec.GetComponent<Renderer>().sortingOrder--;
		}
		 
		delTransform.GetComponent<Renderer>().sortingOrder = decorationsList.Count;
	}

	
	//************************************
	//buttons
	//************************************


	public void btnBuyDecoration()
	{
		SoundManager.Instance.Play_ButtonClick();
	 	if(bTutorial   ) return;
		if(DataManager.Instance.UnlockDecorationPrice <= Shop.Coins )
	   {
			Transform btn = GameObject.Find(SelectedItemToBuy).transform;
			Button btnC = btn.GetComponent<Button>();
			btn.Find("ImageLock").GetComponent<Image>().enabled =false;
			AddEvtListenerClick(btnC,btn.name);
			btn.GetComponent<Image>().color = new Color (1,1,1,1);
			 
			MenuItems.menuItemsDictionary[SelectedItemToBuy].Locked = false;
			//snimanje pprefs
	//		Debug.Log("+ "+MenuItems.lockedMenuItems.Count);
			DataManager.Instance.SetUnlockedMenuItems(SelectedItemToBuy);
			MenuItems.lockedMenuItems.Remove(MenuItems.menuItemsDictionary[SelectedItemToBuy]);
	//		Debug.Log("- "+MenuItems.lockedMenuItems.Count);
			StartCoroutine("WaitBuyDecoration");
		}
		else
		{
			//nema dovoljno novca
			SoundManager.Instance.Play_Sound(SoundManager.Instance.NoMoney);
			PrethodniMeni = "Decoration";
			menuManager.ClosePopUpMenu (PopUpPause);
			shopManager.ShowPopUpShop();

			BlockAll.blocksRaycasts = true;
			StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
		}
	}

	IEnumerator WaitBuyDecoration()
	{
		//oduzimanje novca
		BlockAll.blocksRaycasts = true;

		//TODO: ODUZMI NOVAC
		Shop.Instance.txtDispalyCoins = txtCoins;
		Shop.Instance.AnimiranjeDodavanjaNovcica(-DataManager.Instance.UnlockDecorationPrice ,null, "");




		yield return new WaitForSeconds(0.5f);
		SoundManager.Instance.Play_Sound(SoundManager.Instance.UnlockNewItem);


		SelectedItemToBuy = "";

		yield return new WaitForSeconds(1.0f);
		menuManager.ClosePopUpMenu (PopUpBuyDecoration);
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
	}



	public void btnExitBuyDecorationMenu()
	{
		SelectedItemToBuy = "";
		menuManager.ClosePopUpMenu (PopUpBuyDecoration);
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
	 	
	} 

	string temporaryUnlockedDecorations = "";

	public void btnTemporaryUnlockDecoration()
	{
		StartCoroutine(SetBlockAll(2,false));
		Shop.Instance.WatchVideo();
	}

	public void EndWatchingVideo( bool bFinishWatching)
	{
		//if(bFinishWatching)
		//{
			temporaryUnlockedDecorations += SelectedItemToBuy+";";
			//Zamena slike na dugmetu
			Transform btn = GameObject.Find(SelectedItemToBuy).transform;
			Button btnC = btn.GetComponent<Button>();
			btn.Find("ImageLock").GetComponent<Image>().enabled =false;
			AddEvtListenerClick(btnC,btn.name);
			btn.GetComponent<Image>().color = new Color (1,1,1,1);

			BlockAll.blocksRaycasts = true;

			SoundManager.Instance.Play_Sound(SoundManager.Instance.UnlockNewItem);

			SelectedItemToBuy = "";

			menuManager.ClosePopUpMenu (PopUpBuyDecoration);
			StartCoroutine(SetBlockAll(1.0f,false));
		//}
	}
	












	
	public void btnStartClick()
	{
		//menuManager.ClosePopUpMenu (PopUpTask);
		StartCoroutine("StartGame");
		if(bTutorial) transform.SendMessage("TutorialSteps",1);
	}
	
	
	
	public void btnHomeClicked()
	{
		DataManager.Instance.SelectLevel(   DataManager.Instance.LastLevel );
		BlockAll.blocksRaycasts = true;
		 
		SoundManager.Instance.Play_ButtonClick();
		//DataManager.IncrementButtonClickCount();
		StartCoroutine(WaitToLoadLevel("MainScene",1));
	}
	
	public void btnReplyClicked()
	{
		BlockAll.blocksRaycasts = true;
		 
		SoundManager.Instance.Play_ButtonClick();
		//DataManager.IncrementButtonClickCount();
		//StartCoroutine(WaitToLoadLevel("Gameplay4",1));
		StartCoroutine(WaitToLoadLevel("CustomerScene",1));
	}
	

	public void btnPauseClicked()
	{
		if(bTutorial) return;
		if(bStartGame)
		{
			BlockAll.blocksRaycasts = true;
			StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));

			SoundManager.Instance.Play_ButtonClick();
			menuManager.ShowPopUpMenu (PopUpPause);
		}
	}
	
	public void btnPlayClicked()
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));

		SoundManager.Instance.Play_ButtonClick();
		menuManager.ClosePopUpMenu (PopUpPause);
	}
	
	
	public void btnLoadNextClicked()
	{
		BlockAll.blocksRaycasts = true;
		 
	 
		StartCoroutine(WaitToLoadLevel("CustomerScene",1));
	}

	bool bFinisClicked = false;

	public void btnFinishClicked()
	{
		if(!bFinisClicked)
		{
            AdsManager.Instance.ShowInterstitial();
			BlockAll.blocksRaycasts = true;
			StartCoroutine(SetBlockAll(1,false));


			if(bTutorial  &&  TutorialGP4.TutorialPhase ==14 )
			{

				Camera.main.SendMessage("NextPhase");
			}	
			else if( bTutorial   ) return;

			if(CakeBase.sprite == null)
			{
				SoundManager.Instance.Play_Sound( SoundManager.Instance.WrongIngredient);
				return;
			}
			SoundManager.Instance.Play_ButtonClick();


			DataManager.Instance.CustomerServed = true;
			string usedDecorations = "";

			usedDecorations += CakeBase.sprite.name+ ";";
			if(CakeFill.sprite!=null) usedDecorations += CakeFill.sprite.name+ ";";

			for (int i = (decorationsList.Count-1); i>=0;i--)
			{
				usedDecorations += decorationsList[i].name.Split('*')[1]+";";

			}

			DataManager.Instance.FinishedCakeDecorations = usedDecorations;
			Debug.Log("usedDecorations: "+ usedDecorations);
			//GameObject.Find("ScreenCaptureCamera").SendMessage("CaptureScreen");
			bFinisClicked = true;
			StartCoroutine(ShowEndMenu());
		}
	}
	
	//**************************************************
	
	
	IEnumerator WaitToLoadLevel(string leveName, float timeW = 0)
	{
		yield return new WaitForSeconds(timeW);
		//Application.LoadLevel( leveName );
		LevelTransition.Instance.HideScene(leveName);
	}
	


	IEnumerator ShowEndMenu()
	{
		BlockAll.blocksRaycasts = true;
		 
		bEnableMove = false;
	
		yield return new WaitForSeconds(.5f);
		//menuManager.ShowPopUpMenu(PopUpLevelComplete);
		btnLoadNextClicked();
	}

	IEnumerator StartGame()
	{
		yield return new WaitForSeconds(0.2f);
		bStartGame = true;
		bEnableMove = true;
		StartCoroutine( PodesiMeni(1));
	}
		
	IEnumerator ShowPopUp()
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));

		bEnableMove = false;
		yield return new WaitForSeconds(1);
		//menuManager.ShowPopUpMenu(PopUpTask);
		btnStartClick();
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
		
		if(PrethodniMeni == "Decoration")
		{
			ShowBuyDecorationMenu(SelectedItemToBuy);
		}
		PrethodniMeni = "";
		ShopManager.bShowShop = false;
	}
	
	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		
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

}
