// Copyright (C) 2015 Duncan Freeman
using UnityEngine;

public class GridOptimized : MonoBehaviour {
	
	public Color mainColor = new Color(1f,1f,1f,1f);
	public Color subColor = new Color(1f,1f,1f,1f);
	public float mainIncrement = 1000f;
	public float subIncrement = 100f;
	public bool subGrid = true;
	public float sizeMin = -5000f;
	public float sizeMax = 5000f;
	public bool showGrid = true;
	public Vector3 offset = Vector3.zero;
	float y = 0; //Floor
	
	void OnPostRender() {
		if (showGrid) {
			CreateLineMaterial ();
            GL.PushMatrix();
            lineMaterial.SetPass (0);

			offset = new Vector3(((float)(int)(transform.position.x / mainIncrement))* mainIncrement, ((float)(int)(transform.position.y / mainIncrement))* mainIncrement, ((float)(int)(transform.position.z / mainIncrement))* mainIncrement);

			GL.Begin (GL.LINES);

			GL.Color(subColor);
			if (subGrid) {
				for (float z = sizeMin; z < sizeMax; z += subIncrement) {
					GL.Vertex3 (sizeMin + offset.x, y, z + offset.z); //x Lines
					GL.Vertex3 (sizeMax + offset.x, y, z + offset.z);
				}
				
				for (float x = sizeMin; x < sizeMax; x += subIncrement) {
					GL.Vertex3 (x + offset.x, y, sizeMin + offset.z); //x Lines
					GL.Vertex3 (x + offset.x, y, sizeMax + offset.z);
				}
			}

			GL.Color(mainColor);
			for (float y = sizeMin; y < sizeMax; y += mainIncrement) {
				for (float z = sizeMin; z < sizeMax; z += mainIncrement) {
					GL.Vertex3 (sizeMin + offset.x, y + offset.y, z + offset.z); //x Lines
					GL.Vertex3 (sizeMax + offset.x, y + offset.y, z + offset.z);
				}
				
				for (float x = sizeMin; x < sizeMax; x += mainIncrement) {
					GL.Vertex3 (x + offset.x, y + offset.y, sizeMin + offset.z); //x Lines
					GL.Vertex3 (x + offset.x, y + offset.y, sizeMax + offset.z);
				}
			}
			
			for (float z = sizeMin; z < sizeMax; z += mainIncrement) {
				for (float x = sizeMin; x < sizeMax; x += mainIncrement) {
					GL.Vertex3 (x + offset.x, sizeMin + offset.y, z + offset.z);//y Lines
					GL.Vertex3 (x + offset.x, sizeMax + offset.y, z + offset.z);
				}
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

    public void showLineGrid (bool input) {
		showGrid = input;
	}
	
	
}
