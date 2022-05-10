using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MineSweeper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        int[] buttonstatus = new int[101];      //how many mines are adjacent
        int[] chainedzero = new int[101];       //used to remove chained adjacent zeros
        bool[] flagged = new bool[101];         //if the button is flagged or not
        Random generator = new Random();
        Button[] field = new Button[101];
        public static bool start = false;       
        public static int pressed;
        public static int flags = 0;
        public static int time = 0;
        public static int correctflags = 0;
        int[] flagging = new int[101];

        private void openbutton(int opening)    //subroutine to open the said button
        {
            if (field[opening].BackColor != SystemColors.ButtonFace)
            {
                if (buttonstatus[opening] == 0)
                {
                    field[opening].ForeColor = SystemColors.Control;
                }
                else if (buttonstatus[opening] == 1)
                {
                    field[opening].ForeColor = Color.LightBlue;
                }
                else if (buttonstatus[opening] == 2)
                {
                    field[opening].ForeColor = Color.Blue;
                }
                else if (buttonstatus[opening] == 3)
                {
                    field[opening].ForeColor = Color.Orange;
                }
                else if (buttonstatus[opening] == 4)
                {
                    field[opening].ForeColor = Color.Red;
                }
                else if (buttonstatus[opening] == 5)
                {
                    field[opening].ForeColor = Color.Crimson;
                }
                else if (buttonstatus[opening] == 6)
                {
                    field[opening].ForeColor = Color.Black;
                }
                if (flagged[opening] == false)
                {
                    field[opening].Text = Convert.ToString(buttonstatus[opening]);
                    field[opening].BackColor = SystemColors.ButtonFace;
                    textBox1.Select();  //selects the hidden textbox remove hover lines
                }
            }
        }

        private void gameplay(object sender, MouseEventArgs e)      //routine that runs when a button is pressed
        {
            for (int i = 1; i <= 100; i++)
            {
                if (field[i] == sender)     //gets which button is pressed
                {
                    pressed = i;
                }
            }
            if (start == false) //runs the first time to start the game
            {
                do
                {
                    for (int i = 1; i <= 100; i++)
                    {
                        buttonstatus[i] = 0;
                    }
                    int bombsplaced = 10;       //bombs to be placed
                    int placing;                
                    timer2.Enabled = true;
                    do
                    {
                        placing = generator.Next(1, 101);   //places ten bombs randomally
                        if (buttonstatus[placing] != 99)    //restricts from placing in a place where a bomb is already present
                        {
                            buttonstatus[placing] = 99;
                            bombsplaced = bombsplaced - 1;
                        }

                    } while (bombsplaced > 0);
                    int column = 0;
                    int bombindicator = 0;
                    for (int i = 1; i <= 100; i++)      //checks each button for all of it adjacent, adding one in their buttonstatus for each adjacent bomb
                    {
                        column = (i % 10);                 //gets the button column
                        if (buttonstatus[i] != 99)
                        {
                            if (i > 10)                 //does not run if its the top most column, does not need to check above it
                            {
                                if (column != 1)           //does not run if its the left most column, does not need to check to it left
                                {
                                    if (buttonstatus[i - 11] == 99)
                                    {
                                        bombindicator++;
                                    }
                                }


                                if (buttonstatus[i - 10] == 99)
                                {
                                    bombindicator++;
                                }

                                if (column != 0)           //does not run if its the right most column, does not need to check to it right
                                {
                                    if (buttonstatus[i - 9] == 99)
                                    {
                                        bombindicator++;
                                    }
                                }
                            }

                            if (column != 1)
                            {
                                if (buttonstatus[i - 1] == 99)
                                {
                                    bombindicator++;
                                }
                            }

                            if (column != 0)
                            {
                                if (buttonstatus[i + 1] == 99)
                                {
                                    bombindicator++;
                                }
                            }


                            if (i <= 90)            //does not run if its the button column, does not need to check under it
                            {
                                if (column != 1)
                                {
                                    if (buttonstatus[i + 9] == 99)
                                    {
                                        bombindicator++;
                                    }
                                }


                                if (buttonstatus[i + 10] == 99)
                                {
                                    bombindicator++;
                                }

                                if (column != 0)
                                {
                                    if (buttonstatus[i + 11] == 99)
                                    {
                                        bombindicator++;
                                    }
                                }
                            }
                            buttonstatus[i] = bombindicator;        
                            bombindicator = 0;
                        }
                    }
                } while (buttonstatus[pressed] != 0);           //repeats this until a game is created, where the pressed button has the status zero
                start = true;
            }
            if (field[pressed].BackColor != SystemColors.ButtonFace) //buttonface is seen as a already pressed button
            {
                if (e.Button == MouseButtons.Left)          //if the input is left click
                {
                    if (buttonstatus[pressed] == 0)         //if a 0 tile is pressed, it must open an area of zeros
                    {
                        int chain = 2;
                        int repeating = 0;
                        do
                        {
                            repeating++;
                            if (repeating >= 2)
                            {
                                pressed = chainedzero[repeating];
                                openbutton(pressed);
                            }
                            int column;
                            column = (pressed % 10);
                            if (pressed > 10)
                            {
                                if (column != 1)        //runs the same adjacent finding routine as above, but this time checking for 0
                                {
                                    if (buttonstatus[pressed - 11] == 0 && field[pressed - 11].BackColor != SystemColors.ButtonFace)
                                    {
                                        chainedzero[chain] = (pressed - 11); //the tiles that are zeros which are unopened, is inputted into the array
                                        chain++;
                                    }
                                    openbutton(pressed - 11);
                                }

                                if (buttonstatus[pressed - 10] == 0 && field[pressed - 10].BackColor != SystemColors.ButtonFace)
                                {
                                    chainedzero[chain] = (pressed - 10);
                                    chain++;
                                }
                                openbutton(pressed - 10);

                                if (column != 0)
                                {
                                    if (buttonstatus[pressed - 9] == 0 && field[pressed - 9].BackColor != SystemColors.ButtonFace)
                                    {
                                        chainedzero[chain] = (pressed - 9);
                                        chain++;
                                    }
                                    openbutton(pressed - 9);
                                }
                            }

                            if (column != 1)
                            {
                                if (buttonstatus[pressed - 1] == 0 && field[pressed - 1].BackColor != SystemColors.ButtonFace)
                                {
                                    chainedzero[chain] = (pressed - 1);
                                    chain++;
                                }
                                openbutton(pressed - 1);
                            }

                            if (column != 0)
                            {
                                if (buttonstatus[pressed + 1] == 0 && field[pressed + 1].BackColor != SystemColors.ButtonFace)
                                {
                                    chainedzero[chain] = (pressed + 1);
                                    chain++;
                                }
                                openbutton(pressed + 1);
                            }


                            if (pressed <= 90)
                            {
                                if (column != 1)
                                {
                                    if (buttonstatus[pressed + 9] == 0 && field[pressed + 9].BackColor != SystemColors.ButtonFace)
                                    {
                                        chainedzero[chain] = (pressed + 9);
                                        chain++;
                                    }
                                    openbutton(pressed + 9);
                                }

                                if (buttonstatus[pressed + 10] == 0 && field[pressed + 10].BackColor != SystemColors.ButtonFace)
                                {
                                    chainedzero[chain] = (pressed + 10);
                                    chain++;
                                }
                                openbutton(pressed + 10);

                                if (column != 0)
                                {
                                    if (buttonstatus[pressed + 11] == 0 && field[pressed + 11].BackColor != SystemColors.ButtonFace)
                                    {
                                        chainedzero[chain] = (pressed + 11);
                                        chain++;
                                    }
                                    openbutton(pressed + 11);
                                }
                            }
                        } while (chainedzero[repeating + 1] != 0);  //continues until the array is empty and all 0 in the area are opened

                        for (int i = 0; i <= 100; i++)
                        {
                            chainedzero[i] = 0;
                        }
                    }

                    if (flagged[pressed] == false)      //only runs if the tile does not have a flag on it
                    {
                        if (buttonstatus[pressed] == 99)    //runs when a mine is pressed, you lose the game
                        {
                            for (int i = 1; i <= 100; i++)
                            {
                                field[i].Enabled = false;
                                if (buttonstatus[i] == 99)
                                {
                                    field[i].BackColor = Color.Crimson;
                                }
                            }
                            timer2.Enabled = false;
                            BackColor = Color.Crimson;
                            timer1.Enabled = true;
                            timer3.Enabled = true;
                        }
                        else
                        {
                            openbutton(pressed);        //simply opens the tile if its not 0 or a bomb
                        }
                    }
                }
                else if (e.Button == MouseButtons.Right)    //run if right click is pressed, flagging the tile
                {
                    if (flagged[pressed] == false)          //if there is no flag, it places a flag
                    {
                        field[pressed].BackColor = Color.YellowGreen;
                        flagged[pressed] = true;
                        flags++;
                        flagging[flags] = pressed;
                        label1.Text = Convert.ToString(10 - flags);
                        if (buttonstatus[pressed] == 99)    //if a flag is correctly placed, counts the number of correct flags
                        {
                            correctflags++;
                        }
                        if (flags == 10)                    
                        {
                            if (correctflags == 10)         //if there are exactly 10 flags and all of them are correct, win the game
                            {
                                timer2.Enabled = false;
                                MessageBox.Show("Congratulations, you solved the minefield in " + time + " seconds!", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                button101.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        flagged[pressed] = false;           //if the tile is already flagged, unflag the tile
                        field[pressed].BackColor = SystemColors.ControlLight;
                        flags--;
                        if (buttonstatus[pressed] == 99)
                        {
                            correctflags--;                 //if the tile was correct, removes one point from the correctflag integer
                        }
                        label1.Text = Convert.ToString(10 - flags);
                    }

                }
            }
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            field[1] = button1;
            field[2] = button2;
            field[3] = button3;
            field[4] = button4;
            field[5] = button5;
            field[6] = button6;
            field[7] = button7;
            field[8] = button8;
            field[9] = button9;
            field[10] = button10;
            field[11] = button11;
            field[12] = button12;
            field[13] = button13;
            field[14] = button14;
            field[15] = button15;
            field[16] = button16;
            field[17] = button17;
            field[18] = button18;
            field[19] = button19;
            field[20] = button20;
            field[21] = button21;
            field[22] = button22;
            field[23] = button23;
            field[24] = button24;
            field[25] = button25;
            field[26] = button26;
            field[27] = button27;
            field[28] = button28;
            field[29] = button29;
            field[30] = button30;
            field[31] = button31;
            field[32] = button32;
            field[33] = button33;
            field[34] = button34;
            field[35] = button35;
            field[36] = button36;
            field[37] = button37;
            field[38] = button38;
            field[39] = button39;
            field[40] = button40;
            field[41] = button41;
            field[42] = button42;
            field[43] = button43;
            field[44] = button44;
            field[45] = button45;
            field[46] = button46;
            field[47] = button47;
            field[48] = button48;
            field[49] = button49;
            field[50] = button50;
            field[51] = button51;
            field[52] = button52;
            field[53] = button53;
            field[54] = button54;
            field[55] = button55;
            field[56] = button56;
            field[57] = button57;
            field[58] = button58;
            field[59] = button59;
            field[60] = button60;
            field[61] = button61;
            field[62] = button62;
            field[63] = button63;
            field[64] = button64;
            field[65] = button65;
            field[66] = button66;
            field[67] = button67;
            field[68] = button68;
            field[69] = button69;
            field[70] = button70;
            field[71] = button71;
            field[72] = button72;
            field[73] = button73;
            field[74] = button74;
            field[75] = button75;
            field[76] = button76;
            field[77] = button77;
            field[78] = button78;
            field[79] = button79;
            field[80] = button80;
            field[81] = button81;
            field[82] = button82;
            field[83] = button83;
            field[84] = button84;
            field[85] = button85;
            field[86] = button86;
            field[87] = button87;
            field[88] = button88;
            field[89] = button89;
            field[90] = button90;
            field[91] = button91;
            field[92] = button92;
            field[93] = button93;
            field[94] = button94;
            field[95] = button95;
            field[96] = button96;
            field[97] = button97;
            field[98] = button98;
            field[99] = button99;
            field[100] = button100;
            for (int i = 1; i <= 100; i++)
            {
                field[i].MouseDown += gameplay;         //subscribes all of the button clicks to the gameplay routine
            }
        }

        private void timer1_Tick(object sender, EventArgs e)        //timer for a simple flashing background when you lose
        {
            BackColor = SystemColors.Control;
            timer1.Enabled = false;
        }

        private void timer2_Tick(object sender, EventArgs e)        //the timer, ticking each second on top right
        {
            time++;
            label2.Text = Convert.ToString(time);
        }

        private void timer3_Tick(object sender, EventArgs e)        //the 3 second delay to display the restart button after the game ends from a failure
        {
            button101.Visible = true;
            timer3.Enabled = false;
        }

        private void button101_Click(object sender, EventArgs e)        //the restart button, resets all of the variables used
        {
            for (int i = 1; i <= 100; i++)
            {
                buttonstatus[i] = 0;
                chainedzero[i] = 0;
                flagged[i] = false;
                flagging[i] = 0;
                field[i].BackColor = SystemColors.ActiveBorder;
                field[i].Text = "";
                field[i].Enabled = true;
            }
            start = false;
            pressed = 0;
            flags = 0;
            time = 0;
            label2.Text = "0";
            correctflags = 0;
            button101.Visible = false;
        }
    }
}
