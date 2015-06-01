using UnityEngine;
using System.Collections;

[RequireComponent (typeof (ConstantForce2D))]
public class Playermove : MonoBehaviour {
	[Range(0f, 100.0f)]
	public float forwardMaxSpeed = 5.0f;
	[Range(0f, 100.0f)]
	public float turnMaxSpeed = 2.5f;
	[Range(0f, 0.05f)]
	public float forwardAcc = 5.0f;
	[Range(0f, 10.0f)]
	public float turnAcc = 5.0f;
	public ConstantForce2D myConstantForce2D;
	public float currSpeed = 0.0f;
	float currTurnSpeed = 0.0f;
	float v = 0f;
	float h = 0f;
	Vector2 forwardSpeedVec;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		v = Input.GetAxis ("Vertical");
		h = Input.GetAxis ("Horizontal");
	}
	void FixedUpdate () {
		currSpeed = Mathf.Lerp(currSpeed, forwardMaxSpeed * v, forwardAcc);
		currTurnSpeed = Mathf.Lerp(currSpeed, turnMaxSpeed * -h, turnAcc);
		forwardSpeedVec = new Vector2 (0, currSpeed);
		myConstantForce2D.relativeForce = forwardSpeedVec;
		myConstantForce2D.torque = currTurnSpeed * 50f;
	}
}
