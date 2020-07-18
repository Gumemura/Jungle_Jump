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
	public Transform lootCanvas;


	// Use this for initialization
	void Start () {
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
			countHoldButton += countIncrease * Time.deltaTime;
			if(countHoldButton >= countLimit){
				lootCanvas.gameObject.SetActive(true);
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

	//Called when button is pressed
	public void Pointer_Down(){
		canCountHold = true;
	}

	//Called when button is released
	public void Pointer_Up(){
		if(countHoldButton < countLimit){
			OnClick();
		}
		canCountHold = false;
		countHoldButton = 0;
	}
}


