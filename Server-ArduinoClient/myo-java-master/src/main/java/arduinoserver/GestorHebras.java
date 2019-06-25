package arduinoserver;

import static arduinoserver.Procesador.danioYaInflingido;
import com.thalmic.myo.AbstractDeviceListener;
import com.thalmic.myo.Hub;
import com.thalmic.myo.Myo;
import com.thalmic.myo.Pose;
import java.io.IOException;
import java.util.logging.Level;
import java.util.logging.Logger;

public class GestorHebras extends Thread{
    
    private Procesador procesador;
    private int tipoCliente;
    
    public GestorHebras(Procesador proc, int contador)
    {
        procesador = proc;
        tipoCliente = contador;
    }

    public void run(){
      //diferenciamos si el cliente es arduino o la app de unity
      if(tipoCliente % 2 == 0)
      {
            Thread t = procesador;
            Hub hub = new Hub("com.example.HelloMyo");
            Myo myo = hub.waitForMyo(10000);
            hub.addListener(new AbstractDeviceListener() {
                @Override
                public void onPose(Myo myo, long timestamp, Pose pose) {
                    System.out.println(String.format("Myo switched to pose %s.", pose.toString()));
                    if(pose.toString().contains("FIST")){
                        procesador.inflingirDanioEnemigo = true;
                    }
                    else if(pose.toString().contains("REST"))
                    {
                        procesador.procesa(' ');
                        if(procesador.danioYaInflingido){
                            procesador.danioYaInflingido=false;
                            procesador.stopFire = true;
                        }
                        
                    }
                    else if(pose.toString().contains("SPREAD"))
                    {
                        procesador.procesa('w');
                    }
                    else if(pose.toString().contains("IN"))
                    {
                        procesador.procesa('a');
                    }
                    else if(pose.toString().contains("OUT"))
                    {
                        procesador.procesa('d');
                    }
                    else
                    {
                        procesador.procesa(' ');
                    }
                }
            });
            while (true) {
                try {
                    if(procesador.inputStream.available() == 0)
                        hub.run(1000 / 20);
                    else{
                        procesador.procesa('l');
                    }
                   
                } catch (IOException ex) {
                    Logger.getLogger(GestorHebras.class.getName()).log(Level.SEVERE, null, ex);
                }
            }
      }
      else{
          procesador.procesa('r');
      }
      
      
    }
}