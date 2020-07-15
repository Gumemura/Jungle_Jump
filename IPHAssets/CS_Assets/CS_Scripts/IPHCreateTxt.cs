using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IPHCreateTxt : MonoBehaviour {

	void CreateTxt(){
		//Creates a .txt document if its doesnt exists

		//Path to the file
		//Usualy would be used Application.DataPath, but for mobile devices, Application.persistentDataPath works better because we 
		//may not have direct write access to the dataPath of the project
		string path = Application.persistentDataPath + "\\configLog.txt";

		//Verifying if .txt already exists
		if(!File.Exists(path)){
			File.WriteAllText(path, 
				"snd 1\nmsc 1\n--achievements\ns3p 0\ns10 0\nc2p 0\nc5p 0\nj2p 0\ndco 0\ndga 0\ndca 0\ndpo 0\ndpa 0\n");
		}
	}

	// Use this for initialization
	void Start () {
		CreateTxt();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
