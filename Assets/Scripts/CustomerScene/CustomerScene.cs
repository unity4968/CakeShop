using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class CustomerScene : MonoBehaviour {


	public MenuManager menuManager;
//	public GameObject PopUpOrder;
	public GameObject PopUpLevelComplete;

	MenuItems menuItems;


	//public Image OrderImgSprite;

	public CharacterChanger characterChanger;
	public Animator animChar;

	public GameObject CustomerOrderMenu;

	public Image CakeBase;
	public Image Cream;
	public Image Decoration1;
	public Image Decoration2;
	public Image Decoration3;
	public Image Decoration4;
	public Image Decoration5;
	public Image Decoration6;

	Vector2[] orderMenuDecorationsPositions = new Vector2[8];

	Sprite[] ItemiSprites ;
	StatusBar statusBar;

	public Text[] txtCoins;
	public Text[] txtCoinsPerDay;
	public Text txtLevelWin;

	public static int PrevCustomerIndex = -1;
	public ParticleSystem psOrder;

	static int OrderCounter = 0; //potrebno je da se u svakoj drugoj porudzbini ubaci neki sastojak koji nije otklucan

	public Animator animWinMenu;
	public GameObject[] WinPopupStars;


	void Awake()
	{

		Time.timeScale = 1;
		DataManager.InitDataManager();
 
		DataManager.Instance.GetCustomerAndOrder();

		Shop.InitShop();
		Shop.Instance.txtDispalyCoins = txtCoins;


		orderMenuDecorationsPositions[0] =  CakeBase.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[1] =  Cream.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[2] =  Decoration1.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[3] =  Decoration2.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[4] =  Decoration3.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[5] =  Decoration4.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[6] =  Decoration5.rectTransform.anchoredPosition;
		orderMenuDecorationsPositions[7] =  Decoration6.rectTransform.anchoredPosition;

		CustomerOrderMenu.SetActive(false);

		 
	}

	// Use this for initialization
	void Start () {



		psOrder.Stop();
		psOrder.GetComponent<Renderer>().sortingOrder = -2;
		foreach(Transform ch in psOrder.transform)
		{
			ch.GetComponent<Renderer>().sortingOrder = -2;
		}

		//********************
		statusBar = GameObject.Find("StatusBar").GetComponent<StatusBar>();

		statusBar.Init(DataManager.Instance.SelectedLevel,DataManager.Instance.SelectedLevelCoinsEarned, DataManager.Instance.SelectedLevelCustomersServed, 
		               DataManager.Instance.SelectedLevelData.CustomersPerLevel, Shop.Coins);
		//

		//********************

		if( animChar == null ) animChar = characterChanger.transform.GetChild(0).GetComponent <Animator>();
		//StartCoroutine(CharacterChanger());

		Time.timeScale = 1;
		StartCoroutine(WaitToShowCustomer());



		menuItems = new MenuItems();

		if(	!SoundManager.Instance.menuMusic.isPlaying)
		{
			SoundManager.Instance.Stop_GameplayMusic();
			SoundManager.Instance.Play_MenuMusic();
		}

		statusBar.SetPosition();
		menuManager.EscapeButonFunctionStack.Push("btnHomeClicked");
	}

	IEnumerator WaitToShowCustomer()
	{
 
		if(DataManager.Instance.LastCustomerIndex == -1 ) 
		{
			//yield return new WaitForSeconds(.2f);
			//statusBar.SetPosition();
			yield return new WaitForSeconds(1.8f);
		}
		yield return new WaitForSeconds(.2f);
		ShowCustomer();
		yield return new WaitForSeconds(.2f);
		statusBar.SetPosition();
	}


	void ShowCustomer()
	{
		try
		{
			if( DataManager.Instance.LastCustomerIndex == -1  )// && DataManager.Instance.CustomerServed ==true ) //nije odabrana musterija
			{
				int indexC = PrevCustomerIndex;
				int i = 0;
				while(indexC == PrevCustomerIndex && i<100)
				{
					i++;
					indexC =  Mathf.FloorToInt(  Random.Range(  0,  DataManager.Instance.SelectedLevelData.UnlockedCustomers));
				}
				DataManager.Instance.LastCustomerIndex = indexC;
				characterChanger.ChangeCharacter(    DataManager.Customers [DataManager.Instance.LastCustomerIndex]);
				if( animChar == null ) animChar = characterChanger.transform.GetChild(0).GetComponent <Animator>();
				animChar.Play("CharacterIArriving");
	 
				CreateCakeOrder();

				DataManager.Instance.CustomerServed = false;
				DataManager.Instance.SaveCustomerAndOrder();
				SoundManager.Instance.Play_Sound(SoundManager.Instance.CustomerArrival);
				 

			}
			else
			{
				if( animChar == null ) animChar = characterChanger.transform.GetChild(0).GetComponent <Animator>();
				if( DataManager.Customers.Length <= DataManager.Instance.LastCustomerIndex )
				{
					DataManager.Instance.LastCustomerIndex = -1;
					ShowCustomer();
					return;
				}
				characterChanger.ChangeCharacter(   DataManager.Customers [DataManager.Instance.LastCustomerIndex]);
				animChar.Play("CharacterIdle");
			}


			StartCoroutine(ShowCustomerOrder());
		}
		catch
		{
			Debug.Log("GRESKA PRILIKOM PRIKAZIVANJA MUSTERIJE");
			DataManager.Instance.LastCustomerIndex = -1;
			LevelTransition.Instance.HideScene("CustomerScene");
		}
	}


	void CreateCakeOrder()
	{

		if(DataManager.Instance.Tutorial <4)
		{
			DataManager.Instance.CustomerOrder = "m1_01;m2_02;m4_04";
			return;
		}

		OrderCounter++;

		bool bSearch = true;
		string key = "";
		 
		int item = 0;
		int Esc = 0;
		DecorationType decorationType = DecorationType.Empty;
		//BAZA

		while(bSearch  && Esc <100000)
		{
			item = Mathf.FloorToInt( Random.Range(1,16));
			key = "m1_"+item.ToString().PadLeft(2,'0');
			if(MenuItems.menuItemsDictionary.ContainsKey(key) && MenuItems.menuItemsDictionary[key].Locked == false)
			{
				bSearch = false;
				DataManager.Instance.CustomerOrder = key+";";
				decorationType = MenuItems.menuItemsDictionary[key].decorationType;
			}
			Esc++;
		}

		int creamInCustomerOrder = Mathf.FloorToInt( Random.Range( 1, 5) );
		if(creamInCustomerOrder >3) creamInCustomerOrder = 2;
		else creamInCustomerOrder = 1;
		//CREAM
		if(creamInCustomerOrder == 2)
		{
			bSearch = true;
			Esc = 0;
			while(bSearch && Esc <100000)
			{
				if(decorationType == DecorationType.CakeShapeCylinder)
					item = Mathf.FloorToInt( Random.Range(1,6));

				if(decorationType == DecorationType.CakeShapeStar)
					item = Mathf.FloorToInt( Random.Range(6,11));

				if(decorationType == DecorationType.CakeShapeHeart)
					item = Mathf.FloorToInt( Random.Range(11,16));

				key = "m2_"+item.ToString().PadLeft(2,'0');
				if(MenuItems.menuItemsDictionary.ContainsKey(key) && MenuItems.menuItemsDictionary[key].Locked == false)
				{
					bSearch = false;
					DataManager.Instance.CustomerOrder += key+";";
				}
				Esc++;
			}
		}


//		//perle se ne zadaju
//		bSearch = true;
//		Esc = 0;
//		while(bSearch && Esc <100000)
//		{
//			 
//			item = Mathf.FloorToInt( Random.Range(1,33));
//			
//			key = "m3_"+item.ToString().PadLeft(2,'0');
//			if(MenuItems.menuItemsDictionary.ContainsKey(key) && MenuItems.menuItemsDictionary[key].Locked == false)
//			{
//				bSearch = false;
//				DataManager.Instance.CustomerOrder += key+";";
//			}
//			Esc++;
//		}


		//izbor ukrasa

		int _menu = 4;
		for (int i = creamInCustomerOrder; i<DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer;i++)
		{
			bSearch = true;
			_menu = Mathf.FloorToInt( Random.Range(4,6));
			Esc = 0;

			while(bSearch && Esc <100000)
			{

				if(_menu ==4 ) item = Mathf.FloorToInt( Random.Range(1,16));
				if(_menu ==5 ) item = Mathf.FloorToInt( Random.Range(1,49));
			
				key = "m"+_menu.ToString()+"_"+item.ToString().PadLeft(2,'0');

 
				if(MenuItems.menuItemsDictionary.ContainsKey(key) &&  MenuItems.menuItemsDictionary[key].Locked == false)
				{
					bSearch = false;
					DataManager.Instance.CustomerOrder += key+";";
				}
				Esc++;
			}

		}
 
	 
		if(DataManager.Instance.SelectedLevel <6 || ( DataManager.Instance.SelectedLevel >=6 && OrderCounter %2  == 0) )
		{
			bSearch = true;
			_menu = Mathf.FloorToInt( Random.Range(4,6));
			Esc = 0;
			
			//SVI ITEMI SU  IZ GRUPE OTKLJUCANIH
			while(bSearch && Esc <100000)
			{
				if(_menu ==4 ) item = Mathf.FloorToInt( Random.Range(1,16));
				if(_menu ==5 ) item = Mathf.FloorToInt( Random.Range(1,49));
				
				key = "m"+_menu.ToString()+"_"+item.ToString().PadLeft(2,'0');

				if(MenuItems.menuItemsDictionary.ContainsKey(key) &&  MenuItems.menuItemsDictionary[key].Locked == false)
				{
					bSearch = false;
					DataManager.Instance.CustomerOrder += key+";";
				}
				Esc++;
			}
		}
		else
		{

			//POSLEDNJI ITEM JE ZAKLJUCAN
		 
			_menu = Mathf.FloorToInt( Random.Range(4,6));

			if(_menu ==4 ) item = Mathf.FloorToInt( Random.Range(1,16));
			if(_menu ==5 ) item = Mathf.FloorToInt( Random.Range(1,49));
			
			//key = "m"+_menu.ToString()+"_"+item.ToString().PadLeft(2,'0');


			List <MenuItemData> lockedMenuItemsGroup  = new List< MenuItemData>();
			foreach(MenuItemData it in  MenuItems.lockedMenuItems)
			{
				if(it.Name.StartsWith(  "m"+_menu.ToString()+"_"))
				{
					lockedMenuItemsGroup.Add(it);
				}
			}

			if(lockedMenuItemsGroup.Count >0)
			{
				int ind =  Mathf.FloorToInt( Random.Range(0,lockedMenuItemsGroup.Count));
				key = lockedMenuItemsGroup[ind].Name;
			}
			else
			{
				if(_menu ==4 ) _menu = 5;
				else _menu = 4;

				foreach(MenuItemData it in  MenuItems.lockedMenuItems)
				{
					if(it.Name.StartsWith(  "m"+_menu.ToString()+"_"))
					{
						lockedMenuItemsGroup.Add(it);
					}
				}

				if(lockedMenuItemsGroup.Count >0)
				{
					int ind =  Mathf.FloorToInt( Random.Range(0,lockedMenuItemsGroup.Count));
					key = lockedMenuItemsGroup[ind].Name;
				}
				else
				{
					_menu = Mathf.FloorToInt( Random.Range(4,6));
					if(_menu ==4 ) item = Mathf.FloorToInt( Random.Range(1,16));
					if(_menu ==5 ) item = Mathf.FloorToInt( Random.Range(1,49));
					
					key = "m"+_menu.ToString()+"_"+item.ToString().PadLeft(2,'0');
					 

				}
			}
 
			if(MenuItems.menuItemsDictionary.ContainsKey(key)  )
			{
				bSearch = false;
				DataManager.Instance.CustomerOrder += key+";";
				 
			}
			 
			//*******kraj poslednji element
			 

		}
 
	}

	bool noCream = false;
	void SetOrderImages()
	{ 
	 
		string[] items = DataManager.Instance.CustomerOrder.Split(';');
		Cream.enabled = false;

		if(!items[1].StartsWith("m2") )noCream = true;
		else noCream = false;

		for(int i = 0; i<items.Length;i++)
		{

			string key = items[i];
			 


			if(MenuItems.menuItemsDictionary.ContainsKey(key))//&& MenuItems.menuItemsDictionary[key].Locked == false)
			{
				ItemiSprites =(Sprite[]) Resources.LoadAll<Sprite>( "Decorations/"+MenuItems.menuItemsDictionary[key].Atlas);
				foreach(Sprite sp in ItemiSprites)
				{
					if(noCream)
					{
						if(sp.name ==  key ) 
						{
							if(i == 0) CakeBase.sprite = sp;
							if(i == 1 ) Decoration1.sprite = sp;
							if(i == 2) Decoration2.sprite = sp;
							if(i == 3) Decoration3.sprite = sp;
							if(i == 4) Decoration4.sprite = sp;
							if(i == 5) Decoration5.sprite = sp;
							if(i == 6) Decoration6.sprite = sp;
						}
					}
					else
					{
						if(sp.name ==  key ) 
						{
							if(i == 0) CakeBase.sprite = sp;
							if(i == 1  )   { Cream.sprite = sp; Cream.enabled = true;}
							if(i == 2) Decoration1.sprite = sp;
							if(i == 3) Decoration2.sprite = sp;
							if(i == 4) Decoration3.sprite = sp;
							if(i == 5) Decoration4.sprite = sp;
							if(i == 6) Decoration5.sprite = sp;
							if(i == 7) Decoration6.sprite = sp;
						}
					}
				}
			}
		}

	}


	IEnumerator ShowCustomerOrder()
	{
		//DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer =2;

		CakeBase.rectTransform.anchoredPosition = orderMenuDecorationsPositions[0];
		Cream.rectTransform.anchoredPosition = orderMenuDecorationsPositions[1] ;
		Decoration1.rectTransform.anchoredPosition = orderMenuDecorationsPositions[2];
		Decoration2.rectTransform.anchoredPosition = orderMenuDecorationsPositions[3];
		Decoration3.rectTransform.anchoredPosition = orderMenuDecorationsPositions[4];
		Decoration4.rectTransform.anchoredPosition = orderMenuDecorationsPositions[5];
		Decoration5.rectTransform.anchoredPosition = orderMenuDecorationsPositions[6];
		Decoration6.rectTransform.anchoredPosition = orderMenuDecorationsPositions[7];

		SetOrderImages();




		if(!noCream )
		{
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 2)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = false;
				Decoration3.enabled = false;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( -Decoration1.rectTransform.anchoredPosition.x,60);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 10,30);
				Cream.rectTransform.anchoredPosition+=new Vector2( 0,30);
			}
			else if (DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 3)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = false;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( -Decoration1.rectTransform.anchoredPosition.x,10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2( -Decoration2.rectTransform.anchoredPosition.x,25);
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 10,15);
				Cream.rectTransform.anchoredPosition+=new Vector2( 0,15);
			}
			
			if(     DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 4)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2(- Decoration1.rectTransform.anchoredPosition.x,0);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(-Decoration2.rectTransform.anchoredPosition.x,0);
				Decoration3.rectTransform.anchoredPosition+=new Vector2(-Decoration3.rectTransform.anchoredPosition.x,0);
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 10,0);
				Cream.rectTransform.anchoredPosition+=new Vector2( 0,0);
			}
			
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 5)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = false;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( 0,10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(0,25);
				Decoration4.rectTransform.anchoredPosition+=new Vector2( 0,10);
				Decoration5.rectTransform.anchoredPosition+=new Vector2(0,25);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 10,15);
				Cream.rectTransform.anchoredPosition+=new Vector2( 0,15);
			}
			
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 6)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = false;
				
				Decoration3.rectTransform.anchoredPosition+=new Vector2( -Decoration3.rectTransform.anchoredPosition.x,0);
				CakeBase.rectTransform.anchoredPosition+=new Vector2( 10,0);
				Cream.rectTransform.anchoredPosition+=new Vector2(0,0);
			}
			
			//*************************************************************************
		}
		else
		{
			if(  DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 2)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = false;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( -Decoration1.rectTransform.anchoredPosition.x,10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2( -Decoration2.rectTransform.anchoredPosition.x,25);
				CakeBase.rectTransform.anchoredPosition+=new Vector2( -10,15);
				
			}
			
			
			if (DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 3)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = false;
				Decoration5.enabled = false;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2(- Decoration1.rectTransform.anchoredPosition.x,-10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(-Decoration2.rectTransform.anchoredPosition.x,-10);
				Decoration3.rectTransform.anchoredPosition+=new Vector2(-Decoration3.rectTransform.anchoredPosition.x,-10);
				CakeBase.rectTransform.anchoredPosition+=new Vector2( -10,0);
			}
			
			
			
			if(     DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 4)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = false;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( 0,10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(0,25);
				Decoration4.rectTransform.anchoredPosition+=new Vector2( 0,10);
				Decoration5.rectTransform.anchoredPosition+=new Vector2(0,25);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( -10,15);
				
			}
			
			
			
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 5)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = false;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(0,-10);
				Decoration3.rectTransform.anchoredPosition+=new Vector2( -Decoration3.rectTransform.anchoredPosition.x,-10);
				Decoration4.rectTransform.anchoredPosition+=new Vector2(0,-10);
				Decoration5.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( -10,0);
			}
			
			
			if(   DataManager.Instance.SelectedLevelData.MaxIngredientsPerCustomer == 6)
			{
				Decoration1.enabled = true;
				Decoration2.enabled = true;
				Decoration3.enabled = true;
				Decoration4.enabled = true;
				Decoration5.enabled = true;
				Decoration6.enabled = true;
				
				Decoration1.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				Decoration2.rectTransform.anchoredPosition+=new Vector2(0,-10);
				Decoration3.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				Decoration4.rectTransform.anchoredPosition+=new Vector2(0,-10);
				Decoration5.rectTransform.anchoredPosition+=new Vector2( 0,-10);
				Decoration6.rectTransform.anchoredPosition+=new Vector2(0,-10);
				
				CakeBase.rectTransform.anchoredPosition+=new Vector2( -10,0);
				
				
			}
			
		}



		yield return new WaitForSeconds(.18f);
		CustomerOrderMenu.SetActive(true);

		if(DataManager.Instance.FinishedCakeDecorations!="" ) StartCoroutine(WaitTestCustomerServed ());//  TestIsCustomerServed();

	}

 
	public void btnCustomerOrder_Yes( )
	{
		BlockAll.blocksRaycasts = true;
//		menuManager.ClosePopUpMenu(PopUpOrder);
		StartCoroutine("WaitToLoadGameplay");
		SoundManager.Instance.Play_ButtonClick();

		psOrder.Stop();
		psOrder.Play();
	}

	IEnumerator WaitToLoadGameplay()
	{
		yield return new WaitForSeconds(1f);
		 LevelTransition.Instance.HideScene("Gameplay1");
	}



	IEnumerator WaitToLoadLevel(string leveName, float timeW = 0)
	{
		yield return new WaitForSeconds(timeW);
		//Application.LoadLevel( leveName );
		LevelTransition.Instance.HideScene(leveName);
	}

	IEnumerator WaitTestCustomerServed()
	{
		yield return new WaitForSeconds(1.1f);
		TestIsCustomerServed();
	}

	void TestIsCustomerServed()
	{

		//*************************************************************
		//ocenjivanje
		CustomerOrderMenu.SetActive(false);

		string[] TrazeniSastojci = DataManager.Instance.CustomerOrder.Split(new char[]{';'}, System.StringSplitOptions.RemoveEmptyEntries);
		string[] IskorisceniSastojci = DataManager.Instance.FinishedCakeDecorations.Split(new char[]{';'}, System.StringSplitOptions.RemoveEmptyEntries);
		int pogodjeno = 0;
		for(int i = 0;i<TrazeniSastojci.Length;i++)
		{
			for(int j = 0;j<IskorisceniSastojci.Length;j++)
			{
				if(TrazeniSastojci[i]==IskorisceniSastojci[j])
				{
					IskorisceniSastojci[j] = "-";
					//if( !TrazeniSastojci[i].StartsWith("m1")    )
					//{
						pogodjeno++;
				//	}
					break;
				}
			}
		}
		//************************************************************
		DataManager.Instance.FinishedCakeDecorations = "";

		//povecavanje novicica

		int EarnedCoins = pogodjeno * DataManager.Instance.SelectedLevelData.CoinsPerIngredient;  //TODO
 
		Shop.Instance.txtFields = txtCoinsPerDay;
		Shop.Instance.AnimiranjeDodavanjaVrednosti (   DataManager.Instance.SelectedLevelCoinsEarned, EarnedCoins,    "" );
		DataManager.Instance.SelectedLevelCoinsEarned +=EarnedCoins;

		Shop.Instance.txtDispalyCoins = txtCoins;
		Shop.Instance.AnimiranjeDodavanjaNovcica(EarnedCoins, null ,"");


		//Shop.Coins +=EarnedCoins;


		//azuriranje interfejsa
		if( DataManager.Instance.LastCustomerIndex >=0 && DataManager.Instance.LastCustomerIndex < DataManager.Customers.Length)
		{
			if(DataManager.Customers [DataManager.Instance.LastCustomerIndex] == 3 ||  DataManager.Customers [DataManager.Instance.LastCustomerIndex] == 4 
			   ||  DataManager.Customers [DataManager.Instance.LastCustomerIndex] == 5 ||  DataManager.Customers [DataManager.Instance.LastCustomerIndex] == 6)
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.CustomerHappyDeparting);
			}
			else
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.CustomerHappyDeparting_female);
			}
		}

		PrevCustomerIndex = DataManager.Instance.LastCustomerIndex;
		DataManager.Instance.CustomerOrder = "";
		DataManager.Instance.CustomerServed = true;
		DataManager.Instance.LastCustomerIndex = -1;


		if( animChar == null ) animChar = characterChanger.transform.GetChild(0).GetComponent <Animator>();
		animChar.Play("CharacterHappyDeparting");



		DataManager.Instance.SelectedLevelCustomersServed++;


		statusBar.Init(DataManager.Instance.SelectedLevel,DataManager.Instance.SelectedLevelCoinsEarned, DataManager.Instance.SelectedLevelCustomersServed, 
		               DataManager.Instance.SelectedLevelData.CustomersPerLevel, Shop.Coins);
		//da li je nivo zavrsen
		if(DataManager.Instance.SelectedLevelCustomersServed == DataManager.Instance.SelectedLevelData.CustomersPerLevel)
		{
			int stars = 0;
			if(DataManager.Instance.SelectedLevelCoinsEarned >=  DataManager.Instance.SelectedLevelData.CoinsFor3Stars) stars = 3;
			else if(DataManager.Instance.SelectedLevelCoinsEarned >  (  DataManager.Instance.SelectedLevelData.CoinsPerIngredient) ) stars = 2;
			else if(DataManager.Instance.SelectedLevelCoinsEarned >0) stars = 1;

			//if(DataManager.Instance.SelectedLevelCoinsEarned >= ( DataManager.Instance.SelectedLevelData.CoinsFor3Stars - 2*DataManager.Instance.SelectedLevelData.CoinsPerIngredient) )  stars = 1;
			//	pogodjeno * DataManager.Instance.SelectedLevelData.CoinsPerIngredient




			if(stars>0)
			{

				txtLevelWin.text =  DataManager.Instance.SelectedLevel.ToString();
				//AZURIRANJE ZVEZDICA
				if( stars> DataManager.Instance.LevelProgressList[DataManager.Instance.SelectedLevel ].Stars ) 
				{
					DataManager.Instance.LevelProgressList[DataManager.Instance.SelectedLevel ].Stars  = stars;
					DataManager.Instance.SaveLocalData();
				}

				if(DataManager.Instance.SelectedLevel ==DataManager.Instance.LastLevel )
				{
 
					//otkljucavanje nivoa
					DataManager.Instance.UnlockNextLevel(); 
				}
			}
				
			StartCoroutine(ShowWinMenu(stars));
		 
		}
		else
		{
			Time.timeScale = 1;
			StartCoroutine(WaitToShowCustomer());
			if(DataManager.Instance.SelectedLevel ==DataManager.Instance.LastLevel )
			{
				DataManager.Instance.LastLevelCustomersServed = DataManager.Instance.SelectedLevelCustomersServed;
				DataManager.Instance.LastLevelCoinsEarned = DataManager.Instance.SelectedLevelCoinsEarned;

				 
			}
			else
			{
				DataManager.Instance.PrevLevelCustomersServed = DataManager.Instance.SelectedLevelCustomersServed;
				DataManager.Instance.PrevLevelCoinsEarned = DataManager.Instance.SelectedLevelCoinsEarned;

			}
			//DataManager.Instance.SaveLastLevelData();
		}

	}

	IEnumerator ShowWinMenu(  int stars)
	{
		yield return new WaitForSeconds(1);
		BlockAll.blocksRaycasts = false;
		StartCoroutine(SetBlockAll(1.5f,false));

		for (int i = 0;i<3;i++)
		{
			//WinPopupStars[i].SetActive(stars>i);
			WinPopupStars[i].GetComponent<Image>().enabled = (stars > i);
		}
 
		menuManager.ShowPopUpMenu(PopUpLevelComplete);
		StartCoroutine(SetWinMenuTrigger(stars>0));
		//SoundManager.Instance.Play_Sound(SoundManager.Instance.Win);
		SoundManager.Instance.PlayWin_Sound( );


	}

	IEnumerator SetWinMenuTrigger(bool bWin)
	{
		yield return new WaitForEndOfFrame();
		 
		if(bWin) animWinMenu.SetTrigger("tWin");	
		else animWinMenu.SetTrigger("tLose");
	}



	public void btnHomeClicked()
	{
		 
		DataManager.Instance.SelectLevel(   DataManager.Instance.LastLevel );
		 


		BlockAll.blocksRaycasts = true;
		//StartCoroutine(WaitToLoadLevel("MapScene",1));
		StartCoroutine(WaitToLoadLevel("MainScene",1));
		 SoundManager.Instance.Play_ButtonClick();
		StartCoroutine(SetBlockAll(1.5f,false));
		// DataManager.IncrementButtonClickCount();
	}
	

 
	public void btnReplyClicked()
	{
		DataManager.Instance.CustomerOrder = "";
		DataManager.Instance.CustomerServed = true;
		DataManager.Instance.LastCustomerIndex = -1;

		DataManager.Instance.PrevLevel = 1;
		DataManager.Instance.LastLevel =  DataManager.Instance.LevelProgressList.Count;

		BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(1.5f,false));
		DataManager.Instance.SelectLevel(  DataManager.Instance.SelectedLevel );
		StartCoroutine(WaitToLoadLevel("CustomerScene",1));
		 SoundManager.Instance.Play_ButtonClick();
		 
	}
	
	public void btnLoadNextClicked()
	{
		 BlockAll.blocksRaycasts = true;
		StartCoroutine(SetBlockAll(1.5f,false));

		if(DataManager.Instance.SelectedLevel +1 >60)
		{
			BlockAll.blocksRaycasts = true;
			StartCoroutine(WaitToLoadLevel("MapScene",1));
			SoundManager.Instance.Play_ButtonClick();
			StartCoroutine(SetBlockAll(1.5f,false));
            AdsManager.Instance.ShowInterstitial();
			return;
		}
		   
		if(DataManager.Instance.SelectedLevel +1 == DataManager.Instance.LastLevel )
		{
			DataManager.Instance.SelectLevel(   DataManager.Instance.LastLevel );
			StartCoroutine(WaitToLoadLevel("CustomerScene",1));
		}
		else
		{
			DataManager.Instance.SelectLevel( DataManager.Instance.SelectedLevel +1);
			StartCoroutine(WaitToLoadLevel("CustomerScene",1));
		}
 
		 SoundManager.Instance.Play_ButtonClick();
	}


	//*********************************************
	public CanvasGroup BlockAll;
	public GameObject popUpShop;
 
	
	public void ClosePopUpShop()
	{
		BlockAll.blocksRaycasts = true;
		menuManager.ClosePopUpMenu (popUpShop);
		StartCoroutine(SetBlockAll(DataManager.animInactiveTime,false));
		SoundManager.Instance.Play_ButtonClick();
	}
	
 
	
	IEnumerator SetBlockAll(float time, bool blockRays)
	{
		
		if(BlockAll == null) BlockAll = GameObject.Find("ForegroundBlockAll").GetComponent<CanvasGroup>();
		yield return new WaitForSeconds(time);
		BlockAll.blocksRaycasts = blockRays;
		
	}

	public void btnServedClicked()
	{
		DataManager.Instance.FinishedCakeDecorations = DataManager.Instance.CustomerOrder;
		TestIsCustomerServed();
 
	}

	public void btnShowHideBanner()
	{
		
		GameObject.Find("StatusBar").GetComponent<StatusBar>().SetPosition();
	}

}
