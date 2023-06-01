using UnityEngine;
using System.Collections;

public class MainScene : MonoBehaviour {
	MenuManager menuManager;
	public DailyRewards dailyRewards;


	void Awake () {
        Time.timeScale = 1;
	}
	 

	void Start () {
		menuManager = GameObject.Find("Canvas").GetComponent<MenuManager>();
		menuManager.EscapeButonFunctionStack.Push("ExitGame");

		//SoundManager.Instance.Play_MenuMusic();

		if(	!SoundManager.Instance.menuMusic.isPlaying)
		{
			SoundManager.Instance.Stop_GameplayMusic();
			SoundManager.Instance.Play_MenuMusic();
		}

//		AdsManager.Instance.bPlayVideoReward = false;
//		AdsManager.Instance.IsVideoRewardAvailable(5);

		Shop.Instance.txtDispalyCoins = null;

		 
	}


	public void ExitGame () {
		Debug.Log("EXIT");
		Application.Quit();
	}

	public void btnPlayClicked()
	{
		BlockAll.blocksRaycasts = true;
		LevelTransition.Instance.HideScene("MapScene");
	 	
		//menuManager.LoadScene("MapScene");
		SoundManager.Instance.Play_ButtonClick();
	}

	//*********************************************
	public CanvasGroup BlockAll;
	public GameObject popUpShop;
	 

	public void ClosePopUpByName(string popUpName)
	{
		GameObject popUp  = GameObject.Find(popUpName);
		if(popUp!=null) menuManager.ClosePopUpMenu(popUp);
		if( menuManager.EscapeButonFunctionStack.Count >0 &&  menuManager.EscapeButonFunctionStack.Peek().Contains("*"+popUpName) ) menuManager.EscapeButonFunctionStack.Pop ();
	}

	public void CloseDailyReward()
	{
		DailyRewards dr = GameObject.Find("DailyReward").GetComponent<DailyRewards>();
		if(dr !=null) dr.Collect();
	}

	public void ClosePopUpRate()
	{
		Rate rate = GameObject.Find("PopUpRate").GetComponent<Rate>();
		if(rate !=null) rate.HideRateMenu(rate.gameObject);

	}

	public void ClosePopUpShop()
	{
		SoundManager.Instance.Play_ButtonClick();

		BlockAll.blocksRaycasts = true;
		menuManager.ClosePopUpMenu (popUpShop);
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
		if( menuManager.EscapeButonFunctionStack.Count >0 &&  menuManager.EscapeButonFunctionStack.Peek()== "ClosePopUpShop" ) menuManager.EscapeButonFunctionStack.Pop ();
	}

	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		 
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		 
	}


	//ovo je funkcija koja sluzi samo da pusti zvuk
	public void btnClicked_PlaySound()
	{
		SoundManager.Instance.Play_ButtonClick();
		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
	}

 	//*******************GASENJE MENIJA NA ESC
}
