using MisExchangeAdapters.Parser.HL7;
using ru.novolabs.ExchangeDTOs;
using ru.novolabs.HL7;
using ru.novolabs.MisExchange.ExchangeHelpers.HL7.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ru.novolabs.MisExchange.ExchangeHelpers.HL7
{
    public class TCPIPClientServerTransport
    {
        public String LocalHost { get; set; }
        public String RemoteHost { get; set; }
        public Int32 LocalPort { get; set; }
        public Int32 RemotePort { get; set; }
        public Encoding Encoding { get; set; }
        private TcpClient Client { get; set; }
        private TcpListener Server { get; set; }

        private TriggerList Triggers { get; set; }

        protected const byte ch_STX = 2; // start of text
        protected const byte ch_ETX = 3; // end of text
        protected const byte ch_EOT = 4; // end of transmission
        protected const byte ch_ENQ = 5; // enquiry
        protected const byte ch_ACK = 6; // acknowledge
        protected const byte ch_LF = 10; // line feed, new line
        protected const byte ch_CR = 13; // carriage return
        protected const byte ch_NAK = 21; // negative acknowledge
        protected const byte ch_ETB = 23; // end of trans. block

        private byte[] ch_StartBlock = new byte[] { 11 };
        private byte[] ch_EndBlock = new byte[] { 28 };
        private byte[] ch_Frame = new byte[] { 13 };
        private byte[] ch_Enq = new byte[] { 5 };

        internal const string DefaultASTMFrameTemplate = "([{0}].)(.*)({1}[{2}].*{1}{3})";
        public Queue<string> _buffer = new Queue<string>();

        private Int32 _timeOut;

        private const byte max_frame_length = 240; // 240 data characters + 7 overhead characters
        TcpClient client;
        public byte[] GetBytes()
        {
            if (client == null || client.Connected == false)
                client = Server.AcceptTcpClient();
            //Invoke(new SetAvailableDelegate(SetAvailable), bSend, true);
            NetworkStream stream = client.GetStream();

            // Получение массива байт из сокета и интерпретация их как строки в кодировке UTF8
            String requestString = String.Empty;
            var buffer = new byte[1024];
            Int32 bytesRead = 0;
            List<byte> data = new List<byte>();
            if (stream.DataAvailable)
            {
                do
                {

                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 1024)
                        data.AddRange(buffer);
                    else
                        data.AddRange(buffer.Take(bytesRead));

                    //requestString += Encoding.UTF8.GetString(buffer, 0, bytesRead);
                }
                while (bytesRead == buffer.Length);
            }
            //if (Client != null  && Client.Connected)
            //{
            //    NetworkStream NWS = Client.GetStream();
            //    if (NWS.DataAvailable)
            //    {
            //        byte[] buffer = new byte[NWS.Length];
            //        NWS.Read(buffer, 0, (int)NWS.Length);
            //        return buffer;
            //    }
            //}
            return data.ToArray();
        }

        public void SendBytes(byte[] bytes)
        {
            Client = client;
            if (Client == null || !Client.Connected)
            {
                Client = new TcpClient(RemoteHost, RemotePort);
                if (!Client.Connected)
                    Client.Connect(RemoteHost, RemotePort);
            }
            if (Client != null && Client.Connected && bytes != null && bytes.Length != 0)
            {
                Client.Client.Send(bytes);
            }
        }



        public void Init(HelperSettings settings)
        {
            Triggers = new TriggerList();
            
            //Триггер на фрейм (закрываем по тегу [CR])
            //Triggers.Add(new Trigger(TriggerTypes.DataFrame, null, 0, ch_Frame ));
            //Триггер на нахождение блока (заключен между тегами [VT])
            Triggers.Add(new Trigger(TriggerTypes.EndOfTransmission, null, 0, new byte[] { ch_EndBlock[0], ch_CR }));
            //Триггер на Enq
            Triggers.Add(new Trigger(TriggerTypes.Enquiry, null, 0, ch_Enq));
            //Команда не подтверждена
            Triggers.Add(new Trigger(TriggerTypes.NotAcknowledge, null, 0, new byte[] { ch_NAK }));
            //// Триггер, срабатывающий при приходе символа ENQ ("запрос на соединение")
            //Triggers.Add(new Trigger(TriggerTypes.Enquiry, ".*" + RegexSequence(ch_EOT), 0, null));
            //// Триггер, срабатывающий при приходе фрагмента сообщения
            //Triggers.Add(new Trigger(TriggerTypes.DataFrame, String.Format(DefaultASTMFrameTemplate, RegexSequence(ch_STX), RegexSequence(ch_CR), RegexSequence(ch_ETX), RegexSequence(ch_LF)), 0, null));
            //// Триггер, срабатывающий при приходе символа EOT ("конец передачи")
            //Triggers.Add(new Trigger(TriggerTypes.EndOfTransmission, null, 0, new byte[] { ch_EOT }));
            //// Триггер, срабатывающий при приходе символа ACK ("подтверждаю")
            //Triggers.Add(new Trigger(TriggerTypes.Acknowledge, null, 0, new byte[] { ch_ACK }));
            //// Триггер, срабатывающий при приходе символа NAK ("не подтверждаю")
            //Triggers.Add(new Trigger(TriggerTypes.NotAcknowledge, null, 0, new byte[] { ch_NAK }));
            
            LocalHost = settings.LocalTCPHost;
            RemoteHost = settings.RemoteTCPHost;
            LocalPort = settings.LocalTCPPortNumber;
            RemotePort = settings.RemoteTCPPortNumber;
            _timeOut = settings.TimeOut;
            Encoding = Encoding.UTF8;
            Connect();
        }

        Thread ListenThread;
        private void Connect()
        {
            

            IPAddress localAddr = IPAddress.Parse(LocalHost);
            //IPAddress localAddr = Dns.Resolve("localhost").AddressList[0];
            Server = new TcpListener(localAddr, LocalPort);
            Server.Start();
            DataReceivedEv += DataReceived;
            ListenThread = new Thread(new ThreadStart(Listen));
            ListenThread.Start();
        }

        protected string RegexSequence(byte ch)
        {
            string chHexView = ch.ToString("X2");
            return @"\x" + chHexView;
        }
        
        public void Listen()
        {
            
            //Invoke(new SetAvailableDelegate(SetAvailable), bStartListen, false);

            //while (true)
            //{
            //    try
            //    {
                    

            //        //Invoke(new UpdateDisplayDelegate(UpdateDisplay), new object[] { requestString });
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message);
            //    }
            //}

            while (true)
                Read();
        }

        List<Byte> Buffer = new List<byte>();
        public void Read()
        {
            byte[] gotit = GetBytes();
            if (gotit != null)
            {
                Buffer.AddRange(gotit);
                TriggerResult match = Triggers.CheckString(Buffer.ToArray(), this.Encoding);
                if (match != null)
                {
                    Byte[] cut = Buffer.GetRange(0, match.Index).ToArray();
                    Buffer.RemoveRange(0, match.Index);
                    if (DataReceivedEv != null)
                    {
                        try
                        {
                            DataReceivedEv(this, new MessageReceivedEventArgs(cut, match.Id));
                        }
                        catch (Exception ex)
                        {
                            Log.WriteError(ex.ToString());
                        }
                    }
                }
            }
        }

        public delegate void MessageReceivedEventHandler(object sender, MessageReceivedEventArgs e);
        public event MessageReceivedEventHandler DataReceivedEv;

        protected void DataReceived(object sender, MessageReceivedEventArgs e)
        {
            //Debug.WriteLine("DataReceived. Thread.CurrentThread.ManagedThreadId = " + Thread.CurrentThread.ManagedThreadId.ToString());
            byte[] bytes = e.Message;
            switch (e.TriggerId)
            {
                case TriggerTypes.Enquiry:
                    //SendAck();
                    break;
                case TriggerTypes.DataFrame:
                    //string messageFrame = NormalizeString(Encoding.GetString(e.Message));
                    //string messageFrame = Encoding.GetString(e.Message);
                    //_buffer.Enqueue(messageFrame);
                    //SendAck();
                    break;
                case TriggerTypes.EndOfTransmission:
                    try
                    {
                        List<String> messageFrames = Encoding.GetString(e.Message).Split(new String[] {"\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                        foreach (String messageFrame in messageFrames)
                            _buffer.Enqueue(messageFrame);

                        if (_buffer.Count > 0)
                        {
                            ProcessMessage();
                            SendAck();
                        }
                        else
                            SendNak("CANT READ: MESSAGE IS EMPTY");
                    }
                    catch (Exception ex)
                    {
                        SendNak(ex.Message.Substring(0, 80));
                        throw;
                    }
                    finally
                    {
                        _buffer.Clear();
                    }
                    break;
                case TriggerTypes.Acknowledge:
                    AcknowledgementReceived();
                    break;
                case TriggerTypes.NotAcknowledge:
                    throw new Exception("Frame not acknowledged");
                default:
                    break;
            }
        }

        // Объект синхронизации потоков чтения/записи (сигнал, говорящий о том, что принимающая сторона в лице анализатора подтверждает получение очередного фрагмента данных)
        private ManualResetEventSlim _acknowledgementReceivedEvent = new ManualResetEventSlim(false);
        private void AcknowledgementReceived()
        {
            _acknowledgementReceivedEvent.Set();
        }

        

        public delegate void HL7QueryEventHandler(object sender, HL7QueryEventArgs e);

        public event HL7QueryEventHandler HL7QueryEvent;

        protected void RaiseOnQueryEvent(Request request)
        {
            if (HL7QueryEvent != null)
                HL7QueryEvent(this, new HL7QueryEventArgs() { Request = request });
        }
        //public event OnWorkListQueryEventHandler OnWorkListQueryEvent;

        //protected LISDriverWorklist RaiseOnWorkListQueryEvent(LISDriverWorklist worklist)
        //{
        //    if (OnWorkListQueryEvent != null)
        //        return OnWorkListQueryEvent(this, new WorkListEventArgs() { WorkList = worklist });
        //    else
        //        return null;
        //}

        private void ProcessMessage()
        {
            string astmStrMessage = String.Join("\r\n", _buffer);
            Log.WriteText("ASTM message received: \r\n\r\n" + astmStrMessage + "\r\n");
            // TODO: Write this method
            Request request = new Request();
            request = HL7MessageParser.BuildDTORequest(astmStrMessage);
            RaiseOnQueryEvent(request);
            
            //LISDriverWorklist worklist, queryReply;
            //AstmMessageType messageType = _adapter.ParseMessage(astmStrMessage, out worklist);
            //switch (messageType)
            //{
            //    case AstmMessageType.Results:
            //        RaiseOnResultsEvent(worklist);
            //        break;
            //    case AstmMessageType.Query:
            //        queryReply = RaiseOnWorkListQueryEvent(worklist);
            //        Send(queryReply);
            //        break;
            //    default:
            //        break;
            //}
        }

        protected void Send(string data)
        {
            Queue<byte[]> frames = GetMessageFrames(data);
            new Thread(SendFrames).Start(frames);
        }


        private void SendFrames(object obj)
        {
            Queue<byte[]> frames = (Queue<byte[]>)obj;

            _acknowledgementReceivedEvent.Reset();
            SendBytes(new byte[] { ch_ENQ });
            //_timer
            bool ackReceived = _acknowledgementReceivedEvent.Wait(_timeOut);
            if (!ackReceived)
            {
                LogAckTimedOut();
                return;
            }
            while (frames.Count > 0)
            {
                byte[] frame = frames.Dequeue();
                _acknowledgementReceivedEvent.Reset();
                SendBytes(frame);
                ackReceived = _acknowledgementReceivedEvent.Wait(_timeOut);
                if (!ackReceived)
                {
                    LogAckTimedOut();
                    return;
                }
            }
            SendBytes(new byte[] { ch_EOT });
        }

        private void LogAckTimedOut()
        {
            Log.WriteError("ACK waiting timed out. Timeout value = [{0}]", _timeOut);
            Log.WriteText("");
        }

        private Queue<byte[]> GetMessageFrames(string data)
        {
            if (String.IsNullOrEmpty(data))
                throw new ArgumentException("Incorrect message");

            Queue<byte[]> messageFrames = new Queue<byte[]>();
            Queue<string> recordQueue = new Queue<string>(data.Split(new string[] { "\r\n" }, StringSplitOptions.None));
            int sequenceNumber = 0;
            while (recordQueue.Count > 0)
            {
                sequenceNumber = (sequenceNumber < 7) ? ++sequenceNumber : 0;
                string record = sequenceNumber.ToString() + recordQueue.Dequeue();
                GetRecordFrames(record).ForEach(array => messageFrames.Enqueue(array));
            }
            return messageFrames;
        }


        private List<byte[]> GetRecordFrames(string record)
        {
            int frameCount = record.Length / max_frame_length;
            frameCount = (record.Length % max_frame_length) > 0 ? ++frameCount : frameCount;
            List<byte[]> result = new List<byte[]>(capacity: frameCount);

            List<byte> frame = null;
            if (frameCount > 1)
            {
                // Формируем промежуточные фреймы (в конце блока символ [ETB])
                for (int i = 0; i < frameCount - 1; i++)
                {
                    frame = this.Encoding.GetBytes(record.Take(max_frame_length).ToArray()).ToList();
                    frame.AddRange(new byte[] { ch_CR, ch_ETB });
                    frame.AddRange(this.Encoding.GetBytes(CalcCheckSum(frame)));
                    frame.AddRange(new byte[] { ch_CR, ch_LF });
                    frame.Insert(0, ch_STX);
                    result.Add(frame.ToArray());
                    record = new String(record.Skip(max_frame_length).ToArray());
                }
            }
            // Формируем последний/единственный фрейм (в конце блока символ [ETX])
            frame = this.Encoding.GetBytes(record.Take(max_frame_length).ToArray()).ToList();
            frame.AddRange(new byte[] { ch_CR, ch_ETX });
            frame.AddRange(this.Encoding.GetBytes(CalcCheckSum(frame)));
            frame.AddRange(new byte[] { ch_CR, ch_LF });
            frame.Insert(0, ch_STX);
            result.Add(frame.ToArray());

            return result;
        }

        private void SendAck()
        {
            
            List<String> data = _buffer.ToList();
            String Ack = HL7MessageParser.BuildAck(data);
            SendBytes(Encoding.GetBytes(Ack));
        }

        private void SendNak()
        {
            List<String> data = _buffer.ToList();
            String Ack = HL7MessageParser.BuildAck(data, "ERROR");
            SendBytes(Encoding.GetBytes(Ack));
        }

        private void SendNak(String Error)
        {
            List<String> data = _buffer.ToList();
            String Ack = HL7MessageParser.BuildAck(data, Error);
            SendBytes(Encoding.GetBytes(Ack));
        }

        private string NormalizeString(string str)
        {
            Regex regex = new Regex(".*" + RegexSequence(ch_EOT), RegexOptions.Singleline);
            string result = regex.Matches(str)[0].Groups[2].ToString();
            return result;
        }

        public Int32 CheckSequences(byte[] Sequence, byte[] buffer)
        {
            Int32 seqIndex = -1;
            if (Sequence != null && Sequence.Length != 0)
            {
                for (Int32 i = 0; i < Sequence.Length && i < buffer.Length; i++)
                {
                    Boolean found = true;
                    for (Int32 j = 0; j < Sequence.Length; j++)
                    {
                        if (Sequence[j] != buffer[i + j])
                        {
                            found = false;
                            break;
                        }
                    }

                    if (found)
                    {
                        seqIndex = i;
                        break;
                    }
                }
            }
            return seqIndex;
        }


        /// <summary>
        /// Вычисляет контрольную сумму массива байт
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private string CalcCheckSum(List<byte> bytes)
        {
            int sum = 0;
            foreach (byte b in bytes)
                sum += b;
            sum %= 256;
            return sum.ToString("X2");
        }

        private void Disconnect()
        {
            if (Client != null && Client.Connected)
                Client.Close();
            if (Server != null)
                Server.Stop();
        }

        ~TCPIPClientServerTransport()
        {
            Disconnect();
        }

        public void Stop()
        {
            Disconnect();
        }
    }

    public class MessageReceivedEventArgs : EventArgs
    {
        public MessageReceivedEventArgs(byte[] msg, Int32 triggerId)
        {
            Message = msg; TriggerId = triggerId;
        }
        public byte[] Message { get; set; }
        public Int32 TriggerId { get; set; }
    }


    public enum AstmMessageType
    {
        Unknown,
        Results,
        Query,
    }
}
