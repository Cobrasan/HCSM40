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
    public partial class ioMonitorfrm : Form
    {
        public ioMonitorfrm()
        {
            InitializeComponent();
        }

        private struct ioDsp
        {
            public int bit;
            public PictureBox pic;
            public Label IoNoLabel;
            public Label IoCommentLabel;
        }

        private ioDsp[] ioDspArr1 = null, ioDspArr2 = null;
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);

            // Io表示定義を行う
            ioDspDefine();

#if FOR_JN03SDC
            // ボタンの定義を行う
            setIoSelectBtnEvent(btnIo1In1, SystemConstants.IO_DISPLAY_PORT, SystemConstants.IO_MONITOR_BOARD_1_IN1);
            setIoSelectBtnEvent(btnIo1In2, SystemConstants.IO_DISPLAY_PORT, SystemConstants.IO_MONITOR_BOARD_1_IN2);
            setIoSelectBtnEvent(btnIo1Out1, SystemConstants.IO_DISPLAY_PORT, SystemConstants.IO_MONITOR_BOARD_1_OUT1);
            setIoSelectBtnEvent(btnIo1Out2, SystemConstants.IO_DISPLAY_PORT, SystemConstants.IO_MONITOR_BOARD_1_OUT2);
            setIoSelectBtnEvent(btnIo2In1, SystemConstants.IO_DISPLAY_PORT, SystemConstants.IO_MONITOR_BOARD_2_IN1);
            setIoSelectBtnEvent(btnIo2In2, SystemConstants.IO_DISPLAY_PORT, SystemConstants.IO_MONITOR_BOARD_2_IN2);
            setIoSelectBtnEvent(btnIo2Out1, SystemConstants.IO_DISPLAY_PORT, SystemConstants.IO_MONITOR_BOARD_2_OUT1);
            setIoSelectBtnEvent(btnIo2Out2, SystemConstants.IO_DISPLAY_PORT, SystemConstants.IO_MONITOR_BOARD_2_OUT2);
