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
	public Text typingStatus;
	public int WPM;
	AudioSource messageReceived;
	AudioSource messageSent;
	// Use this for initialization
	void Start () {
		chatLog = new Queue<Sentence>();
		LoadScript();
		currentMessage = chatLog.Dequeue();
		messageReceived = GetComponent<AudioSource>();
		messageSent = GetComponents<AudioSource>()[1];
		typingStatus.text = userName + " is typing...";
		typingStatus.gameObject.SetActive(false);
		StartCoroutine(SendMessage());
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Return)) {
			MessageEntered();
		}
	}

	public void MessageEntered() {
		messageSent.Play();
		chatText.text += "\n<color=red>DADMAN</color>: " + inputtedText.text.ToLower();
		if(CheckMessage()) {
				SetCurrentMessage();
				StartCoroutine(SendMessage());
				//Invoke("SendMessage", currentMessage.responseTime +chatDelay * Random.value);
				//SendMessage();
			}
			else {
				//Invoke("SendMissedMessage", currentMessage.responseTime + chatDelay * Random.value);
				StartCoroutine(SendMissedMessage());
			}
			inputtedText.text = "";
			leadText.text = "";
	}

	IEnumerator SendMessage() {
		yield return new WaitForSeconds(Mathf.Clamp(chatDelay * Random.value, 1, chatDelay));
		bool stopTyping = Random.value < (currentMessage.responseTime / 10);
		float characterSpeed = 1f / (((float)WPM * 5f) / 60f);
		float typingTime = currentMessage.call.ToCharArray().Length * characterSpeed;
		typingStatus.gameObject.SetActive(true);
		if(stopTyping) {
			float preTime = typingTime * Random.value;
			yield return new WaitForSeconds(preTime);
			typingStatus.gameObject.SetActive(false);
			yield return new WaitForSeconds(1 * Random.value);
			typingStatus.gameObject.SetActive(true);
			yield return new WaitForSeconds(Mathf.Abs(typingTime - preTime));
		}
		else yield return new WaitForSeconds(typingTime);
		typingStatus.gameObject.SetActive(false);
		chatText.text += "\n<color=blue>" + userName + "</color>: " + currentMessage.call;
		leadText.text = currentMessage.response.ToLower();
		messageReceived.Play();
	}
		
	IEnumerator SendMissedMessage() {
		yield return new WaitForSeconds(Mathf.Clamp(chatDelay * Random.value, 1, chatDelay));
		float characterSpeed = 1f / (((float)WPM * 5f) / 60f);
		float typingTime = currentMessage.call.ToCharArray().Length * characterSpeed;
		typingStatus.gameObject.SetActive(true);
		yield return new WaitForSeconds(typingTime);
		typingStatus.gameObject.SetActive(false);
		chatText.text += "\n<color=blue>" + userName + "</color>: " + currentMessage.missedResponses[Random.Range(0,currentMessage.missedResponses.Length - 1)];
		leadText.text = currentMessage.response.ToLower();
	}
		
	void SetCurrentMessage() {
		if(chatLog.Count > 0) {
			currentMessage = chatLog.Dequeue();
		}
		else {
			currentMessage = new Sentence("OK I gotta get dinner. Get some rest!", "", "", new string[] {""}, 5);
			Invoke("ResetGame", 5);
		}
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
		chatLog.Enqueue(new Sentence("how are u", "alright", "alright", new string[] {"what?"}, 1));
		chatLog.Enqueue(new Sentence("what's wrong", "I have a cold", "cold", new string[] {"a what", "huh?", "pardon?"}, 3));
		chatLog.Enqueue(new Sentence("Shit, hope you feel better", "How are you", "you", new string[] {"wat", "who?"}, 6));
		chatLog.Enqueue(new Sentence("Just finishing up some homework for school", "can i help", "help", new string[] {"can you what?", "what are you saying"}, 3));
		chatLog.Enqueue(new Sentence("know anything about bubble sort", "I know it sux", "sux", new string[] {"it what?", "its good?"}, 1));
		chatLog.Enqueue(new Sentence("lol okay", "what class is this for", "class", new string[] {"huh?", "what do u mean"}, 2));
		chatLog.Enqueue(new Sentence("AP comp sci", "following in your fathers footsteps", "footsteps", new string[] {"in your what?"}, 2));
		chatLog.Enqueue(new Sentence("are you feeling OK", "I feel fine", "fine", new string[]{"u dont seem fine", "wut"}, 1));
		chatLog.Enqueue(new Sentence("r u sure", "yes im sure", "sure", new string[]{ "you can't even type that"}, 1));
		chatLog.Enqueue(new Sentence("your text is really garbled", "i think its just the cold", "cold", new string[] {"I dont understand", "wat"}, 1));
		chatLog.Enqueue(new Sentence("are you sure", "leave it alone", "alone", new string[] {"huh"}, 1));
		chatLog.Enqueue(new Sentence("im worried", "its just hard to talk now", "talk", new string[] {"see what I mean", "this seems bad"}, 1));
	}

	void ResetGame() {
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}
