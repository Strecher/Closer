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
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
   
    public partial class mainForm : Form
    {

        public static bool stream_enable = false;

        //метод, выключающий ридонли
        public void ReadOnlyFalse()
        {
            hostBox.ReadOnly = false;
            portBox.ReadOnly = false;
        }



        public mainForm()
        {
            InitializeComponent();

            //создаем поток для клиента 
            //ThreadPool.QueueUserWorkItem(delegate { new Client(hostBox.Text, Convert.ToInt32(portBox.Text)).Start(); });
        }
      
        private void startButton_Click(object sender, EventArgs e)
        {
            stream_enable = !stream_enable;
            if (stream_enable == true) 
            {
                hostBox.ReadOnly = true;
                portBox.ReadOnly = true;
                
                    ThreadPool.QueueUserWorkItem(delegate 
                    {
                        new Client(hostBox.Text, Convert.ToInt32(portBox.Text), stream_enable).Start();
                       // Client.ReadOnly += mainForm.ReadOnlyFalse;
                    });
                   
            }
            else
            {
                hostBox.ReadOnly = false;
                portBox.ReadOnly = false;
                //остановить поток
            }
           
        }


        private void mainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            stream_enable = false;
        }

       
    }
}
