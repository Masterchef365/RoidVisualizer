// Copyright (C) 2015 Duncan Freeman
using UnityEngine;
using System.Collections;

public class GridOptimized : MonoBehaviour {
	
	public Color mainColor = new Color(1f,1f,1f,1f);
	public Color subColor = new Color(1f,1f,1f,1f);
	public float mainIncrement = 1000f;
	public float subIncrement = 100f;
	public bool subGrid = true;
	public float sizeMin = -5000f;
	public float sizeMax = 5000f;
	private Material lineMat;
	public bool showGrid = true;
	public Vector3 offset = Vector3.zero;
	float y = 0; //Floor
	
	void OnPostRender() {
		if (showGrid) {
			lineMat = CreateLineMaterial ();
			lineMat.SetPass (0);

			offset = new Vector3(((float)(int)(transform.position.x / mainIncrement))* mainIncrement, ((float)(int)(transform.position.y / mainIncrement))* mainIncrement, ((float)(int)(transform.position.z / mainIncrement))* mainIncrement);

			GL.Begin (GL.LINES);

			GL.Color(subColor);
			if (subGrid) {
				for (float z = sizeMin; z < sizeMax; z += subIncrement) {
					//GL.Color (subColor * (1 - (Vector3.Distance(new Vector3(transform.position.x, transform.position.y, z), transform.position) * fallOff)));
					GL.Vertex3 (sizeMin + offset.x, y, z + offset.z); //x Lines
					GL.Vertex3 (sizeMax + offset.x, y, z + offset.z);
				}
				
				for (float x = sizeMin; x < sizeMax; x += subIncrement) {
					//GL.Color (subColor * (1 - (Vector3.Distance(new Vector3(x, transform.position.y, transform.position.z), transform.position) * fallOff)));
					GL.Vertex3 (x + offset.x, y, sizeMin + offset.z); //x Lines
					GL.Vertex3 (x + offset.x, y, sizeMax + offset.z);
				}
			}

			GL.Color(mainColor);
			for (float y = sizeMin; y < sizeMax; y += mainIncrement) {
				for (float z = sizeMin; z < sizeMax; z += mainIncrement) {
					//GL.Color (mainColor * (1 - (Vector3.Distance(new Vector3(transform.position.x, y, z), transform.position) * fallOff)));
					GL.Vertex3 (sizeMin + offset.x, y + offset.y, z + offset.z); //x Lines
					GL.Vertex3 (sizeMax + offset.x, y + offset.y, z + offset.z);
				}
				
				for (float x = sizeMin; x < sizeMax; x += mainIncrement) {
					//GL.Color (mainColor * (1 - (Vector3.Distance(new Vector3(x, y, transform.position.z), transform.position) * fallOff)));
					GL.Vertex3 (x + offset.x, y + offset.y, sizeMin + offset.z); //x Lines
					GL.Vertex3 (x + offset.x, y + offset.y, sizeMax + offset.z);
				}
			}
			
			for (float z = sizeMin; z < sizeMax; z += mainIncrement) {
				for (float x = sizeMin; x < sizeMax; x += mainIncrement) {
					//GL.Color (mainColor * (1 - (Vector3.Distance(new Vector3(x, transform.position.y, z), transform.position) * fallOff)));
					GL.Vertex3 (x + offset.x, sizeMin + offset.y, z + offset.z);//y Lines
					GL.Vertex3 (x + offset.x, sizeMax + offset.y, z + offset.z);
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
