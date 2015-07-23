/*
Copyright (C) 2015 Duncan Freeman

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Manages the in-game point parkers
public class PointManager : MonoBehaviour {

	//Local only
	public float scaleDivisor = 200f; //The coordinates in SE are WAY too big to look at easily in Unity. The sizes are divided by this number.
	public List<GameObject> pointObjects; //In-game objects
	public List<GPSDefinition.GPSPoint> locationCache; //All the locations from import sources (Unfiltered)
	public string[] asteroidFlags = {"Au", "Gold", "Pt", "Platinum", "H2O", "Ice", "Fe", "Iron", "Ag", "Silver", "Co", "Cobalt", "Mg", "Magnesium", "Si", "Silicon", "Ni", "Nickel", "U", "Uranium"}; //Things that make an asteroid an asteroid

	//Real time configurable
	public bool showTerritory = true;
	public List<string> filters;

	//Configurable
	public GameObject buttonContentPanel;
	public GameObject hostileTerritoryPrefab;
	public GameObject friendlyTerritoryPrefab;
	public GameObject asteroidMarkerPrefab; //What the roids look like
	public GameObject otherMarkerPrefab; //A nice marker for other things (May add other categories soon!)

	public void Start () { //Begin for the first time
		reImport();
		redisplay();
	}

	public void reImport () { //Import/re-import the points from all sources
		//Google sheet ops
		locationCache.Clear ();

		List<string> URL = Importing.readFile(Application.dataPath + @"\URL.txt");
		List<string> sheet = Importing.downloadTSV(URL[0]);
		foreach (string gps in sheet) {
			if (gps.Contains("GPS:")) {
				locationCache.Add(new GPSDefinition.GPSPoint(gps, scaleDivisor));
			}
		}

		//Future site of local GPS importer for Jim's script!
	}

	public void redisplay () { //Display cache contents
		foreach (GameObject obj in pointObjects) { //*flush*
			Destroy(obj);
		}
		List<GPSDefinition.GPSPoint> tempCache = new List<GPSDefinition.GPSPoint>();
		tempCache.Clear();
		tempCache = Importing.filterGPSEntries(locationCache, filters);
		pointObjects.Clear();
		buttonContentPanel.GetComponent<ButtonManager>().populate(tempCache);
		foreach (GPSDefinition.GPSPoint point in tempCache) {
			if (point.name.Contains("TERR")) { //It's a territory!
				string[] territorySubstrings = point.name.Split(';');
				float size;
				if (float.TryParse(territorySubstrings[1], out size) && showTerritory) {
					size = size / scaleDivisor;
					GameObject territoryObject;
					switch (territorySubstrings[0]) {
					case "TERR_US": //Us
						territoryObject = (GameObject)GameObject.Instantiate((GameObject)friendlyTerritoryPrefab, point.unityPosition, Quaternion.identity);
						territoryObject.transform.localScale = new Vector3(size, size, size);
						pointObjects.Add(territoryObject);
						break;

					case "TERR_EN": //Enemy
						territoryObject = (GameObject)GameObject.Instantiate((GameObject)hostileTerritoryPrefab, point.unityPosition, Quaternion.identity);
						territoryObject.transform.localScale = new Vector3(size, size, size);
						pointObjects.Add(territoryObject);
						break;

					}

				}

			} else { //Its a roid or somethin
				bool isAsteroid = false;
				foreach (string flag in asteroidFlags) { //Check to see if it is an asteroid
					if (point.name.Contains(flag)) {
						isAsteroid = true;
						break; //No need to keep checking
					}
				}
				GameObject GO;
				if (isAsteroid) {
					GO = (GameObject)GameObject.Instantiate(asteroidMarkerPrefab);
				} else {
					GO = (GameObject)GameObject.Instantiate(otherMarkerPrefab);
				}
				GO.name = point.name;
				GO.transform.position = point.unityPosition;
				GO.GetComponentsInChildren<TextMesh>()[0].text = point.name; //Get the only text component
				pointObjects.Add(GO);
			}
		}


	}

	public void changeFilters (List<string> newFilters) {
		filters = newFilters;
		redisplay();
	}

	public void displayTerritory (bool show) {
		showTerritory = show;
		redisplay();
	}

	public void addTemporaryPoint (string GPS) {
		locationCache.Add(new GPSDefinition.GPSPoint(GPS, scaleDivisor));
	}

}
