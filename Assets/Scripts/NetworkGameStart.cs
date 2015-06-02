using UnityEngine;
using System.Collections;

public class NetworkGameStart : MonoBehaviour {
	public bool isThisMine = true;
	public NetworkView nView;
	public GameObject camHolder;
	public Canvas myCan;

	// Use this for initialization
	void Start () {
		nView = GetComponent<NetworkView>();
		if (!nView.isMine) {
			isThisMine = false;
			camHolder.SetActive(false);
			myCan.gameObject.SetActive(false);
		}
	}
}
