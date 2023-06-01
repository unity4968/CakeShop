using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

 

public class Gameplay1 : MonoBehaviour {

	public int NextIngerdientNo = 0;
	public Transform[] handPointerPositions;
	public Transform dishPointerPosition;

	public Animator animHandPointer;
	public GameObject  HandPointer;

	public SpriteRenderer  bowlF1;
	public GameObject  bowlF2;
	public SpriteRenderer  bowlF3;
	public SpriteRenderer  bowlF3b;
	public SpriteRenderer  bowlF4;
	public GameObject  bowlF5;
	public GameObject  bowlF6;
	public Animator AnimBowl6;
	public   bool bMenuVisible = false;
	 
	public SpriteRenderer BrokenBowlSpriteRenderer;
	public SpriteRenderer NewBowlSpriteRenderer;

	public bool bSpoonEneabled = false;

	public Spoon spoon;

	public MenuManager menuManager;
 
	public GameObject PopUpPause;
	public GameObject PopUpBuySpoon;
	public GameObject PopUpBuyBowl;
	public GameObject PopUpBuyIngredients;
	public GameObject PopUpShop;

	bool bMI = false;
	bool bMS = false;
	//float tsc = 1;
	bool bStartGame = false;
	float TimeToShowHelp = 10;
	float CurrentTimeToShowHelp = 0;
	bool bShowHelp = false;
 
	public ParticleSystem MilkParticleSysytem;
	public ParticleSystem SugarParticleSysytem;
	public ParticleSystem BalingPowderParticleSysytem;
	public ParticleSystem FlourParticleSysytem;
	public ParticleSystem SaltParticleSysytem;
	public ParticleSystem OilParticleSysytem;


	public Text txtBowlPrice;
	public Text txtSpoonPrice;

	public Text[] txtCoins;


	public Sprite[] IngredientsSprites;
	public Text txtIngredientPrice;
	public Image ImageIngredient;
	StatusBar statusBar;

	string PrethodniMeni ="";

	public ShopManager shopManager;

	public CanvasGroup BlockAll;
	bool bPause = false;

	public Button[] WatchVideoButtons;

	public Button btnBowl;

	public bool bCustomerOrderVisible = false;

	void Awake () {


		BlockAll.blocksRaycasts = true;
		//Time.timeScale = 3;

		DataManager.InitDataManager();
	 
		DataManager.Instance.GetTutorialProgress();
		DataManager.Instance.PopulateGameplay1Data();
		DataManager.Instance.GetPriceData();
		Shop.InitShop();
		DataManager.Instance.GetCustomerAndOrder();
		Shop.InitShop();

		Shop.Instance.txtDispalyCoins = txtCoins;
		Shop.Instance.txtFields = null;
		ShopManager.bShowShop = false;
 


		//podesavanje  cene sastojaka
		if(  DataManager.Instance.NamirniceSeNeTrose!=2  && 
		   ( DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.Ingredients || DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.BowlAndIngr ||DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.SpoonAndIngr)
		   && DataManager.Instance.LastIngredientPriceIncreaseLevel < DataManager.Instance.SelectedLevel)  
		{
			int price = Mathf.CeilToInt( DataManager.Instance.IngredientPrice *   (1+ DataManager.Instance.SelectedLevelData.PercentPriceIncrease/100f));
			DataManager.Instance.IngredientPrice  = price;


			DataManager.Instance.LastIngredientPriceIncreaseLevel = DataManager.Instance.SelectedLevel;
			DataManager.Instance.SavePriceData();
			PlayerPrefs.Save();

		}
//		//BRISI
//		NextIngerdientNo = 7;
//		bSpoonEneabled = true;

		btnBowl.enabled = false;
		btnBowl.interactable = false;
		btnBowl.gameObject.SetActive( false);
		 
		btnBowl.transform.position = BrokenBowlSpriteRenderer.transform.position;
		SetSpoon();
		SetBowl();
		SetIngredients();

		MenuManager.bPopUpVisible = false;
	}

