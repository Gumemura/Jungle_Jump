using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPHCountdownText : MonoBehaviour {

	//Stores initial  font sizr
	internal int initialFontSize;

	//How much will the font expand to give visual feedbak
	public int fontGrow = 1;

	//Limit size font 
	public int fontLimit = 80;

	//counts how many times the font has grow
	internal int growCount = 0;

	//Limit of cycle of grow
	public int growLimit = 5;

	void Start(){
		initialFontSize = transform.GetComponent<Text>().fontSize;
	}

	void LateUpdate(){
		transform.GetComponent<Text>().text = IPHTimeTracker.GetStringTime();
	}

	public void InvokeShake(){
		//The only purpose  of this function is to invoke repeat shaek()
		
		InvokeRepeating("Shake", 0.0f, 0.01f);
	}

	public void Shake(){
		//This function will provide a visual feedback to player by blinking the countdown text
		//Its made by raising and lowering the font

		//Raising the font
		transform.GetComponent<Text>().fontSize += fontGrow;

		//If its going too big or too low, its time to invert 
		//fontGrow > 0 goes for growing
		//fontGrow < 0 goes for lowering
		if(transform.GetComponent<Text>().fontSize >= fontLimit || transform.GetComponent<Text>().fontSize <= initialFontSize){
			fontGrow *= -1;
			growCount++;

			//If its on its original size, it may be time to stop
			//And thats what growCount does: it counts the times a full cycle of growing/lowering is complete
			if(transform.GetComponent<Text>().fontSize <= initialFontSize){
				if(growCount >= growLimit){
					fontGrow = 1;
					growCount = 0;
					CancelInvoke("Shake");
				}	
			}
		}
	}
}
