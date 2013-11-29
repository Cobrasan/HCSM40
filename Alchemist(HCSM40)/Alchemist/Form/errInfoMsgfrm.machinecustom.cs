using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;

namespace Alchemist
{
    public partial class errInfoMsgfrm : Form
    {
        const int SHOW_INFORMATION = 0;
        const int SHOW_ERROR = 1;
        const int SHOW_ERROR_INFORMATION = 2;
        const int LIST_INFORMATION = 0;
        const int LIST_ERROR = 1;

        private void formcustom()
        {

#if ERROR_IMAGE
            pbErrorImage.Visible = true;
            listError.Size = new Size(419, 244);
#else
            pbErrorImage.Visible = false;
            listError.Size = new Size(593, 244);
#endif

        }

        private string getErrPictureFolder()
        {
            string folder = "";
#if ERROR_IMAGE && FOR_JN03SDWP
            folder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\JAM\\Alchemist\\pic\\JN03SD-WP\\";
#endif
            return folder;
        }

    }
}