	void Start () {


		Input.multiTouchEnabled = false;
		//StartCoroutine("ShowPopUp");
		//KADA JE ZAVRSEN PRVI TUTPORIJAL A IGRAC NE ZNA STA DA URADI
		if(DataManager.Instance.Tutorial >=1) 
		{
			InvokeRepeating ("TestHelpNeeded",4f,0.5f);
			HidePointer();
		}
		bowlF1.enabled = false;
		bowlF2.SetActive( false);
		bowlF3.enabled = false;
		bowlF3b.enabled = false;
		bowlF4.enabled = false;
		bowlF5.SetActive( false);
		bowlF6.SetActive( false);

		MilkParticleSysytem.GetComponent<Renderer>().sortingOrder = 9;
		SugarParticleSysytem.GetComponent<Renderer>().sortingOrder = 9;
		BalingPowderParticleSysytem.GetComponent<Renderer>().sortingOrder = 9;
		FlourParticleSysytem.GetComponent<Renderer>().sortingOrder = 9;
		SaltParticleSysytem.GetComponent<Renderer>().sortingOrder = 11;
		OilParticleSysytem.GetComponent<Renderer>().sortingOrder = 11;

		statusBar = GameObject.Find("StatusBar").GetComponent<StatusBar>();
		statusBar.Init(DataManager.Instance.SelectedLevel,DataManager.Instance.SelectedLevelCoinsEarned, DataManager.Instance.SelectedLevelCustomersServed, 
		               DataManager.Instance.SelectedLevelData.CustomersPerLevel, Shop.Coins);
		statusBar.SetPosition();	

	 	StartCoroutine("StartGame");

		if(	SoundManager.Instance.menuMusic.isPlaying)
		{
			SoundManager.Instance.Stop_MenuMusic();
			SoundManager.Instance.Play_GameplayMusic();
		}

//		//ako reklama nije bila spremna, proverava se da li je spremna sada
//		if(!AdsManager.Instance.bVideoRewardReady)
//		{
//			AdsManager.Instance.bPlayVideoReward = false;
//			AdsManager.Instance.IsVideoRewardAvailable();
//		}

		SetWatchVideoButtons();

		 menuManager.EscapeButonFunctionStack.Push("btnPauseClicked");
	}

 

	void Update () {

		if(NextIngerdientNo> 6 &&NextIngerdientNo< 14) bowlF6.transform.rotation  = Quaternion.Slerp(bowlF6.transform.rotation,NextRotatrion,Time.deltaTime);

//		if( DataManager.Instance.PolomljenaCinija == 0 && Input.GetMouseButton(0) )
//		{
//
//		}

	}

	Quaternion NextRotatrion;
 
	//********************kontrola igre***********************

	public void AddDoughRotation(int direction)
	{
		Quaternion q =bowlF6.transform.rotation;
		bowlF6.transform.Rotate(new Vector3(0,0,direction*45));
		NextRotatrion =   bowlF6.transform.rotation;
		bowlF6.transform.rotation = q;

	}

 
	public void HidePointer()
	{
		 iTween.FadeTo(HandPointer,0,0.3f);
		 HandPointer.SetActive(false);

	}

	public void ShowPointer()
	{
		if(DataManager.Instance.Tutorial<1 || bShowHelp) //ako nije prikazan prvi tutorijal
		{
			HandPointer.SetActive(true);
			iTween.FadeTo(HandPointer,1,0.3f);
		}
	}

	IEnumerator WaitGoalPointer()
	{
		animHandPointer.SetBool("circle",true);
		yield return new WaitForSeconds(2f);
		bSpoonEneabled = true;
		spoon.enabled = true;
	}

	public void GoalPointer()
	{
		float TimeHandAnim = 1f;
		iTween.MoveTo( HandPointer  ,dishPointerPosition.position,TimeHandAnim);
		if(NextIngerdientNo >=7)
		{
			animHandPointer.SetBool("circle",true);
			bSpoonEneabled = true;
			spoon.enabled = true;
		}

		CurrentTimeToShowHelp =  -TimeHandAnim;
	}

