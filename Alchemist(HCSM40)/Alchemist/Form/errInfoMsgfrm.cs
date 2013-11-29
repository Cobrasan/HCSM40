using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System;
using System.IO;

namespace Alchemist
{
    public partial class errInfoMsgfrm : Form
    {
        protected Stopwatch reDispTimer = new Stopwatch();
        protected Stopwatch pictureChangeTimer = new Stopwatch();
        int[] errPictureList = new int[0];
        static int errPictureIndex = 0;
        bool closeFlg = false;
        static int[] before_errBit = null;
        static int before_MachineStatus = 0;
        static bool before_IsConnectStatus = true;
        private errImageZoomfrm errImageZoomForm = new errImageZoomfrm();

        // 初期化設定
        public void Initialize()
        {
            Program.MainForm.AddOwnedForm(this);
            this.AddOwnedForm(errImageZoomForm);

            // 機械毎の表示・非表示を行う
            formcustom();

            pictureChangeTimer.Restart();
        }

        public errInfoMsgfrm()
        {
            InitializeComponent();
        }

        // 表示更新用メソッド
        public void refresh()
        {
            int errCode;
            int[] errBit = null;
            int messageID = 0;
            bool infoFlg = false;
            bool errFlg = false;
            bool dispFlg = false;
            bool compareRet = false;
            bool firstErr = false;
            bool isConnect = true;
            string message;

            const int BIT_SEEK_ORIGIN = 0x0001;     /* 原点サーチビット */
            const int BIT_ERROR = 0x0008;           /* エラービット */
            const int BIT_LOT_COMPLETE = 0x0010;    /* ロット設定満了ビット */
            const int BIT_SET_COMPLETE = 0x0020;    /* 設定本数満了ビット */

            bool isConnectStatus = Program.DataController.GetComError() == SystemConstants.COM_ERROR_NORMAL;
            int machineStatus = Program.DataController.GetMachineStatus();
            Program.DataController.GetErrorCode(ref errBit);

            // フォームを閉じてから30秒未満だった場合
            if (closeFlg == true && reDispTimer.ElapsedMilliseconds < SystemConstants.INFOMATION_REDISPLAY_INTERVAL)
            {
                // 前回のエラービットに値が設定されていた場合
                if (before_errBit != null)
                {
                    // 前回のエラービットと比較
                    compareRet = compareErrBit(errBit);
                }

                // 前回のエラー、前回のmachineStatusと比べて、変化なしの場合、処理なし
                // フォームを閉じてから30秒経過後だったら表示処理を行う
                if (compareRet == true && before_MachineStatus == machineStatus && dispFlg == false && before_IsConnectStatus == isConnectStatus)
                {
                    return;
                }

                // 30秒経過フラグを立てる
                dispFlg = true;

                // フォームクローズフラグを落とす
                closeFlg = false;
            }
            // フォームを閉じてから30秒以上だった場合
            else if (closeFlg == true)
            {
                // 最初から(起動時から)エラーが起きていた場合は表示する。
                // errBitの要素数分(8要素)ループする
                for (int elem = 0; elem < errBit.Length; elem++)
                {
                    if (errBit[elem] > 0)
                    {
                        errFlg = true;
                    }
                }

                // エラーがERR_MSG516のみの場合
                if (errFlg == false && (machineStatus & BIT_SEEK_ORIGIN) == 0 && isConnectStatus == false)
                {
                    // 再表示を行わない
                    isConnect = false;
                }
                // 30秒経過フラグを立てる
                dispFlg = true;

                // フォームクローズフラグを落とす
                closeFlg = false;
            }

            try
            {
                // 「原点サーチ中」、「エラー発生中」、「ロット満了」、「設定本数満了」
                // または、isConnectが通信異常でfalseの場合
                if (((machineStatus & BIT_SEEK_ORIGIN) != 0) || ((machineStatus & BIT_ERROR) != 0) ||
                    ((machineStatus & BIT_LOT_COMPLETE) != 0) || ((machineStatus & BIT_SET_COMPLETE) != 0) || (isConnectStatus == false))
                {
                    // 前回のエラービットに値が設定されていた場合
                    if (before_errBit != null)
                    {
                        // 前回のエラービットと比較
                        compareRet = compareErrBit(errBit);
                    }
                    // 前回のエラービットに値が設定されていなかった場合(最初のエラーコード取得時)
                    else
                    {
                        // 最初から(起動時から)エラーが起きていた場合は表示する。
                        // errBitの要素数分(8要素)ループする
                        for (int elem = 0; elem < errBit.Length; elem++)
                        {
                            if (errBit[elem] > 0)
                            {
                                firstErr = true;
                            }
                        }
                        // 「原点サーチ中」、「ロット満了」、「設定本数満了」、またはIsConnectが通信異常かつfalseだった場合
                        if ((machineStatus & BIT_SEEK_ORIGIN) != 0 || ((machineStatus & BIT_LOT_COMPLETE) != 0) ||
                            ((machineStatus & BIT_SET_COMPLETE) != 0) || (isConnectStatus == false))
                        {
                            firstErr = true;
                        }
                    }

                    // 前回のエラー、前回のmachineStatusと比べて、変化なしの場合で
                    if (compareRet == true && before_MachineStatus == machineStatus)
                    {


                        // 30秒経過フラグがfalse、isConnectが前回と変化がなし、起動時エラーではない場合、処理なし
                        if (dispFlg == false && before_IsConnectStatus == isConnectStatus && firstErr == false)
                        {
#if ERROR_IMAGE
                            // エラーイメージを更新する
                            updatePicture();
#endif
                            return;
                        }
                    }

                    // 前回IsConnectの更新
                    before_IsConnectStatus = isConnectStatus;

                    // 前回エラービットの更新
                    before_errBit = errBit;

                    // 前回machineStatusの更新
                    before_MachineStatus = machineStatus;

#if ERROR_IMAGE
                    // エラー画面用配列をクリアする
                    Array.Resize(ref errPictureList, 0);
#endif
                    // 描画処理を最後にまとめて行うため、描画を一時停止
                    listError.BeginUpdate();
                    listInformation.BeginUpdate();

                    // listbox内をクリアする
                    listError.Items.Clear();
                    listInformation.Items.Clear();

                    // 「原点サーチ中」だった場合
                    if ((machineStatus & BIT_SEEK_ORIGIN) != 0)
                    {
                        // ERR_MSG500を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG500);
                        infoFlg = true;
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG500, message);
                    }

                    // 「ロット満了」だった場合
                    if ((machineStatus & BIT_LOT_COMPLETE) != 0)
                    {
#if FOR_JN03SDWP
                        // ERR_MSG025を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG025);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG025, message);
#elif FOR_JN03SDGP
                        // ERR_MSG025を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG025);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG025, message);
#elif FOR_JN03SDC
                        // ERR_MSG501を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG501);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG501, message);
#else
                        // ERR_MSG114を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG114);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG114, message);
#endif
                        infoFlg = true;
                    }

                    // 「設定本数満了」だった場合
                    if ((machineStatus & BIT_SET_COMPLETE) != 0)
                    {
#if FOR_JN03SDWP
                        // ERR_MSG024を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG024);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG024, message);
#elif FOR_JN03SDGP
                        // ERR_MSG024を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG024);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG024, message);
#elif FOR_JN03SDC
                        // ERR_MSG502を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG502);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG502, message);
#else
                        // ERR_MSG113を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG113);
                        addListItem(SystemConstants.ERR_MSG_INFORMATION, SystemConstants.ERR_MSG113, message);
#endif
                        infoFlg = true;
                    }

                    // IsConnectが通信異常でfalseになった時
                    if (isConnectStatus == false)
                    {
                        // ERR_MSG516を取得したものとして扱う
                        message = Utility.GetErrorMessage(SystemConstants.ERR_MSG516);
                        errFlg = true;
                        addListItem(SystemConstants.ERR_MSG_ERROR, SystemConstants.ERR_MSG516, message);

                        // フォームクローズから30秒経過後だった場合
                        if (isConnect == false)
                        {
                            // エラーフラグは立てない(再表示させないため)
                            errFlg = false;
                        }

                    }

                    // errBitの要素数分(8要素)ループする
                    for (int elem = 0; elem < errBit.Length; elem++)
                    {
                        // 1アドレス分(16bit)ループする
                        for (int bit = 0; bit < 16; bit++)
                        {
                            // エラービットが立っていた場合
                            if ((errBit[elem] & (1 << bit)) != 0)
                            {
                                // 立っているエラービットの定数値を算出する
                                messageID = (elem * 16) + bit;

                                // エラーコードの区分を取得する 
                                errCode = Program.DataController.GetErrorType(messageID);

                                // 対応するメッセージをGetMessageStringで取得
                                message = Utility.GetErrorMessage(messageID);

                                // 「情報」区分だった場合
                                if (errCode == SystemConstants.ERR_MSG_INFORMATION)
                                {
                                    infoFlg = true;
                                }
                                // 「エラー」区分だった場合
                                else
                                {
                                    errFlg = true;
                                }

                                // リストにメッセージを追加する
                                addListItem(errCode, messageID, message);
                            }
                        }
                    }


                    // 取得したエラーコードが「情報」のみの場合
                    if (errFlg == false && infoFlg == true)
                    {
                        // Error欄をVisible=falseに設定する
                        panelInformation.Visible = true;
                        panelError.Visible = false;

                        // 座標を設定
                        panelInformation.Location = new Point(3, 3);
                        panelInformation.Size = new Size(620, 573);
                    }
                    // 取得したエラーコードが「エラー」のみの場合
                    else if (infoFlg == false && errFlg == true)
                    {
                        // Information欄をVisible=falseに設定する
                        panelInformation.Visible = false;
                        panelError.Visible = true;

                        // 座標を設定
                        panelError.Location = new Point(3, 3);
                        panelError.Size = new Size(620, 573);
                    }
                    // 取得したエラーコードが「情報」、「エラー」両方
                    else
                    {
                        panelInformation.Visible = true;
                        panelError.Visible = true;

                        // 座標を設定
                        panelInformation.Location = new Point(3, 3);
                        panelInformation.Size = new Size(620, 228);
                        panelError.Location = new Point(3, 231);
                        panelError.Size = new Size(620, 345);
                    }

                    // エラーがERR_MSG516のみの場合(IsConnectがfalseだった場合)
                    if (isConnect == false && infoFlg == false && errFlg == false)
                    {
                        // 再表示を行わない
                        return;
                    }
                    // 情報・エラーメッセージ画面を表示する
                    this.Visible = true;

                }
                // 「エラー発生中」でない場合かつ、IsConnectがtrueの場合、
                else if ((machineStatus & BIT_ERROR) == 0 && isConnectStatus == true)
                {

                    // 前回IsConnectの更新
                    before_IsConnectStatus = isConnectStatus;

                    // 前回エラービットの更新
                    before_errBit = errBit;

                    // 前回machineStatusの更新
                    before_MachineStatus = machineStatus;

                    // フォームを閉じる
                    this.Visible = false;
                }
            }
            finally
            {
                // 描画処理を最後にまとめて行う
                listError.EndUpdate();
                listInformation.EndUpdate();
            }

#if ERROR_IMAGE
            // エラーイメージを更新する
            updatePicture();
#endif
        }

