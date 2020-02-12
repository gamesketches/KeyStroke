using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputChecker : MonoBehaviour {

	public class Sentence {
		public string[] call;
		public string response;
		public string wordToCheckFor;
		public string[] missedResponses;
		public float responseTime;

		public Sentence(string[] newCall, string newResponse, string checkWord, string[] responses, float time) {
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
		StartCoroutine(SendMessage(2.5f));
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
				StartCoroutine(SendMessage(chatDelay));
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

	IEnumerator SendMessage(float waitTime) {
		yield return new WaitForSeconds(Mathf.Clamp(waitTime * Random.value, 1, waitTime));
		foreach(string call in currentMessage.call) {
			bool stopTyping = Random.value < (currentMessage.responseTime / 10);
			Debug.Log(currentMessage.responseTime / 10);
			float characterSpeed = 1f / (((float)WPM * 5f) / 60f);
			float typingTime = call.ToCharArray().Length * characterSpeed;
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
			chatText.text += "\n<color=blue>" + userName + "</color>: " + call;
			messageReceived.Play();
		}
			leadText.text = currentMessage.response.ToLower();
	}
		
	IEnumerator SendMissedMessage() {
		yield return new WaitForSeconds(Mathf.Clamp(chatDelay * Random.value, 1, chatDelay));
		float characterSpeed = 1f / (((float)WPM * 5f) / 60f);
		float typingTime = currentMessage.missedResponses[0].ToCharArray().Length * characterSpeed;
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
			currentMessage = new Sentence(new string[] {"OK I gotta get dinner. Get some rest!"}, "", "", new string[] {""}, 5);
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
		chatLog.Enqueue(new Sentence(new string[] {"When I was a sophomore in high school, everyone used AOL Instant Messenger", 
			"The key to its popularity sat right in the middle of its name: <i>Instant</i> messaging",
			"Unlike email, which has a cadence of hours or days AIM allowed users to type a message into a computer",
			"send it to a friend via a server",
			"and receive one back within seconds.",
			"Communicating in real time gives messages the gravitas of proximity",
			"it collapses physical distance"}, "interesting", "interesting", new string[] {"huh?"}, 1));
		chatLog.Enqueue(new Sentence(new string[] {"AIM was built on a particular genus of feedback loop, that's prevalent in technology",
			"input to computer -> cpu processes -> output to user -> users process",
			"Video games are based around it",
			"There’s a rule of thumb among game designers",
			"the more exaggerated the feedback of your game, the more engaging it becomes",
			"This exaggerated feedback can take various forms",
			"sound effects",
			"particle systems",
			"flashing colors",
			"and is colloquially referred to as “juice”"}, "juice", "juice", new string[] {"No, it's “juice”"}, 1));
		chatLog.Enqueue(new Sentence(new string[] {"I’m not sure about the exact etymology of “juice”",
			"but I always imagine crushing a nice plump tomato in my hands",
			"all the liquid and seeds inside squirting out with a satisfying squish dribbling down my palms onto dry asphalt"}, "games are fun", "fun", new string[] {"One more time?"}, 1));
		chatLog.Enqueue(new Sentence(new string[] {"My first game console was a Nintendo Entertainment System",
			"I got it for Christmas from my mom the year I turned six",
			"The same year I started kindergarten",
			"the year my parents separated"}, "so young", "young", new string[] {"so what?"}, 1));
		chatLog.Enqueue(new Sentence(new string[] {
			"I grew up between two extremes",
			"Mom's house had Nintendo",
			"Dad's house had books",
			"At the start of middle school, my father changed jobs beginning a new life on the road",
			"or really in the sky",
			"I lived almost exclusively with my mom after that"}, "away from your father", "father", new string[] {"huh?"},1));
		chatLog.Enqueue(new Sentence(new string[] {
			"My dad saw our physical distance as an engineering problem",
			"He loved exploring new communication methods",
			"as they were invented",
			"He made me a Yahoo! email address in 1994",
			"called me from an airplane the first week they had phones",
			"sent me emails from his Palm Pilot at the turn of the millenium",
			"Each new channel brought with it the same cycle of excitement, regularity, and disappointment",
			"Excitement at the marvel of the tech",
			"and of <b>FINALLY</b> having a way to easily communicate",
			"then regular use",
			"all before inevitable disappointment as I fell off the wagon",
			"In 2002, he discovered AIM" }, "The legend", "legend", new string[] {"what?"}, 1));
		chatLog.Enqueue(new Sentence(new string[] {
			"At first the excitement: we could chat <i>instantaneously</i> whenever we both had time",
			"As a bonus",
			"he intuited I was already on AIM everyday",
			"Our IMs felt more like check ins than meaningful family time",
			"like conversations with a distant relative",
			"“How's school?”",
			"“How are your grades?”",
			"“Where are you going to college?”"}, "sad", "sad", new string[]{"what?"},1));
		chatLog.Enqueue(new Sentence(new string[] {
			"As a teenager wanting to keep this space exclusive from my parents, I started going invisible whenever my dad logged on",
			"He noticed",
			"My mistake was “logging off” in response to him logging on",
			"every single day",
			"it was like turning off the lights when the doorbell rings",
			"Other times I would just stop responding, not wanting to deal with the stress of talking to him",
			"Sneaking around, trying to keep the internet my safe little friendship garden",
			"defined my early internet presence",
			"Eventually he stopped using AIM"}, "Can you type now?", "type", new string[]{"Can you what?"},1));
		chatLog.Enqueue(new Sentence(new string[] {
			"It's a cold October day in 2009, my senior year in college",
			"Obama is president",
			"AIM is still the dominant web communication platform",
			"but the internet is changing",
			"Facebook rules social networking and Twitter is starting to take off",
			"Google and Facebook have both released chat apps to rival AIM",
			"The economy is in freefall",
			"but we have more ways to talk about it than ever"}, "Weird times", "weird", new string[]{"huh?"}, 1));
		chatLog.Enqueue(new Sentence(new string[] {
			"I'm not talking about it though",
			"I'm sitting in my grandfather's 2-bedroom retirement condo",
			"in Madison, Wisconsin",
			"numb"}, "what happened", "happened", new string[]{"??"}, 2));
		chatLog.Enqueue(new Sentence(new string[] {
			"I had read a terrible eulogy for my father that morning",
			"It had happened suddenly",
			"a dead brain",
			"killed by a series of strokes",
			"Hostile viruses in his blood, had blocked off the blood supply",
			"suicidal explorers from a virus colony on his heart",
			"He had mentioned a persistent fever in the days before",
			"but ignored it",
			"He hated doctors",
			"Eventually he fell",
			"went unconscious",
			"and never came back"}, "im sorry", "sorry", new string[] {"you're what?"}, 1));
		chatLog.Enqueue(new Sentence(new string[] {
			"Now I'm digging through the three cardboard boxes of his possessions, deciding what to take back and what to give away",
			"I find his Macbook Air and open it up",
			"He's still logged on",
			"I start poking around",
			//"It feels like I'm back in one of those hotel rooms, about to install AIM",
			"A weirdly primal desperation begins taking hold",
			"I need to find something",
			"A picture, a document, a recording, a message",
			"anything",
			"anything from when he was alive",
			"Skype opens itself, along with all his “recent“ conversations",
			"I find his last"}, "who was it", "who", new string[]{"??"}, 1));
		chatLog.Enqueue(new Sentence(new string[] {
			"He was chatting with someone using a weird screenname",
			"possibly a girlfriend?",
			"His illness had her worried",
			"The chatlog is comfortably normal digital pillow talk",
			"but something's off",
			"Dad's messages contain a bizarre number of typos",
			"The longer the conversation goes on, the more random letters he accumulates",
			"it's as if his fingers are two inches thick",
			"frequently misspelling words",
			"adding random punctuation",
			"slurred digital speech"}, "oh my god", "god", new string[]{"????"}, 1));
		chatLog.Enqueue(new Sentence(new string[] {
			"It gets bad",
			"So bad she starts begging him to see a doctor",
			"It's strange for anyone to be this bad at typing",
			"He tells her he hates doctors",
			"He's not feeling well",
			"but he'll be fine",
			"it's just a fever",
			"These assurances are all mangled and incoherent",
			"Eventually he types he's going to bed",
			"His final message:",
			"“It'sei hqarte to tuupe niew“",
			"he was trying to say “It's hard to type now“"}, "wow", "wow", new string[]{"You too huh?"}, 1));
			
			
			
		/*chatLog.Enqueue(new Sentence("hey", "hey", "hey", new string[] {"huh?", "what?"}, 1));
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
		chatLog.Enqueue(new Sentence("im worried", "its just hard to talk now", "talk", new string[] {"see what I mean", "this seems bad"}, 1));*/
	}

	void ResetGame() {
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}
}
