using UnityEngine;
using System.Collections;

public class MissileNetworkMover : Photon.MonoBehaviour {
	
	Vector3 position;
	Quaternion rotation;
	float smoothing = 10f;
	// Use this for initialization
	void Start () {
		if (photonView.isMine) {
			GetComponent<Missile>().enabled = true;

		}
		else{
			StartCoroutine("UpdateData");
	

		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator UpdateData()
	{
		while(true)
		{
			transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * smoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * smoothing);
			yield return null;
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);

		}
		else
		{
			position = (Vector3)stream.ReceiveNext();
			rotation = (Quaternion)stream.ReceiveNext();

		}
	}

}
