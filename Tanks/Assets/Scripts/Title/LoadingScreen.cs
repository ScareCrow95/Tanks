using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour {

	int loadProgress = 0;
	//public Slider slider;
	public string sceneName;
	// Use this for initialization
	void Start () {

		StartCoroutine (DisplayLoadingScreen (sceneName));

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator DisplayLoadingScreen(string level)
	{
		
		print (loadProgress);
		AsyncOperation async = Application.LoadLevelAsync (level);
		while (!async.isDone) {
			loadProgress = (int)(async.progress);
			//slider.value = loadProgress;
			yield return null;
		}
	}
}
