using UnityEngine;
using System.Collections;

public class Missile : Photon.MonoBehaviour {
	public float damageRadius = 8;
	public float damage = 60f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision col)
	{
		if (photonView.isMine) {
			AreaDamageEnemies (transform.position, damageRadius, damage);
			PhotonNetwork.Instantiate ("ShellExplosion", transform.position, transform.rotation, 0);

			PhotonNetwork.Destroy (this.gameObject);
		}


	}

	void AreaDamageEnemies(Vector3 location, float radius, float damage)
	{
		Collider[] objectsInRange = Physics.OverlapSphere(location, radius);
		foreach (Collider col in objectsInRange)
		{
			TankNetworkMover enemy = col.GetComponent<TankNetworkMover>();
			if (enemy != null && col.gameObject.tag != "me")
			{
				// linear falloff of effect
				print("3");
				float proximity = (location - enemy.transform.position).magnitude;
				float effect = 1 - (proximity / radius);


				enemy.GetComponent<PhotonView> ().RPC ("ApplyDamage", PhotonTargets.All, damage * effect);
			}
		}

	}
}
