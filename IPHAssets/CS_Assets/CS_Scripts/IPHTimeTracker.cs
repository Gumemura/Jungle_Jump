using System.Collections;
using System.Collections.Generic;
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
	public static int timeTotal;

	//Here the operador can change the countdown duration
	//There is a limit to avoid non-conventional countdowns, like 70 minutes
	[Range(0,24)]
	public int hours;

	[Range(0,60)]
	public int minutes;

	[Range(0,60)]
	public int seconds;

	//Booleans that will inform if the time format must be 00:00:00 (h:m:s), 00:00 (m:s) or just 00 (s)
	private bool insertHour;
	private bool insertMinutes;

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

		txtManipulator = GameObject.Find("TxtManipulator").transform;
		
		StartCoundownTimer();
	}
	 
	void StartCoundownTimer(){
		//Converting everything to seconds
		//Why are we adding 1? Lets supose the timer is 2 minutes. Without the plus one the timer would start at 1:59, due to 
		//the comand "timeTotal -= 1" on UpdateTimer function
		timeTotal = (hours * 3600) + (minutes * 60) + seconds + 1; 

		//Checking which dimension will be displayed on the timer
		insertHour = (hours > 0);
		insertMinutes = (minutes > 0 || hours > 0);

		//Updating timer for each second
		InvokeRepeating("UpdateTimer", 0.0f, 1.0f);
	}
	 
	void UpdateTimer(){
		if(timeTotal > 0){
			//Updating the time. This is why why are adding 1 on timetotal
			timeTotal -= 1;
		}else{
			//If timeTotal is 0, its time to stop!
			CancelInvoke();
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

	//This will be used as a parameter for IPHLootBox class
	public static int GetTotalTime(){
		return timeTotal;
	}

	//And this will provide to IPHCountdownText the new text that should be displayed
	public static string GetStringTime(){
		return timerText;
	}

	void OnApplicationQuit(){
		Debug.Log("Hour: " + System.DateTime.Now.Hour);
		Debug.Log("Minutes: " + System.DateTime.Now.Minute);
		Debug.Log("Seconds: " + System.DateTime.Now.Second);

		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("rmn", timeTotal);
		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("clh", System.DateTime.Now.Hour);
		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("clm", System.DateTime.Now.Minute);
		txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("cls", System.DateTime.Now.Second);
	}
}
