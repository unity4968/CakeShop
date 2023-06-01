using UnityEngine;
using System.Collections;

public class Cable3d : MonoBehaviour {

	public bool bIskoriscen = false;
	public bool bMoveCable = true;

	public Transform[] Bones;
	public float cableScale =1;
	public Transform bonePosPref;

	public Transform Cable;

//	public Transform P0;
	public Transform P1;
	public CableColor CableCol;
	public Transform[] BonesPOS;
	float[] startAngles;
	float startAngleCable;

	float startDist = 1;
	float dist = 1;
	 
 
	public   float coefAngle =.95f;
	public   float coefScale =1.7f;

	 
	void Start () {
		//PODESAVANJE BOJE
		if(CableCol == CableColor.Red)
			Cable.transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,0);
		if(CableCol == CableColor.Green)
			Cable.transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,-0.75f);
		if(CableCol == CableColor.Blue)
			Cable.transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,-0.5f);
		if(CableCol == CableColor.Yellow)
			Cable.transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,-0.25f);


		if(bMoveCable)
		{
			BonesPOS = new Transform[Bones.Length-1] ;
			startAngles= new float[Bones.Length-1] ;

			for(int i = 1; i<Bones.Length;i++)
			{
				Transform tr = (Transform) GameObject.Instantiate(bonePosPref);
				tr.name = "BP_" +Bones[i].name;
				tr.position = Bones[i].position;
				tr.parent = Bones[i].parent;
				Bones[i].parent = Bones[0].parent;
				BonesPOS[i-1] = tr;

				startAngles[i-1] = Bones[i].rotation.eulerAngles.z;
				//-180 d0 180
				if(startAngles[i-1] >180) startAngles[i-1] -=360;

				 
			}

			startAngleCable = Mathf.Atan( (P1.position.y - Bones[1].position.y)/ (P1.position.x - Bones[1].position.x)) * Mathf.Rad2Deg;
			//-180 d0 180
			if(P1.position.x<Bones[1].position.x && P1.position.y < Bones[1].position.y) startAngleCable -=180; //IV kvadrant
			if(P1.position.x<Bones[1].position.x && P1.position.y > Bones[1].position.y) startAngleCable +=180; //III kvadrant

			startDist = Vector2.Distance(Bones[1].position,P1.position);
			 
		}
	}

	void Update () {
		if(bMoveCable)	MoveCable();
	}

	public void Init()
	{
		if(CableCol == CableColor.Red)
			Cable.transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,0);
		if(CableCol == CableColor.Green)
			Cable.transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,-0.75f);
		if(CableCol == CableColor.Blue)
			Cable.transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,-0.5f);
		if(CableCol == CableColor.Yellow)
			Cable.transform.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(0,-0.25f);
	}


	void MoveCable()
	{
		dist = Vector2.Distance(Bones[1].position,P1.position);
		cableScale = 1+ ((dist-startDist) /startDist)*coefScale;
		if(cableScale <0.8f) cableScale =0.8f;

		Bones[1].localScale = new Vector3 (cableScale ,1,1);
		Bones[2].localScale = new Vector3 (cableScale,1,1);
		Bones[3].localScale = new Vector3 (cableScale,1,1);
		Bones[4].localScale = new Vector3 (cableScale,1,1);

		float angle1 = - startAngleCable + Mathf.Atan( (P1.position.y - Bones[1].position.y)/ (P1.position.x - Bones[1].position.x)) * Mathf.Rad2Deg;
		//-180 d0 180
		if(P1.position.x<Bones[1].position.x && P1.position.y < Bones[1].position.y) angle1 -=180; //IV kvadrant
		if(P1.position.x<Bones[1].position.x && P1.position.y > Bones[1].position.y) angle1 +=180; //III kvadrant

	 
		for(int i = 1; i<Bones.Length ;i++)
		{
			Bones[i].position = BonesPOS[i-1].position; //POZICIJA JE  U NASTAVKU PRETHODNE KOSTI
			
			if(i>0 && i<6  )
			{
				Bones[i].rotation = Quaternion.identity;
				Bones[i].Rotate(new Vector3(0,0,  startAngles[i-1] +  (angle1 ) *(coefAngle + 0.2f*(i-3)) )); 
			}
 
			if(i==5)
			{	 
				Bones[i].rotation = Quaternion.identity;
				Bones[i].Rotate(new Vector3(0,0,startAngles[i-1]  ));
			}
		}
	}
}

public enum CableColor
{
	Red,
	Green,
	Blue,
	Yellow
}

