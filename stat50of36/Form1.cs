using System;
using System.Configuration;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace stat50of36
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private List<int> data = new List<int>();
        private int nextint;
        private Dictionary<int, int> dictionary;
        private bool mac = false;
        private string Decryptcode;
        private void fillChart(int y)
        {
            chart1.Series["datapoints"].Points.Clear();
            foreach (KeyValuePair<int, int> entry in dictionary)
            {
                chart1.Series["datapoints"].Points.AddXY(entry.Key, entry.Value);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (int.Parse(textBox1.Text) < 37)
                {
                    textBox1.ForeColor = Color.Green;
                    nextint = int.Parse(textBox1.Text);
                }
                else
                {
                    textBox1.ForeColor = Color.Red;
                    MessageBox.Show("You must enter a value between 1 - 36");
                    textBox1.Clear();
                }
            }
            catch (Exception)
            {

                textBox1.ForeColor = Color.Red;
            }
        }



        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            dictionary = new Dictionary<int, int>(
                Enumerable.Range(1, 36).ToDictionary(i => i, _ => 0)
                );

            //MessageBox.Show("MAC Address: " + macAddress);
            //CustomBusyBox.BusyBox.ShowBusy();
            string key = "no key";
            try
            {
                key = ConfigurationManager.AppSettings["Dcode"];
                Decryptcode = AesOperation.DecryptString("b14ca5898a4e4133bbce2ea2315a1916", key);
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to read the key");
                this.Close();
            }
            try
            {
                //string test = "1062E5E00299,0250F2F5ACE0,005056C00001,005056C00008,00FFBEF89159,00FF7EF1B0B9,";
                //string encrypted = AesOperation.EncryptString("b14ca5898a4e4133bbce2ea2315a1916", test);
                //MessageBox.Show(encrypted);
                //MessageBox.Show(AesOperation.DecryptString("b14ca5898a4e4133bbce2ea2315a1916", encrypted));

                //"9EL4TGcAJOuCgRa+mDrDrBJoBYDDXh8+tH8qjn+JIvPhSvWB0qyemXjPeBAS0z9xjgqs8ZtUKwTHZMz3xg76nQfZp7UwDzIWSeg7Fo99UW4=";

                //Decryptcode = AesOperation.DecryptString("b14ca5898a4e4133bbce2ea2315a1916", "9EL4TGcAJOuCgRa+mDrDrBJoBYDDXh8+tH8qjn+JIvPhSvWB0qyemXjPeBAS0z9xjgqs8ZtUKwTHZMz3xg76nQfZp7UwDzIWSeg7Fo99UW4=");

                checkmac(Decryptcode);
                
                if (mac)
                {

                }
                else
                {
                    MessageBox.Show("You have no right to use this software");
                    this.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Failed to read the key");
                this.Close();
            }
            //CustomBusyBox.BusyBox.CloseBusy();
        }

        private void checkmac(string Decryptcode)
        {
            string macAddress = "";
            //foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            //{
            //    if (nic.OperationalStatus == OperationalStatus.Up)
            //    {
            //        macAddress += nic.GetPhysicalAddress().ToString() + " ";
            //    }
            //}
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    macAddress += nic.GetPhysicalAddress().ToString() + ",";
                }
            }
            //MessageBox.Show(macAddress);
            string[] macset = macAddress.Split(',');
            string[] checkset = Decryptcode.Split(',');
            foreach (string mac1 in macset)
            {
                foreach (string check1 in checkset)
                {
                    if (mac1 == check1)
                    {
                        mac = true;
                    }
                    else
                    {
                    }
                }
                
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Space)
            {
                try
                {
                    if (nextint == 0)
                    {
                        MessageBox.Show("You must enter a value between 1 - 36");
                    }
                    else if (data.Count < 50)
                    {
                        data.Add(nextint);
                        dictionary[nextint]++;
                    }
                    else
                    {

                        dictionary[data[0]]--;
                        dictionary[nextint]++;
                        data.Remove(data[0]);
                        data.Add(nextint);
                    }
                }
                catch (Exception)
                {

                    MessageBox.Show("You must enter a value between 1 - 36");
                }
                label1.Text = data.Count.ToString();

                string listString = "";

                foreach (int number in data)
                {
                    listString += number + ", ";
                }

                label2.Text = listString;

                fillChart(nextint);

                textBox1.Clear();
                nextint = 0;
            }
        }


        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
