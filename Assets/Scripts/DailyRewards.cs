using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using System.Collections.Generic;

/**
  * Scene:MainScene
  * Object:DailyRewards
  * Description: Skripta koja je zaduzena za DailyRewards, svakog novog dana daje korisniku nagradu, ako dolazi za redom svaki dan nagrada se povecava, cim se prekine niz vraca ga na pravi dan
  **/
public class DailyRewards : MonoBehaviour {

	public static int [] DailyRewardAmount = new int[]{0 ,50, 100, 150, 300, 500,  0};
	 
	public static int LevelReward;
	bool rewardCompleted = false;
//	List<int> availableSixthReward=new List<int>();
	int sixDayCount, typeOfSixReward; // typeOfSixReward 0-stars, 1-blades, 2-bomb, 3-laser, 4-tesla
	Text moneyText;
	 
	private  DateTime quitTime;
	string lastPlayDate,timeQuitString;
  
	MenuItems menuItems;
	public Image imgUnlockedDecoration;
	void Start()
	{
		menuItems = new MenuItems();
	}
	 
	public bool TestDailyRewards()
	{
		bool bDailyReward = false ;
		moneyText = GameObject.Find("DailyReward/AnimationHolder/Body/CoinsHolder/AnimationHolder/Text").GetComponent<Text>();
		moneyText.text = Shop.Coins.ToString() ; // ovde upisujete vrednost koju cuvate za coine
		DateTime currentTime = DateTime.Today;
		 
		if(PlayerPrefs.HasKey("LevelReward"))
		{
			LevelReward=PlayerPrefs.GetInt("LevelReward");
		}
		else
		{
			LevelReward=0;
			 PlayerPrefs.SetInt("LevelReward",0);
		}
		
		if(PlayerPrefs.HasKey("VremeQuit"))
		{
			lastPlayDate=PlayerPrefs.GetString("VremeQuit");
			DateTime dt = DateTime.Parse(lastPlayDate) ;
			quitTime = new DateTime(dt.Year,dt.Month,dt.Day) ;
		 	//*********************************
//			//OBRISI test
//  	quitTime =  DateTime.Today .AddDays(-1) ;
//			//**********************************

		}
		else
		{
			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeQuit", timeQuitString);
			//PlayerPrefs.Save();
		}

		if(quitTime.AddDays(1) == currentTime)
		{
			LevelReward++;

			//if(LevelReward < 5)LevelReward = 5;//TODO Brisi

			if(LevelReward ==7) LevelReward = 1;
			//ShowDailyReward(LevelReward);
			bDailyReward = true;
		}
		else if(quitTime.AddDays(1) < currentTime)
		{
			LevelReward = 1;
			//ShowDailyReward(LevelReward);
			bDailyReward = true;
		}
		else if(quitTime  != currentTime)
		{
			LevelReward = 0;
			PlayerPrefs.SetInt("LevelReward",0);
			timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeQuit", timeQuitString);
		}
		return bDailyReward;
	}




	void OnApplicationPause(bool pauseStatus) { //vraca false kad je aktivna app
		if(pauseStatus)
		{
			//izasao iz aplikacuje
		 	timeQuitString = DateTime.Now.ToString();
			PlayerPrefs.SetString("VremeQuit", timeQuitString);
			PlayerPrefs.Save();
			
		}
		else
		{
			//usao u aplikacuju
			
		}
		
		
	}

	 

	public void ShowDailyReward ( )
	{
		int currentDayReward = LevelReward;
 
		gameObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
		 gameObject.GetComponent<Animator>().SetBool("IsOpen",true);
		MenuManager.bPopUpVisible = true;
		Debug.Log("DDD"+MenuManager.bPopUpVisible);

		GameObject currentDay;
		currentDay = GameObject.Find("Day" + currentDayReward.ToString());

		for(int i = 1;i<currentDayReward; i++)
		{
			GameObject.Find("Day" + i.ToString()).transform.GetComponent<Animator>().SetTrigger("EnableImage");

		}
 
		Animator anim =  currentDay.transform.GetComponent<Animator>();
		anim.SetBool("bDailyRewardDay",true);
		for(int i = 1;i<6; i++)
		{
		 	GameObject.Find("Day" + i.ToString()+"/NumberText").transform.GetComponent<Text>().text = DailyRewardAmount[i].ToString() ;
		}
		//SoundManager.Instance.Play_Sound(SoundManager.Instance.Win);
	}
	
	 
	public IEnumerator moneyCounter(int kolicina)
	{
		int current = int.Parse(moneyText.text);
		int suma = current + kolicina;
		int korak = (suma - current)/10;
		while(current != suma)
		{
			current += korak;
			moneyText.text = current.ToString();
			yield return new WaitForSeconds(0.07f);
		}

		yield return new WaitForSeconds(0.2f);
		//GameObject.Find("DailyReward").GetComponent<Animator>().Play("DailyRewardDeparting");

		gameObject.GetComponent<Animator>().SetBool("IsOpen",false);
		MenuManager.bPopUpVisible = false;
		MenuManager mm = GameObject.Find("Canvas").GetComponent<MenuManager>();
		if(mm!=null && mm.EscapeButonFunctionStack.Count > 0 && mm.EscapeButonFunctionStack.Peek() == "CloseDailyReward") mm.EscapeButonFunctionStack.Pop ();
	}

