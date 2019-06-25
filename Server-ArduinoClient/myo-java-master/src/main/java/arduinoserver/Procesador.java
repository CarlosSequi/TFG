package arduinoserver;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.nio.charset.StandardCharsets;
import java.util.Random;
import java.util.Scanner;

public class Procesador extends Thread{
	// stream de lectura (por aquí se recibe lo que envía el cliente)
	public DataInputStream inputStream;
	// stream de escritura (por aquí se envía los datos al cliente)
	private DataOutputStream outputStream;
	// Para que la respuesta sea siempre diferente, usamos un generador de números aleatorios.
	private Random random;
        static char distancia;
        static boolean inflingirDanioEnemigo;
        static boolean danioYaInflingido;
        static boolean stopFire;
        int distanciaAnterior;
	
	// Constructor que tiene como parámetro una referencia al socket abierto en por otra clase
	public Procesador(DataInputStream a, DataOutputStream b) {
		random=new Random();
                inputStream = a;
                outputStream = b;
                distancia = 70;
                distanciaAnterior = distancia;
                inflingirDanioEnemigo = false;
                danioYaInflingido = false;
	}
	
	// Aquí es donde se realiza el procesamiento realmente:
	public void procesa(char c){
            // Como máximo leeremos un bloque de 1024 bytes. Esto se puede modificar.
            byte [] datosRecibidos=new byte[1024];
            int bytesRecibidos=0;
        
            try {
                String textReceived;
		byte [] buffer = new byte[2];
		//System.in.read(buffer);
                if(c == 'r') // leemos datos de la app
                {
                   while(true){
                        //inputStream.read(buffer,0,buffer.length-1);
                        //String str = new String(buffer, "UTF-8");
                        //System.out.println(str);
                       if(inflingirDanioEnemigo)
                       {
                           System.out.println("senial de disparo para tanque1");
                           inflingirDanioEnemigo = false;
                           buffer[0] = (byte)'f';
                           outputStream.write(buffer,0,buffer.length-1);
                           System.out.println("senial de disparo para tanque2");
                           danioYaInflingido = true;
                       }
                       else if(stopFire)
                       {
                           stopFire = false;
                           buffer[0] = (byte)'r';
                           outputStream.write(buffer,0,buffer.length-1);
                           System.out.println("senial de STOP disparo para tanque");
                       }
                       else if(distancia != distanciaAnterior)
                       {
                           distanciaAnterior = distancia;
                            buffer[0] = (byte)distancia;
                            outputStream.write(buffer,0,buffer.length-1);
                            System.out.println("datos enviados hacia Unity");
                       }
                   }
                }
                else if(c == 'l') // lectura datos distancia arduino
                {
                    String cadena = "";
                    while(inputStream.available() > 0)
                    {
                        inputStream.read(buffer,0,buffer.length-1);
                        cadena += new String(buffer, "UTF-8");
                        //System.out.println(cadena);
                        
                    }
                    //distancia = Character.getNumericValue(cadena.charAt(0));
                    distancia = cadena.charAt(0);
                    System.out.println(distancia);
                }
                else //movimiento arduino
                {
                    buffer[0] = (byte)c;
                    outputStream.write(buffer,0,buffer.length-1);
                    System.out.println("datos enviados hacia cliente");
                }
                             	
            } catch (IOException e) {
		System.err.println("Error al obtener los flujos de entrada/salida.");
            }   
            
	}
}

