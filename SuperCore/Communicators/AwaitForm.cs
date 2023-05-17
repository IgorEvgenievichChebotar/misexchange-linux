using System;
using System.IO;
using System.Net;
//using System.Windows.Forms;

namespace ru.novolabs.SuperCore
{
    /*public partial class AwaitForm : Form
    {
        private int awaitFormDelay = 2000; // Задержка отображения формы ожидания
        private IAsyncResult asyncResult = null; // Объект, описывающий состояние асинхронной операции
        private byte[] responseBytes = null;

        public AwaitForm()
        {
            InitializeComponent();
            DialogResult = System.Windows.Forms.DialogResult.None;
        }

        public byte[] GetResponse(byte[] requestBytes, WebRequest request, bool needShow)
        {
            asyncResult = request.BeginGetRequestStream(null, null);
            var requestStream = request.EndGetRequestStream(asyncResult);
            requestStream.Write(requestBytes, 0, requestBytes.Length);
            requestStream.Close();
            asyncResult = request.BeginGetResponse(GetResponseCallback, request);
            // Блорикуем поток либо до окончания выполнения запроса к серверу, либо до истечения awaitFormDelay миллисекунд
            bool requestOperationCompleted = asyncResult.AsyncWaitHandle.WaitOne(awaitFormDelay);
           
            if (requestOperationCompleted)
                GetResponseBytes(asyncResult);
            else
            {
                this.ShowDialog();
                if (this.DialogResult == DialogResult.Cancel)
                    throw new NlsOperationCanceledException();
            }
            return this.responseBytes;
        }

        private void GetResponseBytes(IAsyncResult asyncResult)
        {
            WebRequest request = (WebRequest)asyncResult.AsyncState;
            WebResponse response = request.EndGetResponse(asyncResult);
            using (response)
            {
                using (var responceStream = response.GetResponseStream())
                {
                    using (var ms = new MemoryStream())
                    {
                        responceStream.CopyTo(ms);
                        this.responseBytes = ms.ToArray();
                    }
                }
            }
        }

        private void GetResponseCallback(IAsyncResult asyncResult)
        {
            if (this.Visible)
            {
                GetResponseBytes(this.asyncResult);
                this.DialogResult = DialogResult.OK;
            }
        }

        private Form _backForm = null;
        public Form BackForm
        {
            get
            {
                return _backForm;
            }
            set
            {
                if (value != null)
                {
                    _backForm = value;
                    int x = _backForm.Location.X + (_backForm.Width - this.Width) / 2;
                    int y = _backForm.Location.Y + (_backForm.Height - this.Height) / 2;
                    this.Location = new System.Drawing.Point(x, y);
                }
            }
        }

        private void AwaitForm_Shown(object sender, EventArgs e)
        {
            //if (this.asyncResult.IsCompleted)
            //{
            //    GetResponseBytes(this.asyncResult);
            //    this.DialogResult = DialogResult.OK;
            //}
        }
    }*/
}