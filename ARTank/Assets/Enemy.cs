using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Enemy : MonoBehaviour {

	public GameObject healthBar;
	private Animator animator;
	private static bool enemyInContact; // lo declaramos static porque solo nos interesa saber
										// si hay un enemigo en contacto o no. Si no lo ponemos
										// asi no ejecutara la conexion TCP, pues hace uso de este
										// miembro nada mas comenzar el programa. Asi mismo, en el editor,
										// el script TankBehavior ha de tener asignado un enemigo cualquiera.
	public TankBehavior tank;
	public int health;
	// Use this for initialization
	void Start () {
		healthBar.SetActive(false);
		enemyInContact = false;
		health = 25;
		animator = GetComponent<Animator>();
		if(gameObject.tag == "enemigo")
			animator.Play("shuffle");
		else if(gameObject.tag == "enemigo2")
			animator.Play("idle");
		else if(gameObject.tag == "enemigo3")
			animator.Play("reggaeton");
	}
	
	// Update is called once per frame
	void Update () {
		if(enemyInContact)
		{
			//Debug.Log("Tanque luchando con enemigo");
			if(tank.getHealth() > 0)
				tank.decreaseHealth((float)0.1);
			else
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

	}

	void OnTriggerEnter (Collider other)
	{
		//Debug.Log("YASSSSSSSS-----------------" + gameObject.tag);
		
			tank.enemy = this;
			tank.setNewStatus("ENEMY DETECTED");
			healthBar.SetActive(true);
			enemyInContact = true;
			if(gameObject.tag == "enemigo")
			animator.Play("atack2");
		else if(gameObject.tag == "enemigo2")
			animator.Play("atack");
		else if(gameObject.tag == "enemigo3")
			animator.Play("RoundKick");
		
	}

	void OnTriggerExit (Collider other)
	{
		Debug.Log("perdido el contacto con el enemigo");
		enemyInContact = false;
		healthBar.SetActive(false);
		if(gameObject.tag == "enemigo")
			animator.Play("hombros");
		else if(gameObject.tag == "enemigo2")
			animator.Play("idle");
		else if(gameObject.tag == "enemigo3")
			animator.Play("hombros");
		tank.setNewStatus("You escaped from your enemy!");
	}

	public void decreaseHealth(int amount)
	{	
		health -= amount;
	}

	public bool isEnemyInContact()
	{
		return enemyInContact;
	}

	public void setIsEnemyInContact(bool b)
	{
		enemyInContact = b;
	}
 
 	public void increaseTankHealthOnDeath(int amount)
 	{
 		tank.increaseHealth(amount);
 	}

 	public void Destruir(){
 		animator.Play("death");
 		TankBehavior.defeatedEnemies++;
 		Destroy(gameObject);
 	}
}
