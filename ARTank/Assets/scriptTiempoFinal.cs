using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;

public class scriptTiempoFinal : MonoBehaviour {

	public Text tiempoFinal;

	void Start() {
        tiempoFinal = GameObject.Find("TextoTiempo").GetComponent<Text>();
    }

	// Use this for initialization
	void Update () {
		tiempoFinal.text = scriptDatosFinalJuego.time.ToString("f0");
	}
	
}
