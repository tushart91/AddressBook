using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace AddressBook
{
    public partial class Form1 : Form
    {
        int index;
        List<Person> people = new List<Person>();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (!Directory.Exists(path + "\\AddressBook"))
                Directory.CreateDirectory(path + "\\AddressBook");
            if (!File.Exists(path + "\\AddressBook\\settings.xml"))
            {
                XmlTextWriter xw = new XmlTextWriter(path + "\\AddressBook\\settings.xml", Encoding.UTF8);
                xw.WriteStartElement("People");
                xw.WriteEndElement();
                xw.Close();
            }
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(path + "\\AddressBook\\settings.xml");
            foreach (XmlNode xNode in xdoc.SelectNodes("People/Person"))
            {
                Person p = new Person();
                p.name = xNode.SelectSingleNode("Name").InnerText;
                p.emailID = xNode.SelectSingleNode("Email").InnerText;
                p.faceBook = xNode.SelectSingleNode("Facebook").InnerText;
                p.phoneNo = xNode.SelectSingleNode("Phone").InnerText;
                p.additionalNotes = xNode.SelectSingleNode("Notes").InnerText;
                p.birthday = DateTime.FromFileTime( Convert.ToInt64(xNode.SelectSingleNode("Birthday").InnerText));
                people.Add(p);
                listBox.Items.Add(p.name);
            }
        }

        private void listBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                index = listBox.SelectedIndex;
                textBox1.Text = people[index].name;
                textBox2.Text = people[index].emailID;
                textBox3.Text = people[index].faceBook;
                textBox4.Text = people[index].phoneNo;
                textBox5.Text = people[index].additionalNotes;
                dateTimePicker1.Value = people[index].birthday;
            }
            catch { }
        }

        private void add_Click(object sender, EventArgs e)
        {
            Person p = new Person();
            p.name = textBox1.Text;
            p.emailID = textBox2.Text;
            p.faceBook = textBox3.Text;
            p.birthday = dateTimePicker1.Value;
            p.phoneNo = textBox4.Text;
            p.additionalNotes = textBox5.Text;
            people.Add(p);
            listBox.Items.Add(p.name);
            textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
            dateTimePicker1.Value = DateTime.Now;
        }

        private void save_Click(object sender, EventArgs e)
        {
            try
            {
                people[index].name = textBox1.Text;
                people[index].emailID = textBox2.Text;
                people[index].faceBook = textBox3.Text;
                people[index].phoneNo = textBox4.Text;
                people[index].additionalNotes = textBox5.Text;
                people[index].birthday = dateTimePicker1.Value; ;
                listBox.Items[index] = textBox1.Text;
            }
            catch
            { }
        }

        private void remove_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = textBox5.Text = "";
                dateTimePicker1.Value = DateTime.Now;
                people.RemoveAt(index);
                listBox.Items.RemoveAt(index);
            }
            catch { }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            XmlDocument xdoc = new XmlDocument();
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            xdoc.Load(path + "\\AddressBook\\settings.xml");
            XmlNode xnode = xdoc.SelectSingleNode("People");
            xnode.RemoveAll();
            foreach (Person p in people)
            {
                XmlNode xTop = xdoc.CreateElement("Person");
                XmlNode xName = xdoc.CreateElement("Name");
                XmlNode xEmail = xdoc.CreateElement("Email");
                XmlNode xFacebook = xdoc.CreateElement("Facebook");
                XmlNode xNotes = xdoc.CreateElement("Notes");
                XmlNode xBirthday = xdoc.CreateElement("Birthday");
                XmlNode xPhone = xdoc.CreateElement("Phone");
                xName.InnerText = p.name;
                xEmail.InnerText = p.emailID;
                xFacebook.InnerText = p.faceBook;
                xNotes.InnerText = p.additionalNotes;
                xBirthday.InnerText = p.birthday.ToFileTime().ToString();
                xPhone.InnerText = p.phoneNo;
                xTop.AppendChild(xName);
                xTop.AppendChild(xEmail);
                xTop.AppendChild(xFacebook);
                xTop.AppendChild(xNotes);
                xTop.AppendChild(xBirthday);
                xTop.AppendChild(xPhone);
                xdoc.DocumentElement.AppendChild(xTop);
            }
            xdoc.Save(path + "\\AddressBook\\settings.xml");
        }
    }
    class Person
    {
        public string name
        {
            get;
            set;
        }
        public string emailID
        {
            get;
            set;
        }
        public string faceBook
        {
            get;
            set;
        }
        public string phoneNo
        {
            get;
            set;
        }
        public string additionalNotes
        {
            get;
            set;
        }
        public DateTime birthday
        {
            get;
            set;
        }
    }
}
