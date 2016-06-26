using UnityEngine;
using System.Collections;

public class TankMaterial : Photon.MonoBehaviour {

	// Use this for initialization
	void Start () {
		if (!photonView.isMine) {
			Renderer rend = GetComponent<Renderer> ();
			rend.material.color = new Color (Random.Range (.3f, .7f), Random.Range (.3f, .7f), Random.Range (.3f, .7f), 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
