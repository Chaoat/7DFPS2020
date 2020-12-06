using System.Collections;
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
	private Rigidbody body;

	private Quaternion targetRotation;
	private Vector3 targetPosition;

	//private Vector3 lastMousePos;
	// Start is called before the first frame update
	void Start()
    {
		body = GetComponent<Rigidbody>();

		InitJoints();

		activeHand = leftHand;
		inactiveHand = rightHand;

		//lastMousePos = Input.mousePosition;
	}

	void InitJoints() {
		leftHand.armLength = 2f;
		leftHand.handSize = 0.1f;
		leftHand.restingPoint = new Vector3(-0.3f, -0.2f, 0.5f);
		leftHand.side = -1;

		rightHand.armLength = 2f;
		rightHand.handSize = 0.1f;
		rightHand.restingPoint = new Vector3(+0.3f, -0.2f, 0.5f);
		rightHand.side = 1;
	}

    // Update is called once per frame
    void Update()
    {
		body.angularVelocity = Vector3.zero;
		if (grabbing) {
			//Vector3 mouseChange = Input.mousePosition - lastMousePos;
			Vector2 mouseChange = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
			moveBody(mouseChange);

			// body.angularVelocity = Vector2.zero;

			transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 3*Time.deltaTime*Quaternion.Angle(transform.rotation, targetRotation));
			//transform.position = transform.position + (3*Time.deltaTime)*(targetPosition - transform.position);
			body.velocity = 2 * (targetPosition - transform.position);

			if (!Input.GetMouseButton(0)) {
				activeHand.releaseHold();
				inactiveHand.releaseHold();
				grabbing = false;
			}
		} else {
			activeHand = (Input.mousePosition.x < Screen.width / 2.0f) ? leftHand : rightHand;
			inactiveHand = (Input.mousePosition.x < Screen.width / 2.0f) ? rightHand : leftHand;
			checkHandMovement(activeHand);
		}

		inactiveHand.moveToPoint = true;
		inactiveHand.targetPoint = transform.position + inactiveHand.armLength * (inactiveHand.restingPoint.x * transform.right + inactiveHand.restingPoint.y * transform.up + inactiveHand.restingPoint.z * transform.forward);
		//lastMousePos = Input.mousePosition;
		leftHand.setVelocity(body.velocity);
		rightHand.setVelocity(body.velocity);
	}

	void checkHandMovement(HandScript hand) {
		Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
		RaycastHit[] hits = Physics.RaycastAll(ray);
		int chosenHit = -1;
		for (int i = 0; i < hits.Length; i++)
		{
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

		if (chosenHit != -1)
		{
			hand.targetPoint = hits[chosenHit].point;
			if ((hand.targetPoint - transform.position).magnitude > hand.armLength) {
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

	void moveBody(Vector3 mouseChange) {
		Vector3 change = 0.1f * (mouseChange[1] * -transform.forward + mouseChange[0] * -transform.right);

		targetPosition = targetPosition + change;
	}

	void checkGrab(HandScript hand, RaycastHit hit) {
		if (Vector3.Distance(hit.point, hand.transform.position) < hand.handSize) {
			grabbing = true;
			hand.grabHold(hit);

			Vector3 upVector = VectorFunctions.parallelVectorToNormal(transform.up, hit.normal);
			if (upVector == Vector3.zero) {
				print("shit");
				upVector = -hand.side * Vector3.Cross(transform.forward, hit.normal);
			}
			Vector3 forwardVector = hand.side * Vector3.Cross(upVector, hit.normal);

			// print(upVector.normalized);

			targetRotation = Quaternion.LookRotation(forwardVector, upVector);

			targetPosition = hit.point + 0.7f * hit.normal - 1f*forwardVector.normalized;

			//print(VectorFunctions.findVectorInDirection(new Vector3(-1, 0, 0), new Vector3(1, 0, 0)));
		} else {
			grabbing = false;
			hand.releaseHold();
		}
	}
}
