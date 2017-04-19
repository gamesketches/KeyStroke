﻿using System.Collections;
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

	// Use this for initialization
	void Start () {
		int xOffset = 50;
		int yOffset = 30;
		GameObject canvas = GameObject.Find("Canvas");
		Vector3 startPos = new Vector3(100, 80, 0);
		keyCodeKeys = new Dictionary<KeyCode, string>();
		allKeys = new Dictionary<KeyCode, LetterKey>();
		foreach(string line in new string[] {"QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM"}) {
			for(int i = 0; i < line.Length; i++) {
				KeyCode keyCode = ParseKeyCode(line[i].ToString());//(KeyCode)System.Enum.Parse(typeof(KeyCode), line[i].ToString());
				keyCodeKeys.Add(keyCode, line[i].ToString());
				GameObject temp = Instantiate(Resources.Load<GameObject>("LetterKey"), canvas.transform);
				temp.GetComponent<RectTransform>().position = startPos;
				LetterKey newLetterKey = temp.GetComponent<LetterKey>();
				newLetterKey.SetKey(keyCode);
				allKeys.Add(keyCode, newLetterKey);

				startPos.x += xOffset;
			}
			startPos.x -= xOffset * line.Length * 0.95f;
			startPos.y -= yOffset;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.inputString.Length > 0) {
			outputString.text += keyCodeKeys[ParseKeyCode(Input.inputString[0].ToString())];
			SwapKeys();
		}
	}

	void SwapKeys() {
		string firstLetter = "QWERTYUIOPASDFGHJKLZXCVBNM"[Random.Range(0, 26)].ToString();
		string secondLetter;
		do {
			secondLetter = "QWERTYUIOPASDFGHJKLZXCVBNM"[Random.Range(0, 26)].ToString();
		} while(firstLetter == secondLetter);
		LetterKey firstLetterKey = allKeys[ParseKeyCode(firstLetter.ToString())];
		LetterKey secondLetterKey = allKeys[ParseKeyCode(secondLetter.ToString())];
		firstLetterKey.SetKeyLabel(secondLetter);
		secondLetterKey.SetKeyLabel(firstLetter);
		keyCodeKeys[ParseKeyCode(firstLetter)] = secondLetter;
		keyCodeKeys[ParseKeyCode(secondLetter)] = firstLetter;
	}

	KeyCode ParseKeyCode(string letter) {
		return (KeyCode)System.Enum.Parse(typeof(KeyCode), letter.ToUpper());
	}

	KeyCode ParseKeyCode(char letter) {
		return (KeyCode)System.Enum.Parse(typeof(KeyCode), letter.ToString().ToUpper());
	}
}
