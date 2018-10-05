using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffManager : MonoBehaviour {

	public PlayerController player;

	private void Start()
	{
		player = player.GetComponent<PlayerController>();
	}

	private void OnEnable()
	{
		HUD.OnSlowBuff += HUD_OnSlowBuff;
		HUD.OnBulletBuff += HUD_OnBulletBuff;
		HUD.OnNullBuff += HUD_OnNullBuff;
		HUD.OnEnemyBuff += HUD_OnEnemyBuff;
		HUD.OnGrenadeBuff += HUD_OnGrenadeBuff;
		HUD.OnGunBuff += HUD_OnGunBuff;
		HUD.OnHurtBuff += HUD_OnHurtBuff;
		HUD.OnHealthBuff += HUD_OnHealthBuff;
	}

	private void OnDisable()
	{
		HUD.OnSlowBuff -= HUD_OnSlowBuff;
		HUD.OnBulletBuff -= HUD_OnBulletBuff;
		HUD.OnNullBuff -= HUD_OnNullBuff;
		HUD.OnEnemyBuff -= HUD_OnEnemyBuff;
		HUD.OnGrenadeBuff -= HUD_OnGrenadeBuff;
		HUD.OnGunBuff -= HUD_OnGunBuff;
		HUD.OnHurtBuff -= HUD_OnHurtBuff;
		HUD.OnHealthBuff -= HUD_OnHealthBuff;
	}

	private void HUD_OnHealthBuff()
	{
		player.currentHealth /= 2;
		string msg = "You lose half of health!";
		CallChestPopup(msg);
	}

	private void HUD_OnHurtBuff()
	{
		player.TakeDamage(1);
		string msg = "You got hurt!";
		CallChestPopup(msg);
	}

	private void HUD_OnGunBuff()
	{
		InventoryManager.instance.LoseWeapon(InventoryManager.instance.equippedWeapon);
		string msg = "You lose the equipped weapon!";
		CallChestPopup(msg);
	}

	private void HUD_OnGrenadeBuff()
	{
		InventoryManager.instance.AddAmmoForWeapon("333", -2);
		string msg = "You lose 2 grenades!";
		CallChestPopup(msg);
	}

	private void HUD_OnEnemyBuff()
	{
		GameManager.instance.timeLeft -= 5;
		string msg = "Time left reduce 5 seconds!";
		CallChestPopup(msg);
	}

	private void HUD_OnNullBuff()
	{
		string msg = "Nothing Happened~";
		CallChestPopup(msg);
	}

	private void HUD_OnBulletBuff()
	{
		InventoryManager.instance.AddAmmoForWeapon(InventoryManager.instance.equippedWeapon, -5);
		string msg = "You missed 5 bullets!";
		CallChestPopup(msg);
	}

	private void HUD_OnSlowBuff()
	{
		player.speed *= 0.8f;
		string msg = "You're slowing down ";
		CallChestPopup(msg);
	}

	void CallChestPopup(string msg)
	{
		UIManager.instance.Push<ChestResultPopup>().Init(msg, AfterSpinning);
	}

	void AfterSpinning()
	{
		UIManager.instance.HUD.spinWheel.SetActive(false);
		UIManager.instance.HUD.openButton.interactable = true;
		GameManager.instance.gamePause = false;
	}
}
