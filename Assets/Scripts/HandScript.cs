using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{
	public float armLength;
	public float handSize;
	public Vector3 restingPoint;
	public float side;

	public bool moveToPoint;
	public Vector3 targetPoint;

	private FixedJoint holdJoint;

	private bool grabbing;
	private Vector3 grabPos;

	private Rigidbody body;

	// Start is called before the first frame update
	void Start()
    {
		grabbing = false;

		body = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

	private void LateUpdate()
	{
		if (grabbing) {
			transform.position = grabPos;
		} else {
			if (moveToPoint) {
				transform.position = transform.position + (3 * Time.deltaTime) * (targetPoint - transform.position);
			}
		}
	}

	public bool grabHold(RaycastHit grabTarget) {
		if (Vector3.Distance(grabTarget.point, transform.position) <= handSize) {
			grabPos = grabTarget.point + grabTarget.normal*handSize;
			grabbing = true;
			return true;
		} else {
			return false;
		}
	}

	public void releaseHold() {
		grabbing = false;
	}
}
