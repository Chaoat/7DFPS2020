using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float oxygenLeft;
	public float oxygenMax = 200;

	public RectTransform tankLeft;

	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        oxygenLeft = oxygenLeft - Time.deltaTime;

		tankLeft.localScale = new Vector2(1, oxygenLeft/oxygenMax);
    }
}
