﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPHLootBox : MonoBehaviour {

	public float speed = 1.0f; //how fast it shakes
	public float amount = 1.0f; //how much it shakes

	public Transform timeTracker;

	// Use this for initialization
	void Start () {
		timeTracker = GameObject.Find("LootBoxTime").transform;
	}
	
	//Update is called once per frame
	void Update () {
		ChangeButtonText();
	}

	public void OnClick(){
		//Checking if still in countdown by verifying if timeTotal (from IPHCountdown) is greater then 0
		// if(transform.Find("TimeManager").GetComponent<IPHTimeTracker>().timeTotal > 0){

		if(timeTracker.GetComponent<IPHTimeTracker>().timeTotal > 0){
			//if still on countdown, feedback indicating it
			transform.Find("TextCountdow").GetComponent<IPHCountdownText>().InvokeShake();
		}else{ 
			timeTracker.GetComponent<IPHTimeTracker>().SetNewCountdown();
		}
	}

	private void ChangeButtonText(){
		transform.Find("TextCountdow").gameObject.SetActive(timeTracker.GetComponent<IPHTimeTracker>().timeTotal > 0);
		transform.Find("TextRewards").gameObject.SetActive(timeTracker.GetComponent<IPHTimeTracker>().timeTotal <= 0);
	}
}


