package arduinoserver;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;
import GUI.RobotControlPanel;
import java.io.DataInputStream;
import java.io.DataOutputStream;

public class Servidor {
    
    //static char movimientoMyo = 'c';
    
    public static void main(String[] args)throws IOException  {
        
                // Puerto de escucha
		int port=8989;   
                ServerSocket socketServidor = new ServerSocket(port);
                int contador = 0;
	
            while(true)
            {
                Socket s = null;
		try {
                    // Aceptamos una nueva conexión del cliente 1 con accept()
                    s=socketServidor.accept();
                    System.out.println("Peticion de conexion del cliente aceptada" + s);
                    DataInputStream dis = new DataInputStream(s.getInputStream());
                    DataOutputStream dos = new DataOutputStream(s.getOutputStream());
                    Procesador procesador=new Procesador(dis,dos);
                    Thread t = new GestorHebras(procesador,contador);
                    contador++;
                    t.start();
                    
		} catch (IOException e) {
			System.err.println("Error al escuchar en el puerto "+port);
		}
            }

	}
    
}