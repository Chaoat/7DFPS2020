using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Camera mainCam;
	public HandScript leftHand;
	public float armLength;

	private bool grabbing = false;

	// Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetMouseButton(0)) {
			grabbing = true;
		} else {
			grabbing = false;
		}

		if (grabbing) {
			
		} else {
			Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] hits = Physics.RaycastAll(ray);
			int chosenHit = -1;
			for (int i = 0; i<hits.Length; i++) {
				if (chosenHit == -1) {
					chosenHit = i;
				} else {
					if (hits[i].distance < hits[chosenHit].distance) {
						chosenHit = i;
					}
				}
			}

			if (chosenHit != -1) {
				moveHandToPoint(leftHand, ray.GetPoint(Mathf.Min(hits[chosenHit].distance, armLength)));
			}
		}
    }

	void moveHandToPoint(HandScript hand, Vector3 point) {
		hand.transform.position = hand.transform.position + (3*Time.deltaTime)*(point - hand.transform.position);
	}
}
