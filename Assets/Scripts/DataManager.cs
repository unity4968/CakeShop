using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DataManager : MonoBehaviour {

	public static DataManager Instance = null;
	 
	
	public static void InitDataManager()
	{
		GameObject container;
		if(Instance == null) 
		{ 	
			if(GameObject.Find("DATA_MANAGER") == null)
			{
				container = new GameObject();
				container.name = "DATA_MANAGER";
			}
			else 	container = GameObject.Find("DATA_MANAGER");
			 

			if(container.GetComponent<DataManager>() == null) 	
			{
				Instance = container.AddComponent<DataManager>(); 
				Instance.GetPurchasesAndGameStartCount();
				BrojStartovanjaIgre++;
				Instance.SetPurchasesAndGameStartCount();
			}
			else 
				Instance = container.GetComponent<DataManager>();

			DontDestroyOnLoad(container);
			Instance.InitLevelsList();
		}

	}

	public static float animInactiveTime = 1.2f; 
 
	//****************************************
	//GENERAL DATA
	//****************************************
	public int Tutorial = 0; //NIVO NA KOME JE ODGLEDAN TUTORIAL
	public int TutorialMiniGame1 = 0;
	public int TutorialMiniGame2 = 0;
	public string UserId = "";
	public string Username = "";
	public DateTime LastGameDate;
	public string LevelProgress = "";

	public int LastLevel = 1;
	public int LastLevelCustomersServed = 0;
	public int LastLevelCoinsEarned = 0;

	public LevelData SelectedLevelData;
	public int SelectedLevel = 1;
	public int SelectedLevelCustomersServed = 0;
	public int SelectedLevelCoinsEarned = 0;
	
	public int PrevLevelCustomersServed = 0; 	
	public int PrevLevelCoinsEarned = 0;			
	public int PrevLevel = -1;


	public int IngredientPrice = 20;
	public int OvenRepairPrice = 50;

	public int UnlockDecorationPrice = 100;

	public int LastIngredientPriceIncreaseLevel=0;	//NIVO NA KOME JE ZADNJI PUT PROMENJENA CENA SASTOJAKA
	public int LastOvenRepairPriceIncreaseLevel=0;//NIVO NA KOME JE ZADNJI PUT PROMENJENA CENA POPRAVKE

	public Dictionary<int, LevelData>  LevelsList = new Dictionary<int, LevelData>();
	public Dictionary<int, LevelProgressData>  LevelProgressList = new Dictionary<int, LevelProgressData>();

	public static int[] Customers  = new int[] {1,2,0,3,4,5,6,7};

	public int LastCustomerIndex = -1; //  0- 7 , -1 ako nema porudzbine u tom danu,  
	//customer order
	public string  CustomerOrder = "";
	public bool CustomerServed = true; //ako je zavrsen nivo Gameplay4 - true 

	public string  FinishedCakeDecorations = "";

	public static int BrojStartovanjaIgre = 0;


	public void InitLevelsList()
	{
 
		if(LevelsList.Count == 0)
		{

			 //podaci o svim nivoima - koliko ima musterija na odredjenom nivou, koliko je musterija otkljucano, maksimalni broj sastojaka u porudzbini...

			LevelsList.Add(1, new LevelData{  CustomersPerLevel = 1,  UnlockedCustomers = 1,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 4, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
 		 	LevelsList.Add(2, new LevelData{  CustomersPerLevel = 2,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 15, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
 		 	LevelsList.Add(3, new LevelData{  CustomersPerLevel = 2,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 20, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
		 
			//TEST
// 		 	 LevelsList.Add(7, new LevelData{  CustomersPerLevel = 1,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 10, ExpItem = ExpendableItems.Bowl, FixedPrice=  11, PercentPriceIncrease= 0} );
//			LevelsList.Add(8, new LevelData{  CustomersPerLevel = 1,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 10, ExpItem = ExpendableItems.Spoon, FixedPrice= 12, PercentPriceIncrease= 0} );
//			LevelsList.Add(9, new LevelData{  CustomersPerLevel = 1,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 10, ExpItem = ExpendableItems.Bowl, FixedPrice=  11, PercentPriceIncrease= 0} );
//			LevelsList.Add(10, new LevelData{  CustomersPerLevel = 1,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 10, ExpItem = ExpendableItems.Bowl, FixedPrice=  11, PercentPriceIncrease= 0} );


			LevelsList.Add(4, new LevelData{  CustomersPerLevel = 2,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,   CoinsFor3Stars = 20, ExpItem = ExpendableItems.Bowl, FixedPrice= 10, PercentPriceIncrease= 0} );
	
			LevelsList.Add(5, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 25, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(6, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 25, ExpItem = ExpendableItems.Spoon, FixedPrice= 10, PercentPriceIncrease= 0} );
			LevelsList.Add(7, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 25, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(8, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 25, ExpItem = ExpendableItems.Bowl, FixedPrice= 10, PercentPriceIncrease= 0} );
			LevelsList.Add(9, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 30, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(10, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 4,  MaxIngredientsPerCustomer = 2, CoinsPerIngredient = 5,  CoinsFor3Stars = 30, ExpItem = ExpendableItems.Spoon, FixedPrice= 10, PercentPriceIncrease= 0} );


			LevelsList.Add(11, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.Ingredients, FixedPrice= 0, PercentPriceIncrease= 20} );
			LevelsList.Add(12, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.Bowl, FixedPrice= 10, PercentPriceIncrease= 0} );
			LevelsList.Add(13, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(14, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.Spoon, FixedPrice= 10, PercentPriceIncrease= 0} );
			LevelsList.Add(15, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.Oven, FixedPrice= 0, PercentPriceIncrease= 20} );
			LevelsList.Add(16, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.Bowl, FixedPrice= 20, PercentPriceIncrease= 0} );
			LevelsList.Add(17, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(18, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.Spoon, FixedPrice= 20, PercentPriceIncrease= 0} );
			LevelsList.Add(19, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(20, new LevelData{  CustomersPerLevel = 3,  UnlockedCustomers = 5,  MaxIngredientsPerCustomer = 3, CoinsPerIngredient = 5, CoinsFor3Stars = 35, ExpItem = ExpendableItems.Bowl, FixedPrice= 20, PercentPriceIncrease= 0} );

			LevelsList.Add(21, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.SpoonAndIngr, FixedPrice= 20, PercentPriceIncrease= 20} );
			LevelsList.Add(22, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(23, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.Bowl, FixedPrice= 30, PercentPriceIncrease= 0} );
			LevelsList.Add(24, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.Spoon, FixedPrice= 30, PercentPriceIncrease= 0} );
			LevelsList.Add(25, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(26, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.Bowl, FixedPrice= 30, PercentPriceIncrease= 0} );
			LevelsList.Add(27, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.Spoon, FixedPrice= 30, PercentPriceIncrease= 0} );
			LevelsList.Add(28, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(29, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.Bowl, FixedPrice= 40, PercentPriceIncrease= 0} );
			LevelsList.Add(30, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 6,  MaxIngredientsPerCustomer = 4, CoinsPerIngredient = 5, CoinsFor3Stars = 75, ExpItem = ExpendableItems.SpoonAndOven, FixedPrice= 40, PercentPriceIncrease= 20} );
		
			LevelsList.Add(31, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.Ingredients, FixedPrice= 0, PercentPriceIncrease= 20} );
			LevelsList.Add(32, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.Bowl, FixedPrice= 40, PercentPriceIncrease= 0} );
			LevelsList.Add(33, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.Spoon, FixedPrice= 40, PercentPriceIncrease= 0} );
			LevelsList.Add(34, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(35, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.Bowl, FixedPrice= 50, PercentPriceIncrease= 0} );
			LevelsList.Add(36, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.Spoon, FixedPrice= 50, PercentPriceIncrease= 0} );
			LevelsList.Add(37, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(38, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.Bowl, FixedPrice= 50, PercentPriceIncrease= 0} );
			LevelsList.Add(39, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.Spoon, FixedPrice= 50, PercentPriceIncrease= 0} );
			LevelsList.Add(40, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(41, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.BowlAndIngr, FixedPrice= 60, PercentPriceIncrease= 20} );
			LevelsList.Add(42, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.Spoon, FixedPrice= 60, PercentPriceIncrease= 0} );
			LevelsList.Add(43, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(44, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.Bowl, FixedPrice= 60, PercentPriceIncrease= 20} );
			LevelsList.Add(45, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 7,  MaxIngredientsPerCustomer = 5, CoinsPerIngredient = 6, CoinsFor3Stars = 105, ExpItem = ExpendableItems.SpoonAndOven, FixedPrice= 60, PercentPriceIncrease= 20} );

			LevelsList.Add(46, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(47, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.Bowl, FixedPrice= 70, PercentPriceIncrease= 0} );
			LevelsList.Add(48, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.Spoon, FixedPrice= 70, PercentPriceIncrease= 0} );
			LevelsList.Add(49, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(50, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.Bowl, FixedPrice= 70, PercentPriceIncrease= 0} );
			LevelsList.Add(51, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.SpoonAndIngr, FixedPrice= 70, PercentPriceIncrease= 20} );
			LevelsList.Add(52, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(53, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.Bowl, FixedPrice= 80, PercentPriceIncrease= 0} );
			LevelsList.Add(54, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.Spoon, FixedPrice= 80, PercentPriceIncrease= 0} );
			LevelsList.Add(55, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(56, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.Bowl, FixedPrice= 80, PercentPriceIncrease= 0} );
			LevelsList.Add(57, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.Spoon, FixedPrice= 80, PercentPriceIncrease= 0} );
			LevelsList.Add(58, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.None, FixedPrice= 0, PercentPriceIncrease= 0} );
			LevelsList.Add(59, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.Bowl, FixedPrice= 90, PercentPriceIncrease= 0} );
			LevelsList.Add(60, new LevelData{  CustomersPerLevel = 4,  UnlockedCustomers = 8,  MaxIngredientsPerCustomer = 6, CoinsPerIngredient = 6, CoinsFor3Stars = 120, ExpItem = ExpendableItems.Spoon, FixedPrice= 90, PercentPriceIncrease= 0} );

		}
	}





	//PODACI IZ PLAYER PREFS
	
	public  void GetLocalData()
	{
		//string _LastUserId =  PlayerPrefs.GetString("LastUserId",""); 
		//string _LastUsername = PlayerPrefs.GetString("LastUsername","");
		string _LevelProgress = PlayerPrefs.GetString("LevelProgress", "");
 
		//UserId =  Crypto.DecryptStringAES(  _LastUserId); 
		//Username = Crypto.DecryptStringAES(  _LastUsername); 
		LevelProgress = Crypto.DecryptStringAES( _LevelProgress);
		
	 
		PopulateLevelProgressList();

	}



	
	
	public   void SaveLocalData()
	{
		 
 
		//PlayerPrefs.SetString("LastUserId", Crypto.EncryptStringAES(  UserId)  ); 
		//PlayerPrefs.SetString("LastUsername",Crypto.EncryptStringAES(  Username)  ); 
		 
		UpdateLevelsDataString();
 
		PlayerPrefs.SetString("LevelProgress",Crypto.EncryptStringAES(  LevelProgress)  ); 
 
	}




	public void SaveCustomerAndOrder()
	{
		if(LastCustomerIndex ==-1)
			PlayerPrefs.SetString("CustomerOrder","" ); 
		else
			PlayerPrefs.SetString("CustomerOrder",Crypto.EncryptStringAES(  LastCustomerIndex.ToString()+ "*"+CustomerOrder)  ); 

		PlayerPrefs.Save();
	}

	public void GetCustomerAndOrder()
	{

		LastCustomerIndex = -1;
		CustomerOrder = "";
		CustomerServed = true;
	    string tmp =	PlayerPrefs.GetString("CustomerOrder",""  ); 
		if(tmp!="")
		{
			int indexC = -1;
			if(int.TryParse(tmp.Split('*')[0],out indexC))
			{
				LastCustomerIndex = indexC;
				CustomerOrder = tmp.Split('*')[1];
				CustomerServed = false;
			}
		}
		 
	}


	public  void GetLastGameDate()
	{
		string _LastGameDate = PlayerPrefs.GetString("LastGameDate", ""); 
		DateTime _DT = DateTime.Now;
	 
		string __LastGameDate = Crypto.DecryptStringAES(  _LastGameDate);  
		if( DateTime.TryParse(__LastGameDate,out _DT ))  LastGameDate = _DT; 
	}
	
	
	
	
	
	public   void SaveLastGameDate()
	{
		PlayerPrefs.SetString("LastGameDate",  Crypto.EncryptStringAES(LastGameDate.ToString("s"))); 
	}


	public void SaveTutorialProgress( )
	{
		string sTut = Tutorial.ToString()+TutorialMiniGame1.ToString()+TutorialMiniGame2.ToString();
		PlayerPrefs.SetString("TUT", sTut) ;
	}

	public int GetTutorialProgress()
	{

		string sTut = PlayerPrefs.GetString("TUT", "") ;
		if(sTut != "")  
		{
			int _T;
			if( int.TryParse((sTut.Substring(0,1)),out _T) ) Tutorial= _T;
			if( int.TryParse((sTut.Substring(1,1)),out _T) ) TutorialMiniGame1 = _T;
			if( int.TryParse((sTut.Substring(2,1)),out _T) ) TutorialMiniGame2 = _T;
		}
		else   //if(Tutorial ==0)
		{
			SaveTutorialProgress();
			LastLevel = 1;
			//LastLevelCoinsEarned = 0;
			//LastLevelCustomersServed = 0;
			//Shop.Coins = Shop.CoinsStartGame;
			//LevelProgress = "2;1;3;1;1;1;2;3;3;2;3;2;1;2;3;2;1;2;3;2;3;2;3;2;3;2;3;2;1;2;3;2;1;2;3;2;1;2;3;2;1;2;2;3;2;1;2;3;2;1;2;3;2;3;2;3;2;3;2";
			LevelProgress = "0";

			SaveLastLevelData();
			SaveLocalData();
			PlayerPrefs.Save();
		
			PopulateLevelProgressList();
				
		}

		return Tutorial;
	}

	public void SavePriceData()
	{
		string tmp = IngredientPrice.ToString() + ";" + LastIngredientPriceIncreaseLevel.ToString() + ";" + OvenRepairPrice.ToString()+ ";" + LastOvenRepairPriceIncreaseLevel.ToString();
		PlayerPrefs.SetString("Data1",  Crypto.EncryptStringAES(tmp) ); 
	}

	public void GetPriceData()
	{
		string tmp  =   PlayerPrefs.GetString("Data1",  "" );
//		string __tmp = Crypto.DecryptStringAES(  tmp); 

		int _T;

 
		string[] data = tmp.Split(new char[] {';'},StringSplitOptions.RemoveEmptyEntries);
		if(data.Length == 4)
		{
			if( int.TryParse(data[0],out _T) ) IngredientPrice= _T;
			if( int.TryParse(data[1],out _T) ) LastIngredientPriceIncreaseLevel= _T;
			if( int.TryParse(data[2],out _T) ) OvenRepairPrice= _T;
			if( int.TryParse(data[3],out _T) ) LastOvenRepairPriceIncreaseLevel= _T;
		}
		else
		{
			PlayerPrefs.SetString("Data1",  Crypto.EncryptStringAES("20;0;50;0" ) ); 
			IngredientPrice = 20;
			LastIngredientPriceIncreaseLevel= 0;
			OvenRepairPrice= 50;
			LastOvenRepairPriceIncreaseLevel= 0;

		}
	}


	public void SaveLastLevelData()
	{
 
		string tmp = LastLevel.ToString() + ";" + LastLevelCustomersServed.ToString() + ";" + LastLevelCoinsEarned.ToString()+";"+Shop.Coins.ToString();
		PlayerPrefs.SetString("Data2",  Crypto.EncryptStringAES(tmp) ); 
	 
	}
	
	public void GetLastLevelData()
	{
		string tmp  =   PlayerPrefs.GetString("Data2",  "" );
//		string __tmp = Crypto.DecryptStringAES(  tmp); 
		
		int _T;
 
		string[] data = tmp.Split(new char[] {';'},StringSplitOptions.RemoveEmptyEntries);
		if(data.Length == 4)
		{
			if( int.TryParse(data[0],out _T) ) LastLevel= _T;
			if( int.TryParse(data[1],out _T) ) LastLevelCustomersServed= _T;
			if( int.TryParse(data[2],out _T) ) LastLevelCoinsEarned= _T;
			if( int.TryParse(data[3],out _T) ) Shop.Coins= _T;
		}
	}
	


	public void PopulateLevelProgressList()
	{

		if(LevelProgress !="")
		{
			string[] sLevelsData = LevelProgress.Split(new char[] {';'},StringSplitOptions.RemoveEmptyEntries);
				LevelProgressList.Clear();

			 GetLastLevelData();

 			 
			for(int i = 0; i<sLevelsData.Length; i++)
			{
				int _stars = int.Parse(sLevelsData[i]);
				LevelProgressList.Add((i+1), new LevelProgressData() { Stars = _stars } );
			} 
		}

		//ZA SLUCAJ DA SE NE SLAZU
		if(LastLevel != LevelProgressList.Count) {
			LastLevel =  LevelProgressList.Count;
//			LastLevelCoinsEarned = 0;
//			LastLevelCustomersServed = 0;
//			Shop.Coins = Shop.CoinsStartGame;
//			SaveLastLevelData();
		}
	}


	 

	public void UnlockNextLevel(  )
	{
		if(!LevelProgressList.ContainsKey(LastLevel +1))
		{

			LastLevel ++;
			if(LastLevel>LevelsList.Count) LastLevel = LevelsList.Count; //kada se predju svi nivoi...
			else 
			 LevelProgressList.Add(LastLevel, new LevelProgressData() { Stars = 0 } );

			LastLevelCoinsEarned = 0;
			LastLevelCustomersServed = 0;
			 
			StartCoroutine("SaveUnlockedLevel");
		}


	}

	IEnumerator SaveUnlockedLevel()
	{
		SaveLastLevelData();

		if( LastLevel == 10) 
		yield return new WaitForSeconds(0.2f);
		SaveLocalData();
	}


	public void UpdateLevelsDataString()
	{
		 
		for(int i=1;i<=LevelProgressList.Count;i++)
		{
			if(i == 1) LevelProgress = "";
			else LevelProgress +=";";
			
			LevelProgress +=   LevelProgressList[i].Stars ;
			
		}
		
	}


	//ODABIRANJE NIVOA
	public void SelectLevel(int level)
	{
		SelectedLevel = level;
		if(SelectedLevel>LevelsList.Count) SelectedLevel = LevelsList.Count;


		SelectedLevelData = LevelsList[level];

		//AKO SE IGRA POSLEDNJI NIVO
		if(level == LastLevel)
		{
			SelectedLevelCoinsEarned = LastLevelCoinsEarned;
			SelectedLevelCustomersServed = LastLevelCustomersServed;

			if(PrevLevel !=LastLevel)
	 
			{
				CustomerOrder = "";
				CustomerServed = true;
				LastCustomerIndex = -1;
				SaveCustomerAndOrder();
			}

			PrevLevelCoinsEarned = LastLevelCoinsEarned;
			PrevLevelCustomersServed = LastLevelCustomersServed;
			PrevLevel = LastLevel;


		}
		else 	if(level == PrevLevel)//AKO SE IGRA NEKI PRETHODNI NIVO
		{
			SelectedLevelCoinsEarned = PrevLevelCoinsEarned;
			SelectedLevelCustomersServed = PrevLevelCustomersServed;
		}
		else //AKO SE PRVI PUT UKLJUCUJE NEKI PRETHODNI NIVO 
		{
			SelectedLevelCoinsEarned = 0;
			SelectedLevelCustomersServed = 0;

			PrevLevelCoinsEarned = 0;
			PrevLevelCustomersServed = 0;
			PrevLevel = level;

			 CustomerOrder = "";
			 CustomerServed = true;
			 LastCustomerIndex = -1;
			SaveCustomerAndOrder();

		}
	}

	 
	public void GetPurchasesAndGameStartCount() 
	{
		string Data8 = "";
		string _Data8 = PlayerPrefs.GetString("Data8", "");
		
		 
		Data8 = Crypto.DecryptStringAES( _Data8);

 
		if(Data8 !="" && Data8.Length >4)
		{
			Shop.UnbreakableSpoon = int.Parse(Data8.Substring(0,1)); 
			Shop.UnbreakableBowl = int.Parse(Data8.Substring(1,1)); 
			Shop.InfiniteSupplies =  int.Parse(Data8.Substring(2,1)); 
			
			Shop.RemoveAds =   int.Parse(Data8.Substring(3,1)); 

			BrojStartovanjaIgre =   int.Parse(Data8.Substring(4, (Data8.Length -4))); 
		}
		else
		{
			BrojStartovanjaIgre++;
			SetPurchasesAndGameStartCount() ;
		}
	}

	public void SetPurchasesAndGameStartCount() 
	{  
		string Data8 = Shop.UnbreakableSpoon.ToString() +
			Shop.UnbreakableBowl.ToString() +
				Shop.InfiniteSupplies.ToString() +
				Shop.RemoveAds.ToString() +
				BrojStartovanjaIgre.ToString() ;

		PlayerPrefs.SetString("Data8",Crypto.EncryptStringAES(  Data8)  ); 


		PlayerPrefs.Save();
 

	}

	//****************************************
	//GAMEPLAY 1
	//****************************************
	public int NivoPoslednjegLomljenjaVarjace = 0; //-1 ako ne moze da se polomi
	public int PolomljenaVarjaca = 1;//0- jeste ,1 - nije , 2- ne lomi se;
	public int MaxEggs = 10;
	public int MaxMilk = 5;
	public int MaxSugar = 8;
	public int MaxBakiungPowder = 11;
	public int MaxFlour = 7;
	public int MaxSalt = 9;
	public int MaxOil = 10;
	public int NivoPoslednjegLomljenjaCinije = 0;  
	public int PolomljenaCinija = 1;//0- jeste ,1 - nije , 2- ne lomi se;

	public int RemainingEggs = 10;
	public int RemainingMilk = 5;
	public int RemainingSugar = 8;
	public int RemainingBakiungPowder = 11;
	public int RemainingFlour = 7;
	public int RemainingSalt = 9;
	public int RemainingOil = 10;
	public int NamirniceSeNeTrose = 0; //0-ne, 2-da
 
	public void PopulateGameplay1Data()
	{
		string gp1 = PlayerPrefs.GetString("GP1","");

		if(gp1 != "" && gp1.Length ==17)
		{

			NivoPoslednjegLomljenjaVarjace = int.Parse(gp1.Substring(0,2));
			PolomljenaVarjaca = int.Parse(gp1.Substring(2,1) );

			RemainingEggs = int.Parse(gp1.Substring(3,2));
			RemainingMilk = int.Parse(gp1.Substring(5,1));
			RemainingSugar = int.Parse(gp1.Substring(6,1));
			RemainingBakiungPowder = int.Parse(gp1.Substring(7,2));
			RemainingFlour = int.Parse(gp1.Substring(9,1));
			RemainingSalt = int.Parse(gp1.Substring(10,1));
			RemainingOil = int.Parse(gp1.Substring(11,2));

			NivoPoslednjegLomljenjaCinije = int.Parse(gp1.Substring(13,2));
			PolomljenaCinija = int.Parse(gp1.Substring(15,1) );
			NamirniceSeNeTrose= int.Parse(gp1.Substring(16,1) );
		}
		else  SetGameplay1Data();
	}

	public void SetGameplay1Data()
	{
		string gp1 =  
			NivoPoslednjegLomljenjaVarjace.ToString().PadLeft(2,'0')+
		 	PolomljenaVarjaca.ToString()   +
			RemainingEggs.ToString().PadLeft(2,'0')+
			RemainingMilk.ToString()+
			RemainingSugar.ToString()+
			RemainingBakiungPowder.ToString().PadLeft(2,'0')+
			RemainingFlour.ToString()+
			RemainingSalt.ToString()+
			RemainingOil.ToString().PadLeft(2,'0') +
			NivoPoslednjegLomljenjaCinije.ToString().PadLeft(2,'0')+
			PolomljenaCinija.ToString() +
			NamirniceSeNeTrose.ToString();
	 
		PlayerPrefs.SetString("GP1",gp1);

	}



	//****************************************
	//GAMEPLAY 3
	//****************************************

	public int NivoPoslednjegKvaraPeci = 0;
	public int PokvarenaPec = 1;//0- jeste ,1 - nije , 2- ne kvari se;
	public int PreostaliBrojUpotrebaPeci = 5;
 

	public void PopulateGameplay3Data()
	{
		string gp3 = PlayerPrefs.GetString("GP3","");

		if(gp3 != "" && gp3.Length ==5)
		{
			NivoPoslednjegKvaraPeci = int.Parse(gp3.Substring(0,2));
			PokvarenaPec = int.Parse(gp3.Substring(2,1) );
			PreostaliBrojUpotrebaPeci = int.Parse(gp3.Substring(3,2) );
		}
		else  SetGameplay3Data();
	}

	public void SetGameplay3Data()
	{
		string gp3 = NivoPoslednjegKvaraPeci.ToString().PadLeft(2,'0')
			+ PokvarenaPec.ToString()  + PreostaliBrojUpotrebaPeci.ToString().PadLeft(2,'0');;
		
		PlayerPrefs.SetString("GP3",gp3);
		//PlayerPrefs.Save();
	}
 
 


	//****************************************
	//GAMEPLAY 4
	//****************************************

	public string[] MeniOtkljucano = new string[5];
	 


	public void PopulateUnlockedMenuItems()
	{

		MeniOtkljucano[0] = PlayerPrefs.GetString("Data3","");
		MeniOtkljucano[1] = PlayerPrefs.GetString("Data4","");
		MeniOtkljucano[2] = PlayerPrefs.GetString("Data5","");
		MeniOtkljucano[3] = PlayerPrefs.GetString("Data6","");
		MeniOtkljucano[4] = PlayerPrefs.GetString("Data7","");
		
		 
	}
	
	public void SetUnlockedMenuItems(  string _name)
	{
		int _menu = int.Parse( _name.Split('_')[0].Remove(0,1));
		string item= _name.Split('_')[1];
		MeniOtkljucano[_menu-1] += (item+";");
		switch( _menu)
		{
			case 1: PlayerPrefs.SetString("Data3",MeniOtkljucano[0] ); break;
			case 2: PlayerPrefs.SetString("Data4",MeniOtkljucano[1] ); break;
			case 3: PlayerPrefs.SetString("Data5",MeniOtkljucano[2] ); break;
			case 4: PlayerPrefs.SetString("Data6",MeniOtkljucano[3] ); break;
			case 5: PlayerPrefs.SetString("Data7",MeniOtkljucano[4] ); break;
		}
	 
		//PlayerPrefs.Save();
	}
}


//*************************************************************

//PODACI O SVAKOM NIVOU
public class LevelData
{
	 
	public int CustomersPerLevel = 0;
	public int UnlockedCustomers = 0;
	public int MaxIngredientsPerCustomer = 0;
	public int CoinsPerIngredient = 0;
	public int CoinsFor3Stars = 0;
	public ExpendableItems ExpItem = ExpendableItems.None;
	public int FixedPrice= 0;
	public int PercentPriceIncrease= 0;
}

//PODACI O SVAKOM ODIGRANOM NIVOU
public class LevelProgressData
{
	public int Stars =0;
}

public enum ExpendableItems
{
	None,
	Spoon,
	Bowl,
	Oven,
	Ingredients,
	SpoonAndOven,
	SpoonAndIngr,
	BowlANdOven,
	BowlAndIngr


}
 



 
