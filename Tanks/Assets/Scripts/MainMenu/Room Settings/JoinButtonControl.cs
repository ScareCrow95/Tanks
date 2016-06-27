using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class JoinButtonControl : MonoBehaviour {

	Button bt;
	public GameObject NetworkManager;
	 Text joinRoomName;

	 NetworkSetup ns;
	// Use this for initialization
	void Start () {
		bt = GetComponent<Button> ();
		bt.onClick.AddListener (EnterRoom);
		NetworkManager = GameObject.FindGameObjectWithTag ("NM");
		ns = NetworkManager.GetComponent<NetworkSetup> ();
	}
	
	// Update is called once per frame
	void EnterRoom () {
				joinRoomName = bt.gameObject.transform.parent.gameObject.GetComponentInChildren<Text> ();

				ns.joinRoom (joinRoomName.text);
	}
}
