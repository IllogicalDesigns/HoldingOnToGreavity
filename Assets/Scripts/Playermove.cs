using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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
	NetworkView nView;
	public AudioClip[] impactScounds;
	public bool raceStarted = false;
	public bool checkpoint1 = false;
	public bool checkpoint2 = false;
	public int currentLap = 0;
	public Text lapCounter;
	Vector3 origPos;

	void OnCollisionEnter2D() {
		int tmpInt = Random.Range (0, impactScounds.Length);
		AudioSource.PlayClipAtPoint (impactScounds[tmpInt], new Vector3 (5, 1, 2), MainMenu.myVolume);
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == "CheckPoint") {
			if(col.name == "Checkpoint1"){
				checkpoint1 = true;
			}
			if(col.name == "Checkpoint2" && checkpoint1){
				checkpoint2 = true;
			}
		}
		if (col.tag == "StartGate") {
			if(checkpoint1 && checkpoint2){
				currentLap ++;
				checkpoint1 = false;
				checkpoint2 = false;
				lapCounter.text = currentLap.ToString();
			}
		}
	}

	// Use this for initialization
	void Start () {
		nView = GetComponent<NetworkView>();
		lapCounter.text = currentLap.ToString();
		origPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (nView.isMine && raceStarted) {
			v = Input.GetAxis ("Vertical");
			h = Input.GetAxis ("Horizontal");
		}
		if (currentLap > 4) {
			lapCounter.text = "Win";
			nView.RPC("ILose", RPCMode.Others);
			StartCoroutine(IWinWait());
			nView.RPC("RestartGui", RPCMode.Server);
		}
	}
	void FixedUpdate () {
		if (nView.isMine && raceStarted) {
			currSpeed = Mathf.Lerp (currSpeed, forwardMaxSpeed * v, forwardAcc);
			currTurnSpeed = Mathf.Lerp (currSpeed, turnMaxSpeed * -h, turnAcc);
			forwardSpeedVec = new Vector2 (0, currSpeed);
			myConstantForce2D.relativeForce = forwardSpeedVec;
			myConstantForce2D.torque = currTurnSpeed * 50f;
		}
	}
	IEnumerator IWinWait (){
		yield return new WaitForSeconds (2f);
		raceStarted = false;
		currentLap = 0;
		checkpoint1 = false;
		checkpoint2 = false;
		transform.position = origPos;
	}
	IEnumerator IloseWait (){
		yield return new WaitForSeconds (2f);
		transform.position = origPos;
		nView.RPC("RestartGui", RPCMode.Server);
	}
		[RPC]
		public void ILose ()
		{
		raceStarted = false;
		currentLap = 0;
		checkpoint1 = false;
		checkpoint2 = false;
		}
}
