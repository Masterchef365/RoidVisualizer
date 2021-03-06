﻿// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class DrawPathLines : MonoBehaviour {

	public List<Vector3> points; 
	public float scaleFactor = 500f;
	public Color lineColor = new Color(1f,1f,1f);
	public Text distanceOutput;
	public float distance;

	public void parseLines (List<string> distancePoints) {
		points.Clear ();
		distance = 0f;
		if (distancePoints.Count > 0) { //Check for null
			foreach (string gps in distancePoints) {
				string[] coord = gps.Split (':');
				if (coord.Length > 4) {
					float x, y, z;
					//string name = coord [1];
					float.TryParse (coord [2], out x);
					float.TryParse (coord [3], out y);
					float.TryParse (coord [4], out z);
					points.Add (new Vector3 (x / scaleFactor, y / scaleFactor, z / scaleFactor));
				}
			}
		}

		for (int i = 1; i < points.Count; i++) { //Count distance
			distance += Vector3.Distance(points[i-1] * scaleFactor, points[i] * scaleFactor);
		}
		if (points.Count > 1) {
			distanceOutput.text = "Total Distance: " + ((int)(distance / 1000f)).ToString () + " Km";
		} else {
			distanceOutput.text = "Total Distance: ";
		}
	}

	void OnPostRender () {
		if (points.Count > 1) {
			CreateLineMaterial ();
            GL.PushMatrix();
            lineMaterial.SetPass(0);
			GL.Begin (GL.LINES);
			GL.Color (lineColor);
			for (int i = 1; i < points.Count; i++) {
				GL.Vertex3 (points [i - 1].x, points [i - 1].y, points [i - 1].z);
				GL.Vertex3 (points [i].x, points [i].y, points [i].z);
			}
			GL.End ();
            GL.PopMatrix();
		}
	}

    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            var shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

}