	public void NextPointer()
	{
		if(NextIngerdientNo < handPointerPositions.Length) 
		{
			CurrentTimeToShowHelp = 0;
			ShowPointer();
			if( bShowHelp &&  (NextIngerdientNo == Ingredient.SelectedIngredientNo   || Spoon.bDrag))
			{
				GoalPointer();
				return;
			}
			if(NextIngerdientNo >=13 && animHandPointer !=null && animHandPointer.gameObject.activeSelf)  animHandPointer.SetBool("circle",false);
			if(DataManager.Instance.PolomljenaCinija != 0 ) iTween.MoveTo( HandPointer ,handPointerPositions[NextIngerdientNo].position,1f);
			else iTween.MoveTo( HandPointer ,dishPointerPosition.position,1f);

		}
		else HidePointer();
	}

	public void SetIngredient()
	{


		if(NextIngerdientNo == 1) bowlF1.enabled = true;
		else if(NextIngerdientNo == 2) 
		{
			bowlF2.SetActive( true);
			bowlF2.GetComponent<Animator>().SetTrigger(  "insertMilk");
		}
		else if(NextIngerdientNo == 3) bowlF3.enabled = true;
		else if(NextIngerdientNo == 4) bowlF3b.enabled = true;
		else if(NextIngerdientNo == 5) bowlF4.enabled = true;
		else if(NextIngerdientNo == 7) bowlF5.SetActive( true);
		else if(NextIngerdientNo ==9) 
		{
			StartCoroutine("HideMix");
			//poslednja slika
			bowlF6.SetActive( true);
			AnimBowl6.SetTrigger("phase01");
		}
		else if(NextIngerdientNo ==10)  AnimBowl6.SetTrigger("phase02");
		else if(NextIngerdientNo ==11)  AnimBowl6.SetTrigger("phase03");
		else if(NextIngerdientNo ==12)  AnimBowl6.SetTrigger("phase04");
		else if(NextIngerdientNo ==13)  AnimBowl6.SetTrigger("phase05");


		if(NextIngerdientNo ==7)
		{
			//DataManager.Instance.UpotrebaVarjace++;
			DataManager.Instance.SetGameplay1Data();
			StartCoroutine("WaitGoalPointer");
			//bSpoonEneabled = true;
			//spoon.enabled = true;
		}

		if(NextIngerdientNo >=14)
		{
			bSpoonEneabled = false;
			spoon.ReturnSpoonToStartPlace();
			StartCoroutine("ShowEndMenu");
		}
	}

	IEnumerator ShowEndMenu()
	{
		BlockAll.blocksRaycasts = true;
		CancelInvoke("TestHelpNeeded");
		if(DataManager.Instance.Tutorial == 0) 
		{
			DataManager.Instance.Tutorial = 1;
			DataManager.Instance.SaveTutorialProgress();
		}
		Ingredient.bEnableMove = false;
		HidePointer();
		yield return new WaitForSeconds(.2f);
		//menuManager.ShowPopUpMenu(PopUpLevelComplete);
		btnLoadNextClicked();
		
		
	}


	IEnumerator HideMix()
	{
		yield return new WaitForSeconds(0.31f);
		bowlF1.enabled = false;
		bowlF2.SetActive( false);
		bowlF3.enabled = false;
		bowlF3b.enabled = false;
		bowlF4.enabled = false;
		bowlF5.SetActive( false);
	}
	//******************************************

 


	//************************************
	//buttons
	//***********************************
 
	IEnumerator StartGame()
	{
		 
		//ako je polomljena cinija prikazuje se meni za kupovinu cinije
		if(DataManager.Instance.PolomljenaCinija ==0 )
		{
			yield return new WaitForSeconds(0.1f);
			BuyBowl();
		}
		//ako je ranije polomljena varjaca prikazuje se meni za kupovinu varjace
		else if(DataManager.Instance.PolomljenaVarjaca ==0  )
		{
			yield return new WaitForSeconds(0.1f);
			BuySpoon();
		}
		else
		{
			BlockAll.blocksRaycasts = false;
			yield return new WaitForSeconds(0.3f);
		}
		ShowPointer();
		Ingredient.bEnableMove = true;
		bStartGame = true;
		btnBowl.transform.position = BrokenBowlSpriteRenderer.transform.position;
	}

	public void btnHomeClicked()
	{
		DataManager.Instance.SelectLevel(   DataManager.Instance.LastLevel );
		//Time.timeScale = tsc;
		StartCoroutine(WaitToLoadLevel("MainScene",1));
		SoundManager.Instance.Play_ButtonClick();
        AdsManager.Instance.ShowInterstitial();
	}

