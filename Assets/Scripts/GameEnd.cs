using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnd : UIScreen {

	public GameObject winText;
	public GameObject failText;

	private void OnEnable()
	{
		if (GameManager.instance.gameWin)
		{
			winText.SetActive(true);
			failText.SetActive(false);
		}
		else
		{
			winText.SetActive(false);
			failText.SetActive(true);
		}
	}


	public void OnRestartButton()
	{
		SceneManager.LoadScene("GameScene");
		UIManager.instance.Push(typeof(HUD));
	}

	public void OnMainMenuScene()
	{
		SceneManager.LoadScene("MainMenu");
	}
}
