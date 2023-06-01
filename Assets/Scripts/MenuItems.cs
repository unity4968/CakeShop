using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MenuItems   {
	public static Dictionary <string,MenuItemData> menuItemsDictionary = new Dictionary<string, MenuItemData>();
	public static List <MenuItemData> lockedMenuItems  = new List< MenuItemData>();

	public static bool bAllItemsUnlocked = false;
	 
	public MenuItems()
	{
	 
		if( menuItemsDictionary.Count == 0)
		{
			//OBLICI
			menuItemsDictionary.Add("m1_01",new MenuItemData {Atlas = "CakeShapes", Name = "m1_01", decorationType = DecorationType.CakeShapeCylinder, Locked = false });
			menuItemsDictionary.Add("m1_02",new MenuItemData { Atlas = "CakeShapes", Name = "m1_02", decorationType = DecorationType.CakeShapeCylinder, Locked = false });
			menuItemsDictionary.Add("m1_03",new MenuItemData { Atlas = "CakeShapes", Name = "m1_03", decorationType = DecorationType.CakeShapeCylinder, Locked = false });
			menuItemsDictionary.Add("m1_04",new MenuItemData { Atlas = "CakeShapes", Name = "m1_04", decorationType = DecorationType.CakeShapeCylinder, Locked = true });
			menuItemsDictionary.Add("m1_05",new MenuItemData { Atlas = "CakeShapes", Name = "m1_05", decorationType = DecorationType.CakeShapeCylinder, Locked = true });

			menuItemsDictionary.Add("m1_06",new MenuItemData {Atlas = "CakeShapes", Name = "m1_06", decorationType = DecorationType.CakeShapeStar, Locked = false });
			menuItemsDictionary.Add("m1_07",new MenuItemData { Atlas = "CakeShapes", Name = "m1_07", decorationType = DecorationType.CakeShapeStar, Locked = false });
			menuItemsDictionary.Add("m1_08",new MenuItemData { Atlas = "CakeShapes", Name = "m1_08", decorationType = DecorationType.CakeShapeStar, Locked = false });
			menuItemsDictionary.Add("m1_09",new MenuItemData { Atlas = "CakeShapes", Name = "m1_09", decorationType = DecorationType.CakeShapeStar, Locked = true });
			menuItemsDictionary.Add("m1_10",new MenuItemData { Atlas = "CakeShapes", Name = "m1_10", decorationType = DecorationType.CakeShapeStar, Locked = true });

			menuItemsDictionary.Add("m1_11",new MenuItemData {Atlas = "CakeShapes", Name = "m1_11", decorationType = DecorationType.CakeShapeHeart, Locked = false });
			menuItemsDictionary.Add("m1_12",new MenuItemData { Atlas = "CakeShapes", Name = "m1_12", decorationType = DecorationType.CakeShapeHeart, Locked = false });
			menuItemsDictionary.Add("m1_13",new MenuItemData { Atlas = "CakeShapes", Name = "m1_13", decorationType = DecorationType.CakeShapeHeart, Locked = false });
			menuItemsDictionary.Add("m1_14",new MenuItemData { Atlas = "CakeShapes", Name = "m1_14", decorationType = DecorationType.CakeShapeHeart, Locked = true });
			menuItemsDictionary.Add("m1_15",new MenuItemData { Atlas = "CakeShapes", Name = "m1_15", decorationType = DecorationType.CakeShapeHeart, Locked = true });
			//**************************************************************************************************************************

			//FILOVI
			menuItemsDictionary.Add("m2_01",new MenuItemData {Atlas = "CakeCream", Name = "m2_01", decorationType = DecorationType.CreamCylinder, Locked = false });
			menuItemsDictionary.Add("m2_02",new MenuItemData { Atlas = "CakeCream", Name = "m2_02", decorationType = DecorationType.CreamCylinder, Locked = false });
			menuItemsDictionary.Add("m2_03",new MenuItemData { Atlas = "CakeCream", Name = "m2_03", decorationType = DecorationType.CreamCylinder, Locked = false });
			menuItemsDictionary.Add("m2_04",new MenuItemData { Atlas = "CakeCream", Name = "m2_04", decorationType = DecorationType.CreamCylinder, Locked = true });
			menuItemsDictionary.Add("m2_05",new MenuItemData { Atlas = "CakeCream", Name = "m2_05", decorationType = DecorationType.CreamCylinder, Locked = true });

			menuItemsDictionary.Add("m2_06",new MenuItemData {Atlas = "CakeCream", Name = "m2_06", decorationType = DecorationType.CreamStar, Locked = false });
			menuItemsDictionary.Add("m2_07",new MenuItemData { Atlas = "CakeCream", Name = "m2_07", decorationType = DecorationType.CreamStar, Locked = false });
			menuItemsDictionary.Add("m2_08",new MenuItemData { Atlas = "CakeCream", Name = "m2_08", decorationType = DecorationType.CreamStar, Locked = false });
			menuItemsDictionary.Add("m2_09",new MenuItemData { Atlas = "CakeCream", Name = "m2_09", decorationType = DecorationType.CreamStar, Locked = true });
			menuItemsDictionary.Add("m2_10",new MenuItemData { Atlas = "CakeCream", Name = "m2_10", decorationType = DecorationType.CreamStar, Locked = true });

			menuItemsDictionary.Add("m2_11",new MenuItemData {Atlas = "CakeCream", Name = "m2_11", decorationType = DecorationType.CreamHeart, Locked = false });
			menuItemsDictionary.Add("m2_12",new MenuItemData { Atlas = "CakeCream", Name = "m2_12", decorationType = DecorationType.CreamHeart, Locked = false });
			menuItemsDictionary.Add("m2_13",new MenuItemData { Atlas = "CakeCream", Name = "m2_13", decorationType = DecorationType.CreamHeart, Locked = false });
			menuItemsDictionary.Add("m2_14",new MenuItemData { Atlas = "CakeCream", Name = "m2_14", decorationType = DecorationType.CreamHeart, Locked = true });
			menuItemsDictionary.Add("m2_15",new MenuItemData { Atlas = "CakeCream", Name = "m2_15", decorationType = DecorationType.CreamHeart, Locked = true });
			//**************************************************************************************************************************
			
			//PERLE
			menuItemsDictionary.Add("m3_01",new MenuItemData {Atlas = "DecorationPearls", Name = "m3_01", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_02",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_02", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_03",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_03", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_04",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_04", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_05",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_05", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_06",new MenuItemData {Atlas = "DecorationPearls", Name = "m3_06", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_07",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_07", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_08",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_08", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_09",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_09", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_10",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_10", decorationType = DecorationType.Pearls, Locked = false });

			menuItemsDictionary.Add("m3_11",new MenuItemData {Atlas = "DecorationPearls", Name = "m3_11", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_12",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_12", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_13",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_13", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_14",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_14", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_15",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_15", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_16",new MenuItemData {Atlas = "DecorationPearls", Name = "m3_16", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_17",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_17", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_18",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_18", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_19",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_19", decorationType = DecorationType.Pearls, Locked = false });
			menuItemsDictionary.Add("m3_20",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_20", decorationType = DecorationType.Pearls, Locked = false });

			menuItemsDictionary.Add("m3_21",new MenuItemData {Atlas = "DecorationPearls", Name = "m3_21", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_22",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_22", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_23",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_23", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_24",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_24", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_25",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_25", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_26",new MenuItemData {Atlas = "DecorationPearls", Name = "m3_26", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_27",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_27", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_28",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_28", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_29",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_29", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_30",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_30", decorationType = DecorationType.Pearls, Locked = true });

			menuItemsDictionary.Add("m3_31",new MenuItemData {Atlas = "DecorationPearls", Name = "m3_31", decorationType = DecorationType.Pearls, Locked = true });
			menuItemsDictionary.Add("m3_32",new MenuItemData { Atlas = "DecorationPearls", Name = "m3_32", decorationType = DecorationType.Pearls, Locked = true });

			//**************************************************************************************************************************
			
			//PERLE
			menuItemsDictionary.Add("m4_01",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m4_01", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit1"});
			menuItemsDictionary.Add("m4_02",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_02", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit1"});
			menuItemsDictionary.Add("m4_03",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_03", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit2"});
			menuItemsDictionary.Add("m4_04",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_04", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit3"});
			menuItemsDictionary.Add("m4_05",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_05", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit3"});
			menuItemsDictionary.Add("m4_06",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m4_06", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit3"});
			menuItemsDictionary.Add("m4_07",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_07", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit4" });
			menuItemsDictionary.Add("m4_08",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_08", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit3"});
			menuItemsDictionary.Add("m4_09",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_09", decorationType = DecorationType.Fruit, Locked = true , PrefabName = "PrefFruit1"});
			menuItemsDictionary.Add("m4_10",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_10", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit2"});

			menuItemsDictionary.Add("m4_11",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m4_11", decorationType = DecorationType.Fruit, Locked = true , PrefabName = "PrefFruit2"});
			menuItemsDictionary.Add("m4_12",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_12", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit2"});
			menuItemsDictionary.Add("m4_13",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_13", decorationType = DecorationType.Fruit, Locked = true , PrefabName = "PrefFruit2"});
			menuItemsDictionary.Add("m4_14",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_14", decorationType = DecorationType.Fruit, Locked = false , PrefabName = "PrefFruit4"});
			menuItemsDictionary.Add("m4_15",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m4_15", decorationType = DecorationType.Fruit, Locked = true , PrefabName = "PrefFruit4"});
			menuItemsDictionary.Add("m4_16",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m4_16", decorationType = DecorationType.Fruit, Locked = true , PrefabName = "PrefFruit5"});

			//**************************************************************************************************************************
			
			//STICKERS
			menuItemsDictionary.Add("m5_01",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_01", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker1"});
			menuItemsDictionary.Add("m5_02",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_02", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker1"});
			menuItemsDictionary.Add("m5_03",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_03", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker1" });
			menuItemsDictionary.Add("m5_04",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_04", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker1"});
			menuItemsDictionary.Add("m5_05",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_05", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker2"});
			menuItemsDictionary.Add("m5_06",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_06", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker2"});
			menuItemsDictionary.Add("m5_07",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_07", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker2"});
			menuItemsDictionary.Add("m5_08",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_08", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker2"});
			menuItemsDictionary.Add("m5_09",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_09", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker3"});
			menuItemsDictionary.Add("m5_10",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_10", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker3"});
			
			menuItemsDictionary.Add("m5_11",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m5_11", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker3"});
			menuItemsDictionary.Add("m5_12",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_12", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker3"});
			menuItemsDictionary.Add("m5_13",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_13", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker3"});
			menuItemsDictionary.Add("m5_14",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_14", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker3"});
			menuItemsDictionary.Add("m5_15",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_15", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker3"});
			menuItemsDictionary.Add("m5_16",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m5_16", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker5"});
			menuItemsDictionary.Add("m5_17",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_17", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker4"});
			menuItemsDictionary.Add("m5_18",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_18", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker4"});
			menuItemsDictionary.Add("m5_19",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_19", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker4"});
			menuItemsDictionary.Add("m5_20",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_20", decorationType = DecorationType.Sticker, Locked = false , PrefabName = "PrefSticker4"});

			menuItemsDictionary.Add("m5_21",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m5_21", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker4"});
			menuItemsDictionary.Add("m5_22",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_22", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker4"});
			menuItemsDictionary.Add("m5_23",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_23", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker7"});
			menuItemsDictionary.Add("m5_24",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_24", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker5"});
			menuItemsDictionary.Add("m5_25",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_25", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker6"});
			menuItemsDictionary.Add("m5_26",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m5_26", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker6"});
			menuItemsDictionary.Add("m5_27",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_27", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker6"});
			menuItemsDictionary.Add("m5_28",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_28", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker6"});
			menuItemsDictionary.Add("m5_29",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_29", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker6"});
			menuItemsDictionary.Add("m5_30",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_30", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker6"});

			menuItemsDictionary.Add("m5_31",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m5_31", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker7" });
			menuItemsDictionary.Add("m5_32",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_32", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker5"});
			menuItemsDictionary.Add("m5_33",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_33", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker5"});
			menuItemsDictionary.Add("m5_34",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_34", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker5"});
			menuItemsDictionary.Add("m5_35",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_35", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker5"});
			menuItemsDictionary.Add("m5_36",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m5_36", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker5"});
			menuItemsDictionary.Add("m5_37",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_37", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker9"});
			menuItemsDictionary.Add("m5_38",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_38", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker7"});
			menuItemsDictionary.Add("m5_39",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_39", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker7"});
			menuItemsDictionary.Add("m5_40",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_40", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker5"});

			menuItemsDictionary.Add("m5_41",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m5_41", decorationType = DecorationType.Sticker, Locked = true  , PrefabName = "PrefSticker8"});
			menuItemsDictionary.Add("m5_42",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_42", decorationType = DecorationType.Sticker, Locked = true  , PrefabName = "PrefSticker8"});
			menuItemsDictionary.Add("m5_43",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_43", decorationType = DecorationType.Sticker, Locked = true  , PrefabName = "PrefSticker8"});
			menuItemsDictionary.Add("m5_44",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_44", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker8"});
			menuItemsDictionary.Add("m5_45",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_45", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker9"});
			menuItemsDictionary.Add("m5_46",new MenuItemData {Atlas = "FruitsAndStickers", Name = "m5_46", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker7"});
			menuItemsDictionary.Add("m5_47",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_47", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker7"});
			menuItemsDictionary.Add("m5_48",new MenuItemData { Atlas = "FruitsAndStickers", Name = "m5_48", decorationType = DecorationType.Sticker, Locked = true , PrefabName = "PrefSticker5"});
			 
		}

		//AKO SU OTKLJUCANI SVI UKRASI
//		if(bAllItemsUnlocked && false)  
//		{
//			foreach(KeyValuePair<string,MenuItemData>  kvp in menuItemsDictionary)
//			{
//				kvp.Value.Locked = false;
//			}
//			lockedMenuItems.Clear();
//		}
// 		else
//		{
			lockedMenuItems.Clear();
			//PODESAVANJE OTKLJUCANIH ITEMA
			DataManager.Instance.PopulateUnlockedMenuItems(); 

			//DataManager.Instance. MeniOtkljucano[0] = "04;10;" ;//TODO: brisi - test

			int decoration =1;
			bool bNextMenu = false;
			string key ="";
			for(int menu = 1;menu<=5;menu++)
			{
				bNextMenu = false;
				decoration =1;
				key = "m"+menu.ToString()+"_"+decoration.ToString().PadLeft(2,'0');
				while(!bNextMenu)
				{
					if(menuItemsDictionary[key].Locked ) 
					{
						if(! DataManager.Instance.MeniOtkljucano[menu-1].Contains(decoration.ToString().PadLeft(2,'0')+";"))
							lockedMenuItems.Add(menuItemsDictionary[key]);
						else
						{
//							Debug.Log("OTKLJUCAN " + key);
							menuItemsDictionary[key].Locked = false;
						}

				   }
					decoration++;
					key = "m"+menu.ToString()+"_"+decoration.ToString().PadLeft(2,'0');
					if(!menuItemsDictionary.ContainsKey(key)) 
					{
						bNextMenu = true;
					}
				}
			 
			}
//		}

 

	}




	public  Dictionary <int,MenuItemData>  ReturmMenu(int menu, DecorationType decType)
	{
		Dictionary <int,MenuItemData> m = new Dictionary<int, MenuItemData>();
		string test = "m"+menu.ToString()+"_";
		int i = 1;
		if(menu!=2 || decType == DecorationType.Empty) //ako nije odabrana baza vracaju se sve stavke iz drugog menija
		{
			foreach(  KeyValuePair<string,MenuItemData>  kvp in menuItemsDictionary)
			{
				if( kvp.Key.StartsWith (test))
				{
					if(menu == 3) kvp.Value.PrefabName = "PrefPearls";
					m.Add( i,kvp.Value);
					i++;
				}
			}
		}
		else
		{
			foreach(  KeyValuePair<string,MenuItemData>  kvp in menuItemsDictionary)
			{
				if( kvp.Key.StartsWith(test) ) 
				{
					if( (decType == DecorationType.CakeShapeCylinder && kvp.Value.decorationType ==  DecorationType.CreamCylinder)
					   || (decType == DecorationType.CakeShapeStar && kvp.Value.decorationType ==  DecorationType.CreamStar)
					   || (decType == DecorationType.CakeShapeHeart && kvp.Value.decorationType ==  DecorationType.CreamHeart))
					{
						m.Add(i,kvp.Value);
						i++;
					}
				}
			}
		}
		return m;
	}
	/*
	public static string ReturnDollImage()
	{
		string ret = "";
		switch(DailyRewards.nagrada)
		{
		case 1:
			ret = "m1_01";
			break;
		case 2:
			ret = "m1_02";
			break;
		case 3:
			ret = "m1_03";
			break;
		case 4:
			ret = "m1_04";
			break;
		case 5:
			ret = "m1_05";
			break;
		case 6:
			ret = "m1_06";
			break;
		case 7:
			ret = "m1_07";
			break;
		case 8:
			ret = "m1_08";
			break;
		case 9:
			ret = "m1_09";
			break;
		}


		if(menuItemsDictionary.Count >0)
		{
			menuItemsDictionary["m1_02"].Locked = (DailyRewards.nagrada <2);
			menuItemsDictionary["m1_03"].Locked = (DailyRewards.nagrada <3);
			menuItemsDictionary["m1_04"].Locked = (DailyRewards.nagrada <4);
			menuItemsDictionary["m1_05"].Locked = (DailyRewards.nagrada <5);
			menuItemsDictionary["m1_06"].Locked = (DailyRewards.nagrada <6);
			menuItemsDictionary["m1_07"].Locked = (DailyRewards.nagrada <7);
			menuItemsDictionary["m1_08"].Locked = (DailyRewards.nagrada <8);
			menuItemsDictionary["m1_09"].Locked = (DailyRewards.nagrada <9);
		}
		return ret;
	}

	*/ 
}

public enum DecorationType
{
	Empty,
	CakeShapeCylinder,
	CakeShapeStar,
	CakeShapeHeart,
	CreamCylinder,
	CreamStar,
	CreamHeart,
	Pearls,
	Fruit,
	Sticker
 
}

 
public class MenuItemData
{
	public string  Atlas;
	public string Name;
	public DecorationType decorationType; 
	public bool Locked;
	public string PrefabName;
}

 
