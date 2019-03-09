using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardManager : MonoBehaviour {

	public struct KeyStroke {
		public string characterString;
		public LetterKey targetKey;
		public KeyCode keyCode;
	}
	Dictionary<KeyCode, string> keyCodeKeys;
	Dictionary<KeyCode, LetterKey> allKeys;

	public Text outputString;
	float chanceOfKeyChange;

	// Use this for initialization
	void Start () {
		chanceOfKeyChange = 0;
		int xOffset = 50;
		int yOffset = 30;
		GameObject canvas = GameObject.Find("Canvas");
		Vector3 startPos = new Vector3(370, 100, 0);
		keyCodeKeys = new Dictionary<KeyCode, string>();
		allKeys = new Dictionary<KeyCode, LetterKey>();
		foreach(string line in new string[] {"QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM"}) {
			for(int i = 0; i < line.Length; i++) {
				KeyCode keyCode = ParseKeyCode(line[i].ToString());
				keyCodeKeys.Add(keyCode, line[i].ToString());
				GameObject temp = Instantiate(Resources.Load<GameObject>("LetterKey"), canvas.transform);
				temp.GetComponent<RectTransform>().position = startPos;
				LetterKey newLetterKey = temp.GetComponent<LetterKey>();
				newLetterKey.SetKey(keyCode);
				newLetterKey.baseKey = keyCode;
				allKeys.Add(keyCode, newLetterKey);

				startPos.x += xOffset;
			}
			startPos.x -= xOffset * line.Length * 0.95f;
			startPos.y -= yOffset;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Backspace)) {
			outputString.text = outputString.text.Remove(outputString.text.Length - 1);
		}
		else if(Input.GetKeyDown(KeyCode.Space)) {
			outputString.text += " ";
		}
		else if(Input.GetKeyDown(KeyCode.Escape)) {
			UnityEngine.SceneManagement.SceneManager.LoadScene(0);
		}
		else if(!Input.GetKeyDown(KeyCode.Return) && Input.inputString.Length > 0) {
			//outputString.text += keyCodeKeys[ParseKeyCode(Input.inputString[0].ToString())].ToLower();
			chanceOfKeyChange += 0.005f;
			if(Random.value < chanceOfKeyChange){ 
				SwapKeys();
			}
		}
	}

	void SwapKeys() {
		GameObject[] keys = GameObject.FindGameObjectsWithTag("key");
		int first = Random.Range(0, 26);
		int second;
		do {
			second = Random.Range(0, 26);
		} while(first == second);
		KeyCode temp = keys[second].GetComponent<LetterKey>().currentKey;
		keys[second].GetComponent<LetterKey>().SetKey(keys[first].GetComponent<LetterKey>().currentKey);
		keys[first].GetComponent<LetterKey>().SetKey(temp); 
		Debug.Log("switched " + keys[first].GetComponent<LetterKey>().currentKey.ToString() + " and " + keys[second].GetComponent<LetterKey>().currentKey.ToString());
		if(LetterKey.revertTime < 10) 
			LetterKey.revertTime += Random.value;
		ConfirmKeyboardParity();
	}

	KeyCode ParseKeyCode(string letter) {
		return (KeyCode)System.Enum.Parse(typeof(KeyCode), letter.ToUpper());
	}

	KeyCode ParseKeyCode(char letter) {
		return (KeyCode)System.Enum.Parse(typeof(KeyCode), letter.ToString().ToUpper());
	}
}
