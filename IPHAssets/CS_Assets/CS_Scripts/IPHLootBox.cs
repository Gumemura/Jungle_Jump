using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPHLootBox : MonoBehaviour {

	public float speed = 1.0f; //how fast it shakes
	public float amount = 1.0f; //how much it shakes

	// Use this for initialization
	void Start () {
		
	}
	
	//Update is called once per frame
	void Update () {
		ChangeButtonText_Expand();
	}

	public void OnClick(){
		//Checking if still in countdown by verifying if timeTotal (from IPHCountdown) is greater then 0
		// if(transform.Find("TimeManager").GetComponent<IPHTimeTracker>().timeTotal > 0){

		if(IPHTimeTracker.GetTotalTime() > 0){
			//if still on countdown, feedback indicating it
			transform.Find("TextCountdow").GetComponent<IPHCountdownText>().InvokeShake();
		}else{
			print("aperto");
		}
	}

	private void ChangeButtonText_Expand(){
		//if(transform.Find("TimeManager").GetComponent<IPHTimeTracker>().timeTotal <= 0){
		if(IPHTimeTracker.GetTotalTime() <= 0){
			transform.Find("TextCountdow").gameObject.SetActive(false);
			transform.Find("TextRewards").gameObject.SetActive(true);
		}
	}
}


