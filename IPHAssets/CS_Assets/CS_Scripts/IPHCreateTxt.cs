using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IPHCreateTxt : MonoBehaviour {

	//Location of configuration file
	public static string path;

	// Use this for initialization
	void Awake () {
		//establishing the path of configuration txt
		//Usualy would be used Application.DataPath, but for mobile devices, Application.persistentDataPath works better because we may not have direct write access to the dataPath of the project
		path = Application.persistentDataPath + "\\configLog.txt";

		CreateTxt();
	}

	private void CreateTxt(){
		//Creates a .txt document if its doesnt exists

		//Verifying if .txt already exists
		if(!File.Exists(path)){
			File.WriteAllText(path, 
				"0snd 1\n0msc 1\n--achievements\n0s3p 0\n0s10 0\n0c2p 0\n0c5p 0\n0j2p 0\n0dco 0\n0dga 0\n0dca 0\n0dpo 0\n0dpa 0\n");
		}
		/// <summary>
		// The first number indicates if the achievements have been acomplished. Its not useful for configuration
		// Each initials on the txt stands for some game feature, as below:
		// snd: sound (0 off; 1 on)
		// msc: music (0 off; 1 on)
		//--achievements
		// s3p: jump 3 plataforms ('saltar 3 plataformas'); counter
		// s10: jump 10 plataforms ('saltar 10 plataformas'); counter
		// c2p: collect 2 power-ups; counter
		// c5p: collect 5 power-ups; counter
		// j2p: play two matches ('jogar duas partidas'); counter
		// dco: unlock rabbit ('desbloquear coelinho'); true/false
		// dga: unlock cat ('debloquear gatinho'); true/false
		// dca: unlock cod ('debloquear cachorrinho'); true/false
		// dpo: unlock pig ('debloquear porquinho'); true/false
		// dpa: unlock panda ('debloquear pandinha'); true/false
		/// </summary>
	}
}
