using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//This class display and update the list with loot itens and its respectly odds and rarity
//How is it made: whenever a button (NEXT or PREVIOUS) is clicekd, the exhibited list is updated with a new interval of itens 
public class IPHButtonNextLoot : MonoBehaviour {
	private string itensPath; //localization of txt with itens info

	public Transform itensName;
	public Transform itensOdd;
	public Transform itensRarity;

	public Transform buttonNext;
	public Transform buttonPrevious;

	[Range(0,7)]
	public int itensDisplayed;

	private string[] lines;

	private int startForLoop;
	// Use this for initialization
	void Start () {
		itensPath = Application.persistentDataPath + "\\itens.txt";
		startForLoop = 0;

		if(itensPath != null){
			lines = File.ReadAllLines(itensPath);

			buttonNext.gameObject.SetActive(lines.Length >= itensDisplayed);
			
			UpdateLootText(startForLoop);
		}
	}

	void TurnButtonsOff(){
		buttonPrevious.gameObject.SetActive(startForLoop != 0);
		buttonNext.gameObject.SetActive(startForLoop + itensDisplayed < lines.Length);
	}

	public void NextLoot(){
		startForLoop += itensDisplayed;
		UpdateLootText(startForLoop);

		TurnButtonsOff();
	}

	public void PreviousLoot(){
		startForLoop -= itensDisplayed;
		UpdateLootText(startForLoop);

		TurnButtonsOff();
	}

	void UpdateLootText(int num) {
		itensName.GetComponent<Text>().text = "";
		itensOdd.GetComponent<Text>().text = "";
		itensRarity.GetComponent<Text>().text = "";

		//TO DO: implement functionaly to accept names with spaces
		for(int i = num; i < itensDisplayed + num; i++){
			if(i < lines.Length){
				string[] itenAllInfos = lines[i].Split(' ');

				itensName.GetComponent<Text>().text += itenAllInfos[0] + "\n";
				itensOdd.GetComponent<Text>().text += itenAllInfos[1] + "\n";
				itensRarity.GetComponent<Text>().text += itenAllInfos[2] + "\n";
			}else{
				break;
			}
		}
	}
}
