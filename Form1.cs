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
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DiscordImageSaver
{
    public partial class Form1 : Form
    {

        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
            System.UInt32 pBC,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
            [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
            System.UInt32 cbSize,
            [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
            System.UInt32 dwMimeFlags,
            out System.UInt32 ppwzMimeOut,
            System.UInt32 dwReserverd
        );

        public Form1()
        {
            InitializeComponent();
        }

        String discordCachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Discord/Cache/";
        String savePath = string.Empty;

        public Dictionary<string, string> ImageTypes { get; private set; }

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

                    if (fileName.StartsWith("data") || fileName.StartsWith("index"))
                    {
                        continue;
                    }

                    byte[] buffer = new byte[256];
                    using (FileStream fileStream = new FileStream(discordCachePath + fileName, FileMode.Open, FileAccess.Read))
                    {
                        if (fileStream.Length >= 256)
                        {
                            fileStream.Read(buffer, 0, 256);
                        } else
                        {
                            fileStream.Read(buffer, 0, buffer.Length);
                        }
                    } try
                    {
                        System.UInt32 mimeType;
                        FindMimeFromData(0, null, buffer, 256, null, 0, out mimeType, 0);
                        System.IntPtr mimeTypePtr = new IntPtr(mimeType);
                        string mime = Marshal.PtrToStringUni(mimeTypePtr);

                        String type = "." + splitText(mime);

                        if (bunifuCheckbox1.Checked)
                        {
                            Directory.Move(file, Path.Combine(savePath, fileName + Path.GetRandomFileName() + type));
                        } else
                        {
                            File.Copy(Path.Combine(discordCachePath, fileName), Path.Combine(savePath, fileName + Path.GetRandomFileName() + type), true);
                        }
                    }
                    catch (Exception) { }

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

        private String splitText(String mimeType)
        {
            String mime = mimeType.Split('/')[1];
            if (mime.StartsWith("x-"))
            {
                mime = mime.Substring(2);
            }
            return mime;
        }
    }
}