        // 前回エラービットとの比較
        private bool compareErrBit(int[] ErrBit)
        {
            // 前回エラービットとの比較
            for (int i = 0; i < ErrBit.Length; i++)
            {
                if (before_errBit[i] != ErrBit[i])
                {
                    return false;
                }
            }
            return true;
        }

        private void addListItem(int ErrCode, int MessageID, string Message)
        {
            string message = "";

            // ERR_MSG127より上だった場合(ERR_MSG500以上)
            if (MessageID > 0x07F)
            {
                // オフセットを設定する
                MessageID = MessageID + 0x00F4;
            }

            // 「情報」区分だった場合
            if (ErrCode == SystemConstants.ERR_MSG_INFORMATION)
            {
                // 「Information」欄に項目を表示する
                // 取得したメッセージの前に「MSG」+「エラー番号」+「: 」を付けて表示
                this.panelInformation.Visible = true;
                message = "MSG" + MessageID + ": " + Message;
                this.listInformation.Items.Add(message);
            }
            else
            {
                // 「Error」欄に項目を表示する
                // 取得したメッセージの前に「MSG」+「エラー番号」+「: 」を付けて表示
                this.panelError.Visible = true;
                message = "MSG" + MessageID + ": " + Message;
                this.listError.Items.Add(message);

#if ERROR_IMAGE
                int errListIndex;
                // 配列サイズを変更し、エラー番号を追加する
                Array.Resize(ref errPictureList, errPictureList.Length + 1);
                errListIndex = errPictureList.Length - 1;
                errPictureList[errListIndex] = MessageID;
#endif
            }
        }

