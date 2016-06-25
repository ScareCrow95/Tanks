using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

	public GameObject myTank;
	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		if (myTank != null)
			transform.position = myTank.transform.position;
	}
}
