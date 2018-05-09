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
    public partial class ClientForm : Form
    {
        public ClientForm()
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


        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
