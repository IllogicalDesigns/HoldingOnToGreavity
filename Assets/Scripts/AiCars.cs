using UnityEngine;
using System.Collections;

public class AiCars : MonoBehaviour {
	public Transform target;
	public AiNodeGraphs myNodeGraph;
	float directionOut = 0;
	public int myNextNode = 0;
	private float close2Node = 0;				//this gets set by the node network
	[Range(0f, 100.0f)]
	public float forwardMaxSpeed = 5.0f;
	[Range(0f, 100.0f)]
	public float turnMaxSpeed = 2.5f;
	[Range(0f, 0.05f)]
	public float forwardAcc = 5.0f;
	[Range(0f, 10.0f)]
	public float turnAcc = 5.0f;
	public ConstantForce2D myConstantForce2D;
	float currSpeed = 0.0f;
	float currTurnSpeed = 0.0f;
	Vector2 forwardSpeedVec;
	[Range(-1f,1f)]
	public float h;
	[Range(-1f,1f)]
	public float v;

	// Use this for initialization
	void Start () {
		myNextNode = 0;
		target = myNodeGraph.myNodes [myNextNode];
		close2Node = (myNodeGraph.detectionRange * myNodeGraph.detectionRange);
		v = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
		CalculateAiWheelSteering ();
	}
	void FixedUpdate () {
		currSpeed = Mathf.Lerp(currSpeed, forwardMaxSpeed * -v, forwardAcc);
		currTurnSpeed = Mathf.Lerp(currSpeed, turnMaxSpeed * h, turnAcc);
		forwardSpeedVec = new Vector2 (0, currSpeed);
		myConstantForce2D.relativeForce = forwardSpeedVec;
		myConstantForce2D.torque = currTurnSpeed * 50f;
	}
	void CalculateAiWheelSteering ()
	{
		directionOfFacing (myNodeGraph.myNodes [myNextNode]);
		float tempDirFloat = Turn2Facing (myNodeGraph.myNodes [myNextNode].position);//1 == facing, -1 == facing away
		h = 0;
		h = tempDirFloat;
		directionOut = tempDirFloat;
		if (h > 0.1f) 
			h = 1f;
		if (h < -0.1f) 
			h = -1f;
		h = Mathf.Clamp (h, -1, 1);
		if (HowCloseAmI (transform.position, target.position) < close2Node) {
			myNextNode ++;
			if (myNextNode > (myNodeGraph.myNodes.Count - 1))
				myNextNode = 0;
			target = myNodeGraph.myNodes [myNextNode];
		}
	}
	float directionOfFacing (Transform other)//1 == facing, -1 == facing away
	{
		Vector3 forward = transform.TransformDirection (Vector3.forward);
		Vector3 toOther = other.position - transform.position;
		directionOut = Vector3.Dot (forward.normalized, toOther.normalized);
		return Vector3.Dot (forward, toOther);
	}
	
	float Turn2Facing (Vector3 target)//suggested 0.5f to see if you are within 180 degresses
	{
		Vector3 dir = (target - transform.position).normalized;
		float direction = Vector3.Dot (dir, transform.forward);
		directionOut = direction;
		return direction;
	}
	
	private float HowCloseAmI (Vector3 pointA, Vector3 pointB)	//this is what you have to do to make this work
	{
		Vector3 offset = pointA - pointB;					//This is our offset from the target
		float sqrLen = offset.sqrMagnitude;					//this does a love function i cant describe to you
		return sqrLen;										//this gives you the product of that sweet love function
	}
}
