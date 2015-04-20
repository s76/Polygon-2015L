using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public  class InfoPanelManager : MonoBehaviour {
	
	private static GameObject infoPanel;

	void Start(){
		infoPanel = GameObject.Find ("Info Panel");
	}

	public static void AddNewMessage(string message){
		infoPanel.GetComponentsInChildren<Text> ()[0].text += message+="\n";
	}

	public static void Clear(){
		infoPanel.GetComponentsInChildren<Text> () [0].text = "";
	}
}

