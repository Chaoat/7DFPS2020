﻿using System.Collections;
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
	private Vector3 rayHit;

	//private Rigidbody body;

	// Start is called before the first frame update
	void Start()
    {
		grabbing = false;

		//body = GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
		// GetComponent<Rigidbody>().velocity = Vector3.zero;

		if (grabbing) {
			Quaternion targetRotation = Quaternion.LookRotation(rayHit - transform.position, transform.parent.forward);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3 * Time.deltaTime * Quaternion.Angle(transform.rotation, targetRotation));
		} else {
			Quaternion targetRotation = Quaternion.LookRotation(transform.parent.forward, transform.parent.up);
			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3 * Time.deltaTime * Quaternion.Angle(transform.rotation, targetRotation));
		}
	}

	private void LateUpdate()
	{
		if (grabbing) {
			transform.position = grabPos;
		} else {
			if (moveToPoint) {
				transform.position = transform.position + (6 * Time.deltaTime) * (targetPoint - transform.position);
			}
		}
	}

	public void grabHold(RaycastHit grabTarget) {
		rayHit = grabTarget.point;
		grabPos = grabTarget.point + grabTarget.normal*handSize;
		grabbing = true;
	}

	public void releaseHold() {
		grabbing = false;
	}

	//public void setVelocity(Vector3 velocity) {
	//	GetComponent<Rigidbody>().velocity = velocity;
	//}
}
