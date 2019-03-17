using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterKey : MonoBehaviour {

	public KeyCode currentKey;
	public KeyCode baseKey;
	Text childText;
	public float pressAnimationTime;
	public float pressScale;
	public static float revertTime;
	float revertTimer;
	Text outputString;
	// Use this for initialization
	void Awake () {
		revertTime = 1;
		revertTimer = -1;
		outputString = GameObject.Find("inputText").GetComponent<Text>();
		childText = GetComponentInChildren<Text>();
		childText.text = currentKey.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(baseKey)) {
			StartCoroutine(Type());
		}
		if(revertTimer > 0) {
			revertTimer -= Time.deltaTime;
			if(revertTimer <= 0) {
				SetKey(baseKey);
			}
		}
	}

	public void SetKey(KeyCode newKey) {
		currentKey = newKey;
		childText.text = currentKey.ToString();
		revertTimer = revertTime; 
	}

	public void SetKeyLabel(string letter) {
		childText.text = letter;
	}

	IEnumerator Type() {
		outputString.text += currentKey.ToString().ToLower();
		Vector3 startingScale = transform.localScale;
		Vector3 endScale = startingScale * pressScale;
		for(float t = 0; t < pressAnimationTime * 2; t += Time.deltaTime) {
			transform.localScale = Vector3.Slerp(startingScale, endScale, Mathf.PingPong(t, pressAnimationTime));
			yield return null;
		}
	}

	public void RevertKey() {
		SetKey(baseKey);
		revertTimer = 0;
	}
}
