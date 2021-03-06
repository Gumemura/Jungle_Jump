﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Analytics;
using System.Collections.Generic;

namespace InfiniteHopper
{
	/// <summary>
	/// Toggles a sound source when clicked on. 
	/// In order to detect clicks you need to attach this script to a UI Button and set the proper OnClick() event.
	/// </summary>
	public class IPHToggleSound:MonoBehaviour
	{
		//will set which sound configuration will be changed (sound or music)
		private string soundTarget;

		//The object that will provide the function to read and write on txt 
		private Transform txtManipulator;

		//The tag of the sound object
		public string soundObjectTag = "GameController";

		// The source of the sound
		public Transform soundObject;		
	
		// The index of the current value of the sound
		internal float currentState = 1;

		//Unlocked players
		private int unlockedPlayers;
	
		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// Awake is used to initialize any variables or game state before the game starts. Awake is called only once during the 
		/// lifetime of the script instance. Awake is called after all objects are initialized so you can safely speak to other 
		/// objects or query them using eg. GameObject.FindWithTag. Each GameObject's Awake is called in a random order between objects. 
		/// Because of this, you should use Awake to set up references between scripts, and use Start to pass any information back and forth. 
		/// Awake is always called before any Start functions. This allows you to order initialization of scripts. Awake can not act as a coroutine.
		///
		/// Why is it start() and not awake(): so it wont conflict with awake from IPHTxtManipulation  
		/// </summary>
		void Start()
		{			
			if ( !soundObject && soundObjectTag != string.Empty )    soundObject = GameObject.FindGameObjectWithTag(soundObjectTag).transform;			
		
			//Reading the configuration's txt and establishing the configurations accordingly to the file
			if(soundObjectTag == "GameController"){
				soundTarget = "snd"; //Sound
			}else{
				soundTarget = "msc"; //Music
			}

			txtManipulator = GameObject.Find("TxtManipulator").transform;

			currentState = txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt(soundTarget);

			SetSound();
		}

	
		/// <summary>
		/// Sets the sound volume
		/// </summary>
		void SetSound()
		{
			if ( !soundObject && soundObjectTag != string.Empty )    soundObject = GameObject.FindGameObjectWithTag(soundObjectTag).transform;			

			Color newColor = GetComponent<Image>().material.color;

			// Update the graphics of the button image to fit the sound state
			if( currentState == 1 )
				newColor.a = 1;
			else
				newColor.a = 0.5f;

			GetComponent<Image>().color = newColor;

			// Set the value of the sound state to the source object
			if( soundObject ) 
				soundObject.GetComponent<AudioSource>().volume = currentState;
		}
	
		/// <summary>
		/// Toggle the sound. Cycle through all sound modes and set the volume and icon accordingly
		/// </summary>
		void ToggleSound()
		{
			currentState = 1 - currentState;

			//determining which souns will be changed
			if(soundObjectTag == "GameController"){
				soundTarget = "snd"; //Sound
			}else{
				soundTarget = "msc"; //Music
			}

			//Writing on the txt
			txtManipulator.GetComponent<IPHTxtManipulation>().WriteTxt(soundTarget, currentState);

			SetSound();

			//Checking how many player are unlocked
			if((int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("tkn") >= 25){
				unlockedPlayers = 25/5 + 1;
			}else{
				int tokens = (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("tkn");

				unlockedPlayers = tokens/5 + 1;
			}

			//Seding eventenabling/disbaling sound
			AnalyticsEvent.Custom("SoundConfigChanged:", new Dictionary<string, object>
			{
				{ "ChangedConfig", soundTarget == "snd"? "SFX": "Music"},
				{ "NewSoundStatus", currentState},

				{ "ActualSession", (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("sss")},
				{ "TopScore", (int)txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("scr")},
				{ "UnlockedPlayers", unlockedPlayers}
			});
		}
	
		/// <summary>
		/// Starts the sound source.
		/// </summary>
		void StartSound()
		{
			if( soundObject )
				soundObject.GetComponent<AudioSource>().Play();
		}
	}
}