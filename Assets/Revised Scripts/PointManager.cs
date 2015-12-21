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
using System.Collections.Generic;

//Manages the in-game point parkers
public class PointManager : MonoBehaviour
{

    //Local only
    public List<GameObject> pointObjects; //In-game objects
    public List<GPSDefinition.GPSPoint> locationCache; //All the locations from import sources (Unfiltered)

    //Real time configurable
    public bool showTerritory = true;
    public List<string> filters;
    public string URL;

    //Configurable
    public float scaleDivisor = 200f; //The coordinates in SE are WAY too big to look at easily in Unity. The sizes are divided by this number.
    public string[] asteroidFlags = { "Au", "Gold", "Pt", "Platinum", "H2O", "Ice", "Fe", "Iron", "Ag", "Silver", "Co", "Cobalt", "Mg", "Magnesium", "Si", "Silicon", "Ni", "Nickel", "U", "Uranium" }; //Things that make an asteroid an asteroid
    public GameObject buttonContentPanel;

    //Marker Prefabs
    public GameObject hostileTerritoryPrefab;
    public GameObject friendlyTerritoryPrefab;
    public GameObject planetMarkerPrefab;
    public GameObject asteroidMarkerPrefab; //What the roids look like
    public GameObject otherMarkerPrefab; //A nice marker for other things (May add other categories soon!)

    public void Start()
    { 
        //Debug here!
    }

    public void boot(string tsvDir)
    {
        URL = tsvDir;
        reImport();
    }

    public void reImport() //Import/re-import the points from all sources
    {   
        //Google sheet ops
        locationCache.Clear();

        if (URL != null) //Make sure we CAN load it...
        { 
            List<string> sheet = Importing.downloadTSV(URL);
            foreach (string gps in sheet)
            {
                if (gps.Contains("GPS:"))
                {
                    locationCache.Add(new GPSDefinition.GPSPoint(gps, scaleDivisor));
                }
            }
            redisplay();
        }
        else
        {
            //SHOW AN ERROR MESSAGE PLS
            Debug.Log("No URL file");
        }


    }

    public void redisplay()
    { //Display cache contents
        foreach (GameObject obj in pointObjects)
        { //*flush*
            Destroy(obj);
        }
        List<GPSDefinition.GPSPoint> tempCache = new List<GPSDefinition.GPSPoint>();
        tempCache.Clear();
        tempCache = Importing.filterGPSEntries(locationCache, filters);
        pointObjects.Clear();
        buttonContentPanel.GetComponent<ButtonManager>().populate(tempCache);
        foreach (GPSDefinition.GPSPoint point in tempCache)
        {
            if (point.name.Contains("VL|"))
            { //It's a volume
                string[] substrings = point.name.Split('|');
                float size;
                if (float.TryParse(substrings[2], out size) && showTerritory)
                {
                    size = size / scaleDivisor;
                    switch (substrings[1])
                    {
                        case "FreindlyTerritory": //Us
                            CreateVolume(point.unityPosition, size, friendlyTerritoryPrefab);
                            break;

                        case "EnemyTerritory": //Enemy
                            CreateVolume(point.unityPosition, size, hostileTerritoryPrefab);
                            break;

                        case "Planet": //Planet
                            CreateVolume(point.unityPosition, size + 10, planetMarkerPrefab);//Add 10 to height to offset mountains
                            break;

                        default:
                            CreateVolume(point.unityPosition, size, planetMarkerPrefab);
                            break;

                    }

                }

            }
            else
            { //Its a roid or somethin
                bool isAsteroid = false;
                foreach (string flag in asteroidFlags)
                { //Check to see if it is an asteroid
                    if (point.name.Contains(flag))
                    {
                        isAsteroid = true;
                        break; //No need to keep checking
                    }
                }
                if (isAsteroid)
                {
                    CreateMarker(point.name, point.unityPosition, asteroidMarkerPrefab);
                }
                else
                {
                    CreateMarker(point.name, point.unityPosition, otherMarkerPrefab);
                }

            }
        }


    }

    //Display object instantiators
    void CreateMarker (string name, Vector3 position, GameObject prefab)
    {
        GameObject GO = Instantiate(prefab);
        GO.name = name;
        GO.transform.position = position;
        GO.GetComponentsInChildren<TextMesh>()[0].text = name; //Get the only text component
        pointObjects.Add(GO);
    }

    void CreateVolume(Vector3 position, float radius, GameObject prefab) //Territories, planets, etc.
    {
        GameObject GO = (GameObject)GameObject.Instantiate(prefab, position, Quaternion.identity);
        GO.transform.localScale = new Vector3(radius, radius, radius);
        pointObjects.Add(GO);
    }


    //UI Call functions
    public void changeFilters(List<string> newFilters)
    {
        filters = newFilters;
        redisplay();
    }

    public void displayTerritory(bool show)
    {
        showTerritory = show;
        redisplay();
    }

    public void addTemporaryPoint(string GPS)
    {
        locationCache.Add(new GPSDefinition.GPSPoint(GPS, scaleDivisor));
    }

}
