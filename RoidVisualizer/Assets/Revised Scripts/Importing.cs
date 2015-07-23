// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

// A class to localize all the stray read and convert functions. 
public class Importing : MonoBehaviour {

	//Download a .tsv from the internet and parse it (Mainly for google sheets) 
	public static List<string> downloadTSV (string URL) {
		WWW googleSheetDL = new WWW (URL);
		while (!googleSheetDL.isDone) {}
		char[] tsvDelim = {'\t','\n'};
		string[] sheetVals = googleSheetDL.text.Split (tsvDelim, System.StringSplitOptions.RemoveEmptyEntries);
		List<string> result = new List<string>();
		foreach (string part in sheetVals) {
			result.Add(part);
		}
		return result;
	}

	//Read all the entries in a file into memory as a List<string>
	public static List<string> readFile (string directory) {
		StreamReader file = new StreamReader(directory);
		List<string> result = new List<string>();
		if (File.Exists(directory)) {
			string line;
			do {
				line = file.ReadLine();
				if (line != "") {
					result.Add(line);
				}
			} while (!file.EndOfStream);
			file.Close();
		}
		return result;
	}

	//Filter large batches of strings with many filters
	public static List<string> filterEntries (List<string> input, List<string> whiteList) {
		List<string> result = new List<string>();
		if (whiteList.Count > 0) {
			foreach (string entry in input) {
				foreach (string filter in whiteList) {
					if (entry.ToLower().Contains(filter.ToLower())) { //.ToLower() is to make it case insensitive
						result.Add(entry);
						break; //If there are repeats in the filter do not precede further
					}
				}
			}
		} else {
			result = input; //No filter has been applied. 
		}
		return result;
	}

	//Filter large batches of GPS Points with many filters
	public static List<GPSDefinition.GPSPoint> filterGPSEntries (List<GPSDefinition.GPSPoint> input, List<string> whiteList) {
		List<GPSDefinition.GPSPoint> result = new List<GPSDefinition.GPSPoint>();
		if (whiteList.Count > 0) {
			foreach (GPSDefinition.GPSPoint entry in input) {
				foreach (string filter in whiteList) {
					if (entry.name.ToLower().Contains(filter.ToLower())) { //.ToLower() is to make it case insensitive
						result.Add(entry);
						break; //If there are repeats in the filter do not precede further
					}
				}
			}
		} else {
			result = input; //No filter has been applied. 
		}
		return result;
	}

	//Convert a bunch of GPS strings into point objects
	public static List<GPSDefinition.GPSPoint> batchConvert (List<string> points, float scaleDivisor) {
		List<GPSDefinition.GPSPoint> result = new List<GPSDefinition.GPSPoint>();
		foreach (string gps in points) {
			result.Add(new GPSDefinition.GPSPoint(gps, scaleDivisor));
		}
		return result;
	}



}
