using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {
	public GameObject main;
	public GameObject host;
	public GameObject join;
	public Text gName;
	public Text numberOfPlayers;
	public string typeName = "UniqueGameName";
	private string gameName = "RoomName";

	// Use this for initialization
	void Start () {
		main.SetActive(true);
		host.SetActive(false);
		join.SetActive(false);
	}
	public void SwitchToHost () {
		main.SetActive(false);
		host.SetActive(true);
		join.SetActive(false);
	}

	void OnServerInitialized()
	{
		Debug.Log("Server Initializied");
	}

	
	public void StartServer()
	{
		typeName = gName.ToString ();
		Network.InitializeServer(4 , 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, gameName);
		Debug.Log (typeName.ToString ());
	}
}
