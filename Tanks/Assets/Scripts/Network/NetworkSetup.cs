using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkSetup : MonoBehaviour {

	public Text connectionText;
	public Transform[] spawnPoints;
	public Camera Cam;
	bool isInLobby = false;
	GameObject player;
	public GameObject Dpad;

	public InputField username;

	public InputField roomName;
	public Slider playerCountSlider;
	public Toggle visible;

	public Toggle gun;
	public Toggle flame;
	public Toggle missile;
	public GameObject bg;

	public GameObject list;

	public string tankType;


	// Use this for initialization
	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("1.0");
		StartCoroutine (UpdateConnectionString());
	}
	
	// Update is called once per frame
	void Update () {
		if (connectionText.text == "Joined") {
			//print ("2");
			bg.GetComponent<Fadeout> ().close ();
			Dpad.SetActive (true);
		}
	}

	IEnumerator UpdateConnectionString () 
	{
		while(true)
		{
			connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
			yield return null;
		}
	
			
	}

	void OnReceivedRoomListUpdate()
	{
		list.GetComponent<GetRoomList> ().RoomList ();
	}

	public void createRoom()
	{
		RoomOptions ro = new RoomOptions (){ isVisible = visible.isOn, maxPlayers = (byte)playerCountSlider.value };
		PhotonNetwork.JoinOrCreateRoom (roomName.text, ro, TypedLobby.Default);
		if(gun.isOn)
			tankType="Minigun";
		if(flame.isOn)
			tankType="FlameTank";
		if(missile.isOn)
			tankType="MissileTank";
	}



	public void joinRoom(string joinRoomName)
	{
		PhotonNetwork.JoinRoom (joinRoomName);
		print ("go");
		if(gun.isOn)
			tankType="Minigun";
		if(flame.isOn)
			tankType="FlameTank";
		if(missile.isOn)
			tankType="MissileTank";
	}


	void createName()
	{
		if(username.text != "")
			PhotonNetwork.player.name = username.text; 
		else
			PhotonNetwork.player.name = "Default Player";
	}

	void OnJoinedLobby()
	{
		isInLobby = true;

	}
	void OnJoinedRoom()
	{
		StopCoroutine (UpdateConnectionString ());	
		StartSpawnProcess (0f);
	}

	void StartSpawnProcess (float respawnTime)
	{
		
		StartCoroutine ("SpawnPlayer", respawnTime);
	}

	IEnumerator SpawnPlayer(float respawnTime)
	{
		yield return new WaitForSeconds(respawnTime);

		int index = Random.Range (0, spawnPoints.Length);
		player = PhotonNetwork.Instantiate (tankType, 
			spawnPoints [index].position,
			spawnPoints [index].rotation,
			0);
		if(tankType == "MissileTank")
			player.GetComponent<MissileTankNetworkMover> ().RespawnMe += StartSpawnProcess;
		if(tankType == "FlameTank")
			player.GetComponent<FlameNetworkMover> ().RespawnMe += StartSpawnProcess;

	}
}
