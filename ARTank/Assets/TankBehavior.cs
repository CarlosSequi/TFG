using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TankBehavior : MonoBehaviour { 
	private int maxHealth;
	private float health;
	private int maxEnergy;
	private int energy; 
	public Enemy enemy;
	public HUD hud;
	public static int defeatedEnemies;

	// Use this for initialization 	
	void Start () {
		maxHealth = 500;
		energy = 0;
		maxEnergy = 500;
		health = 500;
		defeatedEnemies = 0;
	}  	
	// Update is called once per frame
	void Update () {    

	}  	

	public int getMaxHealth(){
		return maxHealth;
	}

	public int getMaxEnergy(){
		return maxEnergy;
	}

	public float getHealth(){
		return health;
	}

	public int getEnergy(){
		return energy;
	}

	public void decreaseHealth(float h){
		health -= h;
	}

	public void increaseHealth(float h){
		health += h;
	}

	public void decreaseEnergy(int e){
		energy -= e;
	}

	public void increaseEnergy(int e){
		energy += e;
	}

	public void atack(){
		Debug.Log("Tanque inflinge daño a enemigo");
		enemy.decreaseHealth(5);
		energy -= 25;
	}

	public bool inContactWithEnemy()
	{
		return enemy.isEnemyInContact();
	}

	public void setNewStatus(string text)
	{
		hud.setNewText(text);
	}
	
}