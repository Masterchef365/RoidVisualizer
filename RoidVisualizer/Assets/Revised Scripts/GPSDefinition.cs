// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;

public class GPSDefinition : MonoBehaviour {
	[System.Serializable]
	public class GPSPoint {
		public Vector3 unityPosition;
		public string name;
		public string originalGPS;
	
		public GPSPoint(string GPS, float scaleDivisor) { //Constructor, converts to unity scale and parses too!
			originalGPS = GPS;
			string[] coord = GPS.Split(':');
			float x,y,z;
			name = coord[1];
			float.TryParse(coord[2], out x);
			float.TryParse(coord[3], out y);
			float.TryParse(coord[4], out z);
			unityPosition = new Vector3(x/scaleDivisor,y/scaleDivisor,z/scaleDivisor);
		}

	}
}
