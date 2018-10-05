using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {

	public float pauseTimeBeforeStart = 3.0f;

	public int totalLevelTime = 120;
	internal int timeLeft;
	public bool gameOver;
	public bool gamePause;
	public bool gameWin;

	public GameObject spinWheel;
	public int chestRefreshIntreval = 10;

	void Start () {

		spinWheel.gameObject.SetActive(false);
		gameOver = false;
		gamePause = false;
		gameWin = false;

		timeLeft = totalLevelTime;
		StartCoroutine(PrepareStart());
	}
	

	void Update () {
		
	}

	IEnumerator PrepareStart()
	{
		gamePause = true;
		yield return new WaitForSeconds(pauseTimeBeforeStart);
		StartCoroutine(StartTiming());
	}

	IEnumerator StartTiming()
	{
		gamePause = false;
		int t = 0;
		while(timeLeft > 0)
		{
			yield return new WaitForSeconds(1);
			if(!gamePause && !gameOver)
			{
				timeLeft--;
				t++;
			}
			if (t == chestRefreshIntreval)
			{
				spinWheel.SetActive(true);
				gamePause = true;
				t = 0;
			}
		}
		gameOver = true;
	}


}
