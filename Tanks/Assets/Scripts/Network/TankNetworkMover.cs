using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;



public class TankNetworkMover : Photon.MonoBehaviour {
	public delegate void Respawn(float time);
	public event Respawn RespawnMe;
	Vector3 position;
	Quaternion rotation;
	float smoothing = 10f;
	[Range(2,100)]
	public float minimumForce = 5;
	[Range(20,300)]
	public float maxForce = 50;
	public float chargeSpeed = 2;
	public float currentForce ;
	public float minUpThrust = 50;
	public float maxUpThrust = 300;
	public float currentUpThrust  ;
	public GameObject missilePoint;								//missile spawn point
	public GameObject cam;								
	GameObject Power; 											//power UI bar
	Image powerUp;												// image attached to power			
	public float Health = 100;
	public Image healthGameObject;
	public float shootDelay = 1.5f;
	bool canShoot = true;
	//public GameObject healthGameObjectOther;


	// Use this for initialization
	void Start () {
		Invoke ("FindHealthBar", .4f);

		if (photonView.isMine) {
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<TankCharacterController> ().enabled = true;
			gameObject.tag = "me";
		
			Power = GameObject.FindGameObjectWithTag("power");
			cam = GameObject.FindGameObjectWithTag("camera");
			cam.GetComponent<CameraControl> ().myTank = this.gameObject;
			cam.transform.position = transform.position;

			powerUp = Power.GetComponent<Image> ();
			Invoke ("FindMissileSpawnPoint", .2f);
			Invoke ("changeColorRed", .41f);


		

		}
		else{
			StartCoroutine("UpdateData");
			gameObject.tag = "other";
			Invoke ("changeColorBlue", .41f);

		}

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
			stream.SendNext (Health);
		
		}
		else
		{
			position = (Vector3)stream.ReceiveNext();
			rotation = (Quaternion)stream.ReceiveNext();
			Health = (float)stream.ReceiveNext ();
		
		}
	}

	 void Update()
	{
		

		if(CrossPlatformInputManager.GetButtonUp("Shoot") && canShoot)
			{
			
			GameObject	missile = PhotonNetwork.Instantiate("Missile",missilePoint.transform.position,missilePoint.transform.rotation,0);
			missile.GetComponent<Rigidbody> ().velocity = transform.forward * currentForce ;
			missile.GetComponent<Rigidbody> ().AddForce(transform.up * currentUpThrust );
			StartCoroutine (ShootDelay (shootDelay));
			canShoot = false;


			currentForce = minimumForce;
			currentUpThrust = minUpThrust;
			powerUp.fillAmount = 0;

			}
		if (CrossPlatformInputManager.GetButton ("Shoot") && canShoot) {
			if(currentForce<maxForce)
				currentForce ++;
			if(currentUpThrust < maxUpThrust)
				currentUpThrust += 5;
		
			powerUp.fillAmount = Mathf.Lerp (0, 1, Mathf.InverseLerp (minimumForce, maxForce, currentForce));     // UI red power Slider
		}
	}

	IEnumerator ShootDelay(float timer)
	{
		yield return new WaitForSeconds (timer);
		canShoot = true;
	}


	void FindMissileSpawnPoint()
	{
		missilePoint = GameObject.FindGameObjectWithTag("missilePoint");

	}
	void FindHealthBar()
	{
		//healthGameObject = GameObject.FindGameObjectWithTag("health");
		Image[] img = GetComponentsInChildren<Image>();
			foreach(Image i in img)
			{
			if (i.type == Image.Type.Filled)
				healthGameObject = i;
			}

	}

	void changeColorRed( )
	{
		healthGameObject.color = Color.red;
	}

	void changeColorBlue( )
	{
		healthGameObject.color = Color.blue;
	}

	[PunRPC]
	public void ApplyDamage(float damage)
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
//		if (Health < 25)
//			healthGameObject.color = Color.red;
	}
}