#endif
        }

        private void ioDspDefine()
        {
            ioDspArr1 = new ioDsp[] { 
                makeIoDsp(0, pbIO1, lblIoCode1, lblIoComment1), 
                makeIoDsp(1, pbIO2, lblIoCode2, lblIoComment2), 
                makeIoDsp(2, pbIO3, lblIoCode3, lblIoComment3),
                makeIoDsp(3, pbIO4, lblIoCode4, lblIoComment4), 
                makeIoDsp(4, pbIO5, lblIoCode5, lblIoComment5), 
                makeIoDsp(5, pbIO6, lblIoCode6, lblIoComment6), 
                makeIoDsp(6, pbIO7, lblIoCode7, lblIoComment7), 
                makeIoDsp(7, pbIO8, lblIoCode8, lblIoComment8), 
                makeIoDsp(8, pbIO9, lblIoCode9, lblIoComment9), 
                makeIoDsp(9, pbIO10, lblIoCode10, lblIoComment10), 
                makeIoDsp(10, pbIO11, lblIoCode11, lblIoComment11), 
                makeIoDsp(11, pbIO12, lblIoCode12, lblIoComment12), 
                makeIoDsp(12, pbIO13, lblIoCode13, lblIoComment13), 
                makeIoDsp(13, pbIO14, lblIoCode14, lblIoComment14), 
                makeIoDsp(14, pbIO15, lblIoCode15, lblIoComment15), 
                makeIoDsp(15, pbIO16, lblIoCode16, lblIoComment16) 
            };

            ioDspArr2 = new ioDsp[] { 
                makeIoDsp(0, pbIO17, lblIoCode17, lblIoComment17), 
                makeIoDsp(1, pbIO18, lblIoCode18, lblIoComment18), 
                makeIoDsp(2, pbIO19, lblIoCode19, lblIoComment19),
                makeIoDsp(3, pbIO20, lblIoCode20, lblIoComment20), 
                makeIoDsp(4, pbIO21, lblIoCode21, lblIoComment21), 
                makeIoDsp(5, pbIO22, lblIoCode22, lblIoComment22), 
                makeIoDsp(6, pbIO23, lblIoCode23, lblIoComment23), 
                makeIoDsp(7, pbIO24, lblIoCode24, lblIoComment24), 
                makeIoDsp(8, pbIO25, lblIoCode25, lblIoComment25), 
                makeIoDsp(9, pbIO26, lblIoCode26, lblIoComment26), 
                makeIoDsp(10, pbIO27, lblIoCode27, lblIoComment27), 
                makeIoDsp(11, pbIO28, lblIoCode28, lblIoComment28), 
                makeIoDsp(12, pbIO29, lblIoCode29, lblIoComment29), 
                makeIoDsp(13, pbIO30, lblIoCode30, lblIoComment30), 
                makeIoDsp(14, pbIO31, lblIoCode31, lblIoComment31), 
                makeIoDsp(15, pbIO32, lblIoCode32, lblIoComment32) 
            };
        }

        private ioDsp makeIoDsp(int bit, PictureBox pic, Label noLabel, Label commentLabel)
        {
            ioDsp dsp = new ioDsp();

            dsp.bit = bit;
            dsp.pic = pic;
            dsp.IoNoLabel = noLabel;
            dsp.IoCommentLabel = commentLabel;

            return dsp;
        }

        public void refresh()
        {
            // IO表示を更新する
            updateIoDsp();

#if FOR_JN03SDC
            // どのボタンが選択されているか取得
            double boardNod = 0.0;
            Program.DataController.ReadWorkData(SystemConstants.IO_DISPLAY_PORT, ref boardNod);
            int boardNo = (int)boardNod;

            // ボタンの表示を更新する
            mainfrm.CheckBtnAnd_ChangeColor((boardNo == SystemConstants.IO_MONITOR_BOARD_1_IN1), btnIo1In1);
            mainfrm.CheckBtnAnd_ChangeColor((boardNo == SystemConstants.IO_MONITOR_BOARD_1_IN2), btnIo1In2);
            mainfrm.CheckBtnAnd_ChangeColor((boardNo == SystemConstants.IO_MONITOR_BOARD_1_OUT1), btnIo1Out1);
            mainfrm.CheckBtnAnd_ChangeColor((boardNo == SystemConstants.IO_MONITOR_BOARD_1_OUT2), btnIo1Out2);
            mainfrm.CheckBtnAnd_ChangeColor((boardNo == SystemConstants.IO_MONITOR_BOARD_2_IN1), btnIo2In1);
            mainfrm.CheckBtnAnd_ChangeColor((boardNo == SystemConstants.IO_MONITOR_BOARD_2_IN2), btnIo2In2);
            mainfrm.CheckBtnAnd_ChangeColor((boardNo == SystemConstants.IO_MONITOR_BOARD_2_OUT1), btnIo2Out1);
            mainfrm.CheckBtnAnd_ChangeColor((boardNo == SystemConstants.IO_MONITOR_BOARD_2_OUT2), btnIo2Out2);
#endif
        }

        private void updateIoDsp()
        {
#if FOR_JN03SDC
            // 現在選択されている番号を取得する
            double boardNo = 0.0;
            Program.DataController.ReadWorkData(SystemConstants.IO_DISPLAY_PORT, ref boardNo);

            int ioStart = 0;
            switch ((int)boardNo)
            {
                case SystemConstants.IO_MONITOR_BOARD_NONE:
                    clearIoDsp();
                    return;
                case SystemConstants.IO_MONITOR_BOARD_1_IN1:
                    ioStart = 200;
                    break;
                case SystemConstants.IO_MONITOR_BOARD_1_IN2:
                    ioStart = 232;
                    break;
                case SystemConstants.IO_MONITOR_BOARD_1_OUT1:
                    ioStart = 100;
                    break;
                case SystemConstants.IO_MONITOR_BOARD_1_OUT2:
                    ioStart = 132;
                    break;
                default:
                    mainfrm.WriteWorkData(SystemConstants.IO_DISPLAY_PORT, SystemConstants.IO_MONITOR_BOARD_NONE);
                    return;
            }

            // IO状態を表示する
            double dio1 = 0.0, dio2 = 0.0;
            Program.DataController.ReadWorkData(SystemConstants.IO_MONITOR1, ref dio1);
            Program.DataController.ReadWorkData(SystemConstants.IO_MONITOR2, ref dio2);
            int io1 = (int)dio1;
            int io2 = (int)dio2;

            for (int i = 0; i < ioDspArr1.Count(); i++)
            {
                {
                    // ビット計算
                    bool bitFlg = (((io1 >> ioDspArr1[i].bit) & 0x0001) > 0);
                    // LED表示を更新
                    int ioNo = ioStart + i;
                    mainfrm.CheckAnd_ChangePicture(bitFlg, ioDspArr1[i].pic, Alchemist.Properties.Resources.LedRedOn, Alchemist.Properties.Resources.LedRedOff);
                    Program.MainForm.refreshControl(ioNo.ToString(), ioDspArr1[i].IoNoLabel);
                    Program.MainForm.refreshControl(Utility.GetMessageString(SystemConstants.IO_ASSIGN_MSG, ioNo), ioDspArr1[i].IoCommentLabel);
                }
                {
                    // ビット計算
                    bool bitFlg = (((io2 >> ioDspArr1[i].bit) & 0x0001) > 0);
                    // LED表示を更新
                    int ioNo = ioStart + 16 + i;
                    mainfrm.CheckAnd_ChangePicture(bitFlg, ioDspArr2[i].pic, Alchemist.Properties.Resources.LedRedOn, Alchemist.Properties.Resources.LedRedOff);
                    Program.MainForm.refreshControl(ioNo.ToString(), ioDspArr2[i].IoNoLabel);
                    Program.MainForm.refreshControl(Utility.GetMessageString(SystemConstants.IO_ASSIGN_MSG, ioNo), ioDspArr2[i].IoCommentLabel);
                }
            }
#endif
        }

        private void clearIoDsp()
        {
            for (int i = 0; i < ioDspArr1.Count(); i++)
            {
                {
                    // LED表示を更新
                    mainfrm.CheckAnd_ChangePicture(false, ioDspArr1[i].pic, Alchemist.Properties.Resources.LedRedOn, Alchemist.Properties.Resources.LedRedOff);
                    Program.MainForm.refreshControl("", ioDspArr1[i].IoNoLabel);
                    Program.MainForm.refreshControl("", ioDspArr1[i].IoCommentLabel);
                }
                {
                    // LED表示を更新
                    mainfrm.CheckAnd_ChangePicture(false, ioDspArr2[i].pic, Alchemist.Properties.Resources.LedRedOn, Alchemist.Properties.Resources.LedRedOff);
                    Program.MainForm.refreshControl("", ioDspArr2[i].IoNoLabel);
                    Program.MainForm.refreshControl("", ioDspArr2[i].IoCommentLabel);
                }
            }
        }

        // 画面表示を更新する
        private void updateDspState(int status, int bit, PictureBox bitPic)
        {
            // ビット計算
            bool bitFlg = (((status >> bit) & 0x0001) > 0);

            // LED表示を更新
            mainfrm.CheckAnd_ChangePicture(bitFlg, bitPic, Alchemist.Properties.Resources.LedRedOn, Alchemist.Properties.Resources.LedRedOff);
        }

        private void ioMonitorfrm_FormClosing(object sender, FormClosingEventArgs e)
        {

#if FOR_JN03SDC
            // 閉じる際に、0を送る
            mainfrm.WriteWorkData(SystemConstants.IO_DISPLAY_PORT, 0);
#endif

            e.Cancel = true;
            this.Visible = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private static void setIoSelectBtnEvent(Control ctl, int workID, int setValue)
        {
            ctl.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                mainfrm.WriteWorkData(workID, setValue);
            });
        }
    }
}
