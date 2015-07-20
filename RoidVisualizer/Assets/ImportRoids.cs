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
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class ImportRoids : MonoBehaviour {
	public GameObject roidPrefab;
	public GameObject otherPrefab;

	public GameObject buttonContentPanel;
	public GameObject buttonPrefab;

	public GameObject territory_Enemy;
	public GameObject territory_Us;

	public float scaleFactor = 1000f;
	public bool showTerritory = true;
	public string filter = "";

	public List<string> localGPS;
	public List<GameObject> objects = new List<GameObject>();
	public List<GameObject> buttons = new List<GameObject>();

	public string[] asteroidFlags = {"Au", "Gold", "Pt", "Platinum", "H2O", "Ice", "Fe", "Iron", "Ag", "Silver", "Co", "Cobalt", "Mg", "Magnesium", "Si", "Silicon", "Ni", "Nickel", "U", "Uranium"};

	public void Start () {
		foreach (GameObject singleObj in objects) {
			Destroy(singleObj);
		}
		foreach (GameObject button in buttons) {
			Destroy(button);
		}

		StreamReader urlFile = new StreamReader (Application.dataPath + @"\URL.txt"); //Load the URL File
		string url = urlFile.ReadLine ();
		urlFile.Close ();
		WWW googleSheetDL = new WWW (url);
		while (!googleSheetDL.isDone) {}
		char[] tsvDelim = {'\t','\n'};
		string[] sheetVals = googleSheetDL.text.Split (tsvDelim);
		ArrayList coordList = new ArrayList ();
		string roidListText = "Points:\n\n";
		foreach (string str in sheetVals) {
			if (str.Contains("GPS:") && str.ToLower().Contains(filter)) {
				coordList.Add(str);
			}
		}
		foreach (string str in localGPS) {
			if (str.Contains("GPS:") && str.ToLower().Contains(filter)) {
				coordList.Add(str);
			}
		}

		for (int i = 0; i < coordList.Count; i++)
		{
			string[] coord = coordList[i].ToString().Split(':');
			float x,y,z;
			string name = coord[1];
			float.TryParse(coord[2], out x);
			float.TryParse(coord[3], out y);
			float.TryParse(coord[4], out z);
			Vector3 pos = new Vector3(x/scaleFactor,y/scaleFactor,z/scaleFactor);
			if (CaseInsensitiveBatchContains(name, asteroidFlags)) {
				objects.Add((GameObject)GameObject.Instantiate(roidPrefab.gameObject, pos, Quaternion.identity)); //Object is an asteroid
				roidListText = roidListText + "<color=#888888ff>- " + name + "</color>\n";
			} else { //Something else (Maybe territory?)
				if (name.Contains("TERR_EN;") && showTerritory) { //Their Territory
					objects.Add((GameObject)GameObject.Instantiate(territory_Enemy.gameObject, pos, Quaternion.identity));
					float size;
					if (float.TryParse(name.Split(';')[1], out size)) { //Change to territory size
						size = size / scaleFactor;
						objects[objects.Count - 1].transform.localScale = new Vector3(size, size, size);
					}

				} else {
					if (name.Contains("TERR_US;") && showTerritory) { //Our Territory
						objects.Add((GameObject)GameObject.Instantiate(territory_Us.gameObject, pos, Quaternion.identity));
						float size;
						if (float.TryParse(name.Split(';')[1], out size)) { //Change to territory size
							size = size /scaleFactor;
							objects[objects.Count - 1].transform.localScale = new Vector3(size, size, size);
						}
					} else { //Not Territory
						objects.Add((GameObject)GameObject.Instantiate(otherPrefab.gameObject, pos, Quaternion.identity));
						roidListText = roidListText + "<color=#ee00ffff>- " + name + "</color>\n";
					}
				}
			}

			buttons.Add((GameObject)GameObject.Instantiate(buttonPrefab.gameObject));
			buttons[buttons.Count - 1].transform.SetParent(buttonContentPanel.transform);
			buttons[buttons.Count - 1].gameObject.name = coordList[i].ToString();
			buttons[buttons.Count - 1].GetComponentsInChildren<Text>()[0].text = name;

			objects[objects.Count - 1].name = coordList[i].ToString(); //Set name to original coord for retrieval later
			objects[objects.Count - 1].GetComponentInChildren<TextMesh>().text = name;

		}
		StartCoroutine (restart()); //Refresh Cycle
	}

	IEnumerator restart() {
		yield return new WaitForSeconds (90);
		Start ();
	}

	public void AddRoid (string GPS) { //Added by text feild
		localGPS.Add (GPS);
		filter = "";
		Start ();
	}

	public void Filter(string input) { //Filter by text feild
		filter = input;
		Start ();
	}

	public void TerritoryDisplay (bool input) { //Toggle button
		showTerritory = input;
		Start ();
	}
	/*
	public void SetScaleFactor (float input) {
		scaleFactor = (1f - input) * 1250f;
		Start ();
	}
	*/

	public void MoveToObserve (string GPS) {
		TextEditor te = new TextEditor (); //Copy to clipboard
		te.content = new GUIContent (GPS);
		te.SelectAll ();
		te.Copy ();

		string[] coord = GPS.Split(':');
		float x,y,z;
		string name = coord[1];
		float.TryParse(coord[2], out x);
		float.TryParse(coord[3], out y);
		float.TryParse(coord[4], out z);
		StartCoroutine (quickMove(transform.position, new Vector3(x/scaleFactor, y/scaleFactor, z/scaleFactor) + (transform.forward) * (scaleFactor / -6f))); //Moved back a bit
	}

	IEnumerator quickMove (Vector3 from, Vector3 to) {
		for (float i = 0; i < 1; i += 0.01f) {
			transform.position = Vector3.Lerp(from, to, i);
			yield return new WaitForSeconds(0.005f);
		}
	}

	public static bool CaseInsensitiveBatchContains(string text, string[] contains) {
		bool detected = false;
		foreach (string str in contains) {
			if (text.ToLower().Contains(str.ToLower())) {
				Debug.Log(str.ToLower());
				detected = true;
				return detected;
			}
		}
		return detected;
	}
	
}

	
