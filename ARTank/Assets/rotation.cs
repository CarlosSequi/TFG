using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotation : MonoBehaviour {

	[SerializeField] private float rotateSpeed= 50;
	[SerializeField] private float floatAmplitude=2.0f;
	[SerializeField] private float floatFrequency = 0.5f;
	private Vector3 startPos;

	// Use this for initialization
	void Start () {
		if(gameObject.tag == "meta"){
			rotateSpeed= 70;
			floatAmplitude=5.0f;
			floatFrequency = 2f;
		}
		else if(gameObject.tag == "arma2"){
			rotateSpeed= 60;
			floatAmplitude=10.0f;
			floatFrequency = 4f;
		}
		else if(gameObject.tag == "arma3"){
			rotateSpeed= 50;
			floatAmplitude=2.0f;
			floatFrequency = 0.5f;
		}
		startPos = transform.position;
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
		Vector3 tempPos = startPos;
		tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;
		transform.position = tempPos;
	}
}
