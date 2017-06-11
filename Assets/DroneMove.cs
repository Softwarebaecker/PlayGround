using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMove : MonoBehaviour {

	Rigidbody drone;
	// Use this for initialization
	void Start () {
		drone = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate()
	{
		MovementUpDown ();
		MovementForward ();
		Rotation ();
		ClampingSpeedValue ();
		Swerwe ();

		drone.AddRelativeForce (Vector3.up * upForce);
		drone.rotation = Quaternion.Euler (
			new Vector3(tiltAmountForward , currentYRotation, tiltAmmountSideways)
		);
	}

	public float upForce;
	void MovementUpDown()
	{
		if (Mathf.Abs (Input.GetAxis ("Vertical")) > 0.2f || Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.2f) {
			if (Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.LeftShift)) {
				drone.velocity = drone.velocity;
			}
			if (Input.GetKey (KeyCode.Space) && Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.Q) || !Input.GetKey (KeyCode.E)) {
				drone.velocity = new Vector3(drone.velocity.x, Mathf.Lerp(drone.velocity.y, 0, Time.deltaTime * 5), drone.velocity.z);
				upForce = 281;
			}
			if (!Input.GetKey (KeyCode.Space) && !Input.GetKey (KeyCode.LeftShift) && Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.E)) {
				drone.velocity = new Vector3(drone.velocity.x, Mathf.Lerp(drone.velocity.y, 0, Time.deltaTime * 5), drone.velocity.z);
				upForce = 110;
			}
			if (Input.GetKey (KeyCode.Q) || Input.GetKey (KeyCode.E)) {
				upForce = 410;
			}
					
		}
		if (Mathf.Abs (Input.GetAxis ("Vertical")) < 0.2f || Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.2f) {
			upForce = 135;
		}

		if (Input.GetKey (KeyCode.Space)) {
			upForce = 450;
			if (Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.2f) {
				upForce = 500;
			}
		} else if (Input.GetKey (KeyCode.LeftShift)) {
			upForce = -200;
		} else if (!Input.GetKey (KeyCode.Space) && !Input.GetKey (KeyCode.LeftShift) && Mathf.Abs (Input.GetAxis ("Vertical")) < 0.2f && Mathf.Abs (Input.GetAxis ("Horizontal")) < 0.2f ) {
			upForce = 98.1f;
		}
	}

	private float movementForwardSpeed = 500.0f;
	private float tiltAmountForward = 0;
	private float titltVelocityForward;
	void MovementForward()
	{
		if (Input.GetAxis ("Vertical") != 0) {
			drone.AddRelativeForce (Vector3.forward * Input.GetAxis ("Vertical") * movementForwardSpeed);
			tiltAmountForward = Mathf.SmoothDamp (tiltAmountForward, 20 * Input.GetAxis ("Vertical"), ref titltVelocityForward, 0.1f);
		}
	}

	private float wantedYRotation;
	private float currentYRotation;
	private float rotateAmountByKeys = 2.5f;
	private float rotationYVelocity;
	void Rotation ()
	{
		if (Input.GetKey (KeyCode.Q)) {
			wantedYRotation -= rotateAmountByKeys;
		}
		if (Input.GetKey (KeyCode.E)) {
			wantedYRotation += rotateAmountByKeys;
		}

		currentYRotation = Mathf.SmoothDamp (currentYRotation, wantedYRotation, ref rotationYVelocity, 0.25f);

	}

	private Vector3 velocityToSmoothDampToZero;
	void ClampingSpeedValue()
	{
		if (Mathf.Abs (Input.GetAxis ("Vertical")) > 0.2f && Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.2f) {
			drone.velocity = Vector3.ClampMagnitude (drone.velocity, Mathf.Lerp (drone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
		}
		if (Mathf.Abs (Input.GetAxis ("Vertical")) > 0.2f && Mathf.Abs (Input.GetAxis ("Horizontal")) < 0.2f) {
			drone.velocity = Vector3.ClampMagnitude (drone.velocity, Mathf.Lerp (drone.velocity.magnitude, 10.0f, Time.deltaTime * 5f));
		}
		if (Mathf.Abs (Input.GetAxis ("Vertical")) < 0.2f && Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.2f) {
			drone.velocity = Vector3.ClampMagnitude (drone.velocity, Mathf.Lerp (drone.velocity.magnitude, 5.0f, Time.deltaTime * 5f));
		}
		if (Mathf.Abs (Input.GetAxis ("Vertical")) < 0.2f && Mathf.Abs (Input.GetAxis ("Horizontal")) < 0.2f) {
			drone.velocity = Vector3.SmoothDamp (drone.velocity, Vector3.zero, ref velocityToSmoothDampToZero, 0.95f);
		}
	}

	private float sideMovementAmount = 300.0f;
	private float tiltAmmountSideways;
	private float tiltAmmountVelocity;
	void Swerwe()
	{
		if (Mathf.Abs (Input.GetAxis ("Horizontal")) > 0.2f) {
			drone.AddRelativeForce (Vector3.right * Input.GetAxis ("Horizontal") * sideMovementAmount);
			tiltAmmountSideways = Mathf.SmoothDamp (tiltAmmountSideways, -20 * Input.GetAxis ("Horizontal"), ref tiltAmmountVelocity, 0.1f);
		} else {
			tiltAmmountSideways = Mathf.SmoothDamp (tiltAmmountSideways, 0, ref tiltAmmountVelocity, 0.1f);
		}
	}

}
