using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour {

	public struct KeyStroke {
		public string characterString;
		public LetterKey targetKey;
		public KeyCode keyCode;
	}
	Dictionary<KeyCode, string> keyCodeKeys;

	// Use this for initialization
	void Start () {
		int xOffset = 50;
		int yOffset = 30;
		GameObject canvas = GameObject.Find("Canvas");
		Vector3 startPos = new Vector3(100, 80, 0);
		foreach(string line in new string[] {"QWERTYUIOP", "ASDFGHJKL", "ZXCVBNM"}) {
			for(int i = 0; i < line.Length; i++) {
				GameObject temp = Instantiate(Resources.Load<GameObject>("LetterKey"), canvas.transform);
				temp.GetComponent<RectTransform>().position = startPos;
				temp.GetComponent<LetterKey>().SetKey((KeyCode)System.Enum.Parse(typeof(KeyCode),line[i].ToString()));
				startPos.x += xOffset;
			}
			startPos.x -= xOffset * line.Length * 0.95f;
			startPos.y -= yOffset;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