	public void btnReplyClicked()
	{
		 
		 StartCoroutine(WaitToLoadLevel("CustomerScene",1));
		SoundManager.Instance.Play_ButtonClick();
        AdsManager.Instance.ShowInterstitial();
	}

	public void btnPauseClicked()
	{
		if(bStartGame && !bMenuVisible && !ShopManager.bShowShop &&  !PopUpEvents.bMenuVisible)
		{
			bMenuVisible = true;
			BlockAll.blocksRaycasts = true;
			 
			bPause = true;
			SoundManager.Instance.Play_ButtonClick();
			bSpoonEneabled = false;

			StartCoroutine(ShowPopup(PopUpPause,.1f,false));
		}

	}

	public void btnPlayClicked()
	{
		SoundManager.Instance.Play_ButtonClick();
		SetMoveElements();
		BlockAll.blocksRaycasts = true;
		StartCoroutine(ClosePopUp(PopUpPause,.1f,false));
		bPause = false;
	}


	public void btnLoadNextClicked()
	{
		BlockAll.blocksRaycasts = true;
        StartCoroutine(WaitToLoadLevel("Gameplay2a",1));
	}
 
	 
	IEnumerator WaitToLoadLevel(string leveName, float timeW = 0)
	{
		yield return new WaitForSeconds(timeW);
		LevelTransition.Instance.HideScene(leveName);
	}

	//**********************************************


 



	//******************BUY  SPOON********************

	public void SetSpoon()
	{
		spoon.Init();
	}


	public void BuySpoon()
	{
		if(!bMenuVisible && !bPause)
		{
			PodesiNovcice();
			//ISPISIVANJE CENE
			 txtSpoonPrice.text = DataManager.Instance.SelectedLevelData.FixedPrice.ToString();

			StartCoroutine("ShowMenuBuySpoon");
		}
	}

	IEnumerator ShowMenuBuySpoon()
	{
		yield return new WaitForSeconds(.1f);
		if(!bMenuVisible && !ShopManager.bShowShop &&  !PopUpEvents.bMenuVisible)
		{
			BlockAll.blocksRaycasts = true;
			bMenuVisible = true;
			bSpoonEneabled = false;
			StartCoroutine(ShowPopup(PopUpBuySpoon,.1f,false));
			menuManager.EscapeButonFunctionStack.Push("ClosePopUpSpoon");
		}
	}



	IEnumerator WaitToSetMoveElements(float timeToWait = 0)
	{	
		yield return new WaitForSeconds(timeToWait);
		SetMoveElements();
	}

	void SetMoveElements()
	{

		if(NextIngerdientNo >=7)
		{
			bSpoonEneabled =  ( DataManager.Instance.PolomljenaVarjaca >0);
			Ingredient.bEnableMove = false;
		}
		
		else
		{
			bSpoonEneabled =   false;
			Ingredient.bEnableMove = true;
			Ingredient.bEnableMove = (DataManager.Instance.PolomljenaCinija > 0);
		}
	}

	public void btnBuySpoonClicked()
	{
		BlockAll.blocksRaycasts = true;
		SoundManager.Instance.Play_ButtonClick();
		if(DataManager.Instance.SelectedLevelData.FixedPrice <= Shop.Coins  )
		{
 
			PrethodniMeni ="";
			 
			//TODO: ODUZMI NOVAC
			Shop.Instance.txtDispalyCoins = txtCoins;
			Shop.Instance.AnimiranjeDodavanjaNovcica(-DataManager.Instance.SelectedLevelData.FixedPrice, null,"");
			
			StartCoroutine(ClosePopUp(PopUpBuySpoon,2,false));
			spoon.NewSpoon();
			HidePointer();
			SetMoveElements();
		}
		else
		{
			//nema dovoljno novca
			SoundManager.Instance.Play_Sound(SoundManager.Instance.NoMoney);
			shopManager.ShowPopUpShop();
			PrethodniMeni = "Spoon";
			StartCoroutine(ClosePopUp(PopUpBuySpoon,0.1f,false));

		}
	}

