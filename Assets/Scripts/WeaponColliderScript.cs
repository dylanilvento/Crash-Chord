using UnityEngine;
using System.Collections;
using System;

public class WeaponColliderScript : MonoBehaviour {

	public bool attack = false;
	public bool block = false;

	float xPos, yPos;

	Vector3 originalPos;

	GameObject player;

	Collider2D weaponCollider;

	// Use this for initialization
	void Start () {

		//originalPos = transform.localPosition;
		weaponCollider = GetComponent<BoxCollider2D>();
		weaponCollider.enabled = false;
		//player = GameObject.Find("Player");
		player = this.transform.parent.gameObject;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HighAttack (bool facingLeft) {

		//originalPos = player.transform.position - transform.position;
		originalPos = transform.localPosition;
		attack = true;
		weaponCollider.enabled = true;

		Vector3 pos = GetPos(facingLeft, 2f, 0f);
		
		transform.position = pos;
		StartCoroutine(ResetAttackPos());

	}

	public void MidAttack (bool facingLeft) {

		//originalPos = player.transform.position - transform.position;
		originalPos = transform.localPosition;
		attack = true;
		weaponCollider.enabled = true;

		Vector3 pos = GetPos(facingLeft, 2f, -1.2f);

		transform.position = pos;
		StartCoroutine(ResetAttackPos());
		
	}

	public void LowAttack (bool facingLeft) {
		
		//originalPos = player.transform.position - transform.position;
		originalPos = transform.localPosition;
		attack = true;
		weaponCollider.enabled = true;
		
		Vector3 pos = GetPos(facingLeft, 2f, -2f);
		
		transform.position = pos;
		StartCoroutine(ResetAttackPos());

	}


	public void StartBlock () {

		originalPos = transform.localPosition;
		//print("test");

	}

	public void HighBlock (bool facingLeft) {

		block = true;
		
		Vector3 pos = GetPos(facingLeft, 1.5f, 0f);
		//weaponCollider.enabled = true;


		transform.position = pos;



	}

	public void MidBlock (bool facingLeft) {

		block = true;
		
		Vector3 pos = GetPos(facingLeft, 1.5f, -1f);
		//weaponCollider.enabled = true;


		transform.position = pos;
		
		
	}

	public void LowBlock (bool facingLeft) {

		block = true;
		
		Vector3 pos = GetPos(facingLeft, 1.5f, -2f);
		//weaponCollider.enabled = true;

		transform.position = pos;
		
	}

	//xDiff is absolute value, yDiff is not
	Vector3 GetPos (bool facingLeft, float xDiff, float yDiff) {

		Vector3 pos;

		if (facingLeft) {
			
			pos = new Vector3(player.transform.position.x - xDiff, player.transform.position.y + yDiff, transform.position.z);
			
		}
		
		else {
			pos = new Vector3(player.transform.position.x + xDiff, player.transform.position.y + yDiff, transform.position.z);
		}

		return pos;
	}


	IEnumerator ResetAttackPos() {
		yield return new WaitForSeconds(0.000009f);
		transform.localPosition = originalPos;
		attack = false;
		weaponCollider.enabled = false;

	}

	public void ResetBlockPos() {

		StartCoroutine(ResetBlockPos2());

	}

	IEnumerator ResetBlockPos2() {

		yield return new WaitForSeconds(0.1f);
		transform.localPosition = originalPos;
		block = false;
		weaponCollider.enabled = false;

	}

}
