using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

	public static Shop Instance = null;


	public static int CoinsStartGame = 200; //novcici na pocetku igre
	public static int Coins = 0;

	public int CoinsToAdd  = 0;
	public int CoinsToAddStart = 0;


	public static int UnbreakableSpoon = 0;
	public static int UnbreakableBowl = 0;
	public static int InfiniteSupplies = 0;

	public static int RemoveAds = 0;
	public bool bShopWatchVideo = false;

	public string ShopItemID = "";

	public Text[] txtDispalyCoins; //SVA POLJA NA SCENI KOJA TREBA DA SE AZURIRAJU PRILIKOM DODAVANJA ILI ODUZIMANJA NOVCICA

	public static void InitShop()
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
			
			
			if(container.GetComponent<Shop>() == null) 	
				Instance = container.AddComponent<Shop>(); 
			else 
				Instance = container.GetComponent<Shop>();
			
			DontDestroyOnLoad(container);
		}
		 
	}



	//***************************WATCH VIDEO******************
	public void WatchVideo( )
	{
 		//zahtev da se prikaze video
		//Debug.Log(  "WATCH VIDEO");
 		AdsManager.Instance.bPlayVideoReward = true;
        AdsManager.Instance.IsVideoRewardAvailable();
 
		//odgovor - brisi
		 // FinishWatchingVideo(true);
	}
	
	public void FinishWatchingVideo(bool bVideoOdgledan)
	{
		 // potvrda da je odgledan video...
		//poziva se iz native...
         
        if(bShopWatchVideo)
        {
			bShopWatchVideo = false;
			AnimiranjeDodavanjaNovcica( 50 ,null,""); //KADA SE ODGLEDA VIDEO DODAJU SE 50 NIVCICA 
		}
		else
			Camera.main.SendMessage("EndWatchingVideo", bVideoOdgledan ,SendMessageOptions.DontRequireReceiver);
        
		SoundManager.Instance.Coins.Stop();
		SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);

	}

	//***************************************************************
	//ODBROJAVANJE NOVCICA
	 
	public void AnimiranjeDodavanjaNovcica(int _CoinsToAdd,  Text txtCoins  = null , string message = "COINS: " )
	{

        CoinsToAddStart =  Coins;
        Coins +=_CoinsToAdd;
        CoinsToAdd = _CoinsToAdd;

        if(txtCoins !=null)
        {
            StartCoroutine(animShopCoins(txtCoins, message ));
        }
        else 
            StartCoroutine(animShopCoinsAllTextFilds( message ));

        SoundManager.Instance.Coins.Stop();
        SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
	}

	IEnumerator animShopCoins( Text txtCoins , string message  )
	{
		//AUDIO.PlaySound(  "shop_coin");
		int  CoinsToAddProg=0;

		int addC = 0;
		int stepUL = Mathf.FloorToInt(CoinsToAdd*0.175f);
		int stepLL = Mathf.FloorToInt(CoinsToAdd*0.19f);
 
		while( (Mathf.Abs(CoinsToAddProg) + Mathf.Abs(addC)) < Mathf.Abs(CoinsToAdd) )
		{
			CoinsToAddProg+=addC;
			txtCoins.text = message+  (CoinsToAddStart + CoinsToAddProg).ToString();
			//Debug.Log(CoinsToAddStart + CoinsToAddProg);
			yield return new WaitForSeconds (0.05f);
			addC = Mathf.FloorToInt(UnityEngine.Random.Range(stepLL, stepUL));
		}
		
		CoinsToAddProg = CoinsToAdd;
		txtCoins.text = message + Coins.ToString();

		DataManager.Instance.SaveLastLevelData();
	}

	IEnumerator animShopCoinsAllTextFilds(   string message = ""  )
	{
		//AUDIO.PlaySound(  "shop_coin");
		int  CoinsToAddProg=0;
		
		int addC = 0;
		int stepUL = Mathf.FloorToInt(CoinsToAdd*0.175f);
		int stepLL = Mathf.FloorToInt(CoinsToAdd*0.22f);
		if(txtDispalyCoins!=null)
		{
			while( (Mathf.Abs(CoinsToAddProg) + Mathf.Abs(addC)) < Mathf.Abs(CoinsToAdd) )
			{
				CoinsToAddProg+=addC;
				for(int i = 0; i<txtDispalyCoins.Length;i++)
				{
					txtDispalyCoins[i].text = message+  (CoinsToAddStart + CoinsToAddProg).ToString();
				}
				//Debug.Log(CoinsToAddStart + CoinsToAddProg);
				yield return new WaitForSeconds (0.05f);
				addC = Mathf.FloorToInt(UnityEngine.Random.Range(stepLL, stepUL));
			}
			
			CoinsToAddProg = CoinsToAdd;

			for(int i = 0; i<txtDispalyCoins.Length;i++)
			{
				txtDispalyCoins[i].text = message + Coins.ToString();
			}


		}
		DataManager.Instance.SaveLastLevelData();
//		Debug.Log(" ** " + Coins);
	}


	
	//********************************************************










	//***************************************************************
	//ODBROJAVANJE DODAVANJA VREDNOSTI
	public Text[] txtFields;
	int StartVal = 0;
	int ValToAdd = 0;

	public void AnimiranjeDodavanjaVrednosti ( int _Start,  int _Add,   string message = "" )
	{
        StartVal =  _Start;
 
        ValToAdd = _Add;
        //StopAllCoroutines();
        if(txtFields !=null)
            StartCoroutine(animValue(  message ));
         
        SoundManager.Instance.Coins.Stop();
        SoundManager.Instance.Play_Sound(SoundManager.Instance.Coins);
	}

	IEnumerator animValue(   string message = ""  )
	{
		//AUDIO.PlaySound(  "shop_coin");
		int  ValToAddProg=0;
		
		int addC = 0;
		int stepUL = Mathf.FloorToInt(ValToAdd*0.175f);
		int stepLL = Mathf.FloorToInt(ValToAdd*0.22f);
		if(txtFields!=null)
		{
			while( (Mathf.Abs(ValToAddProg) + Mathf.Abs(addC)) < Mathf.Abs(ValToAdd) )
			{
				ValToAddProg+=addC;
				for(int i = 0; i<txtFields.Length;i++)
				{
					txtFields[i].text = message+  (StartVal + ValToAddProg).ToString();
				}
 
				yield return new WaitForSeconds (0.05f);
				addC = Mathf.FloorToInt(UnityEngine.Random.Range(stepLL, stepUL));
			}
			
			ValToAddProg = ValToAdd;
//			Debug.Log(StartVal + ValToAddProg);
			for(int i = 0; i<txtFields.Length;i++)
			{
				txtFields[i].text = message + (StartVal +ValToAdd).ToString();
			}
		}
	}
}
