// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SectorView : MonoBehaviour {

	public float scaleFactor = 250f;
	public GameObject viewText;
	void LateUpdate () {
		viewText.GetComponent<Text> ().text = "SECTOR: " + (Mathf.Floor (transform.position.x / scaleFactor)).ToString() + ": " + (Mathf.Floor (transform.position.y / scaleFactor)).ToString() + ": " + (Mathf.Floor (transform.position.z / scaleFactor)).ToString ();
	}
}
