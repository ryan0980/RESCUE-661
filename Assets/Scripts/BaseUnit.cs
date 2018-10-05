using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUnit : MonoBehaviour {

	public int maxHealth;
	public int currentHealth;
	public int attackPower = 1;

	protected Animator anim;
	protected AudioSource audioS;
	public Rigidbody2D rb { get; set; }

	[SerializeField] protected Bullet bulletPrefab;
	[SerializeField] protected GameObject GrenadePrefab;
	[SerializeField] protected Transform gunShootPos;

	internal bool dead = false;

	protected virtual void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		rb.freezeRotation = true;
		anim = GetComponent<Animator>();
		audioS = GetComponent<AudioSource>();
		//AnimatorStateInfo stateinfo = GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

		currentHealth = maxHealth;
	}

	public void TakeDamage(int damage)
	{
		currentHealth -= damage;
		anim.SetTrigger("Hurt");
		audioS.PlayOneShot(AudioManager.instance.hurt);
		if(currentHealth <= 0 && !dead)
		{
			Die();
		}
	}

	protected virtual void Die()
	{
		anim.SetBool("Dead", true);
		anim.SetFloat("Speed", 0);
		audioS.PlayOneShot(AudioManager.instance.dead);
		currentHealth = 0;
		//PlayerManager.instance.currentHealth = 0;
		dead = true;
	}

}
