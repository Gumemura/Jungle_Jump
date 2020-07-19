using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IPHCreateTxt : MonoBehaviour {

	//Location of configuration file
	[HideInInspector]
	public static string path;

	//Location for itens txt
	[HideInInspector]
	public static string itensPath;


	// Use this for initialization
	void Awake () {
		//establishing the path of configuration txt
		//Usualy would be used Application.DataPath, but for mobile devices, Application.persistentDataPath works better because we may not have direct write access to the dataPath of the project
		path = Application.persistentDataPath + "\\configLog.txt";
		itensPath = Application.persistentDataPath + "\\itens.txt";

		CreateTxt();
	}

	private void CreateTxt(){
		//Creates a .txt document if its doesnt exists

		//Verifying if .txt already exists
		if(!File.Exists(path)){
			File.WriteAllText(path, 
				"--sound/ music configuration\nsnd 1\nmsc 1\n--achievements\ns3p 0\ns10 0\nc2p 0\nc5p 0\nj2p 0\ndco 0\ndga 0\ndca 0\ndpo 0\ndpa 0\n--counters\njmp 0\npwr 0\nmat 0\ntkn 0\n--time\nrmn 0\nclh 0\nclm 0\ncls 0\ncld 0\ncln 0\ncly 0\n--active player\nact 0");
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
		//--time
		//rmn: remaing time for countdown
		//clh: closing hour
		//clm: closing minute
		//cls: closing second
		//cld: closing day
		//cln: closing month
		//cly: closing year
		//--active player
		//act: active player
		/// </summary>

		if(!File.Exists(itensPath)){
			File.WriteAllText(itensPath,"Gift1 71 Common\nGift2 71 Common\nGift3 71 Common\nGift4 71 Common\nGift5 71 Common\nGift6 71 Common\nGift7 71 Common\nGift8 71 Common\nGift9 71 Common\nGift10 71 Common\nGift11 71 Common\nGift12 71 Common\nGift13 71 Common\nGift14 71 Common\nGift15 71 Common\nGift16 71 Common\nGift17 71 Common\nGift18 71 Common\nGift19 71 Common\nGift20 71 Common\nGift21 71 Common\nGift22 71 Common\nGift23 71 Common\nGift24 71 Common\nGift25 71 Common\nGift26 71 Common\nGift27 71 Common\nGift28 71 Common\nGift29 71 Common\nGift30 71 Common\nGift31 71 Common\nGift32 71 Common\nGift33 71 Common\nGift34 71 Common\nGift35 71 Common\nGift36 71 Common\nGift37 71 Common\nGift38 71 Common\nGift39 71 Common\nGift40 71 Common\nGift41 71 Common\nGift42 71 Common\nGift43 71 Common\nGift44 71 Common\nGift45 71 Common\nGift46 71 Common\nGift47 71 Common\nGift48 71 Common\nGift49 71 Common\nGift50 71 Common\nGift51 71 Common\nGift52 71 Common\nGift53 71 Common\nGift54 71 Common\nGift55 71 Common\nGift56 71 Common\nGift57 71 Common\nGift58 71 Common\nGift59 71 Common\nGift60 71 Common\nGift61 71 Common\nGift62 71 Common\nGift63 71 Common\nGift64 71 Common\nGift65 71 Common\nGift66 71 Common\nGift67 71 Common\nGift68 71 Common\nGift69 71 Common\nGift70 71 Common\nGift71 71 Common\nGift72 71 Common\nGift73 71 Common\nGift74 71 Common\nGift75 71 Common\nGift76 71 Common\nGift77 71 Common\nGift78 71 Common\nGift79 71 Common\nGift80 71 Common\nGift81 71 Common\nGift82 71 Common\nGift83 71 Common\nGift84 71 Common\nGift85 24 Uncommon\nGift86 24 Uncommon\nGift87 24 Uncommon\nGift88 24 Uncommon\nGift89 24 Uncommon\nGift90 24 Uncommon\nGift91 24 Uncommon\nGift92 24 Uncommon\nGift93 24 Uncommon\nGift94 24 Uncommon\nGift95 24 Uncommon\nGift96 24 Uncommon\nGift97 24 Uncommon\nGift98 24 Uncommon\nGift99 24 Uncommon\nGift100 24 Uncommon\nGift101 24 Uncommon\nGift102 24 Uncommon\nGift103 24 Uncommon\nGift104 24 Uncommon\nGift105 24 Uncommon\nGift106 24 Uncommon\nGift107 24 Uncommon\nGift108 24 Uncommon\nGift109 24 Uncommon\nGift110 24 Uncommon\nGift111 24 Uncommon\nGift112 24 Uncommon\nGift113 24 Uncommon\nGift114 24 Uncommon\nGift115 24 Uncommon\nGift116 24 Uncommon\nGift117 24 Uncommon\nGift118 24 Uncommon\nGift119 24 Uncommon\nGift120 24 Uncommon\nGift121 24 Uncommon\nGift122 24 Uncommon\nGift123 24 Uncommon\nGift124 24 Uncommon\nGift125 24 Uncommon\nGift126 24 Uncommon\nGift127 24 Uncommon\nGift128 24 Uncommon\nGift129 24 Uncommon\nGift130 24 Uncommon\nGift131 24 Uncommon\nGift132 24 Uncommon\nGift133 24 Uncommon\nGift134 24 Uncommon\nGift135 24 Uncommon\nGift136 24 Uncommon\nGift137 24 Uncommon\nGift138 24 Uncommon\nGift139 24 Uncommon\nGift140 24 Uncommon\nGift141 24 Uncommon\nGift142 24 Uncommon\nGift143 24 Uncommon\nGift144 24 Uncommon\nGift145 24 Uncommon\nGift146 24 Uncommon\nGift147 24 Uncommon\nGift148 24 Uncommon\nGift149 24 Uncommon\nGift150 24 Uncommon\nGift151 24 Uncommon\nGift152 24 Uncommon\nGift153 24 Uncommon\nGift154 24 Uncommon\nGift155 24 Uncommon\nGift156 24 Uncommon\nGift157 24 Uncommon\nGift158 24 Uncommon\nGift159 24 Uncommon\nGift160 24 Uncommon\nGift161 24 Uncommon\nGift162 24 Uncommon\nGift163 24 Uncommon\nGift164 24 Uncommon\nGift165 24 Uncommon\nGift166 24 Uncommon\nGift167 24 Uncommon\nGift168 24 Uncommon\nGift169 24 Uncommon\nGift170 24 Uncommon\nGift171 24 Uncommon\nGift172 24 Uncommon\nGift173 24 Uncommon\nGift174 5.3 Rare\nGift175 5.3 Rare\nGift176 5.3 Rare\nGift177 5.3 Rare\nGift178 5.3 Rare\nGift179 5.3 Rare\nGift180 5.3 Rare\nGift181 5.3 Rare\nGift182 5.3 Rare\nGift183 5.3 Rare\nGift184 5.3 Rare\nGift185 5.3 Rare\nGift186 5.3 Rare\nGift187 5.3 Rare\nGift188 5.3 Rare\nGift189 5.3 Rare\nGift190 5.3 Rare\nGift191 5.3 Rare\nGift192 5.3 Rare\nGift193 5.3 Rare\nGift194 5.3 Rare\nGift195 5.3 Rare\nGift196 5.3 Rare\nGift197 5.3 Rare\nGift198 5.3 Rare\nGift199 5.3 Rare\nGift200 5.3 Rare\nGift201 5.3 Rare\nGift202 5.3 Rare\nGift203 5.3 Rare\nGift204 5.3 Rare\nGift205 5.3 Rare\nGift206 5.3 Rare\nGift207 5.3 Rare\nGift208 5.3 Rare\nGift209 5.3 Rare\nGift210 5.3 Rare\nGift211 5.3 Rare\nGift212 5.3 Rare\nGift213 5.3 Rare\nGift214 5.3 Rare\nGift215 5.3 Rare\nGift216 5.3 Rare\nGift217 5.3 Rare\nGift218 5.3 Rare\nGift219 5.3 Rare\nGift220 5.3 Rare\nGift221 5.3 Rare\nGift222 5.3 Rare\nGift223 1 Mythic\nGift224 1 Mythic\nGift225 1 Mythic\nGift226 1 Mythic\nGift227 1 Mythic\nGift228 1 Mythic\nGift229 1 Mythic\nGift230 1 Mythic\nGift231 1 Mythic\nGift232 1 Mythic\nGift233 1 Mythic\nGift234 1 Mythic\nGift235 1 Mythic\nGift236 1 Mythic\nGift237 1 Mythic\nGift238 1 Mythic\nGift239 1 Mythic\nGift240 1 Mythic\nGift241 1 Mythic\nGift242 1 Mythic\nGift243 1 Mythic\nGift244 1 Mythic\nGift245 1 Mythic\nGift246 1 Mythic");
			//Test
			//File.WriteAllText(itensPath,"Gift1 71 Common\nGift2 71 Common\nGift3 71 Common\nGift4 71 Common\nGift5 71 Common\nGift6 71 Common\nGift7 71 Common\nGift8 71 Common\nGift9 71 Common");
		}
	}
}
