using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace FileList
{
    public partial class Form1 : Form
    {
        private StreamWriter _output;
        public Form1()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if(this.folderBrowserDialog1.ShowDialog()==DialogResult.OK)
            {
                this.textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = null;
            try
            {
                _output = File.CreateText(Path.Combine(this.textBox1.Text,"fileList.xls"));
                dir = new DirectoryInfo(this.textBox1.Text);
            }
            catch(Exception ex)
            {
                this.textErrors.Text += ex.Message + "\r\n";
            }
            if (dir!=null)DoSubFolders(dir);
            try { _output.Close(); }
            catch (Exception ex)
            {
                this.textErrors.Text += ex.Message + "\r\n";
            }
            this.textErrors.Text += "delimited file saved as .. " + Path.Combine(this.textBox1.Text, "fileList.xls") + "\r\n";
        }
        protected string GetFullDirNameSafe(DirectoryInfo path)
        {
            string dirName = path.Name;
            DirectoryInfo path1 = path;
            while (path1.Parent != null && path1.Parent.Name != path1.Name)
            {
                path1 = path1.Parent;
                if (!path1.Name.EndsWith("\\"))
                    dirName = path1.Name + "\\" + dirName;
                else
                    dirName = path1.Name + dirName;
            }
            return dirName;
        }
        protected void RenameFilesWithDatedInNames(DirectoryInfo path)
        {
            foreach (var file in path.GetFiles("* ??-??-???? *.*"))
            {
                string dirName = GetFullDirNameSafe(path);
                try
                {
                    if ((file.Name.Length) > 260)
                    {
                        this.textErrors.Text += "The following file cannot be processed because the name is too long:\r\n";
                        this.textErrors.Text += dirName + @"\" + @"\" + file.Name + "\r\n";
                        continue;
                    }
                    var positionOfDate = 0;
                    var filename = file.Name;
                    positionOfDate = filename.IndexOf('-') - 2;
                    if (positionOfDate >= 0)
                    {
                        try
                        {
                            var year = int.Parse(filename.Substring(positionOfDate + 6, 4));
                            var month = int.Parse(filename.Substring(positionOfDate, 2));
                            var day = int.Parse(filename.Substring(positionOfDate + 3, 2));
                            var newDate = new DateTime(year, month, day);
                            var newfilename = filename.Substring(0, positionOfDate) +
                                              filename.Substring(positionOfDate + 10);
                            newfilename = newDate.ToString("yyyyMMdd") + newfilename;
                            this.textErrors.Text += "Renaming " + filename + " to " + newfilename + "\r\n";
                            if (file.DirectoryName != null) file.MoveTo(Path.Combine(file.DirectoryName, newfilename));
                        }
                        catch //(Exception ex)
                        {
                            //this.textErrors.Text += "parsing file name for dates " + ex.ToString() + "\r\n";
                        }
                    }
                }
                catch (System.IO.PathTooLongException ptle)
                {
                    this.textErrors.Text += file.ToString() + "\r\n" + ptle.Data + "\r\n" + ptle.Message;
                }
                catch (Exception ex1)
                {
                    this.textErrors.Text += "parsing filename for dates, part 2 " + ex1.ToString() + "\r\n";
                }
            }

        }
        private DateTime DoFilesDates(DirectoryInfo path)
        {
            var minDate = DateTime.Now;
            string dirName=GetFullDirNameSafe(path);
            if ((dirName.Length)>240)
            {
                this.textErrors.Text += "The following directory cannot be processed because the name is too long:\r\n";
                this.textErrors.Text += dirName + @"\" + path.Name + "\r\n";
                return minDate;
            }
            foreach (var file in path.GetFiles())
            {
                if ((file.Name.Length) > 260)
                {
                    this.textErrors.Text += "The following file cannot be processed because the name is too long:\r\n";
                    this.textErrors.Text += dirName + @"\" + file.Name + "\r\n";
                    continue;
                }
                var filename = file.Name;
                if (filename.Trim().ToLower().Length < 8) continue;
                int dateInt;
                if (int.TryParse(filename.Substring(0, 8), out dateInt))
                {
                    var year = int.Parse(filename.Substring(0, 4));
                    var month = int.Parse(filename.Substring(04, 2));
                    var day = int.Parse(filename.Substring(06, 2));
                    try
                    {
                        var newDate = new DateTime(year, month, day);
                        file.LastWriteTime = newDate;
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                    }
                    catch
                    {
                    }
                }
                if (file.LastWriteTime < minDate)
                    minDate = file.LastWriteTime;
                try
                {
                    _output.WriteLine(file.LastWriteTime.ToString("yyyy-MM-dd") + "|" + dirName + "\\" + file.Name + "|" + dirName.Replace(this.textBox1.Text,string.Empty));
                }
                catch 
                {
                    this.textErrors.Text += "cannot output : " + dirName + "\\" + file.Name + "|" +
                                            file.LastWriteTime.ToString("yyyy-MM-dd") + "\r\n";
                }
            }
            return minDate;
        }
        private void DoSubFolders(DirectoryInfo path)
        {
            DoFilesDates(path);
            foreach(var subdir in path.GetDirectories())
            {
                DoSubFolders(subdir);
            }
            try
            {
                _output.Flush();   
            }
            catch
            {
            }
        }
    }
}
