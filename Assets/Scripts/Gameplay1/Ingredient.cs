using UnityEngine;
using System.Collections;

public class Ingredient : MonoBehaviour {

	public int IngredientNo ;
	Vector3 StartPosition ;
	bool bIskoriscen = false;
	bool bDrag = false;
	Gameplay1 gp1;
	public static int SelectedIngredientNo =-1;
	public static bool bEnableMove = true;

	float x;
	float y;
	Vector3 mousePosition;
	IngredientBar ingB;

	void Start () {
		StartPosition = transform.position;

		gp1 = Camera.main.GetComponent<Gameplay1>();

		GameObject ib = GameObject.Find("IngredientBar"+IngredientNo.ToString());
		if(ib!=null) ingB = ib.GetComponent<IngredientBar>();


	}



	
	 
	void Update(){
		//if(bDrag)
		if(!bIskoriscen &&  bDrag && bEnableMove   &&  !PopUpEvents.bMenuVisible)
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;

			mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(x,y,10.0f));

			if(mousePosition.y>1.6f) mousePosition = new Vector3(mousePosition.x,1.6f,mousePosition.z);
	 
			transform.parent.parent.position = Vector2.Lerp( transform.parent.parent.position, mousePosition, 0.15f);
		}
	}

	void OnMouseDown(){
 
		if(!bIskoriscen &&  !bDrag && bEnableMove && ingB.quantityOfIngredients>0  &&  !gp1.bMenuVisible && !ShopManager.bShowShop  &&  !PopUpEvents.bMenuVisible)
		{
			bDrag = true;
			transform.parent.parent.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.parent.parent.localScale = new Vector3(1.3f,1.3f,1.3f);
			transform.GetComponent<Renderer>().sortingOrder = 10;
			if(gp1.NextIngerdientNo == IngredientNo)
			{
				SelectedIngredientNo = IngredientNo;
				gp1.GoalPointer();

				SoundManager.Instance.Play_Sound(SoundManager.Instance.Ingredient);
			}
			else
			{
				SoundManager.Instance.Play_Sound(SoundManager.Instance.WrongIngredient);
			}
		}
		else if(   !bDrag && bEnableMove && ingB.quantityOfIngredients<1   && !Spoon.bDrag &&  !gp1.bMenuVisible && !ShopManager.bShowShop &&  !PopUpEvents.bMenuVisible )
		{
			 
			SoundManager.Instance.Play_Sound(SoundManager.Instance.NoMoney);
			ingB.TestQuantity();
			 
			Camera.main.SendMessage("ShowBuyIngredientsMenu",  ingB);
		}
		else if( gp1.bMenuVisible || ShopManager.bShowShop  || PopUpEvents.bMenuVisible )
		{
			ingB.GetComponent<Animator>().SetTrigger("tStop");
		}
		 
	}
 
 

	void OnMouseUp(){
		if(!bIskoriscen &&  bDrag  /*&& bEnableMove */)
		{
			transform.parent.parent.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.parent.parent.localScale = Vector3.one;

			 StartCoroutine("MoveBack");

			//iTween.MoveTo( transform.parent.parent.gameObject  ,StartPosition,.3f);
			bDrag = false;
		}
		 
	}

	IEnumerator MoveBack()
	{

		SelectedIngredientNo = -1;
		float pom = 0;
		Vector3 DropPos =  transform.parent.parent.position;
		while(pom<1)
		{
			pom += Time.deltaTime*5;//  0.05f;
			transform.parent.parent.position = Vector3.Lerp(DropPos, StartPosition,pom);
			//yield return new WaitForSeconds(0.02f);

			yield return new WaitForEndOfFrame();
		}
		transform.GetComponent<Renderer>().sortingOrder = 5;
		transform.parent.parent.position = StartPosition;

		IngredientBar.bButtonEnabled = true;
	}

	void OnTriggerEnter2D(Collider2D other) {
		 
		 if(gp1.NextIngerdientNo == IngredientNo)
		{
			bDrag = false;
			bIskoriscen = true;
			 StopCoroutine("MoveBack");
			//iTween.Stop( transform.parent.parent.gameObject );
			ingB.DecreaseQuantity();

			if(IngredientNo ==0)
			{
				StartCoroutine(BreakEgg());
				gp1.NextIngerdientNo++;
			}
			else  if(IngredientNo <= 6)
			{
				transform.GetComponent<Collider2D>().enabled = false;
				StartCoroutine(AddMilk());
				gp1.NextIngerdientNo++;
			}
		 

		}

	}



	IEnumerator BreakEgg()
	{
		IngredientBar.bButtonEnabled = false;
		bEnableMove = false;
		gp1.HidePointer();
		Vector3 eggPos = GameObject.Find("e1").transform.position;
		SoundManager.Instance.Play_Sound(SoundManager.Instance.EggBreak);
		float pom = 0;
		while(pom<1)
		{
			pom +=0.05f;
			transform.parent.parent.position = Vector3.Lerp(transform.parent.parent.position, eggPos,pom);
			yield return new WaitForEndOfFrame();
		}
		 
		transform.parent.parent.position = eggPos;
		transform.parent.parent.GetComponent<Animator>().SetTrigger("Crack");
		yield return new WaitForSeconds(1f);
		gp1.SetIngredient();
		yield return new WaitForSeconds(0.5f);
		transform.parent.parent.localScale = Vector3.one;
		transform.GetComponent<Renderer>().sortingOrder = 5;
		transform.parent.parent.position = StartPosition;

		if(bIskoriscen) 
		{
			transform.GetComponent<SpriteRenderer>().color = new Color(.9f,.9f,.9f,.7f);
		 
		}
		 
		yield return new WaitForSeconds(0.01f);
		gp1.NextPointer();
		gp1.ShowPointer();
		if(!gp1.bMenuVisible || DisplayCustomerOrder. bVisible) bEnableMove = true;
		IngredientBar.bButtonEnabled = true;
	}

	IEnumerator AddMilk()
	{
		IngredientBar.bButtonEnabled = false;
		bEnableMove = false;
		gp1.HidePointer();
		Vector3 MilkPos = GameObject.Find("e3").transform.position;
		float pom = 0;
		while(pom<1f)
		{
			pom +=0.05f;
			transform.parent.parent.position = Vector3.Lerp(transform.parent.parent.position, MilkPos,pom);
			yield return new WaitForEndOfFrame();
		}
		
		transform.parent.parent.position = MilkPos;
		if (IngredientNo == 5)
		{
			transform.parent.parent.GetComponent<Animator>().SetTrigger("Shake");
			transform.Find("ParticleSystem").GetComponent<ParticleSystem>().Play();
		}
		else transform.parent.parent.GetComponent<Animator>().SetTrigger("Start");


		yield return new WaitForSeconds(0.3f);
		//ZVUCI
		if (IngredientNo == 1 || IngredientNo == 6 )
			SoundManager.Instance.Play_Sound(SoundManager.Instance.LiquidIngredient);
		else if (IngredientNo >1)
			SoundManager.Instance.Play_Sound(SoundManager.Instance.PowderIngredient);

		yield return new WaitForSeconds(1.0f);
		gp1.SetIngredient();
		yield return new WaitForSeconds(2f);
		pom = 0;
		
		while(pom<1)
		{
			pom +=0.1f;
			transform.parent.parent.position = Vector3.Lerp(transform.parent.parent.position, StartPosition,pom);
			yield return new WaitForSeconds(0.02f);
			transform.parent.parent.localScale =  Vector3.Lerp(transform.parent.parent.localScale,Vector3.one,-1+pom*2);

			if(pom >0.66f && pom <0.74f)
			{
				gp1.NextPointer();
				gp1.ShowPointer();
				if(!gp1.bMenuVisible || DisplayCustomerOrder. bVisible) bEnableMove = true;
			}
		}
 
		transform.parent.parent.localScale = Vector3.one;
		transform.GetComponent<Renderer>().sortingOrder = 5;
		transform.parent.parent.position = StartPosition;

		if(bIskoriscen) 
		{
			transform.GetComponent<SpriteRenderer>().color = new Color(.9f,.9f,.9f,.7f);
			 
		}
		IngredientBar.bButtonEnabled = true;
		 yield return new WaitForSeconds(0.01f);
		 
//		gp1.NextPointer();
//		gp1.ShowPointer();
//		if(!gp1.bMenuVisible) bEnableMove = true;
		 
	}


}
