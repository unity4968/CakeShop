using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour {

	public GameObject Level;
	public GameObject Customer;
	public GameObject Coins;
	public GameObject CoinsPerDay;
	public GameObject Background;


	public Text txtLevel;
	public Text txtCustomers;
	public Text txtCoinsPerDay;
	public Text txtCoins;

	public ShopManager shopManager;

	// Use this for initialization
	void Start () {


		if(Application.loadedLevelName == "CustomerScene")
		{
			CoinsPerDay.SetActive (true);
			//Background.SetActive (false);
		}
		else if(Application.loadedLevelName == "MapScene")
		{
			CoinsPerDay.SetActive (false);
			//Background.SetActive (false);
			Customer.SetActive (false);
//			Coins.GetComponent<RectTransform>().anchoredPosition = new Vector2(-10,-39);
//			Level.GetComponent<RectTransform>().anchoredPosition = new Vector2(-170,-90);
		}
		else
		{
			CoinsPerDay.SetActive (false);
			//Background.SetActive (true);
		}
	}

	public void Init( int level, int coinsPerDay, int customer, int customersPerDay, int coins)
	{
		SetLevel( level);
		SetCoinsPerDay(coinsPerDay);
		SetCustomer(customer, customersPerDay);
		SetCoins(coins);

	}


	public void SetLevel(int level)
	{
		txtLevel.text = level.ToString().PadLeft(2,'0');
	}

	public void SetCoinsPerDay( int coinsPerDay)
	{
		txtCoinsPerDay.text = coinsPerDay.ToString();
	}

	public void SetCustomer( int customer, int customersPerDay)
	{
		txtCustomers.text = customer.ToString()+"/"+customersPerDay.ToString();
	}

	public void SetCoins( int coins)
	{
		txtCoins.text = coins.ToString();
	}

	public void btnBuyCoinsClicked()
	{
		if(shopManager!=null) shopManager.ShowPopUpShop();
		Debug.Log ("OPEN SHOP");
		SoundManager.Instance.Play_ButtonClick();
	}


	public static float BannerPixelOffset = 0;
	float BannerAnchorOffset = 0;

	void Update()
	{

	}

	public void	SetPixelOffset(int pixel_offset)
	{
		BannerPixelOffset = pixel_offset;
		SetPosition();
	}

	void AdjustStatusBarPosition()
	{
		transform.GetChild(0).position =  Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2f,Screen.height-BannerPixelOffset,10) )  ;
		BannerAnchorOffset =  transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition.y ; 
	}

    public void SetPosition()
    {
        try
        {
            
            if(Application.loadedLevelName == "MapScene")
            {
                GameObject.Find("StatusBar/ImageBackground").GetComponent<RectTransform>().anchoredPosition = new Vector2(0,2);
            }
            else if(Application.loadedLevelName == "Gameplay1" || Application.loadedLevelName == "Gameplay2a" || Application.loadedLevelName == "Gameplay3")
            {
                Camera.main.SendMessage("BannerCorrection");
                
                
                GameObject.Find("Canvas/CustomerOrder").GetComponent<RectTransform>().anchoredPosition = new Vector2(94,-37);
                GameObject.Find("Canvas/StatusBar/ImageBackground").GetComponent<RectTransform>().anchoredPosition = new Vector2(-25,2);
                GameObject.Find("ButtonPause").GetComponent<RectTransform>().anchoredPosition = new Vector2(-65,-60);
                
                if(Application.loadedLevelName == "Gameplay3")
                {
                    Transform pb = GameObject.Find("ProgressBG").transform;
                    
                    pb.localScale =  (1-( (float) Screen.width/ (float) Screen.height - 1.33f)*0.5f) *Vector3.one;
                    pb.GetComponent<RectTransform>().anchoredPosition = new Vector2(80,-50);
                    
                }
                
                if(Application.loadedLevelName == "Gameplay1")
                {
                    Transform bt = GameObject.Find("BackgroundTable").transform;
                    bt.localScale =  (1+( (float) Screen.width/ (float) Screen.height - 1.33f)*0.25f) *Vector3.one;
                }
            }
            else if(Application.loadedLevelName == "CustomerScene")
            {
                GameObject.Find("StatusBar/ImageBackground").GetComponent<RectTransform>().anchoredPosition = new Vector2(0,2);
                Transform canvas1 =     GameObject.Find("Canvas").transform;
                
                
                
                for(int i=0;i<canvas1.childCount;i++) 
                {
                    Transform tr = canvas1.GetChild(i);
                    if(tr.name == "PopUpLevelComplete")
                    {
                        for(int j=0;j<tr.GetChild(0).childCount;j++) 
                        {
                            Transform trPom = tr.GetChild(0).GetChild(j);
                            if(trPom.name == "AnimationHolderWin")
                            {
                                trPom.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,0);
                            }
                        }
                    }
                }
            }
        }
        
        catch { Debug.Log("GRESKA KOD PODESAVANJA BANNERA");}
    }
}
