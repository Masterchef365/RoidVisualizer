// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour {

	public GameObject cameraObject;
	public GameObject buttonPrefab;
	public List<GameObject> buttons;
	public void populate (List<GPSDefinition.GPSPoint> locations) {
		foreach (GameObject button in buttons) {
			Destroy(button);
		}
		foreach (GPSDefinition.GPSPoint point in locations) {
			GameObject newButton = (GameObject)GameObject.Instantiate(buttonPrefab);
			newButton.GetComponent<GPSPointContainer>().point = point;
			newButton.transform.SetParent(transform);
			newButton.GetComponentInChildren<Text>().text = point.name;
			buttons.Add(newButton);
		}
	}

	public void sendClick (GPSDefinition.GPSPoint point) { //Pass it on
		cameraObject.SendMessage("moveToObserve", point);
		TextEditor te = new TextEditor ();
		te.content = new GUIContent (point.originalGPS);
		te.SelectAll ();
		te.Copy ();
	}
}
