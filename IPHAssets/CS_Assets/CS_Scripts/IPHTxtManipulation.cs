using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IPHTxtManipulation : MonoBehaviour {

	//Location of configuration file
	public static string path;

	//All lines of configs txt
	public static string[] lines;

	//The time this instance of the txtmanipulator has been in the game
	internal float instanceTime = 0;

	void Awake(){
		//Imported fom IPH Global Music :)

		//Find all the txtmanipulator objects in the scene
		GameObject[] txtManipulators = GameObject.FindGameObjectsWithTag("TxtManipulation");
		
		//Keep only the txtmanipulator object which has been in the game for more than 0 seconds
		if(txtManipulators.Length > 1){
			foreach(var txtManipulator in txtManipulators)
			{
				if(txtManipulator.GetComponent<IPHTxtManipulation>().instanceTime <= 0 ){
					Destroy(gameObject);
				}    
			}
		}
	}

	// Use this for initialization
	void Start () {
		//Making this persistent because will be used by objects in all scenes
		DontDestroyOnLoad(transform.gameObject);

		//establishing the path of configuration txt
		//Usualy would be used Application.DataPath, but for mobile devices, Application.persistentDataPath works better because we may not have direct write access to the dataPath of the project
		path = Application.persistentDataPath + "\\configLog.txt";

		//Creating an array ofrom the lines of txt
		lines = File.ReadAllLines(path);

	}

	public float ReadTxt(string LineTxt){
		//Reads the configuration txt and returns the estate of settings

		foreach (string line in lines){
			if (line.Substring(0, 3) == LineTxt){
				return float.Parse(line.Substring(4, line.Length - 4).ToString());
			}
		}
		return 1;
	}

	public void WriteTxt(string LineTxt, float NewValue){
		for (int i = 0; i <= lines.Length ; i++){
			if (lines[i].Substring(0, 3) == LineTxt){
				//Changing the line of txt with desired configuration
				lines[i] = LineTxt + " " + NewValue.ToString();
				File.WriteAllLines(path, lines);
				break;
			}
		}
	}
}
