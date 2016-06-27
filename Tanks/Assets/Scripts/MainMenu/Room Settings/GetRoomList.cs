using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GetRoomList : MonoBehaviour {

	 Text[] text;
	public GameObject joinRoomInfo;
	// Use this for initialization
	void Start () {
	
	}

	public void RoomList ()
	{
		print ("2");
		RoomInfo[] rooms = PhotonNetwork.GetRoomList ();
		foreach (RoomInfo room in rooms) {
			GameObject a = Instantiate (joinRoomInfo, transform.position, transform.rotation)as GameObject;
			a.transform.parent = transform;
			a.transform.localScale = new Vector3 (1, 1, 1);

			text = a.GetComponentsInChildren<Text>();
			foreach (Text tex in text) {
				if (tex.gameObject.name == "RoomName") 
					tex.text = room.name;
				if (tex.gameObject.name == "Players")
					tex.text = room.playerCount + "/" + room.maxPlayers;
			}
				
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
