using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCountText : MonoBehaviour {

	public Text playerCount;
	Slider slider;
	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		playerCount.text = slider.value.ToString ();
	}
}
