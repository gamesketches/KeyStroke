using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LetterKey : MonoBehaviour {

	public KeyCode currentKey;
	Text childText;
	public float pressAnimationTime;
	public float pressScale;
	// Use this for initialization
	void Start () {
		childText = GetComponentInChildren<Text>();
		childText.text = currentKey.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(currentKey)) {
			StartCoroutine(Type());
		}
	}

	public void SetKey(KeyCode newKey) {
		currentKey = newKey;
		childText.text = currentKey.ToString();
	}

	IEnumerator Type() {
		Vector3 startingScale = transform.localScale;
		Vector3 endScale = startingScale * pressScale;
		for(float t = 0; t < pressAnimationTime * 2; t += Time.deltaTime) {
			transform.localScale = Vector3.Slerp(startingScale, endScale, Mathf.PingPong(t, pressAnimationTime));
			yield return null;
		}
	}
}