	//******************************************************
 
	
	IEnumerator ClosePopUp(GameObject popup,float time, bool blockRays)
	{
		yield return new WaitForSeconds(time);
		popup.GetComponent<PopUpEvents>().Close();
	}

	public void PopUpClosed (GameObject popup)
	{
		BlockAll.blocksRaycasts = false;
		bMenuVisible = false;
		popup.SetActive(false);
	}


	IEnumerator ShowPopup(GameObject popup,float time, bool blockRays)
	{
		popup.SetActive(true);
		bMenuVisible = true;
 
		yield return new WaitForSeconds(time);
		popup.GetComponent<PopUpEvents>().Open();
	}


	public void PopUpOpened (GameObject popup)
	{
		BlockAll.blocksRaycasts = false;
	}

	//*********************************************













	//******************BUY BOWL********************

	public void SetBowl()
	{
		
		//podesavanje cinije
		if(   DataManager.Instance.PolomljenaCinija !=2 &&  
		   (DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.Bowl  || DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.BowlAndIngr || DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.BowlANdOven)
		   && DataManager.Instance.NivoPoslednjegLomljenjaCinije < DataManager.Instance.SelectedLevel)
		{
			DataManager.Instance.PolomljenaCinija = 0;
			DataManager.Instance.NivoPoslednjegLomljenjaCinije = DataManager.Instance.SelectedLevel;
			DataManager.Instance.SetGameplay1Data();
			
			PlayerPrefs.Save();
		}
		
		if(DataManager.Instance.PolomljenaCinija ==0  )
		{
			btnBowl.enabled = true;
			btnBowl.interactable = true;
			btnBowl.gameObject.SetActive( true);

			BrokenBowlSpriteRenderer.enabled = true;
			NewBowlSpriteRenderer.enabled = false;
		}
		else
		{
			btnBowl.enabled = false;


			BrokenBowlSpriteRenderer.enabled = false;
			NewBowlSpriteRenderer.enabled = true;
			btnBowl.interactable = false;
			btnBowl.gameObject.SetActive( false);


		}
		
		if(PrethodniMeni!="")
		{
			 
			Ingredient.bEnableMove =  true; 
			bSpoonEneabled = false; 
			PrethodniMeni ="";
		}
	}
	


	void NewBowl()
	{
		BrokenBowlSpriteRenderer.enabled = false;
		NewBowlSpriteRenderer.enabled = true;

		btnBowl.enabled = false;

		btnBowl.interactable = false;
		btnBowl.gameObject.SetActive( false);
		
		if(DataManager.Instance.PolomljenaCinija != 2)
			DataManager.Instance.PolomljenaCinija = 1;
		DataManager.Instance.NivoPoslednjegLomljenjaCinije = DataManager.Instance.SelectedLevel;
		DataManager.Instance.SetGameplay1Data();
		PlayerPrefs.Save();

		HidePointer();
	}


	public void BuyBowl()
	{
	 
		BlockAll.blocksRaycasts = true;
		PodesiNovcice();
		//ISPISIVANJE CENE
		txtBowlPrice.text = DataManager.Instance.SelectedLevelData.FixedPrice.ToString();
		StartCoroutine("ShowMenuBuyBowl");
	}



	IEnumerator ShowMenuBuyBowl()
	{
		yield return new WaitForSeconds(.1f);
		if(!bMenuVisible && !ShopManager.bShowShop &&  !PopUpEvents.bMenuVisible)
		{
			bMenuVisible = true;
			BlockAll.blocksRaycasts = true;
			//Ingredient.bEnableMove = false;
			bSpoonEneabled = false;
			//menuManager.ShowPopUpMenu (PopUpBuyBowl);
			StartCoroutine(ShowPopup(PopUpBuyBowl,.1f,false));
			menuManager.EscapeButonFunctionStack.Push("ClosePopUpBowl");
			//yield return new WaitForSeconds(1f);
			//BlockAll.blocksRaycasts = false;
		}
	}

