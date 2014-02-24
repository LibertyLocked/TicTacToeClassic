using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Net.Sockets;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;

namespace TicTacToeClassic
{
    public class MP
    {
        public TcpListener tcpListener;
        public TcpClient tcpClient = new TcpClient();
        public NetworkStream newStream;      // Server
        public StreamReader sr;
        public StreamWriter sw;
        public bool service = false;
        private string ipYou;
        public List<Label> labelList = new List<Label>();
        public bool isGameOver = false;
        public bool myTurn = false;
        public string flagStyle = "";
        public Label myTurnLabel = new Label();
        

        public NetworkStream netStream;      // Client

        public MP()
        {
            InitializeMPGame();
        }

        public void InitializeMPGame()
        {
            labelList.Clear();
            
            Label b1 = new Label();
            b1.Size = new Size(80, 80);             // Line 1 Col 1
            b1.Location = new Point(20, 10);

            #region gen b2 to b9
            Label b2 = new Label();
            b2.Size = b1.Size;                      // Line 1 Col 2
            b2.Location = new Point(b1.Location.X + b1.Size.Width, b1.Location.Y);

            Label b3 = new Label();
            b3.Size = b1.Size;                     // Line 1 Col 3
            b3.Location = new Point(b1.Location.X + b1.Size.Width * 2, b1.Location.Y);

            Label b4 = new Label();
            b4.Size = b1.Size;                     // Line 2 Col 1
            b4.Location = new Point(b1.Location.X, b1.Location.Y + b1.Height);

            Label b5 = new Label();
            b5.Size = b1.Size;                     // Line 2 Col 2
            b5.Location = new Point(b1.Location.X + b1.Size.Width, b1.Location.Y + b1.Height);

            Label b6 = new Label();
            b6.Size = b1.Size;                      // Line 2 Col 3
            b6.Location = new Point(b1.Location.X + b1.Size.Width * 2, b1.Location.Y + b1.Height);

            Label b7 = new Label();
            b7.Size = b1.Size;                     // Line 3 Col 1
            b7.Location = new Point(b1.Location.X, b1.Location.Y + b1.Height * 2);

            Label b8 = new Label();
            b8.Size = b1.Size;                     // Line 3 Col 2
            b8.Location = new Point(b1.Location.X + b1.Size.Width, b1.Location.Y + b1.Height * 2);
            //b8.Text = "8";

            Label b9 = new Label();
            b9.Size = b1.Size;                     // Line 3 Col 3
            b9.Location = new Point(b1.Location.X + b1.Size.Width * 2, b1.Location.Y + b1.Height * 2);
            #endregion

            labelList.Add(b1);
            labelList.Add(b2);
            labelList.Add(b3);
            labelList.Add(b4);
            labelList.Add(b5);
            labelList.Add(b6);
            labelList.Add(b7);
            labelList.Add(b8);
            labelList.Add(b9);
            foreach (Label l in labelList)
            {
                l.Text = "";
                l.BorderStyle = BorderStyle.FixedSingle;
                l.TextAlign = ContentAlignment.MiddleCenter;
                l.ForeColor = Color.Purple;
                l.Font = new System.Drawing.Font("Arial", 40F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                l.BackColor = Color.White;
                l.Click += new EventHandler(block_Click);
                l.MouseEnter += new EventHandler(block_MouseEnter);
                l.MouseLeave += new EventHandler(block_MouseLeave);
            }
            isGameOver = false;
        }

        #region block effects
        private void block_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.White;
        }

        private void block_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Yellow;
        }
        #endregion

        private void block_Click(object sender, EventArgs e)
        {
            if (((Label)sender).Text != "" || isGameOver || !myTurn) return;
            if (flagStyle == "O")     // Client
            {
                ((Label)sender).Text = flagStyle;

                this.tcpClient = new TcpClient();
                this.tcpClient.Connect(this.GetIPAddressYou, 6666);
                
                this.sw = new StreamWriter(this.tcpClient.GetStream());
                //MessageBox.Show(labelList.IndexOf((Label)sender).ToString());
                this.sw.WriteLine(labelList.IndexOf((Label)sender));
                sw.Flush();
                this.tcpClient.Close();
                if (ClientWinningCheck())
                {
                    isGameOver = true;
                    ShowMessageBox("You win");
                    RefreshStart();
                }
                if (CheckDraw()) RefreshStart();
            }
            else if (flagStyle == "X") //  Host
            {
                ((Label)sender).Text = flagStyle;

                this.tcpClient = new TcpClient();
                this.tcpClient.Connect(this.GetIPAddressYou, 7777);
                
                this.sw = new StreamWriter(this.tcpClient.GetStream());
                //MessageBox.Show(labelList.IndexOf((Label)sender).ToString());
                this.sw.WriteLine(labelList.IndexOf((Label)sender));
                sw.Flush();
                this.tcpClient.Close();
                if (HostWinningCheck())
                {
                    isGameOver = true;
                    ShowMessageBox("You win");
                    RefreshStart();
                }
                if (CheckDraw()) RefreshStart();
            }
            myTurn = false;
            myTurnLabel.Text = "TURN:\nOpponent";
        }

