using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour {

	public float explosionRadius = 3.5f;
	public int damage = 2;

	// Use this for initialization
	void Start () {
		StartCoroutine(DelayExplosion());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	IEnumerator DelayExplosion()
	{
		yield return new WaitForSeconds(1.5f);
		GameObject.FindObjectOfType<PlayerController>().GetComponent<AudioSource>().PlayOneShot(AudioManager.instance.grenadeExplosion);
		GameEffectManager.instance.AddWorldEffect("GrenadeExplosion", transform.position + new Vector3(0, 1.5f, 0), 1f, 0.5f);
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
		foreach (var col in cols)
		{
			if (col.GetComponent<BaseUnit>())
			{
				col.GetComponent<BaseUnit>().TakeDamage(damage);
			}
		}
		Destroy(gameObject);
	}
}
