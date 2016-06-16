using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NpcsToCards
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder(NpcTexts.FantacyAllies);

            using (var memory = new MemoryStream(Encoding.ASCII.GetBytes(NpcTexts.FantacyAllies)))
            using (var streamReader = new StreamReader(memory))
            {
                var reader = new NpcReader(streamReader);
                reader.ReadAll();
                document.Npcs = reader.Npcs;
            }
            if (printPreviewDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (printDialog.ShowDialog(this) == DialogResult.OK)
                {
                    document.Print();
                }
            }
        }
    }
}