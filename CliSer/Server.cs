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
        public void Start(string host = "localhost", int port = 24432)
        {
            //порт и хост сигнала равны для теста на локалке. иначе нужно приравнять к порту и хосту этого компьютера
            var signal_host = Encoding.UTF8.GetBytes(host); 
            var signal_port = Encoding.UTF8.GetBytes(port.ToString());

            //коннектимся к клиенту, отправляем сигнал для включения TCP-лучей в нашу сторону
            using (var udpSignal = new UdpClient(host, port)) //создаем UdpClient
                while (true)
                {
                    using (var memoryStream = new MemoryStream())  //создаем временный поток для айпи
                    {
                        //отправляем айпи этого компьютера
                       // memoryStream.WriteByte((byte)(signal_host));
                       // memoryStream.WriteByte((byte)(signal_port));
                        //получаем массив байт
                       // byte[] streamArray = memoryStream.ToArray();
                        udpSignal.Send(signal_host, signal_host.Length);//отправляем host
                        Thread.Sleep(10);//если убрать эту задержку, то не все UDP пакеты приходит, почему - хз
                        udpSignal.Send(signal_port, signal_port.Length);//отправляем host
                        Thread.Sleep(10);//если убрать эту задержку, то не все UDP пакеты приходит, почему - хз [2]
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
    }

    public class Chunk
    {
        public Point Position;
        public Image Image;
    }
}



