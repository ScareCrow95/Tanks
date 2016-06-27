using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Fadeout : MonoBehaviour {
	bool isClose = false;
	public float fadeoutSpeed = 1.5f;

	// Use this for initialization
	void Start () {
		gameObject.AddComponent<CanvasGroup> ();
		transform.GetComponent<CanvasGroup> ().alpha = 1;
		isClose = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (isClose) {

			transform.GetComponent<CanvasGroup> ().alpha -= Time.deltaTime*fadeoutSpeed;

			if(	transform.GetComponent<CanvasGroup> ().alpha <= 0)
			{
				gameObject.SetActive(false);
				isClose= false;
			}
			
		


		}

		
	}
	public void close()
	{
		isClose = true;
	}
}