        private void errInfoMsgfrm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // フォームを閉じてからの時間を計測する
            reDispTimer.Restart();

            // フォームクローズフラグをtrueにする
            closeFlg = true;

            e.Cancel = true;
            Visible = false;
        }

        private void pbErrorData_MouseEnter(object sender, System.EventArgs e)
        {
#if ERROR_IMAGE
            Point screenPoint = pbErrorImage.PointToScreen(new Point(0, 0));

            errImageZoomForm.Location = screenPoint;
            errImageZoomForm.Show();
#endif
        }

        // 画像を更新する
        private void updatePicture()
        {
            // 拡大画面が表示中は更新を行わない
            if (errImageZoomForm.Visible) return;

            // エラーがない場合、イメージを消去する
            if (errPictureList.Length == 0)
            {
                pbErrorImage.Image = null;
                errImageZoomForm.UpdatePicture(null);
            }

            // エラーがあり、２秒経過後に画像を更新する
            if ((errPictureList.Length <= 0) || (pictureChangeTimer.ElapsedMilliseconds < SystemConstants.ERR_PICTURE_REFRESH_INTERVAL)) return;

            // エラー画像番号が範囲を超えている場合 (エラーが解除された場合)は、0から開始する
            if (errPictureIndex > errPictureList.Length - 1) errPictureIndex = 0;

            string imgFolder = getErrPictureFolder();
            int errMsgCode = errPictureList[errPictureIndex++];
            string imgFileName = string.Format(imgFolder + "\\err{0:X3}.jpg", errMsgCode);
            Image errImg;

            try
            {
                if (File.Exists(imgFileName))
                    errImg = Image.FromFile(imgFileName);
                else
                    errImg = Alchemist.Properties.Resources.NoErrImg;
            }
            catch
            {
                errImg = Alchemist.Properties.Resources.NoErrImg;
            }

            // 画像を更新する
            if (pbErrorImage.Image != errImg)
            {
                pbErrorImage.Image = errImg;
                pbErrorImage.Refresh();
            }
            errImageZoomForm.UpdatePicture(errImg);

            // 表示タイマをリセットする
            pictureChangeTimer.Restart();
        }


    }
}