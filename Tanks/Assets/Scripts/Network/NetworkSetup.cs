using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NetworkSetup : MonoBehaviour {

	public Text connectionText;
	public Transform[] spawnPoints;
	public Camera Cam;
	GameObject player;
	// Use this for initialization
	void Start () {
		PhotonNetwork.logLevel = PhotonLogLevel.Full;
		PhotonNetwork.ConnectUsingSettings ("1.0");
	}
	
	// Update is called once per frame
	void Update () {
		connectionText.text = PhotonNetwork.connectionStateDetailed.ToString ();
	}


	void OnJoinedLobby()
	{
		RoomOptions ro = new RoomOptions (){isVisible = true, maxPlayers = 10};PhotonNetwork.JoinOrCreateRoom ("Mike", ro, TypedLobby.Default);
	}
	void OnJoinedRoom()
	{
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
		player = PhotonNetwork.Instantiate ("Tank", 
			spawnPoints [index].position,
			spawnPoints [index].rotation,
			0);
		player.GetComponent<TankNetworkMover> ().RespawnMe += StartSpawnProcess;

	}
}
