using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;
public class Timer : MonoBehaviour {

	public Text timerText;

	public void Start()
	{
		scriptDatosFinalJuego.time = 0.0f;
	}
	
	// Update is called once per frame
	public void Update () {
		scriptDatosFinalJuego.time += Time.deltaTime;
		timerText.text = "" + scriptDatosFinalJuego.time.ToString("f0");
	}
}
