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
    }
}