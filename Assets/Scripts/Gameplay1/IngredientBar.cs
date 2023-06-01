using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IngredientBar : MonoBehaviour {

	int MaxIngredients = 0; 
	 
	public Text txtQuantity;
	public int quantityOfIngredients =0;

	public Image imgPlus;
	public Image imgPlusBG;

	public Transform IBPosition;

	public Ingredients ingType;

	public static bool bButtonEnabled = true;

	void Start () {
		bButtonEnabled = true;
		imgPlus.enabled = false;
		imgPlusBG.enabled = false;

		StartCoroutine(WaitStart());
	}



	void Update()
	{

		transform.position = IBPosition.position;
	}

	IEnumerator WaitStart()
	{
		yield return new WaitForSeconds(0.05f);

		if(ingType == Ingredients.Egg)
		{
			MaxIngredients = DataManager.Instance.MaxEggs;
			quantityOfIngredients =  DataManager.Instance.RemainingEggs;
		}
		else if(ingType == Ingredients.Milk){
			MaxIngredients = DataManager.Instance.MaxMilk;
			quantityOfIngredients =  DataManager.Instance.RemainingMilk;
		}
		else if(ingType == Ingredients.Sugar) {
			MaxIngredients = DataManager.Instance.MaxSugar;
			quantityOfIngredients =  DataManager.Instance.RemainingSugar;
		}
		else if(ingType == Ingredients.BakiungPowder) {
			MaxIngredients = DataManager.Instance.MaxBakiungPowder;
			quantityOfIngredients =  DataManager.Instance.RemainingBakiungPowder;
		}
		else if(ingType == Ingredients.Flour){
			MaxIngredients = DataManager.Instance.MaxFlour;
			quantityOfIngredients =  DataManager.Instance.RemainingFlour;
		}
		else if(ingType == Ingredients.Salt) {
			MaxIngredients = DataManager.Instance.MaxSalt;
			quantityOfIngredients =  DataManager.Instance.RemainingSalt;
		}
		else if(ingType == Ingredients.Oil) {
			MaxIngredients = DataManager.Instance.MaxOil;
			quantityOfIngredients =  DataManager.Instance.RemainingOil;
		}

		SetData();
	}
	
	 
	 

	public void SetData()
	{
		imgPlus.enabled = false;
		imgPlusBG.enabled = false;

		if(Shop.InfiniteSupplies == 2 ) 
		{
			quantityOfIngredients = MaxIngredients;
			transform.GetChild(0).gameObject.SetActive(false);
		}

		if(quantityOfIngredients>MaxIngredients) quantityOfIngredients = MaxIngredients;
		if(quantityOfIngredients<1) 
		{
			quantityOfIngredients = 0;
			imgPlus.enabled = true;
			imgPlusBG.enabled = true;

		}

		txtQuantity.text = quantityOfIngredients.ToString();
	}

	public void btnBuyIngredientClicked()
	{
		if( !bButtonEnabled) return;
		if(quantityOfIngredients == 0) //<MaxIngredients)
		{
			SoundManager.Instance.Play_ButtonClick();
			transform.GetComponent<Animator>().SetTrigger("BuyIngredient");
			Camera.main.SendMessage("ShowBuyIngredientsMenu",  this);
            //FIXME TODO Da se promeni logika kad nestanu ingredients
		}
	}



	public void AddIngredients( bool bOne = false) 
	{
		if(quantityOfIngredients == 0)
		{

			if( bOne)  quantityOfIngredients  =1;
			else  quantityOfIngredients +=  MaxIngredients;

			if(ingType == Ingredients.Egg) DataManager.Instance.RemainingEggs =  quantityOfIngredients;
			else if(ingType == Ingredients.Milk) DataManager.Instance.RemainingMilk =  quantityOfIngredients;
			else if(ingType == Ingredients.Sugar)  DataManager.Instance.RemainingSugar = quantityOfIngredients;
			else if(ingType == Ingredients.BakiungPowder)  DataManager.Instance.RemainingBakiungPowder = quantityOfIngredients;
			else if(ingType == Ingredients.Flour) DataManager.Instance.RemainingFlour = quantityOfIngredients;
			else if(ingType == Ingredients.Salt)  DataManager.Instance.RemainingSalt = quantityOfIngredients;
			else if(ingType == Ingredients.Oil)  DataManager.Instance.RemainingOil = quantityOfIngredients;


			DataManager.Instance.SetGameplay1Data();
		}
	 
		SetData();

	}




	public void DecreaseQuantity()
	{
	 
		if(  Shop.InfiniteSupplies ==2 ) return; //beskonacno namirnica

		if( quantityOfIngredients>0 )
		{
			quantityOfIngredients --;

			if(ingType == Ingredients.Egg) DataManager.Instance.RemainingEggs = quantityOfIngredients;
			else if(ingType == Ingredients.Milk) DataManager.Instance.RemainingMilk = quantityOfIngredients;
			else if(ingType == Ingredients.Sugar)  DataManager.Instance.RemainingSugar = quantityOfIngredients;
			else if(ingType == Ingredients.BakiungPowder)  DataManager.Instance.RemainingBakiungPowder = quantityOfIngredients;
			else if(ingType == Ingredients.Flour) DataManager.Instance.RemainingFlour = quantityOfIngredients;
			else if(ingType == Ingredients.Salt)  DataManager.Instance.RemainingSalt = quantityOfIngredients;
			else if(ingType == Ingredients.Oil)  DataManager.Instance.RemainingOil = quantityOfIngredients;

			DataManager.Instance.SetGameplay1Data();
		}
		else
		{
		 
			transform.GetComponent<Animator>().SetTrigger("NoMoreIngredients");
		}
		SetData();
	}

	public void TestQuantity()
	{
 
		if(quantityOfIngredients<1)
		{	
			 
			transform.GetComponent<Animator>().SetTrigger("NoMoreIngredients");
		}
		SetData();
	}
	 
}

public enum Ingredients
{
	Egg,
	Milk,
	Sugar,
	BakiungPowder,
	Flour,
	Salt,
	Oil
}

