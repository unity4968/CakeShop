using UnityEngine;
using System.Collections;

public class Bowl : MonoBehaviour {

	public ParticleSystem psTesto;
	public GameObject psPosition;
	public Transform Testo;


	Vector3 StartPosition ;
	bool bIskoriscen = false;
	public bool bDrag = false;
	Gameplay2a gp2;
	
	float x;
	float y;
 
	Vector3 diffPos = new Vector3(0,0,0);
	float angle = -10;
	bool bTesto = false;

   Vector3 startPosT = Vector3.zero;
	public Vector3 endPosT = new Vector3(0.6f,0f,0);//new Vector3(0.65f,-0.25f,0);

//	public Vector3 DoughStartOffset;
//	public Vector3 DoughEndOffset;



	float FillTime = 0;

	void Start () {
		StartPosition = transform.position;
		gp2 = Camera.main.GetComponent<Gameplay2a>();


		psTesto.gameObject.SetActive(true);
		psTesto.Stop();
		psTesto.GetComponent<Renderer>().sortingOrder = 5;

	}
	
	void Update () {
		if(bDrag)
		{
			x = Input.mousePosition.x;
			y = Input.mousePosition.y;
			
			
			float distX = gp2.Mold.position.x - transform.position.x - 5.5f;//6
			
			angle =  (-20 + distX/2f *30)*((5+gp2.FillStep)/5f);
			if(angle>0) angle = 0;
			if(angle<-20*((5+gp2.FillStep)/5f)) angle =  -20*((5+gp2.FillStep)/5f);
			
			transform.rotation = Quaternion.Euler(0,0,angle);
			Testo.rotation = Quaternion.Euler(0,0, 0);
			Testo.localPosition = Vector3.Lerp(startPosT,endPosT+ (gp2.FillStep/(1+gp2.FillStep*0.08f))*new Vector3(0.24f,0.18f,0) , angle/-20f);


			if(angle<0 && (transform.position.y<1.2f) )  transform.position = new Vector3( transform.position.x, 1.2f ,0);
			if(angle<0 && (transform.position.x>3f) )  transform.position = new Vector3(3, transform.position.y ,0);

			if(angle <= -19 && !bTesto) 
			{
				psTesto.gameObject.SetActive(true);
				bTesto = true;
				psTesto.Play();
		//		if(!SoundManager.Instance.LiquidIngredient.isPlaying)   SoundManager.Instance.Play_Sound( SoundManager.Instance.LiquidIngredient);
				
			}
			else if(angle > -19 &&  bTesto)   { 
				psTesto.Stop();  bTesto = false; psTesto.gameObject.SetActive(false);
//				if(SoundManager.Instance.LiquidIngredient.isPlaying)   SoundManager.Instance.Stop_Sound( SoundManager.Instance.LiquidIngredient);
			
			}

			if(bTesto) FillTime +=Time.deltaTime;

			if(FillTime > gp2.FillStep*1.5f) 
			{ 
				gp2.FillStep++; 
				gp2.FillDough();
				Testo.GetChild(0).localScale *=.9f;

			}

		}


		if(FillTime>9f && !bIskoriscen) 
		{
			bIskoriscen = true;
			StartCoroutine (MoveBack());
		}
		psTesto.transform.position = psPosition.transform.position;
	
		//transform.localScale = (0.95f - (transform.position.y+5)*.028f)* Vector3.one;
	}
	
	
	
	
	void OnMouseDown(){
		if( gp2.bEnableMove &&  gp2.bEnableBowl &&   !bIskoriscen &&  !bDrag    )
		{
			bDrag = true;
			
			diffPos =transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)   ;
			diffPos = new Vector3(diffPos.x,diffPos.y,0);
			 
			gp2.GoalPointer();
			
			
			
		}
	}
	
	
	
	void OnMouseDrag(){
		if( gp2.bEnableMove &&  gp2.bEnableBowl && !bIskoriscen &&  bDrag  )
		{
			Vector3 posM = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f)) + diffPos;
			if(posM.y>1.5f) posM = new Vector3(posM.x,1.5f,posM.z);
			transform.position = posM;

			//transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f)) + diffPos;
		}
	}
	
	void OnMouseUp(){
		
		
		if( gp2.bEnableMove &&  gp2.bEnableBowl && !bIskoriscen &&  bDrag  )
		{
			transform.position = Camera.main.ScreenToWorldPoint(new Vector3(x ,y,10.0f)) + diffPos;
			//transform.localScale = Vector3.one;
			
			StartCoroutine(MoveBack());
			bDrag = false;
			
		}
		
	}
	
	
	IEnumerator MoveBack(float timeWait=  0)
	{
		yield return new WaitForSeconds(timeWait);
		
		float pom = 0;
		while(pom<1)
		{
			bTesto = false;
			psTesto.Stop();
			psTesto.gameObject.SetActive(false);
//			if(SoundManager.Instance.LiquidIngredient.isPlaying)   SoundManager.Instance.Stop_Sound( SoundManager.Instance.LiquidIngredient);
			if(gp2.FillStep >= 6)
			{
				Testo.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
			}

			pom +=Time.deltaTime*5;
			transform.position = Vector3.Lerp(transform.position, StartPosition,pom);
			float pom2 = (1-pom*2);
			if(pom2<0) pom2 = 0;
			transform.rotation = Quaternion.Euler(0,0,angle*pom2);
			Testo.rotation = Quaternion.Euler(0,0,0);
			Testo.localPosition = Vector3.Lerp(startPosT,endPosT, angle*pom2/-20f);

		 
			yield return new WaitForEndOfFrame();
		}
		
		transform.position = StartPosition;
	}
	
	
	 
}
