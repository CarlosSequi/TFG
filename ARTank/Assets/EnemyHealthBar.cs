using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour {

	Vector3 localScale;
	public Enemy enemy;

	// Use this for initialization
	void Start () {
		localScale = transform.localScale;
	}
	
	// Update is called once per frame
	void Update () {
		if(enemy.health <= 0)
		{
			enemy.setIsEnemyInContact(false);
			enemy.increaseTankHealthOnDeath(120);
			enemy.Destruir();
			enemy.tank.setNewStatus("Defeated enemy, life bar increased!");
		}
		else
		{
			localScale.x = enemy.health;
			transform.localScale = localScale;
		}
		
	}

}
