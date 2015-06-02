using UnityEngine;
using System.Collections;

public class PlayerCameraFollow : MonoBehaviour {
	public Transform target;
	public Vector3 CamOffset;
	[Range(0.0f, 1.0f)]
	public float smoothTime = 0.3F;
	private Vector3 velocity = Vector3.zero;
	NetworkView nView;

	// Use this for initialization
	void Start () {
		nView = GetComponent<NetworkView>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (nView.isMine) {
			Vector3 targetPosition = target.TransformPoint (-CamOffset);
			transform.position = Vector3.SmoothDamp (transform.position, targetPosition, ref velocity, smoothTime);
			//transform.LookAt (target.position);
		}
	}
}
