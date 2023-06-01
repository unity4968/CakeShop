using UnityEngine;
using System.Collections;

public class Spoon : MonoBehaviour {

	Vector3 StartPosition ;
	bool bIskoriscen = false;
	public static bool bDrag = false;
	public Gameplay1 gp1;
	
	public Sprite BrokenSpoon;
	public Sprite  NormalSpoon;
	SpriteRenderer spr;
	float x;
	float y;
	
	int startColliderNo = -11;
	int colliderNo = -1;
	int counter = 0;
	int RotationDirection =1;

	Vector3 deltaPos = Vector3.zero;

	void Start () {
		bDrag = false;
		spr = transform.GetComponent<SpriteRenderer>();
	 
		
		StartPosition = transform.position;
		
		transform.GetComponent<CircleCollider2D>().enabled = false;
		transform.GetComponent<PolygonCollider2D>().enabled = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(bDrag)
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
		}
	}



	void OnMouseDown(){
		if( gp1.bSpoonEneabled &&   !bIskoriscen &&  !bDrag  &&  !gp1.bMenuVisible && !ShopManager.bShowShop  &&  !PopUpEvents.bMenuVisible )
		{
			//AKO JE POLOMLJENA MENJA SE SPRAJT
			
			if(   DataManager.Instance.PolomljenaVarjaca !=2 && 
			   ( DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.Spoon || DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.SpoonAndIngr || DataManager.Instance.SelectedLevelData.ExpItem == ExpendableItems.SpoonAndOven)
			   && DataManager.Instance.NivoPoslednjegLomljenjaVarjace < DataManager.Instance.SelectedLevel)
			{
				DataManager.Instance.PolomljenaVarjaca = 0;
				DataManager.Instance.NivoPoslednjegLomljenjaVarjace = DataManager.Instance.SelectedLevel;
				DataManager.Instance.SetGameplay1Data();
				PlayerPrefs.Save();
				
				spr.sprite = BrokenSpoon;
				Camera.main.SendMessage("BuySpoon");
			}

 			else //NORMALNO SE POMERA
			{	 
				bDrag = true;
				deltaPos =   transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
				
				transform.GetComponent<Renderer>().sortingOrder = 15;
				
				gp1.GoalPointer();
				
				transform.GetComponent<CircleCollider2D>().enabled = true;
				transform.GetComponent<PolygonCollider2D>().enabled = false;
			}
		}
		else if( !gp1.bSpoonEneabled &&   !bIskoriscen &&  !bDrag    && DataManager.Instance.PolomljenaVarjaca == 0  &&  !gp1.bMenuVisible && !ShopManager.bShowShop &&  !PopUpEvents.bMenuVisible )
		{
			Camera.main.SendMessage("BuySpoon");
		}
	}
	
	
	
	void OnMouseDrag(){
		if(gp1.bSpoonEneabled && !bIskoriscen &&  bDrag  &&  !gp1.bMenuVisible &&  !PopUpEvents.bMenuVisible )
		{
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f));
			if(posM.y>3.0f) posM = new Vector3(posM.x,3.0f,posM.z);
			transform.position = posM;

			//transform.position = deltaPos + Camera.main.ScreenToWorldPoint(new Vector3(x,y,10.0f));
		}
		 else if( gp1.bMenuVisible &&  bDrag) StartCoroutine(MoveBack());
	}
	
	void OnMouseUp(){
		
		
		if(gp1.bSpoonEneabled && !bIskoriscen &&  bDrag &&  !gp1.bMenuVisible )
		{
			transform.position = deltaPos + Camera.main.ScreenToWorldPoint(Input.mousePosition);
	 
			StartCoroutine(MoveBack());
			bDrag = false;
			
			transform.GetComponent<CircleCollider2D>().enabled = false;
			transform.GetComponent<PolygonCollider2D>().enabled = true;
		}
		 else if( gp1.bMenuVisible &&  bDrag) StartCoroutine(MoveBack());
		
	}
	

	public void ReturnSpoonToStartPlace()
	{
		StartCoroutine(MoveBack());
	}

	IEnumerator MoveBack1(float timeWait=0)
	{

		yield return new WaitForSeconds(timeWait);
		
		float pom = 0;
		while(pom<1)
		{
			pom +=Time.deltaTime*.5f;
			transform.position = Vector3.Lerp(transform.position, StartPosition,pom);
			yield return new WaitForEndOfFrame();
		}
		transform.GetComponent<Renderer>().sortingOrder = 15;
		transform.position = StartPosition;
	}

	IEnumerator MoveBack(float timeWait=0)
	{
	 
		float pom = 0;
		yield return new WaitForSeconds(timeWait);
		Vector3 DropPos =  transform.position;
		while(pom<1)
		{
			pom += Time.deltaTime*7;//  0.05f;
			transform.position = Vector3.Lerp(DropPos, StartPosition,pom);
			//yield return new WaitForSeconds(0.02f);
			
			yield return new WaitForEndOfFrame();
		}
		transform.GetComponent<Renderer>().sortingOrder = 15;
		//transform.parent.parent.position = StartPosition;
		transform.position = StartPosition;
		
	}





	float mixingSoundStopTime = 0;
	bool bStopCR = false;

	IEnumerator StopMixing()
	{
		mixingSoundStopTime +=Time.deltaTime;
		bStopCR = true;
		while(mixingSoundStopTime <0.2f)
		{

			mixingSoundStopTime +=0.05f;
			yield return new WaitForSeconds(0.05f);
		}
		SoundManager.Instance.Stop_Sound(SoundManager.Instance.MixIngredients);
		bStopCR = false;
		yield return null;

	}
	
	void OnTriggerEnter2D(Collider2D other) 
	{

		int pom = -1;
		if(other.name == "COL_BOWL_1") pom = 1;
		else if(other.name == "COL_BOWL_2") pom = 2;
		else if(other.name == "COL_BOWL_3") pom = 3;
		else if(other.name == "COL_BOWL_4") pom = 4;

		if(pom != -1)
		{
			mixingSoundStopTime = 0;
			if(!SoundManager.Instance.MixIngredients.isPlaying)   SoundManager.Instance.Play_Sound(SoundManager.Instance.MixIngredients);
			 
			if( !bStopCR ) StartCoroutine("StopMixing");
		}
 
		//ODREDJIVANJE SMERA
		if(startColliderNo>-10)
		{
			if   (  (-(startColliderNo +1) == pom) || (startColliderNo== -1 && pom==4)) 
			{
				RotationDirection=-1;
				startColliderNo = -10;
			}
			else if( ((-startColliderNo +1) == pom) || (startColliderNo== -4 && pom==1) ) 
			{
				RotationDirection= 1;
				startColliderNo = -10;
			}
		}

		if(startColliderNo == -11)
		{
			startColliderNo = -pom;
			colliderNo = pom;
		}
		
		if (  (((colliderNo +1) == pom) || (colliderNo== 4 && pom==1)) )// ||   (((colliderNo) == pom+1) || (colliderNo== 1 && pom==4)) )
		{



			colliderNo = pom;
			counter++;
			if(counter <28) gp1.AddDoughRotation(RotationDirection);
			 
		}

		if(counter ==4) 
		{ 
			gp1.NextIngerdientNo++;
			gp1.NextPointer();
			gp1.SetIngredient();
		}
		else if(counter ==8) 
		{
			gp1.NextIngerdientNo++;

			gp1.NextPointer();
			gp1.SetIngredient();
		}
		else if(counter ==12) 
		{
			gp1.NextIngerdientNo++;
			gp1.NextPointer();
			gp1.SetIngredient();
		}
		else if(counter ==16) 
		{
			gp1.NextIngerdientNo++;
			gp1.NextPointer();
			gp1.SetIngredient();
		}
		else if(counter ==20) 
		{
			gp1.NextIngerdientNo++;
			gp1.NextPointer();
			gp1.SetIngredient();
		}
		else if(counter ==24) 
		{
			gp1.NextIngerdientNo++;
			gp1.NextPointer();
			gp1.SetIngredient();
		}
		else if(counter >=28) 
		{
			gp1.NextIngerdientNo++;
			gp1.NextPointer();
			gp1.SetIngredient();
			StartCoroutine(MoveBack(1.2f));

			SoundManager.Instance.Stop_Sound(SoundManager.Instance.MixIngredients);
		}
	}
	
	public void NewSpoon()
	{
		spr = transform.GetComponent<SpriteRenderer>();
		spr.sprite = NormalSpoon;
		
		if(DataManager.Instance.PolomljenaVarjaca != 2) DataManager.Instance.PolomljenaVarjaca = 1; 
		DataManager.Instance.NivoPoslednjegLomljenjaVarjace = DataManager.Instance.SelectedLevel;
		DataManager.Instance.SetGameplay1Data();
		PlayerPrefs.Save();
	}
	
	public void Init()
	{


		if(DataManager.Instance.PolomljenaVarjaca ==0)
		{
			spr = transform.GetComponent<SpriteRenderer>();
			 
			spr.sprite = BrokenSpoon;
			//Camera.main.SendMessage("BuySpoon");
		}
		 else
		{
			spr = transform.GetComponent<SpriteRenderer>();
			spr.sprite = NormalSpoon;
		}
		 
	}
	
	
}
