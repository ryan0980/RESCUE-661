using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : UIScreen {

	public static event Action OnSlowBuff;
	public static event Action OnNullBuff;
	public static event Action OnGrenadeBuff;
	public static event Action OnGunBuff;
	public static event Action OnBulletBuff;
	public static event Action OnHealthBuff;
	public static event Action OnHurtBuff;
	public static event Action OnEnemyBuff;
	private List<Action> allDebuffs = new List<Action>();
	public List<string> resultMessage = new List<string>();


	
	public GameObject[] weaponFrames;
	public GameObject[] weaponWholes;
	public Text[] weaponAmmoCounts;
	public Image[] weaponIcons;
	public GameObject spinWheel;
	public Text timeLeft;
	public Button openButton;
	public PlayerController player;
	public GameObject[] playerHP;

	// Use this for initialization
	void Start () {
		allDebuffs.Add(OnEnemyBuff);
		allDebuffs.Add(OnBulletBuff);
		allDebuffs.Add(OnSlowBuff);
		allDebuffs.Add(OnGunBuff);
		allDebuffs.Add(OnHurtBuff);
		allDebuffs.Add(OnGrenadeBuff);
		allDebuffs.Add(OnNullBuff);	
		allDebuffs.Add(OnHealthBuff);
		

		player = player.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {

		UpdateWeaponIcon();
		UpdatePlayerHealth();
		timeLeft.text = "Time Left: "+ GameManager.instance.timeLeft + " s!";
	}


	//更新装备栏武器图标UI
	private void UpdateWeaponIcon()
	{
		List<string> weaponList = InventoryManager.instance.GetWeaponNameList();
		for (int i = 0; i < weaponWholes.Length; i++)
		{
			if (i < weaponList.Count)
			{
				weaponWholes[i].SetActive(true);
			}
			else
			{
				weaponWholes[i].SetActive(false);
			}
		}
		for (int i = 0; i < weaponList.Count; i++) 
		{
			weaponIcons[i].sprite = Resources.Load<Sprite>("WeaponIcons/" + weaponList[i]);
			weaponAmmoCounts[i].text = InventoryManager.instance.GetWeaponAmmoCount(weaponList[i]).ToString();
			weaponFrames[i].SetActive(weaponList[i] == InventoryManager.instance.equippedWeapon ? true : false);
		}
	}

	//更新玩家血量UI
	void UpdatePlayerHealth()
	{
		for(int i=0; i<playerHP.Length; i++)
		{
			if(i < player.currentHealth)
			{
				playerHP[i].SetActive(true);
			}
			else
			{
				playerHP[i].SetActive(false);
			}
		}
	}



	//当按下Open按钮时
	public void OnOpenButton()
	{
		int index = UnityEngine.Random.Range(0, 8);
		spinWheel.GetComponentInChildren<SpinWheel>().RotateUp(8, index, true, allDebuffs[index]);
		openButton.interactable = false;
	}
}
