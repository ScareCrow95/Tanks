using UnityEngine;
using System.Collections;

public class Missile : Photon.MonoBehaviour {
	public float damageRadius = 8;
	public float damage = 60f;
	public float damageForce = 10f;
    GameObject networkManager;

	NetworkSetup ns;


	// Use this for initialization
	void Start () {
		networkManager = GameObject.FindGameObjectWithTag ("NM");
		ns = networkManager.GetComponent<NetworkSetup> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		if (photonView.isMine) {
			AreaDamageEnemies (transform.position, damageRadius, damage, damageForce);
			PhotonNetwork.Instantiate ("ShellExplosion", transform.position, transform.rotation, 0);

			PhotonNetwork.Destroy (this.gameObject);
		}


	}

	void AreaDamageEnemies(Vector3 location, float radius, float damage , float force)
	{
		Collider[] objectsInRange = Physics.OverlapSphere(location, radius);
		foreach (Collider col in objectsInRange)
		{

			if(ns.tankType == "MissileTank")
			{
				
				MissileTankNetworkMover enemy = col.GetComponent<MissileTankNetworkMover>();
				if (enemy != null && col.gameObject.tag != "me") {

			
					float proximity = (location - enemy.transform.position).magnitude;
					float effect = 1 - (proximity / radius);


					enemy.GetComponent<PhotonView> ().RPC ("ApplyDamage", PhotonTargets.All, damage * effect);


			}

			}

			if(ns.tankType == "FlameTank")
			{

				FlameNetworkMover enemy = col.GetComponent<FlameNetworkMover>();
				if (enemy != null && col.gameObject.tag != "me") {
					// linear falloff of effect
					print ("3");
					float proximity = (location - enemy.transform.position).magnitude;
					float effect = 1 - (proximity / radius);


					enemy.GetComponent<PhotonView> ().RPC ("ApplyDamage", PhotonTargets.All, damage * effect);


				}

			}

		}

	}
}
