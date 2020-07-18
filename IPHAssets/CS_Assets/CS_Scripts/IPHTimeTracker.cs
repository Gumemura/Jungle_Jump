using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

// this function keep record of time to manage when player can open a lootbox
// How its done:
// 1 on txt there will be recorded 2 relevant informations:
// 			(i)		- the remaining time to the button become active (the remaining time to finish the countdown)
//			(ii)	- time and date aplication was closed
// 2 when player opens the game for the second time, we will compare the actual time and date (System.DateTime.Now) with stored time and date (ii)
// 3 calculated the difference between both, we subtract the diference to the remaining countdown time (i)
//
// IMPORTANT DISCLAIMER: this functionality does not work on computer editor because we are using OnApplicationPause() method. To make it work on PC, 
// this method should be replaced by OnApplicationQuit(), as long as wont be execution suspension on PC, but on mobile.
//
// This class could be a child of LootBox button as its data is used by it but we keep it outside ecause it will not be destroyed
// when loading game scene so we can keep track of time when user is playing

public class IPHTimeTracker : MonoBehaviour {

	//Will provide txt manipulation functions
	private Transform txtManipulator;

	//The converted time countdown to string
	//It will be send to class IPHCountdownText to be  displayed as a text
	[HideInInspector]
	public static string timerText;

	//We will convert all the time to seconds and store it in this varibale. We will procede this will because its easier to manipulate time when working 
	//with a single dimension
	[HideInInspector]
	public int timeTotal;

	//Here the operador can change the countdown duration
	//There is a limit to avoid non-conventional/ weirds countdowns, like 70 minutes
	[Range(0,24)]
	public int hours;

	[Range(0,60)]
	public int minutes;

	[Range(0,60)]
	public int seconds;

	//Booleans that will inform if the time format must be 00:00:00 (h:m:s), 00:00 (m:s) or just 00 (s)
	private static bool insertHour;
	private static bool insertMinutes;

	//The time this instance of the time manager has been in the game
	//Used to destroy copies
	internal float instanceTime = 0;
	
	//Awake methos is only used to destroying copies
	void Awake(){
		//Imported fom IPH Global Music :)

		//Find all the time objects in the scene
		GameObject[] timeObjects = GameObject.FindGameObjectsWithTag("TimeManager");
		
		//Keep only the time object which has been in the game for more than 0 seconds
		if(timeObjects.Length > 1){
			foreach(var timeObject in timeObjects)
			{
				if(timeObject.GetComponent<IPHTimeTracker>().instanceTime <= 0 ){
					Destroy(gameObject);
				}    
			}
		}
	}

	void Start (){
		//Finding txtManipulator, who will provide us the functions to read and write on txt
		txtManipulator = GameObject.Find("TxtManipulator").transform;

		//Perpetuating this object between scenes
		DontDestroyOnLoad(transform.gameObject);

		//Starting countdown
		SetInitialCountdown();
	}
	 
	void StartCoundownTimer(){
		//Its important to verify if timeTotal is positive because it can become negative (it will be explained why CaculateRemainingTime() function)
		if(timeTotal > 0){
			//Determining timer format
			insertHour = (hours > 0);
			insertMinutes = (minutes > 0 || hours > 0);

			//Updating timer for each second
			InvokeRepeating("UpdateTimer", 0.0f, 1.0f);	
		}else{
			//Just in case timeTotal is negative
			timeTotal = 0;
		}
	}
	 
	public void UpdateTimer(){
		if(timeTotal > 0){
			//And the countdown begins!
			timeTotal -= 1;
		}else{
			//If timeTotal is 0, its time to stop!
			CancelInvoke("UpdateTimer");
		}

		//Clearing the string
		timerText = "";

		//Converting timerText to a visual format and appending it to a string that will be sent to IPHCountdownText (class of countdown text in editor)
		if(insertHour){
			timerText = Mathf.Floor(timeTotal/3600).ToString("00") + ":";
		}

		if(insertMinutes){
			timerText += Mathf.Floor((timeTotal%3600)/60).ToString("00") + ":"; 
		}

		timerText += Mathf.Floor(timeTotal%60).ToString("00");
	}

	//Here is were stored data on txt and System.DateTime.Now are calculated
	int CaculateRemainingTime(){
		//Checking extreme condition that will dispense the need for calculations 
		//
		//If month or year is diferent, it means that the game has not been played for quite a while (at least one month)
		//or
		//If game has not been played for 2 days, it means that even if the countdown is on its maximum value (24 hours), its already done
		//Exemple, even if we assume countdown is at it max value (24 hours) 
		//if game was last closed on day 10 at 23:59, the countdown will be running until day 11 at 23:59. At day 12 it would be already done
		if(txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cln") != System.DateTime.Now.Month || 
			txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cly") != System.DateTime.Now.Year ||
			txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cld") + 2 <=  System.DateTime.Now.Day){
			//Returnig huge number that can make timeTotal goes negative. This is why we make must make sure timeTotal is always positive, as 
			//its done onStartCoundownTimer()
			return 24 * 3600;
		}else{
			//Bringing txt info to object. Here we are creating a DateTime with year, month, day, hour, minute and second
			//time would be enought for us, but its not possible to create a DateTime with only time
			var closingAppDate = new DateTime((int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cly"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cln"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cld"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("clh"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("clm"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cls"));

			//Acctualy, we dont need year or month information But its stored so we can create a DateTime and execute the line below to easily calcule time 
			//differece between two dates
			return (int)Mathf.Floor((float)(System.DateTime.Now - closingAppDate).TotalSeconds);
		}
	}

	//Returns timerText
	//Is used by IPHCountdownText to update its text
	public static string GetStringTime(){
		return timerText;
	}

	//Starts a new countdown when the previous is over
	//Used by IPHLootBox when the button is clicked
	public void SetNewCountdown(){
		timeTotal = (hours * 3600) + (minutes * 60) + seconds + 1; 
		StartCoundownTimer();
	}

	//Set the countdown remaining time and start it
	//As long its a mobile game, its called on OnApplicationFocus(). Otherwise it would be called on Start
	public void SetInitialCountdown(){
		timeTotal = (int)GameObject.Find("TxtManipulator").transform.GetComponent<IPHTxtManipulation>().ReadTxt("rmn") - CaculateRemainingTime();
		StartCoundownTimer();
	}

	// #if UNITY_EDITOR_WIN
	// void OnApplicationQuit(){
	// #elif UNITY_ANDROID 
	// void OnApplicationPause (bool pauseStatus){
	// #endif

	//Here is where date and time data stored in txt. Its called when apllication is suspended
	//As long its a mobile game, we use OnApplicationPause(). Otherwise it would be used OnApplicationQuit()
	void OnApplicationPause (bool pauseStatus){
		if(pauseStatus){
			//Storing time information when closing so we will be able to calculate remaining time for countdown when the app starts again
			txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("rmn", timeTotal);

			//Storing date and time so we will be able to compare it to starting app's date
			txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("clh", System.DateTime.Now.Hour);
			txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("clm", System.DateTime.Now.Minute);
			txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("cls", System.DateTime.Now.Second);

			txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("cld", System.DateTime.Now.Day);
			txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("cln", System.DateTime.Now.Month);
			txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("cly", System.DateTime.Now.Year);
			CancelInvoke("UpdateTimer");
		}
	}
	//reactivate the countdown. Acts like a Start() method
	void OnApplicationFocus(bool isFocus){
		if(isFocus){;
			SetInitialCountdown();
		}
	}
}