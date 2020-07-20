using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using InfiniteHopper.Types;
using UnityEngine.Analytics;

namespace InfiniteHopper
{
	/// <summary>
	/// This script defines an item which can be picked up.
	/// </summary>
	public class IPHItem:MonoBehaviour 
	{
		//The achievemente manager that will provide us the functions to update data
		internal Transform achievementeManager;

		//txt manipulations functions
		internal Transform txtManipulator;

		//The achievemente manager tag we will use to find AchievementeManager tranform
		public string achievementeManagerTag = "AchievementManager";

		//Diferenciate tokens from power up
		public string powerUpTag = "PowerUp";

		//The tag of the object that can touch this item
		public string hitTargetTag = "Player";
		
		//A list of functions that run when this item is touched by the target
		public TouchFunction[] touchFunctions;
		
		//The effect that is created at the location of this object when it is destroyed
		public Transform hitEffect;
		
		//The sound that plays when this object is touched
		public AudioClip soundHit;
		public string soundSourceTag = "GameController";

		//How many players are unlocked
		private int unlockedPlayers;

		void Start(){
			//Finding the achievemente Manager tranform
			achievementeManager = GameObject.FindWithTag(achievementeManagerTag).transform;
			txtManipulator = GameObject.Find("TxtManipulator").transform;
		}
		
		//This function runs when this obstacle touches another object with a trigger collider
		void  OnTriggerEnter2D ( Collider2D other  ){	

			//Check if the object that was touched has the correct tag
			if ( other.tag == hitTargetTag )
			{
				if(transform.gameObject.tag == powerUpTag){
					//Updating data of collected power-ups 
					achievementeManager.GetComponent<IPHAchievementsManager>().RaiseCounter("pwr");
				}else{
					//Updating data of collected tokens
					achievementeManager.GetComponent<IPHAchievementsManager>().RaiseCounter("tkn");
				}

				//Checking how many player are unlocked
				if((int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("tkn") >= 25){
					unlockedPlayers = 25/5 + 1;
				}else{
					int tokens = (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("tkn");

					unlockedPlayers = tokens/5 + 1;
				}

				//Seding event when coliding with player
				AnalyticsEvent.Custom("PowerUp_Collected", new Dictionary<string, object>
				{
					{ "PowerUpType", transform.gameObject.name},
					{ "ActualSession", (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("sss")},
					{ "TopScore", (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("scr")},
					{ "UnlockedPlayers", unlockedPlayers}
				});



				//Go through the list of functions and runs them on the correct targets
				foreach( TouchFunction touchFunction in touchFunctions )
				{
					//Check that we have a target tag and function name before running
					if ( touchFunction.targetTag != string.Empty && touchFunction.functionName != string.Empty )
					{
						//Run the function
						GameObject.FindGameObjectWithTag(touchFunction.targetTag).SendMessage(touchFunction.functionName, touchFunction.functionParameter);
					}
				}
				
				//If there is a hit effect, create it
				if ( hitEffect )    Instantiate( hitEffect, transform.position, Quaternion.identity);
				
				//Destroy the item
				Destroy(gameObject);
				
				//If there is a sound source and a sound assigned, play it
				if ( soundSourceTag != "" && soundHit )    
				{
					//Reset the pitch back to normal
					GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().pitch = 1;
					
					//Play the sound
					GameObject.FindGameObjectWithTag(soundSourceTag).GetComponent<AudioSource>().PlayOneShot(soundHit);
				}
			}
		}
	}
}