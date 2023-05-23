using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using S7.Net;
using S7.Net.Types;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace BTKEtNoiFactoryIO_C_
{
    public partial class Form1 : Form
    {
        Plc factoryio;
        
        bool conveyor0, conveyor1, conveyor2, conveyor3;
        byte[] Q = new byte[40];
        byte[] I = new byte[4];
        byte[] M = new byte[4];
        int Count_C, Count_TB, Count_T;

        private void button3_Click(object sender, EventArgs e)
        {
            factoryio = new Plc(CpuType.S71500, "192.168.0.100", 0, 1);
            factoryio.Open();
            if (factoryio.Open() == ErrorCode.NoError)
            {
                M[1] |= 0x04;
                factoryio.WriteBytes(DataType.Memory, 1, 1, M);
                M[1] &= 0xfb;
                factoryio.WriteBytes(DataType.Memory, 1, 1, M);
                factoryio.Close();
            }
            else
                MessageBox.Show("Can't Reset");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            factoryio = new Plc(CpuType.S71500, "192.168.0.100", 0, 1);
            factoryio.Open();
            if (factoryio.Open() == ErrorCode.NoError)
            {
                M[1] |= 0x02;
                factoryio.WriteBytes(DataType.Memory, 1, 1, M);
                M[1] &= 0xfd;
                factoryio.WriteBytes(DataType.Memory, 1, 1, M);
                
                factoryio.Close();
            }
            else
                MessageBox.Show("Can't Stop");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Start
            factoryio = new Plc(CpuType.S71500, "192.168.0.100", 0, 1);
            factoryio.Open();
            if (factoryio.Open() == ErrorCode.NoError)
            {
                M[1] |= 0x01;
                factoryio.WriteBytes(DataType.Memory, 1, 0, M);
                M[1] &= 0xfe;
                factoryio.WriteBytes(DataType.Memory, 1, 0, M);
                factoryio.Close();
            }
            else
                MessageBox.Show("Can't Start");
            
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        

       

        private void timer1_Tick(object sender, EventArgs e)
        {

            factoryio = new Plc(CpuType.S71500, "192.168.0.100", 0, 1);

            if (factoryio.Open() == ErrorCode.NoError)
            {
                label1.Text = "Connected";

                Q = factoryio.ReadBytes(DataType.Output, 0, 0, 36);
                I = factoryio.ReadBytes(DataType.Input, 0, 0, 4);
                
                //Conveyor display


                //Conveyor0
                if (Q[0].SelectBit(0)) panel1.BackColor = Color.Green;
                else panel1.BackColor = Color.WhiteSmoke;

                //Conveyor1
                if (Q[0].SelectBit(1)) panel2.BackColor = Color.Green;
                else panel2.BackColor = Color.WhiteSmoke;

                //Conveyor2
                if (Q[0].SelectBit(2)) panel3.BackColor = Color.Green;
                else panel3.BackColor = Color.WhiteSmoke;

                //Conveyor3
                if (Q[0].SelectBit(3)) panel4.BackColor = Color.Green;
                else panel4.BackColor = Color.WhiteSmoke;

                //TableTurn
                if (Q[0].SelectBit(4)) panel5.BackColor = Color.Green;
                else panel5.BackColor = Color.WhiteSmoke;

                //Count
                
                Count_C = (int)Q[31];
                Count_TB = (int)Q[33];
                Count_T = (int)Q[35];
                textBox1.Text = Count_C.ToString();
                textBox2.Text = Count_TB.ToString();
                textBox3.Text = Count_T.ToString();

                //Sensor
                if (I[0].SelectBit(3)) panel8.BackColor = Color.Blue;
                else panel8.BackColor = Color.WhiteSmoke;
                if (I[0].SelectBit(4)) panel7.BackColor = Color.Blue;
                else panel7.BackColor = Color.WhiteSmoke;
                if (I[0].SelectBit(5)) panel6.BackColor = Color.Blue;
                else panel6.BackColor = Color.WhiteSmoke;

                //Run
                if ((bool)factoryio.Read(DataType.Memory, 0, 0, VarType.Bit, 1)) panel9.BackColor = Color.Green;
                else panel9.BackColor = Color.Red;


            }
            else
            {
                label1.Text = "Can't connect";
            }    
        }
    }
}