        public string GetIPAddressMy()
        {
            string ipv4 = String.Empty;
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = IPA.ToString();
                    break;
                }
            }
            if (ipv4 != String.Empty)
            {
                return ipv4;
            }
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = IPA.ToString();
                    break;
                }
            }
            return ipv4;
        }


        public string GetIPAddressYou
        {
            get
            {
                return this.ipYou;
            }
            set
            {
                this.ipYou = value;
            }
        }

        public bool ClientWinningCheck()
        {
            if (labelList[0].Text == "O" && labelList[1].Text == "O" && labelList[2].Text == "O")
            {
                return true;
            }
            else if (labelList[3].Text == "O" && labelList[4].Text == "O" && labelList[5].Text == "O")
            {
                return true;
            }
            else if (labelList[6].Text == "O" && labelList[7].Text == "O" && labelList[8].Text == "O")
            {
                return true;
            }
            else if (labelList[0].Text == "O" && labelList[3].Text == "O" && labelList[6].Text == "O")
            {
                return true;
            }
            else if (labelList[1].Text == "O" && labelList[4].Text == "O" && labelList[7].Text == "O")
            {
                return true;
            }
            else if (labelList[2].Text == "O" && labelList[5].Text == "O" && labelList[8].Text == "O")
            {
                return true;
            }
            else if (labelList[0].Text == "O" && labelList[4].Text == "O" && labelList[8].Text == "O")
            {
                return true;
            }
            else if (labelList[2].Text == "O" && labelList[4].Text == "O" && labelList[6].Text == "O")
            {
                return true;
            }
            else return false;
        }

        public bool HostWinningCheck()
        {
            if (labelList[0].Text == "X" && labelList[1].Text == "X" && labelList[2].Text == "X")
            {
                return true;
            }
            else if (labelList[3].Text == "X" && labelList[4].Text == "X" && labelList[5].Text == "X")
            {
                return true;
            }
            else if (labelList[6].Text == "X" && labelList[7].Text == "X" && labelList[8].Text == "X")
            {
                return true;
            }
            else if (labelList[0].Text == "X" && labelList[3].Text == "X" && labelList[6].Text == "X")
            {
                return true;
            }
            else if (labelList[1].Text == "X" && labelList[4].Text == "X" && labelList[7].Text == "X")
            {
                return true;
            }
            else if (labelList[2].Text == "X" && labelList[5].Text == "X" && labelList[8].Text == "X")
            {
                return true;
            }
            else if (labelList[0].Text == "X" && labelList[4].Text == "X" && labelList[8].Text == "X")
            {
                return true;
            }
            else if (labelList[2].Text == "X" && labelList[4].Text == "X" && labelList[6].Text == "X")
            {
                return true;
            }
            else return false;
        }

        public void RefreshStart()
        {
            foreach (Label l in labelList)
            {
                l.Text = "";
            }
            isGameOver = false;
        }

        public bool CheckDraw()
        {
            int count = 0;
            foreach (Label l in labelList)
            {
                if (l.Text == "") return false;
                else
                {
                    count++;
                    if (count == 9)
                    {
                        isGameOver = true;
                        ShowMessageBox("Draw");
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IPCheck(string ip)
        {
            bool b = true;
            string[] lines = new string[4];
            string s = ".";
            lines = ip.Split(s.ToCharArray(), 4);//分隔字符串
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (Convert.ToInt32(lines[i]) >= 255 || Convert.ToInt32(lines[i]) < 0)
                    {
                        b = false;
                        return b;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return b;
        }

        public Label Gen_MP_MyTurnLabel()
        {
            myTurnLabel.Location = new Point(290, 20);
            myTurnLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            myTurnLabel.AutoSize = true;
            if (myTurn)
                myTurnLabel.Text = "TURN:\nYou";
            else myTurnLabel.Text = "TURN:\nOpponent";
            return myTurnLabel;
        }

        public void ShowMessageBox(string message)
        {
            Thread thread = new Thread(
            () =>
            {
                MessageBox.Show(message);
            });
            thread.Start();
        }

    }
}
