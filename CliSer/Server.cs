using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace WindowsFormsApp1
{

    //TCP
    public class Server
    {
        // в методе идёт передача сигнала для включения потока TCP на клиенте
        public void SendSignal(string host = "192.168.1.12", int port = 24432)

 
        {
            //отправляемые свой порт и хост 
            var signal_host = Encoding.UTF8.GetBytes("192.168.1.98");
            var signal_port = Encoding.UTF8.GetBytes(port.ToString());

            int k = 0;
            //коннектимся к клиенту, отправляем сигнал для включения TCP-лучей в нашу сторону
            using (var udpSendSignal = new UdpClient("192.168.1.12", port)) //создаем UdpClient
               
                while (k < 100)
                {

                    using (var memoryStreamSend = new MemoryStream())  //создаем временный поток для айпи
                    {
                        /* отправляем айпи этого компьютера
                         memoryStream.WriteByte((byte)(signal_host));
                        memoryStream.WriteByte((byte)(signal_port));
                        получаем массив байт
                       byte[] streamArray = memoryStream.ToArray();
                       */


                        udpSendSignal.Send(signal_host, signal_host.Length);//отправляем свой host
                        Thread.Sleep(100);//если убрать эту задержку, то не все UDP пакеты приходят из-за высокой скорости отправки.

                        //можно попробовать прервать, если зависнет на двух компьютерах 
                        //break;

                        /* //порт можно не передавать
                        udpSendSignal.Send(signal_port, signal_port.Length);//отправляем host
                        Thread.Sleep(10);//если убрать эту задержку, то не все UDP пакеты приходит, почему - хз [2]
                        */
                    }
                k = k + 1;
                }

        }






        public  IEnumerable<Image> GetScreenshots(int port = 24432)
            {
                var list = new TcpListener(port);
                list.Start();
                using (var tcp = list.AcceptTcpClient())//принимаем конект
                using (var stream = tcp.GetStream())//создаем сетевой поток
                using (var br = new BinaryReader(stream)) //создаем BinaryReader
                    while (true)//делаем бесконечно
                    {
                        //принимаем длину 
                        int len = br.ReadInt32();
                        //принимаем массив
                        byte[] arr = br.ReadBytes(len);
                        using (var ms = new MemoryStream(arr))//создаем временный поток для сжатого изображения
                        {
                            //создаем изображение
                            yield return Bitmap.FromStream(ms);
                        }
                        
                    }
            }

    }
        




    /*     
                 public IEnumerable<Image> GetScreenshots(int port = 24432)
                 {
                     var list = new TcpListener(port);
                     list.Start();

                     using (var tcp = list.AcceptTcpClient())//принимаем конект
                     using (var stream = tcp.GetStream())//создаем сетевой поток
                     using (var br = new BinaryReader(stream)) //создаем BinaryReader
                         while (true)//делаем бесконечно
                         {
                             //принимаем длину 
                             int len = br.ReadInt32();
                             //принимаем массив
                             byte[] arr = br.ReadBytes(len);
                             using (var ms = new MemoryStream(arr))//создаем временный поток для сжатого изображения
                             {
                                 //создаем изображение
                                 yield return Bitmap.FromStream(ms);
                             }

                         }
                 }
     */

    /*
     //UDP приём скринов
      public IEnumerable<Chunk> GetScreenshots(int port = 24432)
     {
         using (var udp = new UdpClient(port))

             while (true)//делаем бесконечно
             {
                 //принимаем массив
                 IPEndPoint ip = null;
                 var arr = udp.Receive(ref ip);

                 //читаем координаты
                 var x = arr[0] * Client.CHUNK_SIZE;
                 var y = arr[1] * Client.CHUNK_SIZE;

                 using (var memoryStream = new MemoryStream(arr, 2, arr.Length - 2))//создаем временный поток для сжатого изображения
                 {
                     //создаем изображение
                     var bitmap = Bitmap.FromStream(memoryStream);
                     //возвращаем
                     yield return new Chunk { Position = new Point(x, y), Image = bitmap };
                 }
             }
     }
      */
}



