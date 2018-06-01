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
using System.Drawing.Imaging;
using System.Threading;

namespace WindowsFormsApp1
{
      public class Client
    {
        /*
          public const int CHUNK_SIZE = 2048;

          public void Start(string host = "localhost", int port = 24432)
          {
              //размеры экрана
              var screenSize = Screen.PrimaryScreen.Bounds;
              var chunkHash = new Dictionary<Point, int>();

              //конкетимся к серверу, получаем поток
              using (var udp = new UdpClient(host, port)) //создаем UdpClient
              using (var screenshot = new Bitmap(screenSize.Width, screenSize.Height)) //создаем битмап для отправки
              using (var screenGraphics = Graphics.FromImage(screenshot)) //создаем канву
                  while (true)
                  {
                      //захватываем изображение экрана
                      screenGraphics.CopyFromScreen(screenSize.Left, screenSize.Top, 0, 0, screenSize.Size);
                      for (int x = 0; x < screenSize.Width; x += CHUNK_SIZE)
                      {
                          for (int y = 0; y < screenSize.Height; y += CHUNK_SIZE)
                          {
                              using (var bmp = screenshot.Clone(new Rectangle(x, y, CHUNK_SIZE, CHUNK_SIZE), screenshot.PixelFormat)) ;
                              using (var memoryStream = new MemoryStream())  //создаем временный поток для сжатого изображения
                              {
                                  //отправляем координаты кусочка
                                  memoryStream.WriteByte((byte)(x / CHUNK_SIZE));
                                  memoryStream.WriteByte((byte)(y / CHUNK_SIZE));
                                  //конвертируем изображение в массив в формат jpeg
                                  screenshot.Save(memoryStream, ImageFormat.Jpeg);
                                  //получаем массив байт
                                  byte[] streamArray = memoryStream.ToArray();
                                  //отправляем датаграмму (только изменившиеся кусочки)
                                  var point = new Point(x, y);
                                  if (!chunkHash.ContainsKey(point) || chunkHash[point] != streamArray.Length)//хеш кусочка изменился?
                                  {
                                      udp.Send(streamArray, streamArray.Length);//отправляем
                                      chunkHash[point] = streamArray.Length;//сохраняем новый хеш
                                      Thread.Sleep(10);//если убрать эту задержку, то не все UDP пакеты приходит, почему - хз
                                  }
                              }
                          }
                      }
                  }
          }
      } */

        //TCP CHUNKS
        /* public void Start()
                {
                    //размеры экрана
                    var screenSize = Screen.PrimaryScreen.Bounds;
                    var chunkHash = new Dictionary<Point, int>();

                //конкетимся к серверу, получаем поток
                using (var tcp = new TcpClient(host, port)) //создаем TcpClient
                using (var stream = tcp.GetStream()) //получаем сетевой поток
                using (var binaryWriter = new BinaryWriter(stream)) //создаем BinaryWriter
                using (var bmpScreen = new Bitmap(screenSize.Width, screenSize.Height)) //создаем битмап для отправки
                using (var screenshot = Graphics.FromImage(bmpScreen)) //создаем канву
                    while (stream_enable == true)
                    {
                        //захватываем изображение экрана
                        screenshot.CopyFromScreen(screenSize.Left, screenSize.Top, 0, 0, screenSize.Size);
                        for (int x = 0; x < screenSize.Width; x += CHUNK_SIZE)
                        {
                            for (int y = 0; y < screenSize.Height; y += CHUNK_SIZE)
                            {
                                using (var bmpChunk = bmpScreen.Clone(new Rectangle(x, y, CHUNK_SIZE, CHUNK_SIZE), bmpScreen.PixelFormat)) ;
                                using (var memoryStream = new MemoryStream())  //создаем временный поток для сжатого изображения
                                {
                                    //отправляем координаты кусочка
                                    memoryStream.WriteByte((byte)(x / CHUNK_SIZE));
                                    memoryStream.WriteByte((byte)(y / CHUNK_SIZE));
                                    //конвертируем изображение в массив байт в формате jpeg
                                    bmpScreen.Save(memoryStream, ImageFormat.Jpeg);
                                    byte[] streamArray = memoryStream.ToArray(); //получаем массив байт  
                                                                                 //отправляем длину массива данных
                                    binaryWriter.Write(streamArray.Length);
                                    //отправляем массив
                                    binaryWriter.Write(streamArray);
                                    //точно, отправялем
                                    binaryWriter.Flush();
                                }
                            }
                        }
                    }

                }*/

        //TCP
        public void Start()
    {
        //размеры экрана
        var screenSize = Screen.PrimaryScreen.Bounds;
        var chunkHash = new Dictionary<Point, int>();

        //конкетимся к серверу, получаем поток
        try
        {
            TcpClient tcp = new TcpClient(host, port); //создаем TcpClient
            var stream = tcp.GetStream(); //получаем сетевой поток
            var binaryWriter = new BinaryWriter(stream);
            var bmpScreen = new Bitmap(screenSize.Width, screenSize.Height);
            var screenshot = Graphics.FromImage(bmpScreen);
            var memoryStream = new MemoryStream();
            while (stream_enable == true)
            {
                try
                {
                    screenshot.CopyFromScreen(screenSize.Left, screenSize.Top, 0, 0, screenSize.Size);
                    memoryStream.Position = 0;
                    //конвертируем изображение в массив байт в формате jpeg
                    bmpScreen.Save(memoryStream, ImageFormat.Jpeg);
                    byte[] streamArray = memoryStream.ToArray(); //получаем массив байт  
                                                                 //отправляем длину массива данных
                    binaryWriter.Write(streamArray.Length);
                    //отправляем массив
                    binaryWriter.Write(streamArray);
                    //точно, отправялем
                    binaryWriter.Flush();
                    if (stream_enable == false) break;
                }
                catch (IOException e)
                {
                    MessageBox.Show("Разрыв соединения");
                    stream_enable = !stream_enable;
                    ReadOnlyFalse();
                }

            }
        }
        catch (SocketException e)
        {
            MessageBox.Show("Превышено время ожидания");
            stream_enable = !stream_enable;
            ReadOnlyFalse();
        }
    }


}