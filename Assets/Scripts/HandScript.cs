using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
    public Transform shoulder;
	public float armLength;
	public float handSize;
	public Vector3 restingPoint;
	public float side;

	private FixedJoint holdJoint;

	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void grabHold(Rigidbody grabTarget) {
		//holdJoint.connectedBody = grabTarget;
		holdJoint = gameObject.AddComponent<FixedJoint>();
		holdJoint.connectedBody = grabTarget;
		print("Grab");
	}

	public void releaseHold() {
		Destroy(holdJoint);
	}
}
