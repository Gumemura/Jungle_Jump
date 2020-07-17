using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class IPHTimeTracker : MonoBehaviour {
	//This class could be a child of LootBox button as its data is used by it
	//but we ketp it separable because it will not be destroyed when loading game scene so we can keep track of time when user is playing

	//Will provide txt manipulation functions
	private Transform txtManipulator;

	//The converted time countdown to string
	//It will be send to class IPHCountdownText so it will be able to update its display text
	[HideInInspector]
	public static string timerText;

	//We will convert all the time to seconds and store it in this varibale. We will procedure this will because its easier to manipulate time when working 
	//with a single dimension
	[HideInInspector]
	public int timeTotal;

	//Here the operador can change the countdown duration
	//There is a limit to avoid non-conventional countdowns, like 70 minutes
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
	internal float instanceTime = 0;
	
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
		//Don't destroy this object when loading a new scene
		DontDestroyOnLoad(transform.gameObject);

		//Finding txtmanipulator
		txtManipulator = GameObject.Find("TxtManipulator").transform;

		timeTotal = (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("rmn") - CaculateRemainingTime();

		//Converting countdown to seconds and decreasing it
		StartCoundownTimer();

		//Modifying timeTotal accordingly to the txt
		//Will be based on the diference between the horary aplication was last quit and the horary its opened

	}
	 
	void StartCoundownTimer(){
		//Converting everything to seconds
		//Why are we adding 1? Lets supose the timer is 2 minutes. Without the plus one the timer would start at 1:59, due to 
		//the comand "timeTotal -= 1" on UpdateTimer function

		//CaculateRemainingTime(): Modifying timeTotal accordingly to the txt
		//Will be based on the diference between the horary aplication was last quit and the horary its opened

		if(timeTotal > 0){
			//Checking which dimension will be displayed on the timer
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
			//Updating the time. This is why why are adding 1 on timetotal
			timeTotal -= 1;
		}else{
			//If timeTotal is 0, its time to stop!
			CancelInvoke("UpdateTimer");
		}

		//Clearing the string
		timerText = "";


		//Calculating time and appening it to string
		if(insertHour){
			timerText = Mathf.Floor(timeTotal/3600).ToString("00") + ":";
		}

		if(insertMinutes){
			timerText += Mathf.Floor((timeTotal%3600)/60).ToString("00") + ":"; 
		}

		timerText += Mathf.Floor(timeTotal%60).ToString("00");
	}

	//Static methods thats will return data calculate by this class

	//And this will provide to IPHCountdownText the new text that should be displayed
	public static string GetStringTime(){
		return timerText;
	}

	public void SetNewCountdown(){
		timeTotal = (hours * 3600) + (minutes * 60) + seconds + 1; 
		StartCoundownTimer();
	}

	void OnApplicationQuit(){
		//Storing time information when closing so we will be able to calculate remaining time for countdown when the app starts again

		//Storing remaining time
		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("rmn", timeTotal);

		//Storing date and time so we will be able to compare it to starting app's date
		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("clh", System.DateTime.Now.Hour);
		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("clm", System.DateTime.Now.Minute);
		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("cls", System.DateTime.Now.Second);

		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("cld", System.DateTime.Now.Day);
		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("cln", System.DateTime.Now.Month);
		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("cly", System.DateTime.Now.Year);
	}

	int CaculateRemainingTime(){
		//If month or year is diferent, or the game is being played after two days of last close, the countdown will be finished for sure as long as
		//the maximum countdown is 24 hours
		if(txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cln") != System.DateTime.Now.Month || 
			txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cly") != System.DateTime.Now.Year ||
			txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cld") + 2 <=  System.DateTime.Now.Day){
			print("N joga faz tempo");
			return 24 * 3600;
		}else{
			//Creating datetime with last closing app datae
			var closingAppDate = new DateTime((int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cly"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cln"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cld"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("clh"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("clm"),
											  (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("cls"));
			//Else, the player will be playing it at a maximum of two days from the last quit
			//So we can just return diference between two dates in seconds
			return (int)Mathf.Floor((float)(System.DateTime.Now - closingAppDate).TotalSeconds);
		}
	}
}
