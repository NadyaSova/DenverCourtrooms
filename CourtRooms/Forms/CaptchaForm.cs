using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CourtRooms
{
    public partial class CaptchaForm : Form
    {
        public string Captcha { get; set; }

        public CaptchaForm(Bitmap image)
        {
            InitializeComponent();
            picCaptcha.Image = image;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            ReturnCaptcha();
        }

        private void txtCaptcha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                ReturnCaptcha();
        }

        private void ReturnCaptcha()
        {
            this.Captcha = txtCaptcha.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
