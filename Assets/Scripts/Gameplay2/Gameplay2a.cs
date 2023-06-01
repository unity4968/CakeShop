using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Gameplay2a : MonoBehaviour {

	public Transform Mold;
	public Transform MoldDough;
	
	public Transform handPointerPosition;
	public Transform MoldPointerPosition;

	public Animator animHandPointer;
	public GameObject  HandPointer;
	
	public bool bEnableMove = false;
	public   bool bEnableBowl = false;
	public MenuManager menuManager;

	//public GameObject PopUpTask;
	//public GameObject PopUpLevelComplete;
	public GameObject PopUpPause;
	
	public int  FillStep =1;
	public Bowl bowl;
	 
	bool bMB = false;
	float tsc = 1;
	bool bStartGame = false;
	float TimeToShowHelp = 10;
	float CurrentTimeToShowHelp = 0;
	bool bShowHelp = false;
	 
	public int NextPositionNo = 0;
	StatusBar statusBar;
	public Text[] txtCoins;


	void Awake () {
		DataManager.InitDataManager();
		DataManager.Instance.GetTutorialProgress();
	}
	
	void Start () {
		Input.multiTouchEnabled = false;
		StartCoroutine("ShowPopUp");
		MoldDough.GetComponent<SpriteRenderer>().enabled = false;
		if(DataManager.Instance.Tutorial >=2)  InvokeRepeating ("TestHelpNeeded",0.5f,0.5f); 	//KADA JE ZAVRSEN PRVI TUTPORIJAL A IGRAC NE ZNA STA DA URADI


		statusBar = GameObject.Find("StatusBar").GetComponent<StatusBar>();
		statusBar.Init(DataManager.Instance.SelectedLevel,DataManager.Instance.SelectedLevelCoinsEarned, DataManager.Instance.SelectedLevelCustomersServed, 
		               DataManager.Instance.SelectedLevelData.CustomersPerLevel, Shop.Coins);
		statusBar.SetPosition();

		Shop.Instance.txtDispalyCoins = txtCoins;
		Shop.Instance.txtFields = null;

		if(	SoundManager.Instance.menuMusic.isPlaying)
		{
			SoundManager.Instance.Stop_MenuMusic();
			SoundManager.Instance.Play_GameplayMusic();
		}

		menuManager.EscapeButonFunctionStack.Push("btnPauseClicked");
	}
	

	//**********************************

	public void GoalPointer()
	{
		iTween.MoveTo( HandPointer  ,MoldPointerPosition.position,1f);
	}

	public void HidePointer()
	{
		iTween.FadeTo(HandPointer,0,0.3f);
	}
	
	public void ShowPointer()
	{
		if(DataManager.Instance.Tutorial<2 || bShowHelp) //ako nije prikazan drugi tutorijal
		{
			HandPointer.SetActive(true);
			iTween.FadeTo(HandPointer,1,0.3f);
			CurrentTimeToShowHelp = 0;
		}
	}

	public void FillDough()
	{
		CurrentTimeToShowHelp = 0;
		if(FillStep == 2) MoldDough.GetComponent<SpriteRenderer>().enabled = true;
		if(FillStep == 3) HidePointer();
		//if(FillStep <= 6) MoldDough.localPosition += new Vector3 (0,0,0.15f*FillStep);
		if(FillStep == 6)
		{
			bEnableBowl = false;
			StartCoroutine("ShowEndMenu"); 
		}
		iTween.MoveTo(  MoldDough.gameObject, MoldDough.position + new Vector3(0,0.15f,0),0.6f);
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

		 
			bEnableMove = false;
			menuManager.ShowPopUpMenu (PopUpPause);
			SoundManager.Instance.Play_ButtonClick();
		}
	}
	
	public void btnPlayClicked()
	{
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));

		StartCoroutine(ContinueGameplay());
		menuManager.ClosePopUpMenu (PopUpPause);
		SoundManager.Instance.Play_ButtonClick();
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
		LevelTransition.Instance.HideScene(leveName);
	}
	
	
	
	IEnumerator ShowEndMenu()
	{
		CancelInvoke("TestHelpNeeded");
		if(DataManager.Instance.Tutorial <2) 
		{
			DataManager.Instance.Tutorial = 2;
			DataManager.Instance.SaveTutorialProgress();
		}
		bEnableMove = false;
		HidePointer();
		yield return new WaitForSeconds(2);
	 
		btnLoadNextClicked();
	}
	
	
	
	IEnumerator StartGame()
	{
		yield return new WaitForSeconds(1f);
		ShowPointer();
		bEnableMove = true;
		bStartGame = true;
		bEnableBowl = true;
	}
	
	
	IEnumerator ShowPopUp()
	{
		bEnableMove = false;

		iTween.FadeUpdate(HandPointer,0,0);
		yield return new WaitForSeconds(1);
		btnStartClick();
	}
	
	//AKO JE IGRAC ZABORAVIO STA TREBA DA URADI POKERECE SE KURSOR KOJI POKAZUJE POMOC
	void TestHelpNeeded()
	{
		if(bStartGame)
		{
			if(CurrentTimeToShowHelp ==0) bShowHelp = false;
			if(!ShopManager.bShowShop && !MenuManager.bPopUpVisible)  	CurrentTimeToShowHelp+=0.5f;

			if(TimeToShowHelp  == CurrentTimeToShowHelp)
			{
				bShowHelp = true;
				ShowPointer();
				if(bowl.bDrag) GoalPointer();
				else iTween.MoveTo( HandPointer  ,handPointerPosition.position, 1f);
			}
		}
	}
	
	//*********************************************
	public CanvasGroup BlockAll;
	public GameObject popUpShop;

	public void ClosePopUpShop()
	{
		StartCoroutine(ContinueGameplay());
		menuManager.ClosePopUpMenu (popUpShop);
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
		SoundManager.Instance.Play_ButtonClick();
		ShopManager.bShowShop = false;
	}

	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		
	}

	public void StopGameplay()
	{
		bEnableMove = false; 
	}

	IEnumerator ContinueGameplay()
	{
		yield return new WaitForSeconds(1.5f);
		bEnableMove = true; 
	}

	public void btnShowHideBanner()
	{

		GameObject.Find("StatusBar").GetComponent<StatusBar>().SetPosition();
	}


}
