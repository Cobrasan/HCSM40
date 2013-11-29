using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alchemist
{
    public partial class errImageZoomfrm : Form
    {
        public errImageZoomfrm()
        {
            InitializeComponent();
        }

        public void Initialize()
        {

        }

        private void errImageZoomfrm_MouseLeave(object sender, EventArgs e)
        {
            formClose();
        }

        private void pbZoomImage_MouseLeave(object sender, EventArgs e)
        {
            formClose();
        }

        private void formClose()
        {
            Visible = false;
        }

        public void UpdatePicture(Image img)
        {
            if (pbZoomImage.Image != img)
            {
                pbZoomImage.Image = img;
                pbZoomImage.Refresh();
            }
        }
    }
}
