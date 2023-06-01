using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

///<summary>
///<para>Scene:All/NameOfScene/NameOfScene1,NameOfScene2,NameOfScene3...</para>
///<para>Object:N/A</para>
///<para>Description: Sample Description </para>
///</summary>

public class CharacterParser : MonoBehaviour {

	public struct CharacterStruct
	{
		public string characterName;
		public int maxTip;
		public int patience;
	}
	public static List<CharacterStruct> ListOfCharacters=new List<CharacterStruct>();
	// Use this for initialization
	void Start () {
		if( ListOfCharacters.Count == 0 ) ParseXML();
		 
	}
	
	void ParseXML()
	{
		TextAsset aset =(TextAsset)Resources.Load("Characters/Characters");
		XmlDocument xml= new XmlDocument();
		xml.LoadXml(aset.ToString());


		XmlNodeList appNodes = xml.SelectNodes("/xml/character");

//		int number=appNodes.Count;

		foreach (XmlNode node in appNodes)
		{

			CharacterStruct SingleCharacter=new CharacterStruct
			{
				characterName = node.Attributes.GetNamedItem("name").Value
						
			};
			SingleCharacter.maxTip = int.Parse(node.SelectSingleNode("maxtip").InnerText);
			SingleCharacter.patience = int.Parse(node.SelectSingleNode("patience").InnerText);
			ListOfCharacters.Add(SingleCharacter);

		 
		}

	}
}
