using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constantTiling : MonoBehaviour
{
	// Start is called before the first frame update
    void Start()
    {
		float longest = 0;
		float secondLongset = 0;
		for (int i = 0; i<3; i++) {
			float n = transform.localScale[i];
			if (n >= longest) {
				secondLongset = longest;
				longest = n;
			} else if (n >= secondLongset) {
				secondLongset = n;
			}
		}
		
		//print(longest + ":" + secondLongset);
        GetComponent<Renderer>().material.mainTextureScale = new Vector2(longest, secondLongset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
