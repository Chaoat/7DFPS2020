using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorSpriteAtlas : MonoBehaviour
{
	// Start is called before the first frame update
    void Start()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;

		Vector2[] UVs = new Vector2[24];

		float boundary = 1 - 3/18;

		// Front
		UVs[0] = new Vector2(0.0f, 0.0f);
		UVs[1] = new Vector2(1, 0.0f);
		UVs[2] = new Vector2(0.0f, 1);
		UVs[3] = new Vector2(1, 1);

		// Top
		UVs[4] = new Vector2(0.0f, 1);
		UVs[5] = new Vector2(1, 1);
		UVs[8] = new Vector2(0.0f, 0);
		UVs[9] = new Vector2(1, 0);

		// Back
		UVs[6] = new Vector2(1.0f, 0.0f);
		UVs[7] = new Vector2(0, 0.0f);
		UVs[10] = new Vector2(1.0f, 1);
		UVs[11] = new Vector2(0, 1);

		// Bottom
		UVs[12] = new Vector2(1f, 0);
		UVs[13] = new Vector2(0, 0);
		UVs[14] = new Vector2(0, 0.166f);
		UVs[15] = new Vector2(1f, 0.166f);

		// Left
		UVs[16] = new Vector2(0, 0.166f);
		UVs[17] = new Vector2(0, 1);
		UVs[18] = new Vector2(1, 1);
		UVs[19] = new Vector2(1, 0.166f);

		// Right        
		UVs[20] = new Vector2(0, 0.166f);
		UVs[21] = new Vector2(0, 1);
		UVs[22] = new Vector2(1.0f, 1);
		UVs[23] = new Vector2(1.0f, 0.166f);

		mesh.uv = UVs;
	}
}
