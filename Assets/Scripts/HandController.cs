using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Camera mainCam;
	public HandScript leftHand;

	private bool grabbing = false;
	private Rigidbody body;
	private Vector3 restingPosition;

	private Vector3 leftHandRestingPoint;

	//private Vector3 lastMousePos;
	// Start is called before the first frame update
	void Start()
    {
		body = GetComponent<Rigidbody>();

		InitJoints();
		//lastMousePos = Input.mousePosition;
	}

	void InitJoints() {
		ConfigurableJoint leftJoint = leftHand.GetComponent<ConfigurableJoint>();
		leftHand.armLength = leftJoint.linearLimit.limit;
		leftHand.shoulder = leftHand.transform.parent;
		leftHand.handSize = 0.1f;
		leftHand.restingPoint = new Vector3(-0.5f, 0.4f, 1f);
		leftHand.side = -1;
	}

    // Update is called once per frame
    void Update()
    {
		if (grabbing) {
			//Vector3 mouseChange = Input.mousePosition - lastMousePos;
			Vector2 mouseChange = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			moveBody(mouseChange);
			body.angularVelocity = Vector2.zero;

			if (!Input.GetMouseButton(0)) {
				leftHand.releaseHold();
				grabbing = false;
			}
		} else {
			RaycastHit[] hits = Physics.RaycastAll(mainCam.ScreenPointToRay(Input.mousePosition));
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
				moveHandToPoint(leftHand, correctHandFromArm(leftHand, hits[chosenHit].point));

				if (Input.GetMouseButton(0)) {
					checkGrab(leftHand, hits[chosenHit]);
				}
			}
		}
		//lastMousePos = Input.mousePosition;
	}

	Vector3 correctHandFromArm(HandScript hand, Vector3 prospectivePoint) {
		float distance = Vector3.Distance(prospectivePoint, hand.shoulder.position);
		if (distance < hand.armLength) {
			return prospectivePoint;
		} else {
			Ray ray = new Ray(hand.shoulder.position, (prospectivePoint - hand.shoulder.position).normalized);
			return ray.GetPoint(hand.armLength);
		}
	}

	void moveHandToPoint(HandScript hand, Vector3 point) {
		hand.transform.position = hand.transform.position + (3*Time.deltaTime)*(point - hand.transform.position);
	}

	void moveBody(Vector3 mouseChange) {
		Vector3 change = 0.1f * (mouseChange[1] * -transform.forward + mouseChange[0] * -transform.right);
		Vector3 newPos = transform.position + change;
		//body.AddForce(change, ForceMode.Impulse);
		body.velocity = 60*change;
		//body.MovePosition(newPos);
	}

	void checkGrab(HandScript hand, RaycastHit hit) {
		if (Vector3.Distance(hit.point, hand.transform.position) < hand.handSize) {
			grabbing = true;
			hand.grabHold(hit.rigidbody);
			
			//body.MoveRotation(Quaternion.LookRotation(-hit.normal));
			//restingPosition = hit.point - hand.restingPoint;
			//body.MovePosition(restingPosition);

			Vector3 vectorInLine = hit.normal*Vector3.Dot(hit.normal, transform.up);
			Vector3 upVector = transform.up - vectorInLine + calculateHandSideVector(hand);

			//if (upVector == Vector3.zero) {
			//	upVector = calculateHandSideVector(hand);
			//}

			//body.MoveRotation(Quaternion.LookRotation(transform.forward, upVector));
			transform.rotation = Quaternion.LookRotation(transform.forward, upVector);
			print(upVector);
		}
	}

	Vector3 calculateHandSideVector(HandScript hand) {
		return hand.side*transform.right*Vector3.Dot(hand.side * transform.right, hand.shoulder.position);
	}
}
