using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IPHLootBox : MonoBehaviour {

	private Transform timeTracker;

	private bool canCountHold; //Will inform when to start counting how long the button have been pressed
	private float countHoldButton; //Will coun how long the button have been pressed
	public float countIncrease; //How much the counter will increase
	public float countLimit; // Limit to the counter


	// Use this for initialization
	void Start () {
		timeTracker = GameObject.Find("LootBoxTime").transform;
	}
	
	//Update is called once per frame
	void Update () {
		// This function will change the text of the button when timer reach 0
		CheckCountdown();
		CheckHoldButton();

	}

	private void CheckHoldButton(){
		if(canCountHold){
			countHoldButton += countIncrease * Time.deltaTime;
			if(countHoldButton >= countLimit){
				print("Abrir tabela de loot");
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
			timeTracker.GetComponent<IPHTimeTracker>().SetNewCountdown();
		}
	}

	private void CheckCountdown(){
		transform.Find("TextCountdow").gameObject.SetActive(timeTracker.GetComponent<IPHTimeTracker>().timeTotal > 0);
		transform.Find("TextRewards").gameObject.SetActive(timeTracker.GetComponent<IPHTimeTracker>().timeTotal <= 0);
	}

	public void testeDown(){
		canCountHold = true;
	}

	public void testeUp(){
		if(countHoldButton < countLimit){
			OnClick();
		}
		canCountHold = false;
		countHoldButton = 0;
	}
}


