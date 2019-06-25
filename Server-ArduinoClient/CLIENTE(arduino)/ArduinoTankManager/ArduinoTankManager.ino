/*
 *
 *  Client side for Simple Connection (Always Connected) 
 *
 */

#include "ESP8266_TCP.h"
#include <SoftwareSerial.h>
#include <Servo.h>

#define tx 10
#define rx 11

Servo servo_derecha;
Servo servo_izquierda;
int buttonState = 0;
long duracion;
int distancia;
bool objetoDetectado;
bool senialEnviada;

const int ledPIN = 6;

const int signalPin= 7;
#define trigPin 9
#define echoPin 8

#define pin_derecha 2
#define pin_izquierda 3

// ESP8266 Class
ESP8266_TCP wifi;
char caracter_recibido;

SoftwareSerial Serial1(rx,tx); // make RX Arduino line is pin 2, make TX Arduino line is pin 3.
                             // This means that you need to connect the TX line from the esp to the Arduino's pin 2
                             // and the RX line from the esp to the Arduino's pin 3
String cadena;
// Target Access Point
#define ssid         "ESP_AP"
#define pass         "123456789"

// TCP Server IP and port
#define serverIP    "192.168.4.2"
#define serverPort  8989

// Connect this pin to CH_PD pin on ESP8266
#define PIN_RESET    6

// Pin that connected to button to send any message
#define PIN_SEND     8

void setup()
{
  delay(3000);

  // seteamos el codigo para el funcionamiento del led
  Serial.begin(9600);    //iniciar puerto serie
  pinMode(ledPIN , OUTPUT);  //definir pin como salida

  caracter_recibido = ' ';
  
  // Set pin for send command to input mode
  pinMode(PIN_SEND, INPUT);
  
  // We use Serial1 to interface with ESP8266 
  // and use Serial to debugging
  Serial.begin(115200);
  Serial1.begin(115200);
  wifi.begin(&Serial1, &Serial, PIN_RESET);
  
  servo_derecha.attach(pin_derecha);
  servo_izquierda.attach(pin_izquierda);
  pinMode(trigPin, OUTPUT);
  pinMode(echoPin, INPUT);
  objetoDetectado = false;
  senialEnviada = false;
}

void loop()
{   
  
 
  // Auto connect to TCP Server Side when connection timeout
  if(wifi.getRunningState() == WIFI_STATE_UNAVAILABLE) {
    // Connect to TCP Server Side
    Serial.println("Connect!!");
    delay(500);
    wifi.connectTCP(serverIP, serverPort);
    delay(500);
  }
  else{

    // enviamos la informacion de distancia del sensor al servidor
    double distancia = distanciaSensor();
    // imprimimos dicha distancia
    if(distancia < 10 && !objetoDetectado)
    {
      objetoDetectado = true;
    }
    else if(distancia >= 10){
      objetoDetectado = false;
      senialEnviada = false;
    }
  
    if(objetoDetectado && !senialEnviada)
    {
      Serial1.write(distancia);
      // enviamos al puerto serie un 1 para indicar que se ha detectado algo
      wifi.send((String)distancia);
      senialEnviada = true;
      delay(1000);
    }     

    // Recibimos del servidor los comandos de movimiento para el robot
    cadena = "";
    if(Serial1.available()) // check if the esp is sending a message 
    {
      //Serial.write("\nesperando...");
      while(Serial1.available())
      {
        // The esp has data so display its output to the serial window 
        char c = Serial1.read(); // read the next character.
        cadena+=c;
      }  
      //Serial.write(cadena.c_str());
      caracter_recibido = cadena[cadena.length()-1];
      Serial.write(caracter_recibido);
      if(caracter_recibido != ' ')
        caracter_recibido = toupper(caracter_recibido);
    }
  }

  
  // ejecutamos la orden recibida por el servidor
  
  if((caracter_recibido <= 'X')&&(caracter_recibido >= 'V'))
  {
    avanzar();
  }
  else if((caracter_recibido <= 'E')&&(caracter_recibido >= 'C'))
  {
    giroDer();
  }
  else if((caracter_recibido <= 'B')&&(caracter_recibido >= '@'))
  {
    giroIzq();
  }
  else if((caracter_recibido <= 'T')&&(caracter_recibido >= 'R'))
  {
    retroceder();
   
  }
  else if(caracter_recibido == ' ')
    detener();
  
  

}






void avanzar()
{
  servo_derecha.write(55);
  servo_izquierda.write(180);
}

void retroceder()
{
  servo_derecha.write(145);
  servo_izquierda.write(0);
}

void giroIzq()
{
  servo_derecha.write(55);
  servo_izquierda.write(0);
}

void giroDer()
{
  servo_derecha.write(145);
  servo_izquierda.write(180);
}

void detener()
{
  
  servo_derecha.write(82);
  servo_izquierda.write(87);
}

double distanciaSensor()
{
  digitalWrite(trigPin, LOW);     // Nos aseguramos de que el trigger está desactivado
  delayMicroseconds(2);           // Para estar seguros de que el trigger ya está LOW
  digitalWrite(trigPin, HIGH);    // Activamos el pulso de salida
  delayMicroseconds(10);          // Esperamos 10µs. El pulso sigue active este tiempo
  digitalWrite(trigPin, LOW);
  // Cortamos el pulso y a esperar el echo
  // leemos el pin de entrada, el cual devuelve la onda de sonido en microsegundos
  duracion = pulseIn(echoPin, HIGH);

  // calculamos la distancia
  distancia = duracion*0.034/2;

  return distancia;
}
