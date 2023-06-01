using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>


public class CharacterChanger : MonoBehaviour {

	[System.Serializable]
	public struct CharacterStruct
	{
		public Sprite head;
		public Sprite body;
		public Sprite upperArm;
		public Sprite lowerArm;
		public Sprite neck;
		public Sprite fist1;
		public Sprite fist2;
		public Sprite leg;
		public Sprite eyeOutlineNormal;
		public Sprite eyeWhiteNormal;
		public Sprite eyeOutlineMad;
		public Sprite eyeWhiteMad;
		public Sprite eyeLidOpen;
		public Sprite eyeLidClosed;
		public Sprite mouthSad;
		public Sprite mouthNormal;
		public Sprite mouthMad;
		public Sprite mouthHappy;
		public Sprite eyeBall;
		public Sprite eyeBag;
		public Sprite eyeBrown;
		public Sprite nose;
		public Sprite whiteCircle;
		public Sprite downAccessory;
		public Sprite upAccessory;

	}
	public CharacterStruct[] ListOfCharacters;

	[System.Serializable]
	public struct CharacterSprites
	{
		public Image head;
		public Image body;
		public Image neck;
		public Image upperArmLeft;
		public Image upperArmRight;
		public Image lowerArmLeft;
		public Image lowerArmRight;
		public Image handLeft;
		public Image handLeft2;
		public Image handRight;
		public Image handRight2;
		public Image legRight;
		public Image legLeft;
		public Image EyeRightWhiteNormal;
		public Image EyeBallRightNormal;
		public Image EyeRightOutlineNormal;
		public Image EyeClosedRight1Normal;
		public Image EyeClosedRight2Normal;
		public Image EyeRightWhiteMad;
		public Image EyeBallRightMad;
		public Image EyeRightOutlineMad;
		public Image EyeBrowRightMad;
		public Image EyeBagRightMad;
		public Image EyeLeftWhiteNormal;
		public Image EyeBallLeftNormal;
		public Image EyeLeftOutlineNormal;
		public Image EyeClosedLeft1Normal;
		public Image EyeClosedLeft2Normal;
		public Image EyeLeftWhiteMad;
		public Image EyeBallLeftMad;
		public Image EyeLeftOutlineMad;
		public Image EyeBrowLeftMad;
		public Image EyeBagLeftMad;
		public Image mouthSad;
		public Image mouthNormal;
		public Image mouthMad;
		public Image mouthHappy;
		public Image nose;
		public Image shadowBig;
		public Image shadowSmallLeft;
		public Image shadowSmallRight;
		public Image downAccessory;
		public Image upAccessory;
	}
	public CharacterSprites CharacterReferences;
	// Use this for initialization
	void Start () {
 		ChangeCharacter(7);
	}
	
	public void ChangeCharacter(int number)
	{
		CharacterReferences.head.sprite = ListOfCharacters[number].head;
		CharacterReferences.body.sprite = ListOfCharacters[number].body;
		CharacterReferences.neck.sprite = ListOfCharacters[number].neck;
		CharacterReferences.upperArmLeft.sprite = ListOfCharacters[number].upperArm;
		CharacterReferences.upperArmRight.sprite = ListOfCharacters[number].upperArm;
		CharacterReferences.lowerArmLeft.sprite = ListOfCharacters[number].lowerArm;;
		CharacterReferences.lowerArmRight.sprite = ListOfCharacters[number].lowerArm;
		CharacterReferences.handLeft.sprite = ListOfCharacters[number].fist1;
		CharacterReferences.handLeft2.sprite = ListOfCharacters[number].fist2;
		CharacterReferences.handRight.sprite = ListOfCharacters[number].fist1;
		CharacterReferences.handRight2.sprite = ListOfCharacters[number].fist2;
		CharacterReferences.legRight.sprite = ListOfCharacters[number].leg;
		CharacterReferences.legLeft.sprite = ListOfCharacters[number].leg;
		CharacterReferences.EyeRightWhiteNormal.sprite = ListOfCharacters[number].eyeWhiteNormal;
		CharacterReferences.EyeBallRightNormal.sprite = ListOfCharacters[number].eyeBall;
		CharacterReferences.EyeRightOutlineNormal.sprite = ListOfCharacters[number].eyeOutlineNormal;
		CharacterReferences.EyeClosedRight1Normal.sprite = ListOfCharacters[number].eyeLidOpen;
		CharacterReferences.EyeClosedRight2Normal.sprite = ListOfCharacters[number].eyeLidClosed;
		CharacterReferences.EyeRightWhiteMad.sprite = ListOfCharacters[number].eyeWhiteMad;
		CharacterReferences.EyeBallRightMad.sprite = ListOfCharacters[number].eyeBall;
		CharacterReferences.EyeRightOutlineMad.sprite = ListOfCharacters[number].eyeOutlineMad;
		CharacterReferences.EyeBrowRightMad.sprite = ListOfCharacters[number].eyeBrown;
		CharacterReferences.EyeBagRightMad.sprite = ListOfCharacters[number].eyeBag;
		CharacterReferences.EyeLeftWhiteNormal.sprite = ListOfCharacters[number].eyeWhiteNormal;
		CharacterReferences.EyeBallLeftNormal.sprite = ListOfCharacters[number].eyeBall;
		CharacterReferences.EyeLeftOutlineNormal.sprite = ListOfCharacters[number].eyeOutlineNormal;
		CharacterReferences.EyeClosedLeft1Normal.sprite = ListOfCharacters[number].eyeLidOpen;
		CharacterReferences.EyeClosedLeft2Normal.sprite = ListOfCharacters[number].eyeLidClosed;
		CharacterReferences.EyeLeftWhiteMad.sprite = ListOfCharacters[number].eyeWhiteMad;
		CharacterReferences.EyeBallLeftMad.sprite = ListOfCharacters[number].eyeBall;
		CharacterReferences.EyeLeftOutlineMad.sprite = ListOfCharacters[number].eyeOutlineMad;
		CharacterReferences.EyeBrowLeftMad.sprite = ListOfCharacters[number].eyeBrown;
		CharacterReferences.EyeBagLeftMad.sprite = ListOfCharacters[number].eyeBag;
		CharacterReferences.mouthSad.sprite = ListOfCharacters[number].mouthSad;
		CharacterReferences.mouthNormal.sprite = ListOfCharacters[number].mouthNormal;
		CharacterReferences.mouthMad.sprite = ListOfCharacters[number].mouthMad;
		CharacterReferences.mouthHappy.sprite = ListOfCharacters[number].mouthHappy;
		CharacterReferences.nose.sprite = ListOfCharacters[number].nose;
		CharacterReferences.shadowBig.sprite = ListOfCharacters[number].whiteCircle;
		CharacterReferences.shadowSmallLeft.sprite = ListOfCharacters[number].whiteCircle;
		CharacterReferences.shadowSmallRight.sprite = ListOfCharacters[number].whiteCircle;
		CharacterReferences.downAccessory.sprite = ListOfCharacters[number].downAccessory;
		CharacterReferences.upAccessory.sprite = ListOfCharacters[number].upAccessory;
	}
}
