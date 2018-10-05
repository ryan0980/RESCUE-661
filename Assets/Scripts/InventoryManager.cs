using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager> {

	public Dictionary<string, int> _weaponAmmoCount = new Dictionary<string, int>();

	public string equippedWeapon;
	public string defaultWeapon;
	public int maxCapacityOfPac;
	public int currentCapacity;

    public int initPistalAmmo = 75;
    public int initGunAmmo = 30;
    public int initGrenade = 15;

	// Use this for initialization
	void Start () {
		_weaponAmmoCount.Add("111", initPistalAmmo);
		_weaponAmmoCount.Add("222", initGunAmmo);
		_weaponAmmoCount.Add("333", initGrenade);
		defaultWeapon = "111";
		equippedWeapon = defaultWeapon;
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void AddAmmoForWeapon(string name, int count)
	{
		if (_weaponAmmoCount.ContainsKey(name))
		{
			_weaponAmmoCount[name] += count;
			if(_weaponAmmoCount[name] < 0)
			{
				_weaponAmmoCount[name] = 0;
			}
		}
		else
		{
			if(currentCapacity < maxCapacityOfPac)
			{
				_weaponAmmoCount.Add(name, count);
				currentCapacity++;
			}			
		}
	}

	public bool EquipWeapon(string weaponName)
	{
		if(_weaponAmmoCount.ContainsKey(weaponName) && equippedWeapon != weaponName)
		{
			equippedWeapon = weaponName;
			return true;
		}
		return false;
	}

	public void SwitchWeapon()
	{
		int orderOfNextWeapon = 0;
		List<string> weaponList = GetWeaponNameList();
		for(int i=0; i<weaponList.Count; i++)
		{
			if(equippedWeapon == weaponList[i])
			{
				orderOfNextWeapon = i + 1;
				break;
			}
		}
		if(orderOfNextWeapon == weaponList.Count)
		{
			orderOfNextWeapon = 0;
		}
		equippedWeapon = weaponList[orderOfNextWeapon];
	}

	public void HandleAmmoWhenShoot(string weaponName)
	{
		if (_weaponAmmoCount.ContainsKey(weaponName))
		{
			_weaponAmmoCount[weaponName]--;
		}
		else
		{
			Debug.LogWarning("Player doesn't equip this weapon");
		}
	}

	public void LoseWeapon(string weaponName)
	{
		if (_weaponAmmoCount.ContainsKey(weaponName))
		{
			SwitchWeapon();
			_weaponAmmoCount.Remove(weaponName);
		}
	}

	#region GetStats
	public List<string> GetWeaponNameList()
	{
		List<string> list = new List<string>(_weaponAmmoCount.Keys);
		return list;
	}

	public int GetWeaponAmmoCount(string name)
	{
		if (_weaponAmmoCount.ContainsKey(name))
		{
			return _weaponAmmoCount[name];
		}
		return 0;
	}
	#endregion
}
