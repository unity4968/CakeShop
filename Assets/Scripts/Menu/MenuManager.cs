using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

	/**
  * Scene:All
  * Object:Canvas
  * Description: Skripta zaduzena za hendlovanje(prikaz i sklanjanje svih Menu-ja,njihovo paljenje i gasenje, itd...)
  **/
public class MenuManager : MonoBehaviour 
{
	
	public Menu currentMenu;
	Menu currentPopUpMenu;
//	[HideInInspector]
//	public Animator openObject;
	public GameObject[] disabledObjects;
	GameObject ratePopUp, crossPromotionInterstitial;

	public static bool bFirstLoadMainScene = true;
	ShopManager shopManager = null;
	DailyRewards dailyaReward = null;
	public static bool bPopUpVisible = false;

	bool  bMainSceneNativeAdLoaded = false;
	bool  bSettingsClosed = false;
	void Awake()
	{
		foreach( Transform tr in  transform)
		{
			if(tr.name =="TransitionHolder") tr.gameObject.SetActive(true);
		}
	}

	void Start () 
	{
		if(Shop.RemoveAds !=2 &&  DataManager.Instance.Tutorial>=4  ) GameObject.Find("PopUps").GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-30);
		 else if(Application.loadedLevelName=="MainScene") GameObject.Find("PopUps").GetComponent<RectTransform>().anchoredPosition = new Vector2(0,10);
		bPopUpVisible = false;

		if(Application.loadedLevelName=="MainScene")
		{
			crossPromotionInterstitial = GameObject.Find("PopUps/PopUpInterstitial");
			ratePopUp = GameObject.Find("PopUps/PopUpRate");


			shopManager = GameObject.Find("PopUps/PopUpShop").GetComponent<ShopManager>();

			 
			dailyaReward = GameObject.Find("PopUps/DailyReward").GetComponent <DailyRewards>();

		}

		if (disabledObjects!=null) {
			for(int i=0;i<disabledObjects.Length;i++)
				disabledObjects[i].SetActive(false);
		}
		
		if(Application.loadedLevelName!= "MapScene")
			ShowMenu(currentMenu.gameObject);	
		
