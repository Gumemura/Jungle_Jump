using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime;
using UnityEngine.SceneManagement;

//This class will manage all achievements and keep record of progress to conclusion
public class IPHAchievementsManager : MonoBehaviour {
	//The object that will provide the function to read and write on txt 
	private Transform textManipulator;

	//Record the moment the notification will start
	private float startNotification;

	//Amount of time that achievement notification will last on screen
	public float durationNotification = 5;

	//Allow the update method to actvate and deactvate the achievement notification
	private bool canDisplayNotification = false;

	//Sound that will be render when notification appears
	public AudioClip notificationSound;
	
	//entity that will render the sound
	internal GameObject soundSource;

	//Limits to the counter to prevent unnecessary code execution
	//Can be ignored to keep infinite track of player progression
	public const int maxJumps = 10;
	public const int maxPowers = 5;
	public const int maxMatches = 2;
	public const int maxTokens = 25;


	//Public because it will be called from other scripts
	private float plataformJumps = 0;
	private float collectedPower = 0;
	private float matches = 0;
	private float tokens = 0;

	//Array with actions codes. Will be used to update values
	string[] actionCodes = {"jmp", "pwr", "mat", "tkn"} ;

	//IMPROVEMENT: counters, max, and actions code could be integrated on arrays

	//Will be displayed to player
	public Transform AchievementNotification; 

	//Every single achievements
	//Achievement Code on txt | Necessary quantity for acomplishment | Message displayed when acomplished | Is completed?
	ArrayList PlataformJump03 = new ArrayList() {"s3p", 3, "3 plataforms jumped!", 0};
	ArrayList PlataformJump10 = new ArrayList() {"s10", 10, "10 plataforms jumped!", 0};
	ArrayList Play2Matches = new ArrayList() {"j2p", 2, "2 matches played!", 0};
	ArrayList ColletedPowerUp2 = new ArrayList() {"c2p", 2, "2 power-ups collecteds!", 0};
	ArrayList ColletedPowerUp5 = new ArrayList() {"c5p", 5, "5 power-ups collecteds!", 0};
	ArrayList RabbitUnlocked = new ArrayList() {"dco", 5, "Rabbit unlocked!", 0};
	ArrayList CatUnlocked = new ArrayList() {"dga", 10, "Cat unlocked!", 0};
	ArrayList DogUnlocked = new ArrayList() {"dca", 15, "Penguim dog unlocked!", 0};
	ArrayList PigUnlocked = new ArrayList() {"dpo", 20, "Pig unlocked!", 0};
	ArrayList PandaUnlocked = new ArrayList() {"dpa", 25, "Panda unlocked", 0};

	//List with all achievements
	List<ArrayList> AllAchievements = new List<ArrayList>();

