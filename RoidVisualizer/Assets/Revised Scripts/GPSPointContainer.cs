// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;

public class GPSPointContainer : MonoBehaviour {

	public GameObject contentPanel;
	public GPSDefinition.GPSPoint point;

	public void sendClick () {
		contentPanel.SendMessage("sendClick", point);
	}

}
