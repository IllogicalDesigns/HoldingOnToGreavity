using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public GameObject main;
	public GameObject host;
	public GameObject join;
	public Text gName;
	public Text numberOfPlayers;
	public Text debugText;
	public Button startRace;
	public string typeName = "holdingGravity";
	private string gameName = "TestRoom";
	private HostData[] hostList;
	private bool onJoin = false;
	public GameObject prefabCar;
	public static float myVolume = 1.0f;
	public AudioClip[] clips;	//0 = beep //1 = boop
	public NetworkView nView;

	
	// Use this for initialization
	void Start ()
	{
		main.SetActive (true);
		host.SetActive (false);
		join.SetActive (false);
		debugText.gameObject.SetActive (false);
		onJoin = false;
		startRace.gameObject.SetActive (false);
		nView = GetComponent<NetworkView>();
	}

	public void SwitchToHost ()
	{
		main.SetActive (false);
		host.SetActive (true);
		join.SetActive (false);
		debugText.gameObject.SetActive (false);
		onJoin = false;
	}

	public void SwitchToMain ()
	{
		main.SetActive (true);
		host.SetActive (false);
		join.SetActive (false);
		debugText.gameObject.SetActive (false);
		onJoin = false;
	}

	public void SwitchToJoin ()
	{
		main.SetActive (false);
		host.SetActive (false);
		join.SetActive (true);
		debugText.gameObject.SetActive (false);
		onJoin = true;
	}
	public void sendRpc () {
		nView.RPC("startCountDown", RPCMode.Others);
	}
	[RPC]
	public void startCountDown ()
	{
		StartCoroutine (CountDown (3));
		startRace.gameObject.SetActive (false);
	}

	public void StartRace ()
	{
		GameObject[] racers;
		racers = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject player in racers) {
			Playermove tempPlay = player.GetComponent<Playermove> ();
			tempPlay.raceStarted = true;
		}
	}

	public void StartGame ()
	{
		if (Network.isServer) {
			debugText.gameObject.SetActive (false);
			debugText.text = ("Is server? :" + Network.isServer.ToString () + " ");
			main.SetActive (false);
			host.SetActive (false);
			join.SetActive (false);
			debugText.gameObject.SetActive (false);
			onJoin = false;
			startRace.gameObject.SetActive (true);
		}

		if (Network.isClient) {
			debugText.gameObject.SetActive (false);
			debugText.text = ("Is server? :" + Network.isServer.ToString () + " ");
			main.SetActive (false);
			host.SetActive (false);
			join.SetActive (false);
			debugText.gameObject.SetActive (false);
			onJoin = false;
		}
	}

	void OnServerInitialized ()
	{
		Debug.Log ("Server Initializied");
		StartGame ();
		SpawnPlayer ();
	}
	
	public void StartServer ()
	{
		gameName = gName.text;
		bool useNat = !Network.HavePublicAddress ();
		Network.InitializeServer (32, 25000, useNat);
		MasterServer.RegisterHost (typeName, gameName);
		Debug.Log ("Is server? :" + Network.isServer.ToString ());
	}

	void OnGUI ()
	{
		if (!Network.isClient && !Network.isServer && onJoin) {
			//if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
			//StartServer();
			
			//if (GUI.Button(new Rect(100, 250, 250, 100), "Refresh Hosts"))
			//RefreshHostList();
			
			if (hostList != null) {
				for (int i = 0; i < hostList.Length; i++) {
					if (GUI.Button (new Rect (400, 100 + (110 * i), 300, 100), hostList [i].gameName)) {
						JoinServer (hostList [i]);
						StartGame ();
					}
				}
			}
		}
	}

	public void JoinServer (HostData hostData)
	{
		Network.Connect (hostData);
	}
	
	void OnConnectedToServer ()
	{
		Debug.Log ("Server Joined");
		StartGame ();
		SpawnPlayer ();
	}

	public void RefreshHostList ()
	{
		MasterServer.RequestHostList (typeName);
	}
	
	void OnMasterServerEvent (MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList ();
	}

	public static bool IsOdd (int value)
	{
		return value % 2 != 0;
	}
	
	private void SpawnPlayer ()
	{
		Vector3 spawnPos = Vector3.zero;
		if (IsOdd (Network.connections.Length))
			spawnPos = new Vector3 (49 - (Network.connections.Length * 13), 10, 0);
		else
			spawnPos = new Vector3 (49 - (Network.connections.Length * 13), 0, 0);

		Network.Instantiate (prefabCar, spawnPos, Quaternion.identity, 0);
	}

	IEnumerator CountDown (int time2Start)
	{
		debugText.text = time2Start.ToString ();
		debugText.gameObject.SetActive (true);
		AudioSource.PlayClipAtPoint (clips[1], Vector3.one, MainMenu.myVolume);
		while (true) {
			time2Start --;
			if (debugText.text != "GO!") {
				yield return new WaitForSeconds (1f);
				if (time2Start < 1){
					debugText.text = "GO!";
					AudioSource.PlayClipAtPoint (clips[0], Vector3.one, MainMenu.myVolume);
				}
				else{
					debugText.text = time2Start.ToString ();
					AudioSource.PlayClipAtPoint (clips[1], Vector3.one, MainMenu.myVolume);
				}
			} else {
				StartRace ();
				yield return new WaitForSeconds (1f);
				debugText.gameObject.SetActive (false);
				break;
			}
		}
	}
		[RPC]
		public void RestartGui (){
		StartGame ();
		Debug.Log ("restrating game");
	}
}
