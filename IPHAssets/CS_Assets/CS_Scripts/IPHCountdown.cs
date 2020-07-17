using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPHCountdown : MonoBehaviour {

	//The string we will append to the text object
	internal string timerText;

	//Stores initial  font sizr
	internal int initialFontSize;

	//How much will the font expand to give visual feedbak
	public int fontGrow = 1;

	//Limit size font 
	public int fontLimit = 80;

	//counts how many times the font has grow
	internal int growCount = 0;

	//Limit of cycle of grow
	public int growLimit = 0;


	//We will convert all the time do seconds and store it in this varibale. We will procedure this will because its easier to manipulate time when working 
	//wit ha single dimension
	[HideInInspector]
	public int timeTotal;

	//Here the you can determine the size of the countdown
	[Range(0,24)]
	public int hours;

	[Range(0,60)]
	public int minutes;

	[Range(0,60)]
	public int seconds;

	//Booleans that will inform if the time format must be 00:00:00 (h:m:s), 00:00 (m:s) or just 00 (s)
	private bool insertHour;
	private bool insertMinutes;

	
	void Start (){
		StartCoundownTimer();
	}
	 
	void StartCoundownTimer(){
		//Converting everything to seconds
		//Why are we adding 1? Lets supose the timer is 2 minutes. Without the plus one the timer would start at 1:59, due the comand "timeTotal -= 1"
		//on UpdateTimer function
		timeTotal = (hours * 3600) + (minutes * 60) + seconds + 1; 

		//Checking which dimension will be displayed on the timer
		insertHour = (hours > 0);
		insertMinutes = (minutes > 0 || hours > 0);

		//Updating timer after each 1 second
		InvokeRepeating("UpdateTimer", 0.0f, 1.0f);
	}
	 
	void UpdateTimer(){
		if(timeTotal > 0){
			//Updating the time. This is why why are adding 1 on timetotal
			timeTotal -= 1;
		}else{
			CancelInvoke();
		}

		//Clearing the string
		timerText = "";


		//Covnerting time to string
		if(insertHour){
			timerText = Mathf.Floor(timeTotal/3600).ToString("00") + ":";
		}

		if(insertMinutes){
			timerText += Mathf.Floor((timeTotal%3600)/60).ToString("00") + ":"; 
		}

		timerText += Mathf.Floor(timeTotal%60).ToString("00");

		//sending string to text
		transform.GetComponent<Text>().text = timerText;
	}

	public void InvokeShake(){
		//The only purpose  of this function is to invoke repeat shaek()
		initialFontSize = transform.GetComponent<Text>().fontSize;
		InvokeRepeating("Shake", 0.0f, 0.01f);
	}

	public void Shake(){
		//This function will provide a visual feedback to player by blinking the countdown text
		//Its made by raising and lowering the font

		//Raising the font
		transform.GetComponent<Text>().fontSize += fontGrow;

		//If its going too big or too low, its time to invert 
		//fontGrow > 0 goes for growing
		//fontGrow < 0 goes for lowering
		if(transform.GetComponent<Text>().fontSize >= fontLimit || transform.GetComponent<Text>().fontSize <= initialFontSize){
			fontGrow *= -1;
			growCount++;

			//If its on its original size, it may be time to stop
			//And thats what growCount does: it counts the times a full cycle of growing/lowering is complete
			if(transform.GetComponent<Text>().fontSize <= initialFontSize){
				if(growCount >= growLimit){
					fontGrow = 1;
					growCount = 0;
					CancelInvoke("Shake");
				}	
			}
		}
	}
}
