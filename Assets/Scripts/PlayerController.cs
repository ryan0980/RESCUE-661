using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseUnit {

	public KeyCode switchKey = KeyCode.Q;
	public KeyCode jumpKey = KeyCode.Space;

	public float speed = 5f;

	private bool facingRight = true;
	public Transform groundCheck;
	public LayerMask whatIsGround;
	public int machineGunAmmoCount = 3;
	public float machineGunAmmoInterval = 0.1f;
	bool grounded = false;
	float groundRadius = 0.2f;

	[SerializeField] private float jumpForce = 300f;
	[SerializeField] private float throwForce = 600f;
	bool attack = false;


	// Update is called once per frame
	void Update() {

		HandleInput();
	}

	//movement//
	void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("Ground", grounded);

		if (GameManager.instance.gamePause || GameManager.instance.gameOver || dead)
		{
			rb.velocity = new Vector2(0, rb.velocity.y);
			return;
		}

		//float horizontal = Input.GetAxis("Horizontal");
		float horizontal = 1;
		if (!dead && !attack)
		{
			anim.SetFloat("vSpeed", rb.velocity.y);
			anim.SetFloat("Speed", Mathf.Abs(horizontal));
			rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
		}
		if (horizontal > 0 && !facingRight && !dead && !attack)
		{
			Flip(horizontal);
		}

		else if (horizontal < 0 && facingRight && !dead && !attack)
		{
			Flip(horizontal);
		}
	}


	//attacking and jumping//
	private void HandleInput()
	{
		if (GameManager.instance.gamePause || GameManager.instance.gameOver || dead)
		{
			return;
		}

		if (Input.GetMouseButtonDown(0) && InventoryManager.instance.GetWeaponAmmoCount(InventoryManager.instance.equippedWeapon) > 0)
		{
			if(InventoryManager.instance.equippedWeapon == "111")
			{
				NormalGunShoot();
			}else if (InventoryManager.instance.equippedWeapon == "222")
			{
				StartCoroutine(MachineGunShoot());
			}else if (InventoryManager.instance.equippedWeapon == "333")
			{
				GrenadeShoot();
			}
		}

		if (Input.GetKeyDown(switchKey))
		{
			audioS.PlayOneShot(AudioManager.instance.weaponSwitch);
			InventoryManager.instance.SwitchWeapon();
		}

		if (Input.GetKeyDown(KeyCode.LeftAlt) && !dead)
		{
			attack = true;
			anim.SetBool("Attack", true);
			anim.SetFloat("Speed", 0);

		}

		if (grounded && Input.GetKeyDown(jumpKey) && !dead)
		{
			anim.SetTrigger("Jump");
			rb.AddForce(new Vector2(0, jumpForce));
		}

	}


	private void Flip(float horizontal)
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	protected override void Die()
	{
		base.Die();
		GameManager.instance.gameOver = true;
		UIManager.instance.Push(typeof(GameEnd));
	}

	void NormalGunShoot()
	{
		anim.SetTrigger("Shoot");
		audioS.PlayOneShot(AudioManager.instance.oneGunShoot);
		InventoryManager.instance.HandleAmmoWhenShoot(InventoryManager.instance.equippedWeapon);
		Bullet bulletClone = Instantiate(bulletPrefab, gunShootPos.position, gunShootPos.rotation);
		GameEffectManager.instance.AddEffect("GunFire", gunShootPos.gameObject, gunShootPos.position, 1.5f, 0.2f);
		bulletPrefab.owner = this.gameObject;
		bulletPrefab.damage = this.attackPower;
		bulletClone.shootDirection = transform.right;
	}

	IEnumerator MachineGunShoot()
	{
		anim.SetTrigger("Shoot");
		audioS.PlayOneShot(AudioManager.instance.threeGunShoot);
		InventoryManager.instance.HandleAmmoWhenShoot(InventoryManager.instance.equippedWeapon);
		GameEffectManager.instance.AddEffect("GunFire", gunShootPos.gameObject, gunShootPos.position, 1.5f, 0.2f);
		int i = 0;
		while(i < machineGunAmmoCount)
		{
			Bullet bulletClone = Instantiate(bulletPrefab, gunShootPos.position, gunShootPos.rotation);
			bulletPrefab.owner = this.gameObject;
			bulletPrefab.damage = this.attackPower;
			bulletClone.shootDirection = transform.right;
			yield return new WaitForSeconds(machineGunAmmoInterval);
			i++;
		}

	}

	void GrenadeShoot()
	{
		anim.SetTrigger("Throw");
		//rb.AddForce(new Vector2(0, jumpForce));
		GameObject grenade = Instantiate(GrenadePrefab, gunShootPos.position, Quaternion.identity);
		grenade.GetComponent<Rigidbody2D>().AddForce(throwForce * (Vector2.one));
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.GetComponent<EnemyAI>() && !collision.gameObject.GetComponent<EnemyAI>().dead)
		{
			TakeDamage(1);
			Destroy(collision.gameObject);
		}

		if (collision.gameObject.GetComponent<StarPickup>())
		{
			GameManager.instance.timeLeft += 5;
			Destroy(collision.gameObject);
		}

		if (collision.gameObject.GetComponent<VictoryPoint>())
		{
			GameManager.instance.gameOver = true;
			GameManager.instance.gameWin = true;
			anim.SetBool("Victory", true);
			UIManager.instance.Push(typeof(GameEnd));
		}
	}
}
