using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Collections;
using DBExample.DBAccess_Provider;

namespace StationMeteo
{
    public partial class Form1 : Form
    {
        Boolean verif = false; 
        private int cellSizeH = 65; 
        private int cellSizeV = 40; 
    //    List<int> debutTramNumbers = new List<int> { 85, 170, 85 };
        SerialPort port = new SerialPort("COM2", 9600, Parity.None, 8, StopBits.One);
        List<int> allTrams = new List<int>();

        // variable db
        internal static DataSet Local_UserAccess = new DataSet();

        internal static DataTable Local_UserTable = new DataTable();
        internal static DataTable Local_AccessTable = new DataTable();
        public Form1()
        {
            InitializeComponent(); 
            ConfigDataset();

            Tools.Config();
            Adapter.Fill(Local_UserAccess, "Local_AccessTable", "AccessTable");
            Adapter.Fill(Local_UserAccess, "Local_UserTable", "UserTable");


            createDGV();
            port.DataReceived += new SerialDataReceivedEventHandler(Port_DataReceived);
            port.Open();
             
            // dgvSetter(dataGridView1);
        }
        private bool CheckTramId()
        {
            /*
            if (allTrams.ElementAt(0) == debutTramNumbers.ElementAt(0) &&
               allTrams.ElementAt(1) == debutTramNumbers.ElementAt(1) &&
               allTrams.ElementAt(2) == debutTramNumbers.ElementAt(2) &&
               )
            {

            }
            */

            return false;
        }
        private static void ConfigDataset()
        {
            DataColumn UserKey_ID = new DataColumn("A", System.Type.GetType("System.Int16"));
            DataColumn UserName = new DataColumn("B", System.Type.GetType("System.String"));
            DataColumn UserPassword = new DataColumn("C", System.Type.GetType("System.String"));
            DataColumn Access_Id = new DataColumn("D", System.Type.GetType("System.Int16"));

            DataColumn AccessKey_Id = new DataColumn("E", System.Type.GetType("System.Int16"));
            DataColumn AccessName = new DataColumn("F", System.Type.GetType("System.String"));
            DataColumn AllowCreateID = new DataColumn("G", System.Type.GetType("System.Boolean"));
            DataColumn AllowDestroyID = new DataColumn("H", System.Type.GetType("System.Boolean"));
            DataColumn AllowConfigAlarm = new DataColumn("I", System.Type.GetType("System.Boolean"));
            DataColumn UserCreation = new DataColumn("J", System.Type.GetType("System.Boolean"));

            Local_UserTable.TableName = "Local_UserTable";
            Local_AccessTable.TableName = "Local_AccessTable";

            Local_UserAccess.Tables.Add(Local_UserTable);
            Local_UserAccess.Tables.Add(Local_AccessTable);

            UserKey_ID.AutoIncrement = true;
            UserKey_ID.Unique = true;
            UserKey_ID.ColumnName = "UserKey_ID";
            UserKey_ID.DataType = System.Type.GetType("System.Int32");
            Local_UserTable.Columns.Add(UserKey_ID);

            UserName.AutoIncrement = false;
            UserName.Unique = false;
            UserName.ColumnName = "UserName";
            UserName.DataType = System.Type.GetType("System.String");
            Local_UserTable.Columns.Add(UserName);

            UserPassword.AutoIncrement = false;
            UserPassword.Unique = false;
            UserPassword.ColumnName = "UserPassword";
            UserPassword.DataType = System.Type.GetType("System.String");
            Local_UserTable.Columns.Add(UserPassword);

            Access_Id.AutoIncrement = false;
            Access_Id.Unique = false;
            Access_Id.ColumnName = "Access_Id";
            Access_Id.DataType = System.Type.GetType("System.Int32");
            Local_UserTable.Columns.Add(Access_Id);

            AccessKey_Id.AutoIncrement = true;
            AccessKey_Id.Unique = true;
            AccessKey_Id.ColumnName = "AccessKey_Id";
            AccessKey_Id.DataType = System.Type.GetType("System.Int32");
            Local_AccessTable.Columns.Add(AccessKey_Id);

            AccessName.AutoIncrement = false;
            AccessName.Unique = false;
            AccessName.ColumnName = "AccessName";
            AccessName.DataType = System.Type.GetType("System.String");
            Local_AccessTable.Columns.Add(AccessName);

            AllowCreateID.AutoIncrement = false;
            AllowCreateID.Unique = false;
            AllowCreateID.ColumnName = "AllowCreateID";
            AllowCreateID.DataType = System.Type.GetType("System.Boolean");
            Local_AccessTable.Columns.Add(AllowCreateID);

            AllowDestroyID.AutoIncrement = false;
            AllowDestroyID.Unique = false;
            AllowDestroyID.ColumnName = "AllowDestroyID";
            AllowDestroyID.DataType = System.Type.GetType("System.Boolean");
            Local_AccessTable.Columns.Add(AllowDestroyID);

            AllowConfigAlarm.AutoIncrement = false;
            AllowConfigAlarm.Unique = false;
            AllowConfigAlarm.ColumnName = "AllowConfigAlarm";
            AllowConfigAlarm.DataType = System.Type.GetType("System.Boolean");
            Local_AccessTable.Columns.Add(AllowConfigAlarm);

            UserCreation.AutoIncrement = false;
            UserCreation.Unique = false;
            UserCreation.ColumnName = "UserCreation";
            UserCreation.DataType = System.Type.GetType("System.Boolean");
            Local_AccessTable.Columns.Add(UserCreation);

            DataColumn parentColumn = Local_UserAccess.Tables["Local_AccessTable"].Columns["AccessKey_Id"];
            DataColumn childColumn = Local_UserAccess.Tables["Local_UserTable"].Columns["Access_Id"];

            DataRelation relation = new DataRelation("parent2Child", parentColumn, childColumn);

            Local_UserAccess.Tables["Local_UserTable"].ParentRelations.Add(relation);
        }
        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            SerialPort sp = (SerialPort)sender;
       //     string indata = sp.ReadExisting();

