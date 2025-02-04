﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Camera mainCam;
	public HandScript leftHand;
	public HandScript rightHand;

	private HandScript activeHand;
	private HandScript inactiveHand;

	private bool grabbing = false;
	private bool freelooking = false;
	private Rigidbody body;
	private bool lockMouse = false;

	private Quaternion targetRotation;
	private Vector3 targetPosition;

	public AudioClip grabClip;
	public AudioClip releaseClip;

	private AudioSource aSource;

	//private Vector3 lastMousePos;
	// Start is called before the first frame update
	void Start()
    {
		body = GetComponent<Rigidbody>();
		aSource = GetComponent<AudioSource>();

		InitJoints();

		activeHand = leftHand;
		inactiveHand = rightHand;

		//lastMousePos = Input.mousePosition;
	}

	void InitJoints() {
		float armLength = 1.2f;
		float handSize = 0.05f;
		
		leftHand.armLength = armLength;
		leftHand.handSize = handSize;
		leftHand.restingPoint = new Vector3(-0.3f, -0.2f, 0.5f);
		leftHand.side = -1;

		rightHand.armLength = armLength;
		rightHand.handSize = handSize;
		rightHand.restingPoint = new Vector3(+0.3f, -0.2f, 0.5f);
		rightHand.side = 1;
	}

    // Update is called once per frame
    void Update()
    {
		body.angularVelocity = Vector3.zero;

		Vector2 mouseChange = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

		lockMouse = false;

		if (Input.GetMouseButton(1))
		{
			//targetRotation = transform.localRotation*Quaternion.Euler(-(Input.mousePosition.y - Screen.height/2.0f)/5.0f, 0.5f*(Input.mousePosition.x - Screen.width/2.0f)/5.0f, 0);
			//transform.localRotation = Quaternion.RotateTowards(transform.localRotation, targetRotation, 3*Time.deltaTime*Quaternion.Angle(transform.localRotation, targetRotation));
			transform.localRotation = transform.localRotation * Quaternion.Euler(-12 * mouseChange.y, 12 * mouseChange.x, 0);
			freelooking = true;
			lockMouse = true;
		} else {
			freelooking = false;
		}

		if (grabbing) {
			//Vector3 mouseChange = Input.mousePosition - lastMousePos;
			// body.angularVelocity = Vector2.zero;

			if (!freelooking) {
				moveBody(mouseChange, activeHand);
				transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3*Time.deltaTime*Quaternion.Angle(transform.rotation, targetRotation));
			}

			//transform.position = transform.position + (3*Time.deltaTime)*(targetPosition - transform.position);
			body.velocity = 2 * (targetPosition - transform.position);

			lockMouse = true;

			if (!Input.GetMouseButton(0)) {
				activeHand.releaseHold();
				inactiveHand.releaseHold();
				grabbing = false;

				aSource.PlayOneShot(releaseClip);
			}
		} else {
			if (Input.mousePosition.x < 0.45*Screen.width) {
				activeHand = leftHand;
				inactiveHand = rightHand;
			} else if (Input.mousePosition.x > 0.55*Screen.width) {
				activeHand = rightHand;
				inactiveHand = leftHand;
			}
			
			checkHandMovement(activeHand);
		}

		inactiveHand.moveToPoint = true;
		inactiveHand.targetPoint = transform.position + inactiveHand.armLength * (inactiveHand.restingPoint.x * transform.right + inactiveHand.restingPoint.y * transform.up + inactiveHand.restingPoint.z * transform.forward);
		//lastMousePos = Input.mousePosition;
		//leftHand.setVelocity(body.velocity);
		//rightHand.setVelocity(body.velocity);
	}

	void checkHandMovement(HandScript hand) {
		Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll(ray);
		int chosenHit = -1;
		for (int i = 0; i < hits.Length; i++)
		{
			if (!hits[i].collider.isTrigger) {
				if (chosenHit == -1)
				{
					chosenHit = i;
				}
				else
				{
					if (hits[i].distance < hits[chosenHit].distance)
					{
						chosenHit = i;
					}
				}
			}
		}

		if (chosenHit != -1)
		{
			hand.targetPoint = hits[chosenHit].point;
			if (hits[chosenHit].distance > hand.armLength) {
				hand.targetPoint = ray.GetPoint(hand.armLength);
			}
			hand.moveToPoint = true;

			if (Input.GetMouseButton(0))
			{
				checkGrab(hand, hits[chosenHit]);
			}
		}
		else
		{
			hand.targetPoint = ray.GetPoint(0.8f*hand.armLength);
			hand.moveToPoint = true;
		}
	}

	void moveBody(Vector3 mouseChange, HandScript handAnchor) {
		Vector3 change = 0.1f * (mouseChange[1] * -transform.forward + mouseChange[0] * -transform.right);

		Vector3 newPosition = targetPosition + change;

		if ((newPosition - handAnchor.transform.position).magnitude <= handAnchor.armLength) {
			targetPosition = newPosition;
		}
	}

	void checkGrab(HandScript hand, RaycastHit hit) {
		if (hit.distance < hand.armLength) {
			aSource.PlayOneShot(grabClip);

			grabbing = true;
			hand.grabHold(hit);

			Vector3 upVector = VectorFunctions.parallelVectorToNormal(transform.up, hit.normal);
			if (upVector == Vector3.zero) {
				upVector = -hand.side * Vector3.Cross(transform.forward, hit.normal);
				print("altered");
			}
			Vector3 forwardVector = hand.side * Vector3.Cross(upVector, hit.normal);

			//print("normal: " + hit.normal);
			//print("upVector: " + upVector);

			// print(upVector.normalized);

			targetRotation = Quaternion.LookRotation(forwardVector, upVector);

			//targetPosition = hit.point + 0.7f * hit.normal - 1f*forwardVector.normalized;
			targetPosition = hit.point - hand.restingPoint.x * hit.normal - hand.restingPoint.y * upVector.normalized - hand.restingPoint.z * forwardVector.normalized;

			//print(VectorFunctions.findVectorInDirection(new Vector3(-1, 0, 0), new Vector3(1, 0, 0)));
		} else {
			grabbing = false;
			hand.releaseHold();
		}
	}
}