	public void btnBuyNewBowlClicked()
	{
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;
		if(DataManager.Instance.SelectedLevelData.FixedPrice <= Shop.Coins )
		{
			//if(PrethodniMeni!="")
			//{
			//Time.timeScale = tsc;
			Ingredient.bEnableMove =  true; 
			bSpoonEneabled =     false;
			PrethodniMeni ="";
			//}
			
			Shop.Instance.txtDispalyCoins = txtCoins;
			Shop.Instance.AnimiranjeDodavanjaNovcica(-DataManager.Instance.SelectedLevelData.FixedPrice, null,"");
			
			StartCoroutine(ClosePopUp(PopUpBuyBowl,2,false));
			
			NewBowl();
		}
		else
		{
			
			//nema dovoljno novca
			SoundManager.Instance.Play_Sound(SoundManager.Instance.NoMoney);
			shopManager.ShowPopUpShop();
			PrethodniMeni = "Bowl";
			//menuManager.ClosePopUpMenu (PopUpBuyBowl);
			StartCoroutine(ClosePopUp(PopUpBuyBowl,0.1f,false));
		}
	}
	


	//***************************************************


	//***********************INGREDIENTS MENU**************

	IngredientBar ingBar ;

	public void SetIngredients()
	{
		for(int IngredientNo = 0; IngredientNo<=6;IngredientNo++)
		{
			GameObject ib = GameObject.Find("IngredientBar"+IngredientNo.ToString());
			if(ib!=null)  ib.GetComponent<IngredientBar>().SetData();
		}
	}

	public void ShowBuyIngredientsMenu( IngredientBar ingB)
	{
		if(!bMenuVisible && !ShopManager.bShowShop &&  !PopUpEvents.bMenuVisible)
		{
			PodesiNovcice();
			bMenuVisible = true;
			//Ingredient.bEnableMove = false;
			bSpoonEneabled = false;

			ingBar = ingB;
			Ingredients ingType =  ingB.ingType;
			ImageIngredient.sprite=   IngredientsSprites [ (int) ingType];

			BlockAll.blocksRaycasts =  true;
			StartCoroutine(ShowPopup(PopUpBuyIngredients,.1f,false));
			menuManager.EscapeButonFunctionStack.Push("ClosePopUpIngredients");
			//menuManager.ShowPopUpMenu (PopUpBuyIngredients);
			txtIngredientPrice.text = DataManager.Instance.IngredientPrice.ToString();
		}
		
	}



	public void btnBuyIngredientClicked()
	{
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts =  true;
		if(DataManager.Instance.IngredientPrice <= Shop.Coins  )
		{
 
			Shop.Instance.txtDispalyCoins = txtCoins;
			Shop.Instance.AnimiranjeDodavanjaNovcica(-DataManager.Instance.IngredientPrice, null ,"");
 
			ingBar.AddIngredients(false);
			StartCoroutine(ClosePopUp(PopUpBuyIngredients,1.5f,false));

			StartCoroutine(WaitToSetMoveElements(2.0f));
		}
		else
		{
			StartCoroutine( SetBMenuVisible(DataManager.animInactiveTime, false));
			//nema dovoljno novca
			SoundManager.Instance.Play_Sound(SoundManager.Instance.NoMoney);
			shopManager.ShowPopUpShop();
			PrethodniMeni = "Ingredients";
			//menuManager.ClosePopUpMenu (PopUpBuyIngredients );
			StartCoroutine(ClosePopUp(PopUpBuyIngredients,0.1f,false));
		}
	}




	//***************************************************
	public void ClosePopUpIngredients()
	{
		PrethodniMeni ="";
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;
		 
	 
		StartCoroutine(ClosePopUp(PopUpBuyIngredients,0.1f,false));
		 
		SetMoveElements(); 
		CurrentTimeToShowHelp = 0;
		StartCoroutine( SetBMenuVisible(.7f, false));
	}

	public void ClosePopUpBowl()
	{
		if(DataManager.Instance.PolomljenaCinija ==0  )
		{
	 
			btnBowl.enabled = true;
			btnBowl.interactable = true;
			btnBowl.gameObject.SetActive( true);
		}
		else
		{
			btnBowl.enabled = false;
			btnBowl.interactable = false;
			btnBowl.gameObject.SetActive( false);
		}
		PrethodniMeni ="";
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;
		 
		StartCoroutine(ClosePopUp(PopUpBuyBowl,0.1f,false));
		 
		SetMoveElements(); 
		CurrentTimeToShowHelp = 0;
		StartCoroutine( SetBMenuVisible(.7f, false)); 
	}

