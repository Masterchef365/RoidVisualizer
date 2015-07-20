// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	
	public Color mainColor = new Color(1f,1f,1f,1f);
	public Color subColor = new Color(1f,1f,1f,1f);
	public float mainIncrement = 1000f;
	public float subIncrement = 100f;
	public bool subGrid = true;
	public float sizeMin = -5000f;
	public float sizeMax = 5000f;
	public float fallOff = 0.0001f;
	private Material lineMat;
	public bool showGrid = true;
	float y = 0; //Floor
	
	void OnPostRender() {
		if (showGrid) {
			lineMat = CreateLineMaterial ();
			lineMat.SetPass (0);

			GL.Begin (GL.LINES);

			if (subGrid) {
				for (float z = sizeMin; z < sizeMax; z += subIncrement) {
					GL.Color (subColor * (1 - (Vector3.Distance(new Vector3(transform.position.x, transform.position.y, z), transform.position) * fallOff)));
					GL.Vertex3 (sizeMin, y, z); //x Lines
					GL.Vertex3 (sizeMax, y, z);
				}
				
				for (float x = sizeMin; x < sizeMax; x += subIncrement) {
					GL.Color (subColor * (1 - (Vector3.Distance(new Vector3(x, transform.position.y, transform.position.z), transform.position) * fallOff)));
					GL.Vertex3 (x, y, sizeMin); //x Lines
					GL.Vertex3 (x, y, sizeMax);
				}
			}

			GL.Color(mainColor);
			for (float y = sizeMin; y < sizeMax; y += mainIncrement) {
				for (float z = sizeMin; z < sizeMax; z += mainIncrement) {
					GL.Color (mainColor * (1 - (Vector3.Distance(new Vector3(transform.position.x, y, z), transform.position) * fallOff)));
					GL.Vertex3 (sizeMin, y, z); //x Lines
					GL.Vertex3 (sizeMax, y, z);
				}
				
				for (float x = sizeMin; x < sizeMax; x += mainIncrement) {
					GL.Color (mainColor * (1 - (Vector3.Distance(new Vector3(x, y, transform.position.z), transform.position) * fallOff)));
					GL.Vertex3 (x, y, sizeMin); //x Lines
					GL.Vertex3 (x, y, sizeMax);
				}
			}
			
			for (float z = sizeMin; z < sizeMax; z += mainIncrement) {
				for (float x = sizeMin; x < sizeMax; x += mainIncrement) {
					GL.Color (mainColor * (1 - (Vector3.Distance(new Vector3(x, transform.position.y, z), transform.position) * fallOff)));
					GL.Vertex3 (x, sizeMin, z);//y Lines
					GL.Vertex3 (x, sizeMax, z);
				}
			}
			
			GL.End ();
		}
	}
	
	
	
	Material CreateLineMaterial() 
	{
		Material lineMaterial;
		lineMaterial = new Material( "Shader \"Lines/Colored Blended\" {" +
		                            "SubShader { Pass { " +
		                            "    Blend SrcAlpha OneMinusSrcAlpha " +
		                            "    ZTest Always " +
		                            "    ZWrite Off Cull Off Fog { Mode Off } " +
		                            "    BindChannels {" +
		                            "      Bind \"vertex\", vertex Bind \"color\", color }" +
		                            "} } }" );
		lineMaterial.hideFlags = HideFlags.HideAndDontSave;
		lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
		return lineMaterial;
	}
	
	public void showLineGrid (bool input) {
		showGrid = input;
	}
	
	
}
