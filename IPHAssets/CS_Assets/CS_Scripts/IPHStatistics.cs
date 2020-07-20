using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IPHStatistics : MonoBehaviour 
{
    private Transform txtManipulator;
	// Use this for initialization
	void Start() 
	{
        txtManipulator = GameObject.Find("TxtManipulator").transform;
        // Set the statistics values in the statistics canvas
        GameObject.Find("TextDistance").GetComponent<Text>().text = "LONGEST DISTANCE: ??";
		GameObject.Find("TextStreak").GetComponent<Text>().text = "LONGEST STREAK: ??";
        GameObject.Find("TextTokens").GetComponent<Text>().text = "TOTAL FEATHERS: " + txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("tkn");
        GameObject.Find("TextPowerups").GetComponent<Text>().text = "TOTAL POWERUPS: " + txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("pwr");
        GameObject.Find("TextPowerupStreak").GetComponent<Text>().text = "LONGEST POWERUP: ??";
        GameObject.Find("TextCharacters").GetComponent<Text>().text = "CHARACTERS UNLOCKED: " + Mathf.Floor(txtManipulator.GetComponent<IPHTxtManipulation>().ReadTxt("tkn") / 5) ;
    }
}
