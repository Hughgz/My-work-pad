using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace MyWorkPad
{
    public partial class Form1 : Form
    {
        private string file = "";
        private string filePath = "";

        public Form1()
        {
            InitializeComponent();
        }

        

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblDongHo.Text = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt K");
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {

        }
        private void XuLyLuuFile(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = saveFileDialog.FileName;
                }
                else
                {
                    return;
                }
            }
            File.WriteAllText(filePath, rtdDoc.Text);
        }
        private void XuLyMoFile(object sender, EventArgs e)
        {
            DialogResult dr = openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "WorkPad|*.rtf|Text|*.txt";
            if(dr == DialogResult.OK)
            {
                StreamReader streamReader = new StreamReader(openFileDialog1.FileName);
                streamReader.Close();
                file = openFileDialog1.FileName;
                string fileContent = System.IO.File.ReadAllText(file);
                rtdDoc.Text = fileContent;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void lblDongHo_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
           FontDialog font = new FontDialog();
            font.ShowApply = true;
            font.Apply += new EventHandler(XuLyApply);
            if(font.ShowDialog() == DialogResult.OK)
            {
                rtdDoc.SelectionFont = font.Font;
                rtdDoc.SelectionColor = font.Color;
            }
        }
       

        private void XuLyApply(object sender, EventArgs e)
        {

        }

        private void newFile_Click(object sender, EventArgs e)
        {
            rtdDoc.Clear();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void printFile_Click(object sender, EventArgs e)
        {
            DialogResult dr = printDialog1.ShowDialog();
            if(dr == DialogResult.OK)
            {

            }
        }

        private void undoFile_Click(object sender, EventArgs e)
        {
            if(rtdDoc.CanUndo == true)
            {
                rtdDoc.Undo();
            }
        }

        private void redoFile_Click(object sender, EventArgs e)
        {
            if (rtdDoc.CanRedo == true)
            {
                rtdDoc.Redo();
            }
        }

        private void cutFile_Click(object sender, EventArgs e)
        {
            if (rtdDoc.SelectedText != "")
            {
                rtdDoc.Cut();
            }
        }

        private void copyFile_Click(object sender, EventArgs e)
        {
            if (rtdDoc.SelectionLength > 0)
            {
                rtdDoc.Copy();
            }
        }

        private void pasteFile_Click(object sender, EventArgs e)
        {
            if(Clipboard.GetDataObject().GetDataPresent(DataFormats.Text) == true) {
                rtdDoc.Paste();
            }
        }

        private void selectAllFile_Click(object sender, EventArgs e)
        {
            if (rtdDoc.Text == "")
            {
                MessageBox.Show("Không có từ để quét chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                rtdDoc.SelectAll();
            }
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            string searchText = txtFind.Text;
            string documentText = rtdDoc.Text;
            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Vui lòng nhập từ cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = documentText.IndexOf(searchText);
            if (index == -1)
            {
                MessageBox.Show("Không tìm thấy từ cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            rtdDoc.Select(index, searchText.Length);
            rtdDoc.Focus();
        }

        private void findReplaceFile_Click(object sender, EventArgs e)
        {
            string searchText = txtFindText.Text;
            string replaceText = txtReplaceText.Text;
            string documentText = rtdDoc.Text;
            string newText = documentText.Replace(searchText, replaceText);
            rtdDoc.Text = newText;
        }

        private void saveAsFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                System.IO.File.WriteAllText(filePath, rtdDoc.Text);
            }
        }

        private void insertImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg;*.gif;*.png)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string imagePath = openFileDialog.FileName;
                Image image = Image.FromFile(imagePath);
                Clipboard.SetImage(image);
                rtdDoc.Paste();
            }
        }

        private void richTextBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (rtdDoc.SelectionType == RichTextBoxSelectionTypes.Object &&
                    rtdDoc.SelectedRtf != null)
                {
                    rtdDoc.DoDragDrop(rtdDoc.SelectedRtf, DragDropEffects.Move);
                }
            }
        }

        private void richTextBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Rtf))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void richTextBox1_DragDrop(object sender, DragEventArgs e)
        {
            int x = e.X - rtdDoc.Left;
            int y = e.Y - rtdDoc.Top;
            if (e.Data.GetDataPresent(DataFormats.Rtf))
            {
                rtdDoc.SelectedRtf = (string)e.Data.GetData(DataFormats.Rtf);
                rtdDoc.GetPositionFromCharIndex(rtdDoc.SelectionStart);
            }
        }

        private void fontColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                rtdDoc.SelectionColor = colorDialog.Color;
            }
        }

        private void boldText_Click(object sender, EventArgs e)
        {
            Font currentFont = rtdDoc.SelectionFont;
            FontStyle newFontStyle = FontStyle.Bold;

            if (currentFont != null && currentFont.Bold)
                newFontStyle = FontStyle.Regular;

            rtdDoc.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);  
        }

        private void italicText_Click(object sender, EventArgs e)
        {
            Font currentFont = rtdDoc.SelectionFont;
            FontStyle newFontStyle = FontStyle.Italic;

            if (currentFont != null && currentFont.Italic)
                newFontStyle = FontStyle.Regular;

            rtdDoc.SelectionFont = new Font(currentFont.FontFamily, currentFont.Size, newFontStyle);
        }

        private void underlineText_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionCharOffset = rtdDoc.SelectionCharOffset == 0 ? -1 : 0;
        }

        private void normalText_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionFont = new Font(rtdDoc.Font, FontStyle.Regular);
            rtdDoc.SelectionColor = Color.Black;
            
        }

        private void pageColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                rtdDoc.BackColor = colorDialog.Color;
            }
        }
        private void indentNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionIndent = 0;
        }

        private void indent5ptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionIndent = 5;
        }

        private void indent10ptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionIndent = 10;
        }

        private void indent15ptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionIndent = 15;
        }

        private void indent20ptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionIndent = 20;
        }

        private void alignLeft_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionAlignment = HorizontalAlignment.Left;
        }

        private void alignCenter_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionAlignment = HorizontalAlignment.Center;
        }

        private void alignRight_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void addBullets_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionBullet = true;
        }

        private void removeBullets_Click(object sender, EventArgs e)
        {
            rtdDoc.SelectionBullet = false;
        }

        private void help_Click(object sender, EventArgs e)
        {
            member m = new member();
            m.ShowDialog();
        }

        private void colorButton_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                Color selectedColor = colorDialog.Color;
                rtdDoc.SelectionColor = selectedColor;
            }
        }

        private void findButton_Click(object sender, EventArgs e)
        {
            string searchText = txtFindButton.Text;
            string documentText = rtdDoc.Text;
            txtFindButton.Visible = !txtFindButton.Visible;
            if (txtFindButton.Visible)
            {
                txtFindButton.Focus();
            }
            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Vui lòng nhập từ cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            int index = documentText.IndexOf(searchText);
            if (index == -1)
            {
                MessageBox.Show("Không tìm thấy từ cần tìm kiếm.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            rtdDoc.Select(index, searchText.Length);
            rtdDoc.Focus();
        }
    }
}
