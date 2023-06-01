using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HeaderBar : MonoBehaviour {

	Text txtDay;
	Text txtCoinsHeader;
	Text txtCustomerHeader;
 

	void Awake () {
		DataManager.InitDataManager();
		//DataManager.Instance.SelectLevel(6);//obrisi
		 
	 
		Debug.Log("AWAKE");
	}

	// Use this for initialization
	void Start () {
		txtDay = GameObject.Find("txtDay").GetComponent<Text>();
		txtCoinsHeader = GameObject.Find("txtCoinsHeader").GetComponent<Text>();
		txtCustomerHeader = GameObject.Find("txtCustomerHeader").GetComponent<Text>();
		 
		txtDay.text = "DAY: "+ DataManager.Instance.SelectedLevel.ToString();
		txtCoinsHeader.text = DataManager.Instance.SelectedLevelCoinsEarned.ToString();
		txtCustomerHeader.text = "CUSTOMER: "+  DataManager.Instance.SelectedLevelCustomersServed.ToString()+" / " + DataManager.Instance.SelectedLevelData.CustomersPerLevel;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void btnShowCustomerOrder()
	{
		Debug.Log("ShowCustomerOrder");
	}
}
