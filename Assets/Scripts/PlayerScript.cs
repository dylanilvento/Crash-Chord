using UnityEngine;
using System.Collections;
using System;

public class PlayerScript : MonoBehaviour {

	public float maxSpeed = 5f;
	public float test;
	public float move;

	//public float x;

	//[0] = high, [1] = mid, [2] = low
	public bool[] blocks = {false, false, false};
	public bool[] attacks = {false, false, false};

	float weaponPos;

	bool hit = false;
	bool facingLeft = true;
	
	GameObject collidedWith;
	WeaponColliderScript weapColScript;
	
	Animator anim;
	
	
	// Use this for initialization
	void Start () {
		
		anim = GetComponent<Animator>();
		weapColScript = GetComponentInChildren<WeaponColliderScript>();
		
	}
	
	// Update is called once per frame
	//Don't have to use time.deltaTime
	void FixedUpdate () { 
		
		move = Input.GetAxis("Horizontal");
		weaponPos = Input.GetAxis ("Vertical");
		//print(Input.GetJoystickNames()[0]);

		test = weaponPos;

		//anim.SetFloat("Speed", Mathf.Abs(move));
		anim.SetFloat("WeaponPos", weaponPos);
		
		if (!hit) { //need this here, otherwise will never bounce back
			GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);
		}

		if (move < 0 && !facingLeft)
			Flip ();
		else if (move > 0 && facingLeft)
			Flip ();
		
	}
	
	void Update () {

		//x = GetComponent<Rigidbody2D>().velocity.x;

		if (Input.GetButtonDown("Attack")) {

			anim.SetTrigger("Attack");
			if (weaponPos >= 0.01) {

				weapColScript.HighAttack(facingLeft);
				attacks[0] = true;

			}

			else if (weaponPos > -0.01 && weaponPos < 0.01) {

				weapColScript.MidAttack(facingLeft);
				attacks[1] = true;
			}

			else if (weaponPos <= -0.01) {
				
				weapColScript.LowAttack(facingLeft);
				attacks[2] = true;
			}
			
		}

		if (Input.GetButtonDown("Block")) {
			weapColScript.StartBlock();
		}

		if (Input.GetButton("Block")) {


			anim.SetBool("Block", true);

			if (weaponPos >= 0.01) {
				
				weapColScript.HighBlock(facingLeft);
				blocks[0] = true;
				blocks[1] = false;
				blocks[2] = false;
			}
			
			else if (weaponPos > -0.01 && weaponPos < 0.01) {
				
				weapColScript.MidBlock(facingLeft);
				blocks[0] = false;
				blocks[1] = true;
				blocks[2] = false;
				
			}
			
			else if (weaponPos <= -0.01) {
				
				weapColScript.LowBlock(facingLeft);
				blocks[0] = false;
				blocks[1] = false;
				blocks[2] = true;
				
			}
		}

		if (Input.GetButtonUp ("Block")) {

			anim.SetBool("Block", false);
			weapColScript.ResetBlockPos();
			blocks[0] = false;
			blocks[1] = false;
			blocks[2] = false;
		}
		
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

				StartCoroutine(WasHit());
				anim.SetTrigger("Hit");
				anim.SetBool("Block", false);
				GetComponent<Rigidbody2D>().velocity = new Vector2(5f, 3f);
				
			}
			else {

				StartCoroutine(WasHit());
				anim.SetTrigger("Hit");
				anim.SetBool("Block", false);
				GetComponent<Rigidbody2D>().velocity = new Vector2(-5f, 3f);

			}
		}
		
	}
	
	void OnTriggerExit2D (Collider2D other) {
		
		collidedWith = null;
		
	}

	IEnumerator WasHit () {
		hit = true;
		yield return new WaitForSeconds(0.75f);
		hit = false;

	}

	IEnumerator ClearWeapBools () {
		yield return new WaitForSeconds(0.05f);
		attacks[0] = false;
		attacks[1] = false;
		attacks[2] = true;

	}
}
