using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;



public class FlameNetworkMover : Photon.MonoBehaviour {
	public delegate void Respawn(float time);
	public event Respawn RespawnMe;
	Vector3 position;
	Quaternion rotation;
	float smoothing = 10f;
	public GameObject cam;		
	public float Health = 100;
	public Image healthGameObject;
	GameObject shootButton;
	public GameObject Flames;


	// Use this for initialization
	void Start () {
		if (photonView.isMine) {
			Flames.SetActive (false);
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<TankCharacterController> ().enabled = true;
			gameObject.tag = "me";
			shootButton = GameObject.FindGameObjectWithTag("shoot");
			cam = GameObject.FindGameObjectWithTag("camera");
			cam.GetComponent<CameraControl> ().myTank = this.gameObject;
			cam.transform.position = transform.position;

		} else {
			StartCoroutine("UpdateData");
			gameObject.tag = "other";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		if (CrossPlatformInputManager.GetButton ("Shoot") && photonView.isMine) {
			Flames.SetActive (true);		
		} else {
			Flames.SetActive (false);
		}
			
	}

	IEnumerator UpdateData()
	{
		while(true)
		{
			transform.position = Vector3.Lerp(transform.position, position, smoothing);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, smoothing);

			yield return null;
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if(stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			stream.SendNext (Health);

		}
		else
		{
			position = (Vector3)stream.ReceiveNext();
			rotation = (Quaternion)stream.ReceiveNext();
			Health = (float)stream.ReceiveNext ();

		}
	}

	[PunRPC]
	public void ApplyDamageFlame(float damage)
	{
		Health -= damage;
		if(Health<=0 && photonView.isMine)
		{
			if(RespawnMe != null)
				RespawnMe(3f);

			PhotonNetwork.Instantiate ("Explosion", transform.position, transform.rotation, 0);
			PhotonNetwork.Destroy (gameObject);
		}
		healthGameObject.fillAmount = Health / 100;


	}
}
