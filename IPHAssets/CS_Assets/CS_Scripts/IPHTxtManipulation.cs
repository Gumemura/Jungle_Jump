using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IPHTxtManipulation : MonoBehaviour {

	//Location of configuration file
	public static string path;

	//All lines of configs txt
	public static string[] lines;

	// Use this for initialization
	void Awake () {
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
				return float.Parse(line[line.Length - 1].ToString());
			}
		}
		return 1;
	}

	public void WriteTxt(string LineTxt, float NewValue){
		//Writes on the configuration's txt

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
