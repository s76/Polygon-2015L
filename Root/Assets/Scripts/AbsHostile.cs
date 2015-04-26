using UnityEngine;
using System.Collections;

public abstract class AbsHostile : MonoBehaviour
{
	protected string hostileName;
	protected int maxHitpoints;
	protected int hitpoints;
	protected bool alive;

	protected AbsHostile(string name, int hitpoints){
		this.hostileName = name;
		this.maxHitpoints = hitpoints;
		this.hitpoints = hitpoints;
		alive = true;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void TakeDamage(int damage){
		if(alive){
			hitpoints -= damage;
			if (hitpoints <= 0) {
				hitpoints = 0;
				Die();
			}
			Debug.Log ("Taken " + damage + " damage. HP: " + hitpoints + ". Alive: " + alive);
		}
	}

	private void Die(){
		alive = false;
		Vector3 rotation = transform.eulerAngles;
		rotation.x = 90.0f;
		transform.eulerAngles = rotation;
	}
	
	public int GetHitpoints(){
		return hitpoints;
	}

	public bool IsAlive(){
		return alive;
	}
}

