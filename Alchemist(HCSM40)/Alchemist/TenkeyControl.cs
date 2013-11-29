using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Alchemist
{

    public delegate void dataEneterDelegate(TenKeyDataStruct tdata);

    public struct TenKeyDataStruct
    {
        public object obj;
        public double value;
        public int workid;
        public int workidtype;
        public int columindex;
        public int rowindex;
        public int actiontype;
    }

    class TenkeyControl
    {
        public event dataEneterDelegate dataEneterEvent;

        public TenKeyDataStruct tenKeyData;

        // テンキー表示
        public void tenkeyFormShow()
        {
            tenKeyfrm tenKeyForm = new tenKeyfrm();
            tenKeyForm.enterKeyEvent += new enterKeyDelegate(enterKeyEvent);
            tenKeyForm.val = tenKeyData.value;
            tenKeyForm.ShowDialog();
            tenKeyForm.Dispose();
        }

        // テンキーからエンター入力イベント
        public void enterKeyEvent(double value)
        {
            tenKeyData.value = value;
            dataEneterEvent(tenKeyData);
        }

    }
}