		if(Application.loadedLevelName=="MainScene")
		{
			StartCoroutine(DelayStartMainScene());
		}
 
	}

	CanvasGroup BlockAll;
	IEnumerator DelayStartMainScene()
	{
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		BlockAll.blocksRaycasts = true;

		yield return new WaitForSeconds(1.5f);
		if(PlayerPrefs.HasKey("alreadyRated"))
		{
			Rate.alreadyRated = PlayerPrefs.GetInt("alreadyRated");
		}
		else
		{
			Rate.alreadyRated = 0;
		}
		
		if(Rate.alreadyRated==0)
		{
			Rate.appStartedNumber = PlayerPrefs.GetInt("appStartedNumber");
			Debug.Log("appStartedNumber "+Rate.appStartedNumber);
			
			if(Rate.appStartedNumber>=6 )
			{
				Rate.appStartedNumber=0;
				PlayerPrefs.SetInt("appStartedNumber",Rate.appStartedNumber);
				PlayerPrefs.Save();
				//GameObject.Find("Canvas").GetComponent<MenuManager>().
				ShowPopUpMenu(ratePopUp);
				
			}
			else
			{
				if( bFirstLoadMainScene && dailyaReward !=null &&  dailyaReward.TestDailyRewards())
				{	
					dailyaReward.ShowDailyReward();
					//EscapeButonFunctionStack
					EscapeButonFunctionStack.Push("CloseDailyReward");
				}
				else
				{	
					dailyaReward.gameObject.SetActive(false);
				}
			}
		}
		else
		{
			 
			//DailyRewards dailyaReward = GameObject.Find("PopUps/DailyReward").GetComponent<DailyRewards>();
			if( bFirstLoadMainScene  && dailyaReward !=null && dailyaReward.TestDailyRewards())
			{
				dailyaReward.ShowDailyReward();
				EscapeButonFunctionStack.Push("CloseDailyReward");

			}
			else 
			{	
				dailyaReward.gameObject.SetActive(false);
			}
		}
		
		bFirstLoadMainScene = false;

		yield return new WaitForSeconds(1.1f);
		BlockAll.blocksRaycasts = false;
	}

	/// <summary>
	/// Funkcija koja pali(aktivira) objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se upali</param>
	public void EnableObject(GameObject gameObject)
	{
		
		if (gameObject != null) 
		{
			if (!gameObject.activeSelf) 
			{
				gameObject.SetActive (true);
			}
		}
	}

	/// <summary>
	/// Funkcija koja gasi objekat
	/// </summary>
	/// /// <param name="gameObject">Game object koji se prosledjuje i koji treba da se ugasi</param>
	public void DisableObject(GameObject gameObject)
	{
		
		if (gameObject != null) 
		{
			if (gameObject.activeSelf) 
			{
				gameObject.SetActive (false);
			}
		}
	}
	
	/// <summary>
	/// F-ja koji poziva ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadScene(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevel (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}
	
	/// <summary>
	/// F-ja koji poziva asihrono ucitavanje Scene
	/// </summary>
	/// <param name="levelName">Level name.</param>
	public void LoadSceneAsync(string levelName )
	{
		if (levelName != "") {
			try {
				Application.LoadLevelAsync (levelName);
			} catch (System.Exception e) {
				Debug.Log ("Can't load scene: " + e.Message);
			}
		} else {
			Debug.Log ("Can't load scene: Level name to set");
		}
	}

	/// <summary>
	/// Funkcija za prikaz Menu-ja koji je pozvan kao Menu
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
	public void ShowMenu(GameObject menu)
	{
		if (currentMenu != null)
			currentMenu.IsOpen = false;

		menu.gameObject.SetActive (true);
		currentMenu = menu.GetComponent<Menu> ();
		currentMenu.IsOpen = true;
	 
		if( menu.name != "MainMenu" ) bPopUpVisible = true;
	}

	/// <summary>
	/// Funkcija za zatvaranje Menu-ja koji je pozvan kao Meni
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
	public void CloseMenu(GameObject menu)
	{
		 
		if (menu != null) 
		{
			menu.GetComponent<Menu> ().IsOpen = false;
			menu.SetActive (false);
		}
		bPopUpVisible = false;
	}

	/// <summary>
	/// Funkcija za prikaz Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje za prikaz, mora imati na sebi skriptu Menu.</param>
	public void ShowPopUpMenu(GameObject menu)
	{
		menu.gameObject.SetActive (true);
		currentMenu = menu.GetComponent<Menu> ();
	 	currentMenu.IsOpen = true;

//		Debug.Log("PUV");
		bPopUpVisible = true;
	
		if(Application.loadedLevelName == "MainScene")
		{
			 if(menu.name == "PopUpSettings")
			{
				EscapeButonFunctionStack.Push("ClosePopUpByName*PopUpSettings");
				//mainSceneNativeAd.CancelLoading();
				//mainSceneNativeAd.HideNativeAd();
			}

			if(menu.name == "PopUpCrossPromotionOfferWall")
			{
				EscapeButonFunctionStack.Push("ClosePopUpByName*PopUpCrossPromotionOfferWall");
				//mainSceneNativeAd.CancelLoading();
				//mainSceneNativeAd.HideNativeAd();
			}

			if(menu.name == "PopUpInterstitial")
			{
				EscapeButonFunctionStack.Push("ClosePopUpByName*PopUpInterstitial");
				//mainSceneNativeAd.CancelLoading();
				//mainSceneNativeAd.HideNativeAd();
			}
			if(menu.name == "PopUpRate")
			{
				EscapeButonFunctionStack.Push("ClosePopUpRate");
				//mainSceneNativeAd.CancelLoading();
				//mainSceneNativeAd.HideNativeAd();
			}
		}
		if(Application.loadedLevelName == "MapScene")
		{
			if(menu.name == "PopUpSettings") EscapeButonFunctionStack.Push("ClosePopUpByName*PopUpSettings");
		}
		StartCoroutine("ShowPopUpSpeedUp");
	}

	IEnumerator ShowPopUpSpeedUp( )
	{
		Time.timeScale = 1.5f;
		yield return new WaitForSeconds(1.2f);
		Time.timeScale = 1;
	}

	public void ActivateMainSceneNativeAdd()
	{

	}

	/// <summary>
	/// Funkcija za zatvaranje Menu-ja koji je pozvan kao PopUp-a, poziva inace coroutine-u, ima delay zbog animacije odlaska Menu-ja
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje i treba da se skloni, mora imati na sebi skriptu Menu.</param>
	public void ClosePopUpMenu(GameObject menu)
	{
		StartCoroutine("HidePopUp",menu);
		bPopUpVisible = false;
		if(menu.name == "PopUpSettings") bSettingsClosed = true;
	}

	/// <summary>
	/// Couorutine-a za zatvaranje Menu-ja koji je pozvan kao PopUp-a
	/// </summary>
	/// /// <param name="menu">Game object koji se prosledjuje, mora imati na sebi skriptu Menu.</param>
	IEnumerator HidePopUp(GameObject menu)
	{
		Time.timeScale = 1.5f;
		menu.GetComponent<Menu> ().IsOpen = false;
		yield return new WaitForSeconds(1.2f);

		menu.SetActive (false);
		Time.timeScale = 1;
	}

	/// <summary>
	/// Funkcija za prikaz poruke preko Log-a, prilikom klika na dugme
	/// </summary>
	/// /// <param name="message">poruka koju treba prikazati.</param>
	public void ShowMessage(string message)
	{
		Debug.Log(message);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
	/// </summary>
	/// <param name="messageTitleText">naslov koji treba prikazati.</param>
	/// <param name="messageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpMessage(string messageTitleText, string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=messageTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=messageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);

	}

	/// <summary>
	/// Funkcija koja podesava naslov CustomMessage-a, i ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageCustomMessageText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
	/// </summary>
	/// <param name="messageTitleText">naslov koji treba prikazati.</param>
	public void ShowPopUpMessageTitleText(string messageTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=messageTitleText;
	}

	/// <summary>
	/// Funkcija koja podesava poruku CustomMessage, i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpMessageTitleText u redosledu: 1-ShowPopUpMessageTitleText 2-ShowPopUpMessageCustomMessageText
	/// </summary>
	/// <param name="messageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpMessageCustomMessageText(string messageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=messageText;		
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga kao i poruku u dialogu i ova f-ja se poziva iz skripte
	/// </summary>
	/// <param name="dialogTitleText">naslov koji treba prikazati.</param>
	/// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpDialog(string dialogTitleText, string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=dialogTitleText;
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=dialogMessageText;
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}

	/// <summary>
	/// Funkcija koja podesava naslov dialoga, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogCustomMessageText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
	/// </summary>
	/// <param name="dialogTitleText">naslov koji treba prikazati.</param>
	public void ShowPopUpDialogTitleText(string dialogTitleText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/HeaderHolder/TextHeader").GetComponent<Text>().text=dialogTitleText;
	}

	/// <summary>
	/// Funkcija koja podesava poruku dialoga i poziva meni u vidu pop-upa, ova f-ja se poziva preko button-a zajedno za f-jom ShowPopUpDialogTitleText u redosledu: 1-ShowPopUpDialogTitleText 2-ShowPopUpDialogCustomMessageText
	/// </summary>
	/// <param name="dialogMessageText">custom poruka koju treba prikazati.</param>
	public void ShowPopUpDialogCustomMessageText(string dialogMessageText)
	{
		transform.Find("PopUps/PopUpMessage/AnimationHolder/Body/ContentHolder/TextBG/TextMessage").GetComponent<Text>().text=dialogMessageText;		
		ShowPopUpMenu(transform.Find("PopUps/PopUpMessage").gameObject);
	}
 
	//ovo je funkcija koja sluzi samo da pusti zvuk
	public void btnClicked_PlaySound()
	{
		SoundManager.Instance.Play_ButtonClick();
	}

	bool bDisableEsc = false;
	public Stack<string> EscapeButonFunctionStack = new Stack<string>();

	void Update()
	{
		if( !bDisableEsc  && Input.GetKeyDown(KeyCode.Escape) )
		{

			Debug.Log( EscapeButonFunctionStack.Count + "    " + EscapeButonFunctionStack.Peek() );

			if(EscapeButonFunctionStack.Count>0)
			{
				bDisableEsc = true;
				if( EscapeButonFunctionStack.Peek().Contains("*") )
				{
					string go = EscapeButonFunctionStack.Peek().Split('*')[1];

					Camera.main.SendMessage("ClosePopUpByName",( EscapeButonFunctionStack.Peek().Split('*'))[1], SendMessageOptions.DontRequireReceiver);

				}
				else if( EscapeButonFunctionStack.Count ==1 && EscapeButonFunctionStack.Peek() == "btnPauseClicked")
				{
					Camera.main.SendMessage("btnPauseClicked", SendMessageOptions.DontRequireReceiver);
				}
				else
				{

					Camera.main.SendMessage(EscapeButonFunctionStack.Pop(), SendMessageOptions.DontRequireReceiver);
				}
			} 
			StartCoroutine("DisableEsc");
		}
	}

	IEnumerator DisableEsc()
	{
		yield return new WaitForSeconds(2);
		bDisableEsc = false;
	}

    public void OpenPrivacyPolicyLink()
    {
        Application.OpenURL(AdsManager.Instance.privacyPolicyLink);
    }

}
