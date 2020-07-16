using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//This class will manage all achievements and keep record of progress to conclusion
public class IPHAchievementsManager : MonoBehaviour {
	//The object that will provide the function to read and write on txt 
	public Transform TextManipulator;

	//Every single achievements
	//Achievement Code | Atual quantity | Necessary quantity for acomplishment | Message displayed when acomplished | Is completed
	ArrayList PlataformJump03 = new ArrayList() {"s3p", 0, 3, "3 plataforms jumped!", 0};
	ArrayList PlataformJump05 = new ArrayList() {"s10", 0, 10, "10 plataforms jumped!", 0};
	ArrayList ColetedPowerUp2 = new ArrayList() {"c2p", 0, 2, "2 power-ups collecteds!", 0};
	ArrayList ColetedPowerUp5 = new ArrayList() {"c5p", 0, 5, "5 power-ups collecteds!", 0};
	ArrayList Play2Matches = new ArrayList() {"j2p", 0, 2, "2 matches played!", 0};
	ArrayList RabbitUnlocked = new ArrayList() {"dco", 0};
	ArrayList CatUnlocked = new ArrayList() {"dga", 0};
	ArrayList DogUnlocked = new ArrayList() {"dca", 0};
	ArrayList PigUnlocked = new ArrayList() {"dpo", 0};
	ArrayList PandaUnlocked = new ArrayList() {"dpa", 0};

	//Array with all achievements
	List<ArrayList> AllAchievements = new List<ArrayList>();
 
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
		AllAchievements.Add(PlataformJump05);
		AllAchievements.Add(ColetedPowerUp2);
		AllAchievements.Add(ColetedPowerUp5);
		AllAchievements.Add(Play2Matches);
		AllAchievements.Add(RabbitUnlocked);
		AllAchievements.Add(CatUnlocked);
		AllAchievements.Add(DogUnlocked);
		AllAchievements.Add(PigUnlocked);
		AllAchievements.Add(PandaUnlocked);

		GetTxtValues();
	}
	
	// Update is called once per frame
	void Update () {
		foreach (ArrayList achievement in AllAchievements){
		 	print(achievement[0] + " " + achievement[1]);
		}

		UpdateAchievement("c2p", 2);

		// print(AllAchievements[0][3]);

	}

	private void GetTxtValues(){
		//Update the progress of all achievements from the txt
		foreach (ArrayList achievement in AllAchievements){
			achievement[1] = TextManipulator.GetComponent<IPHTxtManipulation>().ReadTxt(achievement[0].ToString());
		}
	}

	public void UpdateAchievement(string AchievementCode, int Value){
		//modify the conditions of the Achievement
		if(IdenfityIndex(AchievementCode) >= 0){
			if(int.Parse(AllAchievements[IdenfityIndex(AchievementCode)][4].ToString()) == 0){
				//Changing the conditions 
				AllAchievements[IdenfityIndex(AchievementCode)][1] = Value + int.Parse(AllAchievements[IdenfityIndex(AchievementCode)][1].ToString());

				//Registring it on txt for persistence 
				TextManipulator.GetComponent<IPHTxtManipulation>().WriteTxt(AllAchievements[IdenfityIndex(AchievementCode)][0].ToString(), int.Parse(AllAchievements[IdenfityIndex(AchievementCode)][1].ToString()));

				//If there is condition for acomplishment the achievement, a message will be display
				if(CheckVictory(int.Parse(AllAchievements[IdenfityIndex(AchievementCode)][1].ToString()), int.Parse(AllAchievements[IdenfityIndex(AchievementCode)][2].ToString()))){
					VictoryMsgBox(AllAchievements[IdenfityIndex(AchievementCode)][3].ToString());
					AllAchievements[IdenfityIndex(AchievementCode)][4] = 1;
				}
			}
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

	private bool CheckVictory(int Value, int VictoryCondition){
		if(Value == VictoryCondition){
			return true;
		}else{
			return false;
		}
	}

	private void VictoryMsgBox(string AcomplishmentText){
		print(AcomplishmentText);
	}
}
