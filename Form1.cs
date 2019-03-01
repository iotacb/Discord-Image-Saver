using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace DiscordImageSaver
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        String discordCachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Discord/Cache/";
        String savePath = string.Empty;

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            Form1.ActiveForm.WindowState = FormWindowState.Minimized;
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            List<String> files = new List<String>(Directory.EnumerateFiles(discordCachePath));
            if (savePath != "")
            {
                bunifuFlatButton3.Enabled = false;
                foreach (String file in files)
                {
                    String fileName = file.Substring(discordCachePath.Length);
                    if (bunifuCheckbox1.Checked)
                    {
                        Directory.Move(file, Path.Combine(savePath, fileName + ".png"));
                    } else
                    {
                        File.Copy(Path.Combine(discordCachePath, fileName), Path.Combine(savePath, fileName + ".png"), true);
                    }
                }
                bunifuFlatButton3.Enabled = true;
            } else
            {
                MessageBox.Show("Please enter a valid path!");
            }
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                savePath = dialog.SelectedPath;
                bunifuMetroTextbox1.Text = savePath;
            }
        }
    }
}
