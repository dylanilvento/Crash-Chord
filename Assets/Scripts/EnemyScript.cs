using UnityEngine;
using System.Collections;
using System;

public class EnemyScript : MonoBehaviour {

	public float maxSpeed = 4f;
	public float test;
	public bool attackedPlayer = false;

	//[0] = high, [1] = mid, [2] = low
	public bool[] blocks = {false, false, false};
	public bool[] attacks = {false, false, false};

	bool blocked;
	bool facingLeft = true;
	bool stopMvmt = false;

	public GameObject player;
	PlayerScript playerScript;
	WeaponColliderScript weapColScript;
	
	GameObject collidedWith;
	
	Animator anim;


	// Use this for initialization
	void Start () {

		anim = GetComponent<Animator>();
		player = GameObject.Find("Player");
		playerScript = player.GetComponent<PlayerScript>();
		weapColScript = GetComponentInChildren<WeaponColliderScript>();

	
	}
	
	// Update is called once per frame
	void Update () {

		if (transform.position.x < player.transform.position.x) {

			if (!stopMvmt) 	{
				if (transform.position.x < player.transform.position.x - 6.0) {

					if (facingLeft)
						Flip();

					maxSpeed = 3.5f;

					GetComponent<Rigidbody2D>().velocity = new Vector2(1 * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

				}

				else if (transform.position.x < player.transform.position.x - 2.5) {

					if (facingLeft)
						Flip();

					maxSpeed = 2f;

					GetComponent<Rigidbody2D>().velocity = new Vector2(1 * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

				}
			}

			if (transform.position.x > player.transform.position.x - 2.5) {
				//if (Math.Abs(playerScript.move) < 0.1 && !attackedPlayer) {

				int choice = Mathf.FloorToInt(UnityEngine.Random.Range(0, 2));
				//print (choice);

				if (choice == 0 && !attackedPlayer) {

					Attack ();

				}

				else if (choice == 1 && !blocked) {

					Block();

				}
			}
		}

		/*else {

			if (!facingLeft)
				Flip();

			GetComponent<Rigidbody2D>().velocity = new Vector2(-1 * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

		}*/

	}

	void Flip () {
		
		facingLeft = !facingLeft;
		Vector3 playerScale = transform.localScale;
		playerScale.x *= -1;
		transform.localScale = playerScale;
	}

	void OnTriggerEnter2D (Collider2D other) {

		collidedWith = other.gameObject;

		if (collidedWith.name == "WeaponCollider") {
			if (facingLeft) {
				anim.SetTrigger("Hit");
				anim.SetBool("Block", false);
				GetComponent<Rigidbody2D>().velocity = new Vector2(3f, 3f);
				StartCoroutine(StopMvmt());
				
			}
			else {
				anim.SetTrigger("Hit");
				anim.SetBool("Block", false);
				GetComponent<Rigidbody2D>().velocity = new Vector2(-3f, 3f);
				StartCoroutine(StopMvmt());
			}
		}

	}

	void OnTriggerExit2D (Collider2D other) {

		collidedWith = null;

	}

	IEnumerator CanAttackAgain() {
		yield return new WaitForSeconds(2);
		attackedPlayer = false;
	}

	IEnumerator CanBlockAgain() {
		yield return new WaitForSeconds(1);
		anim.SetBool("Block", false);
		yield return new WaitForSeconds(2);
		blocked = false;
	}

	IEnumerator StopMvmt () {
		stopMvmt = true;
		yield return new WaitForSeconds(0.5f);
		stopMvmt = false;

	}

	void Block () {

		blocked = true;
		float weaponPos = UnityEngine.Random.Range(-1, 2);
		anim.SetFloat("WeaponPos", weaponPos);
		anim.SetBool("Block", true);

		if (weaponPos >= 0.01) {
			
			weapColScript.HighBlock(facingLeft);
			
		}
		
		else if (weaponPos > -0.01 && weaponPos < 0.01) {
			
			weapColScript.MidBlock(facingLeft);
			
		}
		
		else if (weaponPos <= -0.01) {
			
			weapColScript.LowBlock(facingLeft);
			
		}
		
		StartCoroutine(CanBlockAgain());
		
	}

	void Attack () {

		attackedPlayer = true;
		StartCoroutine(CanAttackAgain());
		float weaponPos = UnityEngine.Random.Range(-1, 2);
		anim.SetFloat("WeaponPos", weaponPos);
		anim.SetTrigger("Attack");

		if (weaponPos >= 0.01) {
			
			weapColScript.HighAttack(facingLeft);
			
		}
		
		else if (weaponPos > -0.01 && weaponPos < 0.01) {
			
			weapColScript.MidAttack(facingLeft);
			
		}
		
		else if (weaponPos <= -0.01) {
			
			weapColScript.LowAttack(facingLeft);
			
		}
		
	}
	
	
}
