using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

namespace InfiniteHopper
{
	/// <summary>
	/// Toggles a sound source when clicked on. 
	/// In order to detect clicks you need to attach this script to a UI Button and set the proper OnClick() event.
	/// </summary>
	public class IPHToggleSound:MonoBehaviour
	{
		//Location of configuration file
		private string path;

		//All lines of configs txt
		private string[] lines;

		//will set which sound configuration will be changed (sound or music)
		private string SoundTarget;

		//The tag of the sound object
		public string soundObjectTag = "GameController";

		// The source of the sound
		public Transform soundObject;		
	
		// The index of the current value of the sound
		internal float currentState = 1;
	
		/// <summary>
		/// Awake is called when the script instance is being loaded.
		/// Awake is used to initialize any variables or game state before the game starts. Awake is called only once during the 
		/// lifetime of the script instance. Awake is called after all objects are initialized so you can safely speak to other 
		/// objects or query them using eg. GameObject.FindWithTag. Each GameObject's Awake is called in a random order between objects. 
		/// Because of this, you should use Awake to set up references between scripts, and use Start to pass any information back and forth. 
		/// Awake is always called before any Start functions. This allows you to order initialization of scripts. Awake can not act as a coroutine.
		/// </summary>
		void Awake()
		{			
			//establishing the path of configuration txt
			path = Application.persistentDataPath + "\\configLog.txt";

			if ( !soundObject && soundObjectTag != string.Empty )    soundObject = GameObject.FindGameObjectWithTag(soundObjectTag).transform;			
		
			//Reading the configuration's txt and establishing the configurations accordingly to the file
			string SoundTarget;
			if(soundObjectTag == "GameController"){
				SoundTarget = "snd"; //Sound
			}else{
				SoundTarget = "msc"; //Music
			}

			currentState = SoundReadTxt(SoundTarget);

			SetSound();
		}

		public int SoundReadTxt(string ButtonFunction){
			//Reads the configuration txt and returns the estate of settings

			lines = File.ReadAllLines(path);

			foreach (string line in lines){
				if (line.Substring(0, 3) == ButtonFunction){
					return int.Parse(line[line.Length - 1].ToString());
				}
			}
			return 1;
		}

		public void WriteTxtSound(string ButtonFunction){
			//Writes on the configuration's txt new settings

			string[] arrLine = File.ReadAllLines(path);

			for (int i = 0; i <= arrLine.Length ; i++){
				if (arrLine[i].Substring(0, 3) == ButtonFunction){
					//Changing the line of txt with desired configuration
					arrLine[i] = ButtonFunction + " " + currentState.ToString();
					File.WriteAllLines(path, arrLine);
					break;
				}
			}	
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
				SoundTarget = "snd"; //Sound
			}else{
				SoundTarget = "msc"; //Music
			}

			//Writing on the txt
			WriteTxtSound(SoundTarget);

			SetSound();
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