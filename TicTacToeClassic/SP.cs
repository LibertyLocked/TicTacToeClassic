using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;

namespace TicTacToeClassic
{
    public class SP
    {
        private int ai = 0;
        public List<Label> labelList = new List<Label>();
        public bool isGameOver = false;
        private Random rSeed = new Random();
        private int aiChoice = 0;
        private int clickCount = 0;
        private bool activeAI = true;
        public StreamWriter sw;
        
        private SP(){ }

        public SP(int aiLevel)      // AI has 2 levels. 0 and 1.
        {
            ai = aiLevel;
        }

        public void InitializeSPGame()
        {
            if (File.Exists("SPLog.txt"))
            {
                sw = File.AppendText("SPLog.txt");
            }
            else sw = File.CreateText("SPLog.txt");

            sw.AutoFlush = true;
            sw.WriteLine("=============================================================");
            sw.WriteLine(String.Format(
                "{0} System: Start logging. SPVer: FIX15", DateTime.Now));
            labelList.Clear();

            rSeed = new Random();

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
            clickCount = 0;
            sw.WriteLine(String.Format(
                "{0} System: New SP game initialized. (CPU Lv{1})", DateTime.Now, ai));
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
            if (((Label)sender).Text != "" || isGameOver == true) return;
            ((Label)sender).Text = "X";
            clickCount++;
            sw.WriteLine(String.Format(
                "{0} Player: Cross at {1}", DateTime.Now, labelList.IndexOf((Label)sender)));

            #region Player winning conditions
            // Winning Conditions
            // 1 2 3 , 4 5 6 , 7 8 9   LINE
            // 1 4 7 , 2 5 8 , 3 6 9   COL
            // 1 5 9 , 3 5 7           CROSS
            if (labelList[0].Text == "X" && labelList[1].Text == "X" && labelList[2].Text == "X")
            {
                PlayerWin();
                return;
            }
            if (labelList[3].Text == "X" && labelList[4].Text == "X" && labelList[5].Text == "X")
            {
                PlayerWin();
                return;
            }
            if (labelList[6].Text == "X" && labelList[7].Text == "X" && labelList[8].Text == "X")
            {
                PlayerWin();
                return;
            }
            if (labelList[0].Text == "X" && labelList[3].Text == "X" && labelList[6].Text == "X")
            {
                PlayerWin();
                return;
            }
            if (labelList[1].Text == "X" && labelList[4].Text == "X" && labelList[7].Text == "X")
            {
                PlayerWin();
                return;
            }
            if (labelList[2].Text == "X" && labelList[5].Text == "X" && labelList[8].Text == "X")
            {
                PlayerWin();
                return;
            }
            if (labelList[0].Text == "X" && labelList[4].Text == "X" && labelList[8].Text == "X")
            {
                PlayerWin();
                return;
            }
            if (labelList[2].Text == "X" && labelList[4].Text == "X" && labelList[6].Text == "X")
            {
                PlayerWin();
                return;
            }
            #endregion

            if (clickCount == 5)
            {
                PlayerDraw();
                return;
            }

            //AI Controls at Level 0
            if (ai == 0)
            {
                aiChoice = rSeed.Next(0, 8);
                while (labelList[aiChoice].Text == "X" || labelList[aiChoice].Text == "O")
                {
                    aiChoice = rSeed.Next(0, 8);
                }
                labelList[aiChoice].Text = "O";
                sw.WriteLine(String.Format("{0} CPU(0): Circle at {1} by random.", DateTime.Now, aiChoice));
            }
            else if (ai >= 1) Level1AIControls();

            #region AI winning conditions
            // Winning Conditions
            // 1 2 3 , 4 5 6 , 7 8 9   LINE
            // 1 4 7 , 2 5 8 , 3 6 9   COL
            // 1 5 9 , 3 5 7           CROSS
            if (labelList[0].Text == "O" && labelList[1].Text == "O" && labelList[2].Text == "O")
            {
                PlayerLose();
                return;
            }
            if (labelList[3].Text == "O" && labelList[4].Text == "O" && labelList[5].Text == "O")
            {
                PlayerLose();
                return;
            }
            if (labelList[6].Text == "O" && labelList[7].Text == "O" && labelList[8].Text == "O")
            {
                PlayerLose();
                return;
            }
            if (labelList[0].Text == "O" && labelList[3].Text == "O" && labelList[6].Text == "O")
            {
                PlayerLose();
                return;
            }
            if (labelList[1].Text == "O" && labelList[4].Text == "O" && labelList[7].Text == "O")
            {
                PlayerLose();
                return;
            }
            if (labelList[2].Text == "O" && labelList[5].Text == "O" && labelList[8].Text == "O")
            {
                PlayerLose();
                return;
            }
            if (labelList[0].Text == "O" && labelList[4].Text == "O" && labelList[8].Text == "O")
            {
                PlayerLose();
                return;
            }
            if (labelList[2].Text == "O" && labelList[4].Text == "O" && labelList[6].Text == "O")
            {
                PlayerLose();
                return;
            }
            #endregion
        }

