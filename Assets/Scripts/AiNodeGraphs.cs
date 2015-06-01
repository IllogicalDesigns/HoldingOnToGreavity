using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AiNodeGraphs : MonoBehaviour {
	public List<Transform> myNodes = new List<Transform> ();
	public bool forceShowNodes = false;
	public bool updateRotation = false;
	public bool isCircle = false;
	public float detectionRange = 20f;
	private int myNodeCount = 0;
	private int mySpeedCount = 0;

	void OnDrawGizmosSelected ()
	{
		if (!forceShowNodes) {
			for (int i = 0; i < myNodes.Count; i++) {
				if (myNodes [i] != null) {
					Gizmos.color = Color.red;
					Gizmos.DrawWireSphere (myNodes [i].position, detectionRange);
					int tempLastCubeIndex = i;
					tempLastCubeIndex = tempLastCubeIndex - 1;
					if (tempLastCubeIndex < 0)
						tempLastCubeIndex = (myNodes.Count - 1);
					if (tempLastCubeIndex > myNodes.Count)
						tempLastCubeIndex = 0;
					if (isCircle) {
						Gizmos.color = Color.blue;
						Gizmos.DrawLine (myNodes [tempLastCubeIndex].position, myNodes [i].position);
					}
					if (updateRotation == true) {
						Gizmos.color = Color.green;
						Vector3 direction = transform.TransformDirection (-myNodes [i].right) * 5f;
						Gizmos.DrawRay (myNodes [i].position, direction);
					}
				}
			}
		}
	}
	
	void OnDrawGizmos ()
	{
		if (forceShowNodes) {
			for (int i = 0; i < myNodes.Count; i++) {
				if (myNodes [i] != null) {
					Gizmos.color = Color.red;
					Gizmos.DrawWireSphere (myNodes [i].position, detectionRange);
					int tempLastCubeIndex = i;
					tempLastCubeIndex = tempLastCubeIndex - 1;
					if (tempLastCubeIndex < 0)
						tempLastCubeIndex = (myNodes.Count - 1);
					if (tempLastCubeIndex > myNodes.Count)
						tempLastCubeIndex = 0;
					if (isCircle) {
						Gizmos.color = Color.blue;
						Gizmos.DrawLine (myNodes [tempLastCubeIndex].position, myNodes [i].position);
					}
					if (updateRotation == true) {
						Gizmos.color = Color.green;
						Vector3 direction = transform.TransformDirection (-myNodes [i].right) * 5f;
						Gizmos.DrawRay (myNodes [i].position, direction);
					}
				}
			}
		}
	}
	// Use this for initialization
	void Start () {
	
	}
	

	public Transform GetClosestWaypoint (Vector3 testFrom)
	{
		Transform tMin = null;
		float minDist = Mathf.Infinity;
		foreach (Transform t in myNodes) {
			float dist = Vector3.Distance (t.position, testFrom);
			if (dist < minDist) {
				tMin = t;
				minDist = dist;
			}
		}
		return tMin;
	}
}
