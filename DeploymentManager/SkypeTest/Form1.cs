using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SKYPE4COMLib;

namespace SkypeTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ISkype skype = new Skype();
            skype.Attach(5, true);
            Debug.WriteLine(skype.ActiveChats.Count);
            foreach (IChat chat in skype.Chats)
            {
                if (chat.Name.Contains("zigunova"))
                {
                    var t = listBox1.Items.Add(chat.Name + "--" + chat.FriendlyName + " *************");
                    Debug.WriteLine(chat.Name);
                }
                else
                {
                    var t = listBox1.Items.Add(chat.Name+"--"+chat.FriendlyName);
                    Debug.WriteLine(chat.Name);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ISkype skype = new Skype(); 
            skype.Attach(5, true);
            skype.Chat["#zigunova.olga/$1978b58b71643582"].SendMessage("just Testing");
        }

    }
}
