// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Clicked : MonoBehaviour {
	void hit() { //Right Clicked
		string gps = gameObject.name;
		TextEditor te = new TextEditor ();
		te.content = new GUIContent (gps);
		te.SelectAll ();
		te.Copy ();
		StartCoroutine (blink());
	}

	IEnumerator blink() {
		gameObject.GetComponent<Renderer> ().material.color = Color.green;
		yield return new WaitForSeconds (0.2f);
		gameObject.GetComponent<Renderer> ().material.color = Color.white;
	}

}