	// Use this for initialization
	void Start () {
		//Updating infos from txt

		soundSource = GameObject.FindGameObjectWithTag("GameController");
		textManipulator = GameObject.Find("TxtManipulator").transform;

		GetTxtValues();
		
		//Checking if the scene loaded is the game scene so we can keep track of the matches played achievement
		if(SceneManager.GetActiveScene().name == "CS_Game"){
			RaiseCounter("mat");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(canDisplayNotification){
			if(startNotification + durationNotification > Time.time){
				AchievementNotification.gameObject.SetActive(true);
			}else{
				AchievementNotification.gameObject.SetActive(false);
				canDisplayNotification = false;
			}
		}
	}

	private void GetTxtValues(){
		//All achievements are unified on a single array for easy manipulation on this function
		AllAchievements.Add(PlataformJump03);
		AllAchievements.Add(PlataformJump10);
		AllAchievements.Add(Play2Matches);
		AllAchievements.Add(ColletedPowerUp2);
		AllAchievements.Add(ColletedPowerUp5);
		AllAchievements.Add(RabbitUnlocked);
		AllAchievements.Add(CatUnlocked);
		AllAchievements.Add(DogUnlocked);
		AllAchievements.Add(PigUnlocked);
		AllAchievements.Add(PandaUnlocked);

		//Update the progress of all achievements from the txt
		foreach (ArrayList achievement in AllAchievements){
			achievement[3] = textManipulator.GetComponent<IPHTxtManipulation>().ReadTxt((string)achievement[0]);
		}

		foreach (string code in actionCodes){
			switch(code){
				case "jmp":
					plataformJumps = textManipulator.GetComponent<IPHTxtManipulation>().ReadTxt(code);
					break;
				case "pwr":
					collectedPower = textManipulator.GetComponent<IPHTxtManipulation>().ReadTxt(code);
					break;
				case "mat":
					matches = textManipulator.GetComponent<IPHTxtManipulation>().ReadTxt(code);
					break;
				case "tkn":
					tokens = textManipulator.GetComponent<IPHTxtManipulation>().ReadTxt(code);
					break;
			}
		}
	}

	public void RaiseCounter(string actionCode){
		//Will increase the counters of jump, power-up and matches and record it on the txt
		switch(actionCode) {
			case "jmp":
				//Is possible to remove this if and keep track of all progrees play is making
				if(plataformJumps < maxJumps){
					textManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("jmp", ++plataformJumps);
					AcomplishmentCheck(PlataformJump03, plataformJumps);
					AcomplishmentCheck(PlataformJump10, plataformJumps);
				}
				break;
			case "pwr":
				if(collectedPower < maxPowers){
					textManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("pwr", ++collectedPower);
					AcomplishmentCheck(ColletedPowerUp2, collectedPower);
					AcomplishmentCheck(ColletedPowerUp5, collectedPower);

				}
				break;
			case "mat":
				if(matches < maxMatches){
					textManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("mat", ++matches);
					AcomplishmentCheck(Play2Matches, matches);
				}
				break;
			case "tkn":
				if(tokens < maxTokens){
					textManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("tkn", ++tokens);
					AcomplishmentCheck(RabbitUnlocked, tokens);
					AcomplishmentCheck(CatUnlocked, tokens);
					AcomplishmentCheck(DogUnlocked, tokens);
					AcomplishmentCheck(PigUnlocked, tokens);
					AcomplishmentCheck(PandaUnlocked, tokens);
				}
				break;
			default:
				break;
		}
	}

	private void AcomplishmentCheck(ArrayList achievement, float counter){
		//Checking if conditions have been meet to complete achievement
		if(counter == (int)achievement[1] && !IsCompleted(achievement)){
			UpdatingCompletedAchievements(achievement);
			AchievementMsgBox((string)achievement[2]);
		}
	}

	private bool IsCompleted(ArrayList Achievement){
		//Return false if achievement is no completed and true otherwise
		return (float)Achievement[3] == 1;
	}

	private void UpdatingCompletedAchievements(ArrayList Achievement){
		//Changin the array item to indicate the achievement have been completed
		Achievement[3] = 1;
		textManipulator.GetComponent<IPHTxtManipulation>().WriteTxt((string)Achievement[0], 1);

		//Changin the txt to perpetuate the data that this achievement have been completed
		AllAchievements[IdenfityIndex((string)Achievement[0])] = Achievement;
	}

	private void AchievementMsgBox(string AcomplishmentText){
		//Renders on screen a message box annoucing the achievement have been completed

		//stores moment when notification will appear
		startNotification = Time.time;

		AchievementNotification.GetComponent<Text>().text = AcomplishmentText;

		canDisplayNotification = true;

		if(notificationSound){
			soundSource.GetComponent<AudioSource>().PlayOneShot(notificationSound);
		}
	}

	private int IdenfityIndex(string AchievementCode){
		//Return the index of the Achievement we want to modify
		int counter = 0;
		foreach (ArrayList achievement in AllAchievements){
			if((string)achievement[0] == AchievementCode){
				return counter;
			}
			counter++;
		}
		return -1;
	}
}
