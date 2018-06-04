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
using System.Net;
using WindowsFormsApp1;


namespace WindowsFormsApp1
{
    public partial class ClientForm : Form
    {
        public ClientForm()
        {
            InitializeComponent();

            //создаем Image для отображения скриншота и Graphics для него
         //   var screenshotBitmap = new Bitmap(1300, 1300);
         //   var screenshot = Graphics.FromImage(screenshotBitmap);

            //создаём переменную для получения айпи
            string receivedIp = "0";
            hostname.Text = receivedIp;

            //создаем PictureBox для отображения скриншотов
        //    var screenshotZone = new PictureBox { Parent = this, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom, Image = screenshotBitmap };

            //определяем сервер
           // var Server = new Server();

            //сервер посылает сигнал:
         //   Server.SendSignal("localhost",24432);

            //определяем клиент
            var tempClient = new Client();

            //Клиенту создаем поток для получения сигнала
            ThreadPool.QueueUserWorkItem(
            delegate
            {
                //Клиент получает сигнал от сервера
                foreach (var signal in tempClient.GetSignal())
                {
                    receivedIp = signal;
                    hostname.Text = receivedIp;

                    if (receivedIp != "0")
                    {
                        ThreadPool.QueueUserWorkItem(delegate { new Client("192.168.1.98", 24432).Start(); });
                    }

                }
                // сигнал получен и записан!
            });

            //создаем поток для клиента (на самом деле это должно запускаться на машине клиента, но для теста, клиент запускается здесь)
           // ThreadPool.QueueUserWorkItem(delegate { new Client(receivedIp, 24432).Start(); });

         /*  //создаем поток для сервера
            ThreadPool.QueueUserWorkItem(
            delegate
            {
                //получаем в цикле скриншоты с клиента
                foreach (var chunk in Server.GetScreenshots())
                {

                    //уничтожаем предыдущее изображение
                    if (screenshotZone.Image != null) screenshotZone.Image.Dispose();
                    //заносим скриншот в PictureBox
                    screenshotZone.Image = chunk;

            */

                    /* //заносим скриншот в PictureBox для чанков
                     gr.DrawImage(chunk.Image, chunk.Position);
                      pb.Invalidate();
                      */
            //    }
          //  }); 
        }

        /* public ClientForm()
         {
             InitializeComponent();

             //создаем PictureBox для отображения скриншотов
             var pb = new PictureBox { Parent = this, Dock = DockStyle.Fill, SizeMode = PictureBoxSizeMode.Zoom };

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
         }*/



        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
