using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;



public class MissileTankNetworkMover : Photon.MonoBehaviour {
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
	public float flameDamage = 2;
	GameObject shootButton;

	bool isFlame = false;
	float isFlameTimer = 3f;
	public GameObject Flames;

	void Start () {
		Invoke ("FindHealthBar", .4f);

		if (photonView.isMine) {
			Flames.SetActive (false);
			GetComponent<Rigidbody>().useGravity = true;
			GetComponent<TankCharacterController> ().enabled = true;
			gameObject.tag = "me";
		
			Power = GameObject.FindGameObjectWithTag("power");
			cam = GameObject.FindGameObjectWithTag("camera");
			shootButton = GameObject.FindGameObjectWithTag("shoot");

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



	void OnParticleCollision(GameObject other)
	{
		if (other.tag == "me") {
			isFlame = true;
			IniFlames ();
		}
	}


	void IniFlames()
	{
		
		ParticleSystem[] ps = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem p in ps) {
			ParticleSystem.EmissionModule em;
			em = p.emission;
			em.rate = new ParticleSystem.MinMaxCurve (22);


		}
	}

	void ExtinguishFlames()
	{
		ParticleSystem[] ps = GetComponentsInChildren<ParticleSystem> ();
		foreach (ParticleSystem p in ps) {
			float a = 22;
			ParticleSystem.EmissionModule em;
			em = p.emission;
			em.rate = new ParticleSystem.MinMaxCurve (a-2f);
		}
	}

	 void Update()
	{
		
		if (isFlame && photonView.isMine) {
			float iniTimer = isFlameTimer;
			Flames.SetActive (true);
			ExtinguishFlames ();
			gameObject.GetComponent<PhotonView> ().RPC ("ApplyDamageFlame", PhotonTargets.All, flameDamage);
			isFlameTimer -= Time.deltaTime;
			if (isFlameTimer <= 0) {
				isFlameTimer = iniTimer;
				isFlame = false;

			}
		}

		if(CrossPlatformInputManager.GetButtonUp("Shoot") && canShoot && photonView.isMine)
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
		if (CrossPlatformInputManager.GetButton ("Shoot") && canShoot && photonView.isMine) {
			if(currentForce<maxForce)
				currentForce ++;
			if(currentUpThrust < maxUpThrust)
				currentUpThrust += 5;
		
			powerUp.fillAmount = Mathf.Lerp (0, 1, Mathf.InverseLerp (minimumForce, maxForce, currentForce));     // UI red power Slider
		}
	}

	IEnumerator ShootDelay(float timer)
	{
		shootButton.GetComponent<Image> ().CrossFadeAlpha (.1f, 0, true);
		shootButton.GetComponent<Image> ().CrossFadeAlpha (1, timer, true);

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

	//to do

//	[PunRPC]
//	public void AddDamageForce( Vector3 location)
//	{
//		if (!photonView.isMine) {
//			print ("2");
//			Rigidbody rb = GetComponent<Rigidbody> ();
//			rb.AddExplosionForce (30, location, 20, 3);
//		}
//	}
//
}
