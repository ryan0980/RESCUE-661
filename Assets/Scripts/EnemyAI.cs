using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : BaseUnit {

	public float attackDistance = 5.0f;
	public float shootInterval = 0.5f;
	private float shootTiming = 0.5f;
	public StarPickup pickupPrefab;

	// Update is called once per frame
	void Update () {

		if(IfCanShoot()) WhenToShoot();
	}

	void WhenToShoot()
	{
		//Ray2D ray = new Ray2D(transform.position, Vector3.left);
		RaycastHit2D[] hitInfos = Physics2D.RaycastAll(gunShootPos.position, Vector3.left, attackDistance);
		foreach (var hitInfo in hitInfos)
		{
			PlayerController player = hitInfo.transform.gameObject.GetComponent<PlayerController>();
			if (player != null) EnemyShoot();
		}
	}

	void EnemyShoot()
	{
		shootTiming += Time.deltaTime;
		if(shootTiming > shootInterval)
		{
			anim.SetTrigger("Shoot");
			Bullet bulletClone = Instantiate(bulletPrefab, gunShootPos.position, Quaternion.identity);
			GameEffectManager.instance.AddEffect("MuzzleFire", gunShootPos.gameObject, gunShootPos.position, 1.0f, 0.2f);
			bulletClone.owner = this.gameObject;
			bulletClone.damage = this.attackPower;
			bulletClone.shootDirection = Vector3.left;
			shootTiming = 0;
		}		
	}

	bool IfCanShoot()
	{
		if (dead)
		{
			return false;
		}
		if (GameManager.instance.gameOver)
		{
			return false;
		}
		if (GameManager.instance.gamePause)
		{
			return false;
		}
		return true;
	}

	protected override void Die()
	{
		base.Die();
		gameObject.GetComponent<Collider2D>().isTrigger = true;
		Instantiate(pickupPrefab, transform.position, Quaternion.identity);
	}
}