        private void PlayerWin()
        {
            isGameOver = true;
            sw.WriteLine(String.Format("{0} RESULT: Player won CPU({1}).", DateTime.Now, ai));
            //sw.Close();
            MessageBox.Show("You win!");
        }
        private void PlayerLose()
        {
            isGameOver = true;
            sw.WriteLine(String.Format("{0} RESULT: CPU({1}) won player.", DateTime.Now, ai));
            //sw.Flush();
            //sw.Close();
            MessageBox.Show("You lose");
        }
        private void PlayerDraw()
        {
            isGameOver = true;
            sw.WriteLine(String.Format("{0} RESULT: CPU({1}) and player got draw.", DateTime.Now, ai));
            //sw.Flush();
            //sw.Close();
            MessageBox.Show("Draw");
        }

        /// <summary>
        /// AI Controls at and above 1
        /// </summary>
        private void Level1AIControls()
        {
            activeAI = true;
            process:

            // ActiveRule#1 and #2 requires aiLevel 2 and above
            // AcviveRule#3 requires aiLevel 3 and above
            #region Special Rules of AI
            if (ai >= 2)
            {
                if (labelList[4].Text == "")  // if the center block is not captured, capture it
                {
                    labelList[4].Text = "O";
                    sw.WriteLine(String.Format("{0} CPU({1}): Circle at 4 by ActiveRule#1", DateTime.Now, ai));
                    return;
                }
                if (clickCount == 1 && labelList[4].Text == "X") // if the center block is captured by player
                {                                               // draw an O at corner
                    switch (rSeed.Next(3))
                    {
                        case 0:
                            labelList[0].Text = "O";
                            sw.WriteLine(String.Format("{0} CPU({1}): Circle at 0 by ActiveRule#2", DateTime.Now, ai));
                            break;
                        case 1:
                            labelList[2].Text = "O";
                            sw.WriteLine(String.Format("{0} CPU({1}): Circle at 2 by ActiveRule#2", DateTime.Now, ai));
                            break;
                        case 2:
                            labelList[6].Text = "O";
                            sw.WriteLine(String.Format("{0} CPU({1}): Circle at 6 by ActiveRule#2", DateTime.Now, ai));
                            break;
                        case 3:
                            labelList[8].Text = "O";
                            sw.WriteLine(String.Format("{0} CPU({1}): Circle at 8 by ActiveRule#2", DateTime.Now, ai));
                            break;
                    }
                    return;
                }
                
                if (ai >= 3)
                {
                    if (clickCount == 2 && labelList[1].Text == "X" && labelList[3].Text == "X")
                    {
                        labelList[0].Text = "O";
                        sw.WriteLine(String.Format("{0} CPU({1}): Circle at 0 by ActiveRule#3", DateTime.Now, ai));
                        return;
                    }
                    if (clickCount == 2 && labelList[1].Text == "X" && labelList[5].Text == "X")
                    {
                        labelList[2].Text = "O";
                        sw.WriteLine(String.Format("{0} CPU({1}): Circle at 2 by ActiveRule#3", DateTime.Now, ai));
                        return;
                    }
                    if (clickCount == 2 && labelList[3].Text == "X" && labelList[7].Text == "X")
                    {
                        labelList[6].Text = "O";
                        sw.WriteLine(String.Format("{0} CPU({1}): Circle at 6 by ActiveRule#3", DateTime.Now, ai));
                        return;
                    }
                    if (clickCount == 2 && labelList[5].Text == "X" && labelList[7].Text == "X")
                    {
                        labelList[8].Text = "O";
                        sw.WriteLine(String.Format("{0} CPU({1}): Circle at 8 by ActiveRule#3", DateTime.Now, ai));
                        return;
                    }
                }
            }
            #endregion

            // ActiveOp and NegativeOp
            #region General Rules of AI
            // General Rules of AI
            if (labelList[0].Text == "" && labelList[1].Text != "" && labelList[2].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(0, 1, 2))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(0, 1, 2))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[1].Text == "" && labelList[0].Text != "" && labelList[2].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(0, 1, 2))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(0, 1, 2))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[2].Text == "" && labelList[0].Text != "" && labelList[1].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(0, 1, 2))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(0, 1, 2))
                {
                    activeAI = true;
                    return;
                }
            }

            if (labelList[3].Text == "" && labelList[4].Text != "" && labelList[5].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(3, 4, 5))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(3, 4, 5))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[4].Text == "" && labelList[3].Text != "" && labelList[5].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(3, 4, 5))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(3, 4, 5))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[5].Text == "" && labelList[3].Text != "" && labelList[4].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(3, 4, 5))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(3, 4, 5))
                {
                    activeAI = true;
                    return;
                }
            }

            if (labelList[6].Text == "" && labelList[7].Text != "" && labelList[8].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(6, 7, 8))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(6, 7, 8))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[7].Text == "" && labelList[6].Text != "" && labelList[8].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(6, 7, 8))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(6, 7, 8))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[8].Text == "" && labelList[6].Text != "" && labelList[7].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(6, 7, 8))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(6, 7, 8))
                {
                    activeAI = true;
                    return;
                }
            }

            if (labelList[0].Text == "" && labelList[3].Text != "" && labelList[6].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(0, 3, 6))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(0, 3, 6))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[3].Text == "" && labelList[0].Text != "" && labelList[6].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(0, 3, 6))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(0, 3, 6))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[6].Text == "" && labelList[0].Text != "" && labelList[3].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(0, 3, 6))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(0, 3, 6))
                {
                    activeAI = true;
                    return;
                }
            }

            if (labelList[1].Text == "" && labelList[4].Text != "" && labelList[7].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(1, 4, 7))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(1, 4, 7))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[4].Text == "" && labelList[1].Text != "" && labelList[7].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(1, 4, 7))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(1, 4, 7))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[7].Text == "" && labelList[1].Text != "" && labelList[4].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(1, 4, 7))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(1, 4, 7))
                {
                    activeAI = true;
                    return;
                }
            }

            if (labelList[2].Text == "" && labelList[5].Text != "" && labelList[8].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(2, 5, 8))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(2, 5, 8))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[5].Text == "" && labelList[2].Text != "" && labelList[8].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(2, 5, 8))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(2, 5, 8))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[8].Text == "" && labelList[2].Text != "" && labelList[5].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(2, 5, 8))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(2, 5, 8))
                {
                    activeAI = true;
                    return;
                }
            }

            if (labelList[0].Text == "" && labelList[4].Text != "" && labelList[8].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(0, 4, 8))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(0, 4, 8))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[4].Text == "" && labelList[0].Text != "" && labelList[8].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(0, 4, 8))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(0, 4, 8))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[8].Text == "" && labelList[0].Text != "" && labelList[4].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(0, 4, 8))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(0, 4, 8))
                {
                    activeAI = true;
                    return;
                }
            }

            if (labelList[2].Text == "" && labelList[4].Text != "" && labelList[6].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(2, 4, 6))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(2, 4, 6))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[4].Text == "" && labelList[2].Text != "" && labelList[6].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(2, 4, 6))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(2, 4, 6))
                {
                    activeAI = true;
                    return;
                }
            }
            if (labelList[6].Text == "" && labelList[2].Text != "" && labelList[4].Text != "")
            {
                if (activeAI)
                {
                    if (Level1AIControls_ActiveOp(2, 4, 6))
                    {
                        activeAI = false;
                        return;
                    }
                }
                else if (Level1AIControls_NegativeOp(2, 4, 6))
                {
                    activeAI = true;
                    return;
                }
            }
            #endregion

            if (activeAI)
            {
                activeAI = false;
                goto process;
            }

            // Randomly pick one
            aiChoice = rSeed.Next(0, 8);
            while (labelList[aiChoice].Text != "")
            {
                aiChoice = rSeed.Next(0, 8);
            }
            labelList[aiChoice].Text = "O";
            sw.WriteLine(String.Format(
                "{0} CPU({1}): Circle at {2} by random", DateTime.Now, ai, aiChoice));
        }

        private bool Level1AIControls_ActiveOp(int num1, int num2, int num3)
        {
            if (labelList[num1].Text == "O" && labelList[num2].Text == "O" 
                || labelList[num2].Text == "O" && labelList[num3].Text == "O"
                || labelList[num1].Text == "O" && labelList[num3].Text == "O")
            {
                int count = 0;
                foreach (Label l in labelList)
                {
                    if ((count == num1 || count == num2 || count == num3) && l.Text == "")
                    {
                        l.Text = "O";
                        sw.WriteLine(String.Format(
                            "{0} CPU({1}): Circle at {2} by ActiveOp", DateTime.Now, ai, labelList.IndexOf(l)));
                        break;
                    }
                    count++;
                }
                return true;
            }            
            else return false;
        }

        private bool Level1AIControls_NegativeOp(int num1, int num2, int num3)
        {
            if (labelList[num1].Text == "X" && labelList[num2].Text == "X"
                || labelList[num2].Text == "X" && labelList[num3].Text == "X"
                || labelList[num1].Text == "X" && labelList[num3].Text == "X")
            {
                int count = 0;
                foreach (Label l in labelList)
                {
                    if ((count == num1 || count == num2 || count == num3) && l.Text == "")
                    {
                        l.Text = "O";
                        sw.WriteLine(String.Format(
                            "{0} CPU({1}): Circle at {2} by NegativeOp", DateTime.Now, ai, labelList.IndexOf(l)));
                        break;
                    }
                    count++;
                }
                return true;
            }
            else return false;
        }
    }
}
