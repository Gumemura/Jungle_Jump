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
				"snd 1\nmsc 1\n--achievements\ns3p 0\ns10 0\nc2p 0\nc5p 0\nj2p 0\ndco 0\ndga 0\ndca 0\ndpo 0\ndpa 0\n--counters\njmp 0\npwr 0\nmat 0\ntkn 0");
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
		//--achievements
		// jmp: counter of jumps
		// pwr: counter of colected power-ups
		// mat: counter of matches
		// tkn: tokens
		/// </summary>
	}
}
