using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {


	public TankBehavior tank;

	// Use this for initialization
	void Start () {
		  
	}

	void Update() {
	
	}

	void OnTriggerEnter (Collider other)
	{
		//Debug.Log("YASSSSSSSS-----------------" + gameObject.tag);
		
			tank.setNewStatus("COLLECTED WEAPON");
			tank.increaseEnergy(170); 
			Destroy(gameObject);
	
	}

	
}
