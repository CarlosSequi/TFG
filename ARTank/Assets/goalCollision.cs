using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goalCollision : MonoBehaviour {

	public TankBehavior tank;

	void Update()
	{
		transform.Rotate(Vector3.up * Time.deltaTime);
	}

	void OnTriggerEnter (Collider other)
	{
		if(gameObject.tag == "goal" && TankBehavior.defeatedEnemies == 3)
		{			
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
		}
		else
		{
			tank.setNewStatus("You must defeat 3 enemies.");
		}
	
	}
}
