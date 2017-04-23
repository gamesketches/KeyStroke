using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour {

	public class Sentence {
		public string call;
		public string response;
		public string wordToCheckFor;
		public string[] missedResponses;
		public float responseTime;

		public Sentence(string newCall, string newResponse, string checkWord, string[] responses, float time) {
			call = newCall;
			response = newResponse;
			wordToCheckFor = checkWord;
			missedResponses = responses;
			responseTime = time;
		}
	}

	Queue<Sentence> chatLog;
	public Text chatText;
	public string userName;
	Sentence currentMessage;
	public Text inputtedText;
	public Text leadText;
	public float chatDelay;
	AudioSource messageReceived;
	AudioSource messageSent;
	// Use this for initialization
	void Start () {
		chatLog = new Queue<Sentence>();
		LoadScript();
		currentMessage = chatLog.Dequeue();
		messageReceived = GetComponent<AudioSource>();
		messageSent = GetComponents<AudioSource>()[1];
		SendMessage();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			messageSent.Play();
			chatText.text += "\n<color=red>Me</color>: " + inputtedText.text.ToLower();
			if(CheckMessage()) {
				currentMessage = chatLog.Dequeue();
				Invoke("SendMessage", 2 + chatDelay * Random.value);
				SendMessage();
			}
			else {
				Invoke("SendMissedMessage", 2 + chatDelay * Random.value);
			}
			inputtedText.text = "";
		}
	}

	void SendMessage() {
		chatText.text += "\n<color=blue>" + userName + "</color>: " + currentMessage.call;
		leadText.text = currentMessage.response.ToLower();
		messageReceived.Play();
	}

	void SendMissedMessage() {
		chatText.text += "\n<color=blue>" + userName + "</color>: " + currentMessage.missedResponses[Random.Range(0,currentMessage.missedResponses.Length - 1)];
	}

	bool CheckMessage() {
		string[] words = inputtedText.text.Split(" ".ToCharArray());
		foreach(string word in words) {
				if(word.ToLower() == currentMessage.wordToCheckFor.ToLower()) {
					return true;
			}
		}
		return false;
	}

	void LoadScript() {
		chatLog.Enqueue(new Sentence("hey", "hey", "hey", new string[] {"huh?", "what?"}, 1));
		chatLog.Enqueue(new Sentence("how are u?", "alright", "alright", new string[] {"?"}, 1));
		chatLog.Enqueue(new Sentence())
	}
}
