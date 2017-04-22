using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour {

	public class Sentence {
		public string call;
		public string[] wordsToCheckFor;
		public string[] missedResponses;
		public float responseTime;

		public Sentence(string newCall, string[] listOfWords, string[] responses, float time) {
			call = newCall;
			wordsToCheckFor = listOfWords;
			missedResponses = responses;
			responseTime = time;
		}
	}

	Queue<Sentence> chatLog;
	public Text chatText;
	public string userName;
	Sentence currentMessage;
	public Text inputtedText;
	// Use this for initialization
	void Start () {
		chatLog = new Queue<Sentence>();
		LoadScript();
		currentMessage = chatLog.Dequeue();
		//chatText = GetComponent<Text>();
		SendMessage();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			chatText.text += "\nMe: " + inputtedText.text;
			if(CheckMessage()) {
				currentMessage = chatLog.Dequeue();
				SendMessage();
			}
			else {
				SendMissedMessage();
			}
			inputtedText.text = "";
		}
	}

	void SendMessage() {
		chatText.text += "\n" + userName + ": " + currentMessage.call;
	}

	void SendMissedMessage() {
		chatText.text += "\n" + userName + ": " + currentMessage.missedResponses[Random.Range(0,currentMessage.missedResponses.Length - 1)];
	}

	bool CheckMessage() {
		string[] words = inputtedText.text.Split(" ".ToCharArray());
		foreach(string word in words) {
			foreach(string checkingWord in currentMessage.wordsToCheckFor) {
				if(word.ToLower() == checkingWord.ToLower()) {
					return true;
				}
			}
		}
		return false;
	}

	void LoadScript() {
		chatLog.Enqueue(new Sentence("Hey", new string[] {"Hey", "Hi", "yo", "sup"}, new string[] {"huh?", "what?"}, 1));
	}
}
