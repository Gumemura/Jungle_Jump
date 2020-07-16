using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime;

//This class will manage all achievements and keep record of progress to conclusion
public class IPHAchievementsManager : MonoBehaviour {
	//The object that will provide the function to read and write on txt 
	public Transform TextManipulator;

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

	//Public because it will be called from other scripts
	private float plataformJumps = 0;
	private float collectedPower = 0;
	private float matches = 0;

	//Will be displayed to player
	public Transform AchievementNotification; 

	//Every single achievements
	//Achievement Code on txt | Necessary quantity for acomplishment | Message displayed when acomplished | Is completed?
	ArrayList PlataformJump03 = new ArrayList() {"s3p", 3, "3 plataforms jumped!", 0};
	ArrayList PlataformJump10 = new ArrayList() {"s10", 10, "10 plataforms jumped!", 0};
	ArrayList ColletedPowerUp2 = new ArrayList() {"c2p", 2, "2 power-ups collecteds!", 0};
	ArrayList ColletedPowerUp5 = new ArrayList() {"c5p", 5, "5 power-ups collecteds!", 0};
	ArrayList Play2Matches = new ArrayList() {"j2p", 2, "2 matches played!", 0};
	ArrayList RabbitUnlocked = new ArrayList() {"dco", 5, "Rabbit unlocked!", 0};
	ArrayList CatUnlocked = new ArrayList() {"dga", 10, "Cat unlocked!", 0};
	ArrayList DogUnlocked = new ArrayList() {"dca", 15, "Penguim dog unlocked!", 0};
	ArrayList PigUnlocked = new ArrayList() {"dpo", 20, "Pig unlocked!", 0};
	ArrayList PandaUnlocked = new ArrayList() {"dpa", 25, "Panda unlocked", 0};

	//List with all achievements
	List<ArrayList> AllAchievements = new List<ArrayList>();

	//Array with actions codes. Will be used to update values
	string[] actionCodes = {"jmp", "pwr", "mat"} ;
 
	// int PlataformJump;
	// int ColetedPowerUp;
	// int Matches;
	// bool RabbitUnlocked;
	// bool CatUnlocked;
	// bool DogUnlocked;
	// bool PigUnlocked;
	// bool PandaUnlocked;

	// Use this for initialization
	void Start () {
		AllAchievements.Add(PlataformJump03);
		AllAchievements.Add(PlataformJump10);
		AllAchievements.Add(ColletedPowerUp2);
		AllAchievements.Add(ColletedPowerUp5);
		AllAchievements.Add(Play2Matches);
		AllAchievements.Add(RabbitUnlocked);
		AllAchievements.Add(CatUnlocked);
		AllAchievements.Add(DogUnlocked);
		AllAchievements.Add(PigUnlocked);
		AllAchievements.Add(PandaUnlocked);

		GetTxtValues();

		soundSource = GameObject.FindGameObjectWithTag("GameController");
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
		//Update the progress of all achievements from the txt
		foreach (ArrayList achievement in AllAchievements){
			achievement[3] = TextManipulator.GetComponent<IPHTxtManipulation>().ReadTxt((string)achievement[0]);
		}

		foreach (string code in actionCodes){
			if(code == "jmp"){
				plataformJumps = TextManipulator.GetComponent<IPHTxtManipulation>().ReadTxt(code);
			}else if(code == "pwr"){
				collectedPower = TextManipulator.GetComponent<IPHTxtManipulation>().ReadTxt(code);
			}else if(code == "mat"){
				matches = TextManipulator.GetComponent<IPHTxtManipulation>().ReadTxt(code);
			}
		}
	}

	public void RaiseCounter(string actionCode){
		//Will increase the counters of jump, power-up and matches and record it on the txt

		switch(actionCode) {
			case "jmp":
				//Is possible to remove this if and keep track of all progrees play is making
				if(plataformJumps < maxJumps){
					TextManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("jmp", ++plataformJumps);
					AcomplishmentCheck_Jump();
				}
				break;
			case "pwr":
				if(collectedPower < maxPowers){
					TextManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("pwr", ++collectedPower);
					//AcomplishmentCheck_PowerUp();
				}
				break;
			case "mat":
				if(matches < maxMatches){
					TextManipulator.GetComponent<IPHTxtManipulation>().WriteTxt("pwr", ++matches);
					//AcomplishmentCheck_Matches();
				}
				break;
			default:
				break;
		}
	}

	private void AcomplishmentCheck_Jump(){
		//Checking if conditions have been meet
		if(plataformJumps ==  (int)PlataformJump03[1] && !IsCompleted(PlataformJump03)){
			//Change the array data to informe that this achievement habe been completed
			UpdatingCompletedAchievements(PlataformJump03);

			//Renders on screen a message box annoucing the achievement have been completed
			AchievementMsgBox((string)PlataformJump03[2]);
		}

		if(plataformJumps ==  (int)PlataformJump10[1] && !IsCompleted(PlataformJump10)){
			//Change the array data to informe that this achievement habe been completed
			UpdatingCompletedAchievements(PlataformJump10);

			//Renders on screen a message box annoucing the achievement have been completed
			AchievementMsgBox((string)PlataformJump10[2]);
		}
	}

	//private void AcomplishmentCheck_PowerUp()

	//private void AcomplishmentCheck_Matches()

	private bool IsCompleted(ArrayList Achievement){
		//Return false if achievement is no completed and true otherwise
		return (float)Achievement[3] == 1;
	}

	private void UpdatingCompletedAchievements(ArrayList Achievement){
		//Changin the array item to indicate the achievement have been completed
		Achievement[3] = 1;
		TextManipulator.GetComponent<IPHTxtManipulation>().WriteTxt((string)Achievement[0], 1);

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
			if(achievement[0] == AchievementCode){
				return counter;
			}
			counter++;
		}
		return -1;
	}
}
