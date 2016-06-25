using UnityEngine;
using System.Collections;

public class MissileSpawnPoint : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (photonView.isMine) {
			gameObject.tag = "missilePoint";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
