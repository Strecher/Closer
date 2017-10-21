using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //создаем PictureBox для отображения скриншотов
            var pb = new PictureBox { Parent = this, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom };

            //создаем поток для клиента (на самом деле это должно запускаться на машине клиента, но для теста, клиент запускается здесь)
            ThreadPool.QueueUserWorkItem(delegate { new Client().Start(); });

            //создаем поток для сервера
            ThreadPool.QueueUserWorkItem(
            delegate
            {
                //получаем в цикле скриншоты с клиента
                foreach (var bmp in new Server().GetScreenshots())
                {
                    //уничтожаем предыдущее изображение
                    if (pb.Image != null) pb.Image.Dispose();
                    //заносим скриншот в PictureBox
                    pb.Image = bmp;
                }
            });
        }
        class Server
        {
            public IEnumerable<Image> GetScreenshots(int port = 24432)
            {
                var list = new TcpListener(port);
                list.Start();

                using (var tcp = list.AcceptTcpClient())//принимаем конект
                using (var stream = tcp.GetStream())//создаем сетевой поток
                using (var br = new BinaryReader(stream)) //создаем BinaryReader
                    while (true)//делаем бесконечно
                    {
                        //принимаем длину массива
                        var len = br.ReadInt32();
                        //принимаем массив
                        var arr = br.ReadBytes(len);
                        using (var ms = new MemoryStream(arr))//создаем временный поток для сжатого изображения
                        {
                            //создаем изображение
                            yield return Bitmap.FromStream(ms);
                        }
                    }
            }
        }

        class Client
        {
            public void Start(string host = "localhost", int port = 24432)
            {
                //размеры экрана
                var rect = Screen.PrimaryScreen.Bounds;

                //конкетимся к серверу, получаем поток
                using (var tcp = new TcpClient(host, port)) //создаем TcpClient
                using (var stream = tcp.GetStream()) //получаем сетевой поток
                using (var bw = new BinaryWriter(stream)) //создаем BinaryWriter
                using (var bmp = new Bitmap(rect.Width, rect.Height)) //создаем битмап для отправки
                using (var gr = Graphics.FromImage(bmp)) //создаем канву
                using (var ms = new MemoryStream()) //создаем временный поток для сжатого изображения
                    while (true) //делаем бесконечно
                    {
                        //захватываем изображение экрана
                        gr.CopyFromScreen(rect.Left, rect.Top, 0, 0, rect.Size);
                        //конвертируем изображение в массив байт в формате jpeg
                        ms.Position = 0;
                        bmp.Save(ms, ImageFormat.Jpeg);
                        var arr = ms.ToArray(); //получаем массив байт
                                                //отправляем длину массива данных
                        bw.Write(arr.Length);
                        //отправляем массив
                        bw.Write(arr);
                        //точно, отправялем
                        bw.Flush();
                    }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
