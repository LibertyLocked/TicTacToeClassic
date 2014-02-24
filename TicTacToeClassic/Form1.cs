using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
//using System.Threading.Tasks;

namespace TicTacToeClassic
{
    public partial class Form1 : Form
    {
        int mode = 0;
        SP spGame = new SP(3);
        MP mpGame = new MP();
        TextBox ipEntryTextbox;
        
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadMenu();
        }

        #region button effects
        private void labelButton_MouseEnter(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.LightBlue;
            ((Label)sender).Font = new System.Drawing.Font("Arial", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void labelButton_MouseLeave(object sender, EventArgs e)
        {
            ((Label)sender).BackColor = Color.Pink;
            ((Label)sender).Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void labelButton_MouseDown(object sender, MouseEventArgs e)
        {
            ((Label)sender).BackColor = Color.LightGray;
            ((Label)sender).Font = new System.Drawing.Font("Arial", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }

        private void labelButton_MouseUp(object sender, MouseEventArgs e)
        {
            ((Label)sender).BackColor = Color.LightBlue;
            ((Label)sender).Font = new System.Drawing.Font("Arial", 21F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
        }
        #endregion

        private void exitButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void spButton_Click(object sender, EventArgs e)
        {
            mode = 1;
            LoadSPGame();
        }

        private void mpButton_Click(object sender, EventArgs e)
        {
            mode = 2;
            LoadMPGame();
        }

        private void back2menuButton_Click(object sender, EventArgs e)
        {
            if (mode == 1)
            {
                if (!spGame.isGameOver)
                {
                    spGame.sw.WriteLine(String.Format(
                        "{0} System: SP Game Aborted. Reason: BACK.", DateTime.Now));
                }
                spGame.sw.Close();
            }
            if (mode == 2)
            {
                //AbortThreads();
            }

            LoadMenu();
        }
        private void restartButton_Click(object sender, EventArgs e)
        {
            if (mode == 1)
            {
                if (!spGame.isGameOver)
                {
                    spGame.sw.WriteLine(String.Format(
                        "{0} System: SP Game Aborted. Reason: RESTART.", DateTime.Now));
                }
                spGame.sw.Close();
                LoadSPGame();
            }
        }

        public void LoadMenu()
        {
            panel1.Controls.Clear();

            Label titleLabel = new Label();
            titleLabel.Text = "TicTacToe XXOO";
            titleLabel.Location = new Point(50, 20);
            titleLabel.Font = new System.Drawing.Font("Arial", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            titleLabel.Size = new System.Drawing.Size(300, 50);

            Label spButton = new Label();
            spButton.Location = new Point(90, 90);
            spButton.Size = new Size(180, 40);
            spButton.TextAlign = ContentAlignment.MiddleCenter;
            spButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            spButton.Text = "Singleplayer";
            spButton.BackColor = Color.Pink;
            spButton.BorderStyle = BorderStyle.FixedSingle;
            spButton.MouseEnter += new EventHandler(labelButton_MouseEnter);
            spButton.MouseLeave += new EventHandler(labelButton_MouseLeave);
            spButton.MouseDown += new MouseEventHandler(labelButton_MouseDown);
            spButton.MouseUp += new MouseEventHandler(labelButton_MouseUp);
            spButton.Click += new EventHandler(spButton_Click);
            
            Label mpButton = new Label();
            mpButton.Location = new Point(90, 140);
            mpButton.Size = new Size(180, 40);
            mpButton.TextAlign = ContentAlignment.MiddleCenter;
            mpButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            mpButton.Text = "Multiplayer";
            mpButton.BackColor = Color.Pink;
            mpButton.BorderStyle = BorderStyle.FixedSingle;
            mpButton.MouseEnter += new EventHandler(labelButton_MouseEnter);
            mpButton.MouseLeave += new EventHandler(labelButton_MouseLeave);
            mpButton.MouseDown += new MouseEventHandler(labelButton_MouseDown);
            mpButton.MouseUp += new MouseEventHandler(labelButton_MouseUp);
            mpButton.Click +=new EventHandler(mpButton_Click);

            Label exitButton = new Label();
            exitButton.Location = new Point(90, 220);
            exitButton.Size = new Size(180, 40);
            exitButton.TextAlign = ContentAlignment.MiddleCenter;
            exitButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            exitButton.Text = "EXIT";
            exitButton.BackColor = Color.Pink;
            exitButton.BorderStyle = BorderStyle.FixedSingle;
            exitButton.MouseEnter += new EventHandler(labelButton_MouseEnter);
            exitButton.MouseLeave += new EventHandler(labelButton_MouseLeave);
            exitButton.Click += new EventHandler(exitButton_Click);
            exitButton.MouseDown += new MouseEventHandler(labelButton_MouseDown);
            exitButton.MouseUp += new MouseEventHandler(labelButton_MouseUp);

            panel1.Controls.Add(titleLabel);
            panel1.Controls.Add(spButton);
            panel1.Controls.Add(mpButton);
            panel1.Controls.Add(exitButton);
        }

        private void LoadSPGame()
        {
            panel1.Controls.Clear();
            Gen_Back2Menu_And_Restart_Button();
            spGame.InitializeSPGame();
            foreach (Label p in spGame.labelList)
            {
                panel1.Controls.Add(p);
            }
        }

        private void LoadMPGame()
        {
            panel1.Controls.Clear();
            Gen_MP_Buttons();
            Gen_Back2Menu_And_Restart_Button();
        }

        private void LoadMPGame_GameStarting()
        {
            timer1.Stop();
            timer1.Enabled = false;
            CheckForIllegalCrossThreadCalls = false;
            panel1.Controls.Clear();
            //Gen_Back2Menu_And_Restart_Button();
            foreach (Label p in mpGame.labelList)
            {
                panel1.Controls.Add(p);
            }
            Label myTurnLabelLink = mpGame.Gen_MP_MyTurnLabel();
            panel1.Controls.Add(myTurnLabelLink);
        }

        private void Gen_Back2Menu_And_Restart_Button()
        {
            Label back2menuButton = new Label();
            back2menuButton.Text = "BACK";
            back2menuButton.TextAlign = ContentAlignment.MiddleCenter;
            back2menuButton.Location = new Point(270, 245);
            back2menuButton.Size = new Size(110, 30);
            back2menuButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            back2menuButton.BackColor = Color.Pink;
            back2menuButton.BorderStyle = BorderStyle.FixedSingle;
            back2menuButton.MouseEnter += new EventHandler(labelButton_MouseEnter);
            back2menuButton.MouseLeave += new EventHandler(labelButton_MouseLeave);
            back2menuButton.MouseDown += new MouseEventHandler(labelButton_MouseDown);
            back2menuButton.MouseUp += new MouseEventHandler(labelButton_MouseUp);
            back2menuButton.Click += new EventHandler(back2menuButton_Click);
            panel1.Controls.Add(back2menuButton);

            Label restartButton = new Label();
            restartButton.Text = "Restart";
            restartButton.TextAlign = ContentAlignment.MiddleCenter;
            restartButton.Location = new Point(270, 205);
            restartButton.Size = new Size(110, 30);
            restartButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            restartButton.BackColor = Color.Pink;
            restartButton.BorderStyle = BorderStyle.FixedSingle;
            restartButton.MouseEnter += new EventHandler(labelButton_MouseEnter);
            restartButton.MouseLeave += new EventHandler(labelButton_MouseLeave);
            restartButton.MouseDown += new MouseEventHandler(labelButton_MouseDown);
            restartButton.MouseUp += new MouseEventHandler(labelButton_MouseUp);
            restartButton.Click += new EventHandler(restartButton_Click);
            panel1.Controls.Add(restartButton);
        }

        private void Gen_MP_Buttons()
        {
            Label hostButton = new Label();
            hostButton.Text = "Host Game";
            hostButton.TextAlign = ContentAlignment.MiddleCenter;
            hostButton.Location = new Point(70, 25);
            hostButton.Size = new Size(170, 35);
            hostButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            hostButton.BackColor = Color.Pink;
            hostButton.BorderStyle = BorderStyle.FixedSingle;
            hostButton.MouseEnter += new EventHandler(labelButton_MouseEnter);
            hostButton.MouseLeave += new EventHandler(labelButton_MouseLeave);
            hostButton.MouseDown += new MouseEventHandler(labelButton_MouseDown);
            hostButton.MouseUp += new MouseEventHandler(labelButton_MouseUp);
            hostButton.Click += new EventHandler(hostButton_Click);
            panel1.Controls.Add(hostButton);

            Label joinButton = new Label();
            joinButton.Text = "Join Game";
            joinButton.TextAlign = ContentAlignment.MiddleCenter;
            joinButton.Location = new Point(70, 70);
            joinButton.Size = new Size(170, 35);
            joinButton.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            joinButton.BackColor = Color.Pink;
            joinButton.BorderStyle = BorderStyle.FixedSingle;
            joinButton.MouseEnter += new EventHandler(labelButton_MouseEnter);
            joinButton.MouseLeave += new EventHandler(labelButton_MouseLeave);
            joinButton.MouseDown += new MouseEventHandler(labelButton_MouseDown);
            joinButton.MouseUp += new MouseEventHandler(labelButton_MouseUp);
            joinButton.Click += new EventHandler(joinButton_Click);
            panel1.Controls.Add(joinButton);
        }



        // Gen 1 hostStatsLabel and initialize connection as HOST
        private void hostButton_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();

            Label hostStatsLabel = new Label();
            //string msg = mpGame.GetIPAddressMy();
            hostStatsLabel.Text = String.Format(
                "Local IP: {0}\nWaiting For LAN Opponent...\n\n"+
            "If you want to end the multiplayer session\n"+
            "click the 'X' at top right corner",mpGame.GetIPAddressMy());
            hostStatsLabel.Font = new System.Drawing.Font("Arial", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            hostStatsLabel.Location = new Point(5, 60);
            hostStatsLabel.AutoSize = true;
            panel1.Controls.Add(hostStatsLabel);

            Thread hostThread = new Thread(new ThreadStart(ThreadProc_Host));
            hostThread.Start();

            timer1.Enabled = true;
            timer1.Start();
        }


        // Gen 1 textbox and 1 button
        private void joinButton_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            Gen_MP_Buttons();
            Gen_Back2Menu_And_Restart_Button();

            ipEntryTextbox = new TextBox();
            ipEntryTextbox.Location = new Point(60, 130);
            ipEntryTextbox.Size = new Size(200, 25);
            panel1.Controls.Add(ipEntryTextbox);

            Button ipConnectButton = new Button();
            ipConnectButton.Location = new Point(280, 130);
            ipConnectButton.Size = new Size(70, 40);
            ipConnectButton.TextAlign = ContentAlignment.MiddleCenter;
            ipConnectButton.Text = "Join";
            ipConnectButton.Click += new EventHandler(ipConnectButton_Click);
            panel1.Controls.Add(ipConnectButton);
        }


        // initialize connection as CLIENT
        private void ipConnectButton_Click(object sender, EventArgs e)
        {
            // Validate IP Entry
            if (ipEntryTextbox.Text == "" || !mpGame.IPCheck(ipEntryTextbox.Text))
            {
                MessageBox.Show("Error: Invalid IP address.", "Unable to Connect");
                ipEntryTextbox.Text = "";
                return;
            }

            this.ThreadProc_Client();
            Thread clientThread = new Thread(new ThreadStart(ThreadProc_ClientServer));
            clientThread.Start();
            
            timer1.Enabled = true;
            timer1.Start();
        }
        private void ThreadProc_ClientServer()
        {
            string ip = mpGame.GetIPAddressMy();
            IPAddress ipAdd = IPAddress.Parse(ip);
            mpGame.tcpListener = new TcpListener(ipAdd, 7777);
            mpGame.tcpListener.Start();
            while (true)
            {
                try
                {
                    mpGame.tcpClient = mpGame.tcpListener.AcceptTcpClient();
                    mpGame.newStream = mpGame.tcpClient.GetStream();
                    if (!mpGame.service)
                    {
                        mpGame.myTurn = false;
                        mpGame.flagStyle = "O";

                        //mpGame.service = false;
                        continue;
                    }
                    mpGame.sr = new StreamReader(mpGame.newStream);
                    int index = Convert.ToInt32(mpGame.sr.ReadLine());
                    if (mpGame.labelList[index].Text == "")
                    {
                        mpGame.labelList[index].Text = "X";
                        if (mpGame.HostWinningCheck())
                        {
                            mpGame.isGameOver = true;
                            ShowMessageBox("You lose");
                            mpGame.RefreshStart();
                        }
                        if (mpGame.CheckDraw()) mpGame.RefreshStart();
                        mpGame.myTurn = true;
                        mpGame.myTurnLabel.Text = "TURN:\nYou";
                    }
                    mpGame.newStream.Close();
                    mpGame.tcpClient.Close();
                }
                catch (Exception)
                { }
            }
        }
        private void ThreadProc_Host()
        {
            string ip = mpGame.GetIPAddressMy();
            IPAddress ipAdd = IPAddress.Parse(ip);
            mpGame.tcpListener = new TcpListener(ipAdd, 6666);
            mpGame.tcpListener.Start();
            while (true)
            {
                //try
                {
                    mpGame.tcpClient = mpGame.tcpListener.AcceptTcpClient();
                    mpGame.newStream = mpGame.tcpClient.GetStream();
                    if (!mpGame.service)
                    {
                        mpGame.myTurn = true;
                        mpGame.flagStyle = "X";
                        // ######################
                        mpGame.sr = new StreamReader(mpGame.newStream);
                        mpGame.GetIPAddressYou = mpGame.sr.ReadLine();
                        // ######################
                        mpGame.service = true;
                        
                        continue;
                    }
                    mpGame.sr = new StreamReader(mpGame.newStream);
                    int index = Convert.ToInt32(mpGame.sr.ReadLine());
                    if (mpGame.labelList[index].Text == "")
                    {
                        mpGame.labelList[index].Text = "O";
                        if (mpGame.ClientWinningCheck())
                        {
                            mpGame.isGameOver = true;
                            ShowMessageBox("You lose");
                            mpGame.RefreshStart();
                        }
                        if (mpGame.CheckDraw()) mpGame.RefreshStart();
                        mpGame.myTurn = true;
                        mpGame.myTurnLabel.Text = "TURN:\nYou";
                    }
                    mpGame.newStream.Close();
                    mpGame.tcpClient.Close();
                }
                //catch(Exception ex)
                { }
            }
        }

        private void ThreadProc_Client()
        {
            mpGame.tcpClient = new TcpClient();
            mpGame.GetIPAddressYou = ipEntryTextbox.Text.Trim();
            try
            {
                mpGame.tcpClient.Connect(IPAddress.Parse(mpGame.GetIPAddressYou), 6666);
            }
            catch
            {
                MessageBox.Show("Error: Host unreachable.", "Unable to Connect");
                Thread.CurrentThread.Abort();
                return;
            }

            // ###############
            mpGame.sw = new StreamWriter(mpGame.tcpClient.GetStream());
            mpGame.sw.WriteLine(mpGame.GetIPAddressMy());
            mpGame.sw.Flush();
            // ###############

            //mpGame.myTurn = false;
            mpGame.myTurn = false;
            mpGame.flagStyle = "O";
            mpGame.service = true;
            mpGame.tcpClient.Close();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mpGame.service)
            {
                LoadMPGame_GameStarting();
                return;
            }
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
