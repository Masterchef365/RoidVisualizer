// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;

public class GPSPointContainer : MonoBehaviour {

	public GameObject contentPanel;
	public GPSDefinition.GPSPoint point;

	void Start() {
		contentPanel = transform.parent.gameObject;
	}

	public void sendClick () {
		contentPanel.SendMessage("sendClick", point);
	}

}
