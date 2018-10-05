using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public GameObject creditsPopup;

	// Use this for initialization
	void Start () {
		creditsPopup.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnStartButton()
	{
		SceneManager.LoadScene("GameScene");
	}

	public void OnCreditsButton()
	{
		creditsPopup.SetActive(true);
	}

	public void OnCancelButton()
	{
		creditsPopup.SetActive(false);
	}

	public void OnQuitButton()
	{
		Application.Quit();
	}
}
