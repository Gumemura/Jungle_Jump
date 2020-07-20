using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IPHLootBox : MonoBehaviour {

	private Transform timeTracker;

	private string itensPath; //localization of txt with itens info

	private string[] lines; //Will store all lines of txt

	private bool canCountHold; //Will inform when to start counting how long the button have been pressed
	private float countHoldButton; //Will count how long the button have been pressed
	public float countHoldButtonIncrease; //How much the counter will increase
	public float countHoldButtonLimit; // Limit to the counter

	public Transform lootInfoCanvas;
	public Transform canvasGiveLoot;
	public Transform lootNameText;


	private string lootNameSelected; //loot item will be given to player
	public int randomCyleTimes;

	// Use this for initialization
	void Start () {
		//Basic conditions for txt acess
		itensPath = Application.persistentDataPath + "\\itens.txt";
		lines = File.ReadAllLines(itensPath);

		timeTracker = GameObject.Find("LootBoxTime").transform;
	}
	
	//Update is called once per frame
	void Update () {
		// This function will change the text of the button when timer reach 0
		CheckCountdown();

		//Checking if the button was just pressed or holding
		CheckHoldButton();

	}

	//Checking if the button was just pressed or holding
	//This is done with Pointer_Down() and Pointer_Up()
	private void CheckHoldButton(){
		if(canCountHold){
			countHoldButton += countHoldButtonIncrease * Time.deltaTime;
			if(countHoldButton >= countHoldButtonLimit){
				lootInfoCanvas.gameObject.SetActive(true);
				canCountHold = false;
			}
		}

	}

	public void OnClick(){
		//Function called when lootbox button is clicked

		//Checking if still in countdown by verifying if timeTotal (from IPHCountdown) is greater then 0
		if(timeTracker.GetComponent<IPHTimeTracker>().timeTotal > 0){
			//if still on countdown, visual feedback ("blink") indicating it
			transform.Find("TextCountdow").GetComponent<IPHCountdownText>().InvokeShake();
		}else{
			canvasGiveLoot.gameObject.SetActive(true);
			lootNameText.GetComponent<Text>().text = SelectedLoot();
			timeTracker.GetComponent<IPHTimeTracker>().SetNewCountdown();
		}
	}

	private void CheckCountdown(){
		transform.Find("TextCountdow").gameObject.SetActive(timeTracker.GetComponent<IPHTimeTracker>().timeTotal > 0);
		transform.Find("TextRewards").gameObject.SetActive(timeTracker.GetComponent<IPHTimeTracker>().timeTotal <= 0);
	}

	//Called when button is pressed
	public void Pointer_Down(){
		canCountHold = true;
	}

	//Called when button is released
	public void Pointer_Up(){
		if(countHoldButton < countHoldButtonLimit){
			OnClick();
		}
		canCountHold = false;
		countHoldButton = 0;
	}

	//Selectiing which type of loot will be given to player
	public string SelectedLoot(){
		float[] weights = {71.0f, 24.0f, 5.3f, 1.0f};
		float sum = 0;

		foreach(float weight in weights){
			sum += weight;
		}

		float prize = Random.Range(0.0f, sum);

		//use recursion?
		if(prize <= weights[0]){
			FindItemOnTxt("Common");
		}else{
			prize -= weights[0];

			if(prize <= weights[1]){
				FindItemOnTxt("Uncommon");
			}else{
				prize -= weights[1];

				if(prize <= weights[2]){
					FindItemOnTxt("Rare");
				}else{
					prize -= weights[2];

					if(prize <= weights[3]){
						FindItemOnTxt("Mythic");
					}else{ 
						prize -= weights[3];
					}
				}
			}
		}
		
		return lootNameSelected;
	}

	//After calculating which item's rarity player will receive, its time to select which specifically item player will receive, among all of the 
	//same rarity
	//How does this function do that:
	// 1 cyle trought array looking for rarity match
	// 2 when found, 'randomSelector' (random number) starts to decrease for each analyzed line 
	// 3 if next line havent same rarity time, index go back to the line where fisrt occurance od rarity type was found
	// 4 then it goes until 'randomSelector' reaches 0, when we select the item
	// resuming, its a "eeny, meeny, miny, mo" algorithm
	void FindItemOnTxt(string rarity){
		int randomSelector = Random.Range(1, randomCyleTimes);
		int initialIndex = 0;
		bool find = false;

		for(int i = 0; i < lines.Length; i++){
			string[] itenAllInfos = lines[i].Split(' ');

			if(itenAllInfos[2] == rarity){
				if(!find){
					initialIndex = i;
				}
				find = true;
				if(randomSelector <= 0){
					lootNameSelected = itenAllInfos[0] + ", " + itenAllInfos[2];
					break;
				}

				if(i == lines.Length - 1){
					i = initialIndex;
				}
				randomSelector--;
			}else{
				if(find){
					i = initialIndex;
				}
			}
		}
	}
}


