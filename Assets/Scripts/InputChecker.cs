using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour {

	public class Sentence {
		public string call;
		public string[] wordsToCheckFor;
		public string[] missedResponses;
	}

	Queue<Sentence> chatLog;
	Text chatText;
	// Use this for initialization
	void Start () {
		chatText = GetComponentInChildren<Text>();
		chatText.text = "Hey";
		chatText.text += "\nYo";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
