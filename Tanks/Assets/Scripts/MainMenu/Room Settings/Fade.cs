using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Fade : MonoBehaviour {
	bool SpellAlpha = false;
	public float timer = 0f;
	public float fadespeed=1f;
	void Start()

	{
		gameObject.AddComponent<CanvasGroup> ();
		transform.GetComponent<CanvasGroup> ().alpha = 0;
	}

	// Update is called once per frame
	void Update () {
		Invoke ("fader", timer); 
	}

	void fader()
	{
		if(!SpellAlpha)
		{
			if (transform.GetComponent<CanvasGroup> ().alpha <= 1) 
			{

				transform.GetComponent<CanvasGroup> ().alpha += Time.deltaTime*fadespeed;

			}
		}
		if (transform.GetComponent<CanvasGroup> ().alpha >= 1) 
		{
			SpellAlpha = true;
		}
	}
}
