using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class HUD : MonoBehaviour {  	
	#region private members 	
	private TcpClient socketConnection; 	
	private Thread clientReceiveThread;
	#endregion  

	public int distanceToObstacle;	
	public Text tankStateMessage;
	public GameObject Fire;
	public static string textoNuevo;
	public bool activeFire;	
	public TankBehavior tank;
	public SimpleHealthBar healthBar;
	public SimpleHealthBar energyBar;


	// Use this for initialization 	
	void Start () {	
		Screen.orientation = ScreenOrientation.Portrait;
		ConnectToTcpServer(); 
		Fire.SetActive(false); 
		activeFire = false;  
		distanceToObstacle = 0;
	}  	

	// Update is called once per frame
	void Update () {      
		if (Input.GetKeyDown(KeyCode.Space)) {             
			SendMessage();     
		}    	 
		healthBar.UpdateBar(tank.getHealth(), tank.getMaxHealth());
		energyBar.UpdateBar(tank.getEnergy(), tank.getMaxEnergy());
		tankStateMessage.text = textoNuevo;
		Fire.SetActive(activeFire);
	}  	

	public void setNewText(string text)
	{
		textoNuevo = text;
	}
	/// <summary> 	
	/// Setup socket connection. 	
	/// </summary> 	
	private void ConnectToTcpServer () { 	
		try {  			
			clientReceiveThread = new Thread (new ThreadStart(ListenForData)); 			
			clientReceiveThread.IsBackground = true; 			
			clientReceiveThread.Start();
		} 		
		catch (Exception e) { 			
			Debug.Log("On client connect exception " + e); 		
		} 	
	}  	
	/// <summary> 	
	/// Runs in background clientReceiveThread; Listens for incomming data. 	
	/// </summary>     
	private void ListenForData() { 
		try { 			
			socketConnection = new TcpClient("192.168.4.2", 8989);  
			if(socketConnection == null)
				Debug.Log("NO SE HA PRODUCIDO LA CONEXION");	
			else
				Debug.Log("CONEXION REALIZADA");			
			Byte[] bytes = new Byte[1024];             
			while (true) { 				
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream()) { 					
					int length; 					
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0) { 						
						var incommingData = new byte[length]; 						
						Array.Copy(bytes, 0, incommingData, 0, length); 						
						// Convert byte array to string message. 						
						string serverMessage = Encoding.ASCII.GetString(incommingData); 
						Debug.Log("YEYEYEYEYEYEYYE-------------" + serverMessage);
						if(tank.inContactWithEnemy() && serverMessage == "f")
						{
							Debug.Log("RECIBIDA ORDEN DISPARO");
							if(tank.getEnergy() > 25)
							{
								tank.atack();
								activeFire = true;
							}
							else
							{
								textoNuevo = "YOU DON'T HAVE ENERGY ENOUGH";
							}
							
						}	
						else if(serverMessage == "r")
						{
							activeFire = false;
						}
						else
						{
							Debug.Log("EN DISTANCIA " + serverMessage);
							Int32.TryParse(serverMessage, out distanceToObstacle);	
							Debug.Log("Distancia a obstaculo: " + distanceToObstacle);
							tank.decreaseHealth(distanceToObstacle);
							if(distanceToObstacle == 0)
							{
								textoNuevo = "NO OBSTACLES";
							}
							else
							{
								textoNuevo = "DISTANCE TO OBSTACLE: " + distanceToObstacle;
							}
							
						}Debug.Log("ASE ARGO???? aj aj aj");


					} 				
				} 			
			}         
		}         
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	}  	

	/// <summary> 	
	/// Send message to server using socket connection. 	
	/// </summary> 	
	private void SendMessage() {         
		if (socketConnection == null) {          
		   Debug.Log("FOS");
			return;         
		}  		
		try { 			
			// Get a stream object for writing. 			
			NetworkStream stream = socketConnection.GetStream(); 			
			if (stream.CanWrite) {                 
				string clientMessage = "Unity"; 				
				// Convert string message to byte array.                 
				byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage); 				
				// Write byte array to socketConnection stream.                 
				stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);                 
				Debug.Log("Client sent his message - should be received by server");             
			}         
		} 		
		catch (SocketException socketException) {             
			Debug.Log("Socket exception: " + socketException);         
		}     
	} 
	

}