            int inbuffer;
            inbuffer = sp.BytesToRead;
            byte[] buffer = new byte[inbuffer];


            port.Read(buffer, 0, inbuffer);

             string test = "";
            // int taille = buffer.Length;
            //MessageBox.Show(taille + "");

            for (int i = 0; i < buffer.Length; i++)
            {
                test += buffer[i] + " / ";
               
            }

            //MessageBox.Show(" value of buffer = " + test);
            //MessageBox.Show(test);
            //   MessageBox.Show(" legnth opf buffer = " + buffer.Length);
            //MessageBox.Show(" "+ buffer[0] + " / "+ buffer[1] + " / " + buffer[2] );
            //Console.WriteLine("Data Received:");
            //    Console.Write(indata);
            //    MessageBox.Show(indata.ToString());
            //   port.Close();

            List<byte> trams = storeBufferInList(buffer);
              delimitationOfTrames(trams);

        }

        // la methode permet de stocker le buffer dans une liste 
        private List<byte> storeBufferInList(Byte[] buffer)
        {
         
            List<byte> allTrams = new List<byte>();
            for (int i = 0; i < buffer.Length; i++)
            {
                allTrams.Add(  buffer[i]);

            }

            return allTrams;
        }
    

        private void createDGV()
        {
       
            int rowCount = 6;// ligne
            int rowLength = 4; // colonne
            DataTable dt = new DataTable();

            for (int col = 0; col < rowLength; col++)
            {
                dt.Columns.Add();
            }
            for (int row = 0; row < rowCount; row++)
            {
                DataRow drow = dt.NewRow();

                for (int i = 0; i < rowLength; i++)
                {
                    drow[i] = "";
                }
                dt.Rows.Add(drow);
            }

            dataGridView1.DataSource = dt;


            //for (int col = 0; col < rowLength; col++)
            //{
            //    dataGridView1.Columns[col].Width = (cellSizeH);
            //}

            //for (int row = 0; row < rowCount; row++)
            //{
            //    dataGridView1.Rows[row].Height = cellSizeV;
            //}

            //dataGridView1.Width = cellSizeH * rowLength + 3;
            //dataGridView1.Height = cellSizeV * rowCount + 3;
            dataGridView1.Visible = true;


            dataGridView1.Rows[0].Cells[0].Value = "Id";
            dataGridView1.Rows[0].Cells[1].Value= "Data";
            dataGridView1.Rows[0].Cells[2].Value= "Type";
            dataGridView1.Rows[0].Cells[3].Value= "Number Data";
            // fillDGV();
        }

       
        private void delimitationOfTrames(List<byte> trams)
        {

            DataTable dt = new DataTable();
       //    MessageBox.Show(" " +trams.ElementAt(0)+ " " + trams.ElementAt(1) + " " + trams.ElementAt(2) + " " + trams.ElementAt(3) + " " + trams.ElementAt(4) + " " + trams.ElementAt(5) );
            if (trams.ElementAt(0) == 85 && trams.ElementAt(1) == 170 && trams.ElementAt(2) == 85) // on verifie le debut de trame
            {
                int data = 0;
                if (trams.ElementAt(3) == 0)  // on verifie le id 
                {
                    Base keepalive = new Base();
                    keepalive.byteData = trams.ElementAt(6);
                    keepalive.byteNumber = trams.ElementAt(4);
                    keepalive.type = trams.ElementAt(5);
                    keepalive.id = trams.ElementAt(3);

                    data = decompositionByteData(trams, trams.ElementAt(4));
                    dataGridView1.Rows[1].Cells[1].Value=data.ToString();
                    dataGridView1.Rows[1].Cells[0].Value=keepalive.id;
                    dataGridView1.Rows[1].Cells[2].Value="Keep alive";
                    dataGridView1.Rows[1].Cells[3].Value= keepalive.byteNumber;

                }
                else if (trams.ElementAt(3) > 0 && trams.ElementAt(3) < 11)
                {
                    Mesure mesure = new Mesure();
                    mesure.byteData = trams.ElementAt(6);
                    mesure.byteNumber = trams.ElementAt(4);
                    mesure.type = trams.ElementAt(5);
                    mesure.id = trams.ElementAt(3);

                    //data = decompositionByteData(trams, trams.ElementAt(4));
                    //dataGridView1.Rows[2].Cells[1].Value = data.ToString();
                    //dataGridView1.Rows[2].Cells[0].Value = mesure.id;
                    //dataGridView1.Rows[2].Cells[2].Value = mesure.type;
                    //dataGridView1.Rows[2].Cells[3].Value = mesure.byteNumber;

                    //   MessageBox.Show(" value of data = " + data);

                    if (!verif)
                    {
                      //  MessageBox.Show(" Dimensions datagridview =  " + dataGridView1.RowCount + " / " + dataGridView1.ColumnCount);
                        verif = true;
                    }
                    if (mesure.type == 1)
                    {
                        data = decompositionByteData(trams, trams.ElementAt(4));
                        dataGridView1.Rows[4].Cells[1].Value = "" + data;
                        dataGridView1.Rows[4].Cells[0].Value = mesure.id;
                        dataGridView1.Rows[4].Cells[2].Value = "Temperature";
                        dataGridView1.Rows[4].Cells[3].Value = mesure.byteNumber;
                    }
                    else if (mesure.type == 4)
                    {
                        data = decompositionByteData(trams, trams.ElementAt(4));
                        dataGridView1.Rows[5].Cells[1].Value = "" + data;
                        dataGridView1.Rows[5].Cells[0].Value = mesure.id;
                        dataGridView1.Rows[5].Cells[2].Value = "Luminosité";
                        dataGridView1.Rows[5].Cells[3].Value = mesure.byteNumber;
                   
                    }
                    else if (mesure.type == 3)
                    {
                        data = decompositionByteData(trams, trams.ElementAt(4));
                        dataGridView1.Rows[3].Cells[1].Value = "" + data;
                        dataGridView1.Rows[3].Cells[0].Value = mesure.id;
                        dataGridView1.Rows[3].Cells[2].Value = "Pression atmosphere";
                        dataGridView1.Rows[3].Cells[3].Value = mesure.byteNumber;

                    }
                    else if (mesure.type ==2)
                    {
                        data = decompositionByteData(trams, trams.ElementAt(4));
                        dataGridView1.Rows[2].Cells[1].Value = "" + data;
                        dataGridView1.Rows[2].Cells[0].Value = mesure.id;
                        dataGridView1.Rows[2].Cells[2].Value = "Humidité";
                        dataGridView1.Rows[2].Cells[3].Value = mesure.byteNumber;
                 
                    }
                }
                else if (trams.ElementAt(3) == 50)
                {
                    Alarm alarm = new Alarm();
                    alarm.byteData = trams.ElementAt(6);
                    alarm.byteNumber = trams.ElementAt(4);
                    alarm.type = trams.ElementAt(5);
                    alarm.id = trams.ElementAt(3);



                    data = decompositionByteData(trams, trams.ElementAt(4));
                    dataGridView1.Rows[3].Cells[1].Value = data.ToString();
                    dataGridView1.Rows[3].Cells[0].Value = alarm.id;
                    dataGridView1.Rows[3].Cells[2].Value ="Alarm";
                    dataGridView1.Rows[3].Cells[3].Value = alarm.byteNumber;
                }
            }


            // 1 = temperature 

        }
        private int decompositionByteData(List<byte> trameList, int nbr)
        {
            byte firstPlacementData = 0;
            byte SecondPlacementData = 0;
            byte Third0PlacementData = 0;
            byte FouthrPlacementData = 0;

            int data = 0;

            if (nbr == 1)
            {
                firstPlacementData = trameList.ElementAt(6);
                data = firstPlacementData;

            }
            else if (nbr == 2)
            {
                firstPlacementData = trameList.ElementAt(6);
                SecondPlacementData = trameList.ElementAt(7);
                data = firstPlacementData;
                data += SecondPlacementData << 8;


            }
            else if (nbr == 3)
            {
                firstPlacementData = trameList.ElementAt(6);
                SecondPlacementData = trameList.ElementAt(7);
                Third0PlacementData = trameList.ElementAt(8);
                data = firstPlacementData;
                data += SecondPlacementData << 8;
                data += Third0PlacementData << 16;


            }
            else if (nbr == 4)
            {
                firstPlacementData = trameList.ElementAt(6);
                SecondPlacementData = trameList.ElementAt(7);
                Third0PlacementData = trameList.ElementAt(8);
                FouthrPlacementData = trameList.ElementAt(9);
                data = firstPlacementData;
                data += SecondPlacementData << 8;
                data += Third0PlacementData << 16;
                data += FouthrPlacementData << 24;

            }

            return data;
        }

       
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void ID_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Max_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
         
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
        private void changeAccessLabel(string accessLevel)
        {
            switch (accessLevel)
            {
                case "1":
                    accessLabel.Text = "accès de level 1";
                    break;
                case "2":
                    accessLabel.Text = "accès de level 2";
                    break;
                case "3":
                    accessLabel.Text = "accès de level 3";
                    break;
                case "4":
                    accessLabel.Text = "accès de level 4";
                    break;
                case "5":
                    accessLabel.Text = "accès de level 5";
                    break;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            String name;
            String pwd;


            int index = 0;
            bool UserFound = false;
            bool pwdOk = false;
            string accessId = "0";

            foreach (DataRow row in Local_UserTable.Rows)
            {
                MessageBox.Show(row["UserName"].ToString());
                if (row["UserName"].ToString() == userName.Text)
                {
                    
                    pwd = row["UserPassword"].ToString();
                    if(pwd == pwdLabel.Text)
                    {
                        pwdOk = true;
                        accessId = row["Access_id"].ToString();
                    }
                    index = Local_UserTable.Rows.IndexOf(row);
                    UserFound = true;
                }
            }
            if (UserFound && pwdOk)
            {
                changeAccessLabel(accessId);
            }
            else
            {
                accessLabel.Text = "vérifier vos identifiant";
            }

        }
    }
}