	public void ClosePopUpSpoon()
	{
		PrethodniMeni ="";
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;

		StartCoroutine(ClosePopUp(PopUpBuySpoon,0.1f,false));
 
		SetMoveElements();
		CurrentTimeToShowHelp = 0;
	 
		StartCoroutine( SetBMenuVisible(.7f, false));
	}

	IEnumerator SetBMenuVisible(float timeW, bool valueMV)
	{
		yield return new WaitForSeconds (timeW);
		bMenuVisible = valueMV;
	}

	IEnumerator SetBShowShop(float timeW, bool valueMV)
	{
		yield return new WaitForSeconds (timeW);
		ShopManager.bShowShop = valueMV;
	}

	//************SHOP MENU*********************

	public void ClosePopUpShop()
	{
		 
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;
		if(PrethodniMeni == "Ingredients")
		{

			if(DataManager.Instance.NamirniceSeNeTrose != 2)
				ShowBuyIngredientsMenu(ingBar);
		}
		if(PrethodniMeni == "Bowl")
		{
			 if(DataManager.Instance.PolomljenaCinija != 2)
				StartCoroutine("ShowMenuBuyBowl");
			else
			{
				SetBowl();
				BlockAll.blocksRaycasts = false;
			}
		}
		if(PrethodniMeni == "Spoon")
		{
			if(DataManager.Instance.PolomljenaVarjaca != 2)
				StartCoroutine("ShowMenuBuySpoon");
			else
			{
				StartCoroutine(SetBlockAll(DataManager.animInactiveTime,  false));
				//kupljeno beskonacno
				SetSpoon();
				SetMoveElements(); 
 
			}
		}
		else
		{
			StartCoroutine(SetBlockAll(DataManager.animInactiveTime,  false));
			 
		}
		menuManager.ClosePopUpMenu (PopUpShop);
		 
		StartCoroutine( SetBMenuVisible(DataManager.animInactiveTime, false));
		StartCoroutine( SetBShowShop(DataManager.animInactiveTime, false));
	}


	//**********************************************


	void PodesiNovcice()
	{
		for(int i = 0; i<txtCoins.Length;i++)
		{
			txtCoins[i].text =   Shop.Coins.ToString();
		}
	}





	string watchVideoPopupName = ""; //ingredient, bowl, spoon

	public void btnWatchVideo( string popupName)
	{
		if(watchVideoPopupName == "")
		{
			watchVideoPopupName = popupName;
			BlockAll.blocksRaycasts = true;
			Shop.Instance.WatchVideo();

			StartCoroutine(SetBlockAll (1.5f,false));
		}
	}


	public void EndWatchingVideo(  bool bFinishWatching)
	{
		//if(bFinishWatching)
		{
			if(watchVideoPopupName == "ingredient")
			{
				ingBar.AddIngredients(true);
				StartCoroutine(ClosePopUp(PopUpBuyIngredients,0.1f,false));
			}
			if(watchVideoPopupName == "bowl")
			{
				StartCoroutine(ClosePopUp(PopUpBuyBowl,0.1f,false));
				NewBowl();
			}
			if(watchVideoPopupName == "spoon")
			{
				StartCoroutine(ClosePopUp(PopUpBuySpoon,0.1f,false));
				spoon.NewSpoon();
				HidePointer();
			}

			watchVideoPopupName = "";
			SetMoveElements(); 
		}
 
	}




	//AKO JE IGRAC ZABORAVIO STA TREBA DA URADI POKERECE SE KURSOR KOJI POKAZUJE POMOC
	void TestHelpNeeded()
	{
		if(bStartGame)
		{
			if(CurrentTimeToShowHelp ==0) bShowHelp = false;
			if(!bPause  && !ShopManager.bShowShop && !MenuManager.bPopUpVisible ) CurrentTimeToShowHelp+=0.5f;
			 
			if(TimeToShowHelp  == CurrentTimeToShowHelp)
			{
				bShowHelp = true;
				NextPointer();
			}
		}
	}

	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		float tsc = Time.timeScale;
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


	public void btnShowHideBanner()
	{

		GameObject.Find("StatusBar").GetComponent<StatusBar>().SetPosition();
	}

}