	void SetActiveDay(int dayNumber)
	{
		 GameObject.Find("Day"+dayNumber+"/Image").GetComponent<Image>().color = new Color(255,255,255,1);	 
	}

	void OnApplicationQuit() {
		timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetString("VremeQuit", timeQuitString);
		PlayerPrefs.Save();

		//Pokreni Notifikaciju za DailyReward na 24h
	}

	public void TakeReward()
	{
		if(!rewardCompleted)
		{
			if(LevelReward!=6)
			{
				StartCoroutine("moneyCounter",DailyRewardAmount[LevelReward]);
			}
			SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
			Shop.Coins +=DailyRewardAmount[LevelReward];
			//ovde cuvajte u playerprefs coine
			DataManager.Instance.SaveLastLevelData();

			rewardCompleted=true;
		}
	}

	public void TakeSixthReward()
	{

		int RandimInt  = Mathf.FloorToInt( UnityEngine.Random.Range(0, MenuItems.lockedMenuItems.Count));

		string key = MenuItems.lockedMenuItems[RandimInt].Name;
		DataManager.Instance.SetUnlockedMenuItems ( key);
		MenuItems.lockedMenuItems.Remove(MenuItems.menuItemsDictionary[key]);


		Sprite[]	ItemiSprites =(Sprite[]) Resources.LoadAll<Sprite>( "Decorations/"+MenuItems.menuItemsDictionary[key].Atlas);
		foreach(Sprite sp in ItemiSprites)
		{
			if(sp.name ==  key ) 
			{
				  imgUnlockedDecoration.sprite = sp;
			}
		}

		//imgUnlockedDecoration

		GameObject.Find("DailyReward/AnimationHolder/Body").GetComponent<Animator>().SetTrigger ("CollectDecoration");
		StartCoroutine(HideSixtDay());

		if(!rewardCompleted)
		{
			rewardCompleted=true;

		
		}

	}

	IEnumerator HideSixtDay()
	{
		yield return new WaitForSeconds(0.8f);
		SoundManager.Instance.Play_Sound(SoundManager.Instance.UnlockNewItem);
		yield return new WaitForSeconds(3.7f);
		gameObject.GetComponent<Animator>().SetBool("IsOpen",false);
		MenuManager.bPopUpVisible = false;

		MenuManager mm = GameObject.Find("Canvas").GetComponent<MenuManager>();
		if(mm!=null)
		{
		 	 
			yield return new WaitForSeconds(1.5f);
			if(mm.EscapeButonFunctionStack.Count > 0 && mm.EscapeButonFunctionStack.Peek() == "CloseDailyReward") mm.EscapeButonFunctionStack.Pop ();
		}
	}


	bool bCollected = false;
	public void Collect()
	{
		if(bCollected) return;
		bCollected = true;
		SoundManager.Instance.Play_ButtonClick();
		GameObject.Find("ButtonCollect").GetComponent<Button>().interactable = false;

		timeQuitString = DateTime.Now.ToString();
		PlayerPrefs.SetString("VremeQuit", timeQuitString);
		PlayerPrefs.SetInt("LevelReward",LevelReward);
		PlayerPrefs.Save();

		if(LevelReward<6)
		{
			TakeReward();
		}
		else
		{
			TakeSixthReward();
		}
	}


	void HideAfterSixtDay()
	{
		//GameObject.Find("DailyReward").GetComponent<Animator>().Play("DailyRewardDeparting");
	}

}
