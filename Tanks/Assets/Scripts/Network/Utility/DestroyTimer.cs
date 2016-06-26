using UnityEngine;
using System.Collections;

public class DestroyTimer : Photon.MonoBehaviour {

	public float timer =2f;
	// Use this for initialization
	void Start () {
		Invoke ("destroyMe", timer);
	}
	
	// Update is called once per frame
	void destroyMe () {
		if(photonView.isMine)
		PhotonNetwork.Destroy (gameObject);
	
	}
}
