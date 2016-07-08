using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

	int loadProgress = 0;
	  Slider slider;
	public string sceneName;
	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider> ();

		StartCoroutine (DisplayLoadingScreen (sceneName));
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator DisplayLoadingScreen(string level)
	{
		
		AsyncOperation async = SceneManager.LoadSceneAsync (level);
		while (!async.isDone) {
			
			slider.value = async.progress;
			yield return null;
		}
	}
}
