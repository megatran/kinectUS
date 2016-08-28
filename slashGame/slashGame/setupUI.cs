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
using System.Threading;
namespace slashGame {
    public partial class setupUI : Form {

        public Form1 f1Parent = null;
        IPAddress myIp = null;

        // TcpClient myClient = null;
        TcpListener tcpListener = null;
        //IPAddress slaveIp;
        TcpClient myClient = null;


        int defport = 32767;
        public setupUI() {
            InitializeComponent();

            try {
                var myHost = Dns.GetHostEntry(Dns.GetHostName());
                var host = Dns.GetHostEntry(Dns.GetHostName());
                foreach(var ip in host.AddressList) {
                    if(ip.AddressFamily == AddressFamily.InterNetwork) {
                        myIp = ip;
                    }
                }


            } catch(System.Exception err) {
                System.Diagnostics.Debug.Print(err.Message);
                myIp = null;
            };

        }

        private void setupUI_Load(object sender, EventArgs e) {
            this.toolStripStatusLabel1.Text = myIp.ToString() + ":" + defport;
            this.textBox1.Text = myIp.ToString() + ":" + defport.ToString();
        }

        private void button3_Click(object sender, EventArgs e) {
            Clipboard.SetText(this.toolStripStatusLabel1.Text);
        }





        private void button2_Click(object sender, EventArgs e) {
            if(int.TryParse(this.tbListenPort.Text, out SelectedListenPort)) {
                Thread t = new Thread(new ThreadStart(ListenThread));
                t.Start();
            }
        }

        public int SelectedListenPort;

        public void ListenThread() {

            // Create an instance of the TcpListener class.
            if(tcpListener != null)
                return;
            try {
                // Set the listener on the local IP address 
                // and specify the port.
                tcpListener = new TcpListener(myIp, SelectedListenPort);
                tcpListener.Start();
                //     this.textBox2.Invoke(
                //           new MethodInvoker(delegate
                //          {
                //             // Show the current time in the form's title bar.
                //            this.textBox2.Text += (DateTime.Now.ToLongTimeString() + "\r\n");
                //       }));

            } catch(Exception e) {
                System.Diagnostics.Debug.Print(e.Message);
                //       this.textBox2.Invoke(
                //             new MethodInvoker(delegate
                //            {
                //              // Show the current time in the form's title bar.
                //            this.textBox2.Text += e.Message + "\r\n";
                //      }));
            }
            while(true) {
                if(this.IsDisposed)
                    break;

                Thread.Sleep(10);
                // Create a TCP socket. 
                // If you ran this server on the desktop, you could use 
                // Socket socket = tcpListener.AcceptSocket() 
                // for greater flexibility.
                var c = tcpListener.AcceptTcpClient();
                var rs = c.GetStream();
                int trycount = 0;
                while(!rs.DataAvailable && trycount++ < 10)
                    System.Threading.Thread.Sleep(100);

                System.IO.StreamReader sr = new System.IO.StreamReader(rs);
                var s = sr.ReadLine();
                for(int i = 0; i < s.Length; i++) {

                    KeyEventArgs MyKey = new KeyEventArgs(Keys.D);
                    if(f1Parent != null) {
                        f1Parent.keyDownListener(null, MyKey);
                        System.Threading.Thread.Sleep(50);
                        f1Parent.keyUpListener(null, MyKey);
                    }
                }



            }

        }
        int SelectedPort;
        IPAddress SelectedIP;

        private void buttonConnect_Click(object sender, EventArgs e) {
            var parse = this.textBox1.Text.Split(':');
            if(parse.Length != 2 || !int.TryParse(parse[1], out SelectedPort) || !IPAddress.TryParse(parse[0], out SelectedIP))
                return;



            myClient = new System.Net.Sockets.TcpClient();
            try {
                myClient.Connect(SelectedIP, SelectedPort);
                if(myClient.Connected) {
                    this.Text = "Conected to " + SelectedIP.ToString() + ":" + SelectedPort.ToString();
                    var sw = new System.IO.StreamWriter(myClient.GetStream());
                    sw.WriteLine("Message from ..." + System.Diagnostics.Process.GetCurrentProcess().Id);
                    sw.Flush();

                    //myClient.Close(); 
                }
            } catch(System.Exception err) {
                // this.textBox1.Text = err.Message + "\r\n";
                System.Diagnostics.Debug.Print(err.Message);
            }

        }
    }
}
