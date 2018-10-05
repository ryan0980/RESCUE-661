using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private Rigidbody2D rb2D;
	public float shootForce = 1000;
	public GameObject owner;
	public Vector3 shootDirection;
	public int damage;

	// Use this for initialization
	void Start () {
		rb2D = GetComponent<Rigidbody2D>();
		rb2D.AddForce(shootDirection * shootForce);

		//8秒后自动销毁
		Destroy(gameObject, 1.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<BaseUnit>() && collision.gameObject != owner)
		{
			collision.GetComponent<BaseUnit>().TakeDamage(damage);

		}
		Destroy(this.gameObject);
	}
}
