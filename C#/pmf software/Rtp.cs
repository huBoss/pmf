using MSR.LST.Net.Rtp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PMF
{
    class Rtp
    {
        private static IPEndPoint ep;
        private RtpSession rtpSession;
        private RtpSender rtpSender;
        private PMF pmfForm;

        MemoryStream ms;

        #region WebCam API
        const short WM_CAP = 1024;
        const int WM_CAP_DRIVER_CONNECT = WM_CAP + 10;
        const int WM_CAP_DRIVER_DISCONNECT = WM_CAP + 11;
        const int WM_CAP_EDIT_COPY = WM_CAP + 30;
        const int WM_CAP_SET_PREVIEW = WM_CAP + 50;
        const int WM_CAP_SET_PREVIEWRATE = WM_CAP + 52;
        const int WM_CAP_SET_SCALE = WM_CAP + 53;
        const int WS_CHILD = 1073741824;
        const int WS_VISIBLE = 268435456;
        const short SWP_NOMOVE = 2;
        const short SWP_NOSIZE = 1;
        const short SWP_NOZORDER = 4;
        const short HWND_BOTTOM = 1;
        public int iDevice = 0;
        public int hHwnd;

        public int iWidth,iHeight;
        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SendMessageA")]
        static extern int SendMessage(int hwnd, int wMsg, int wParam, [MarshalAs(UnmanagedType.AsAny)] 
			object lParam);
        [System.Runtime.InteropServices.DllImport("user32", EntryPoint = "SetWindowPos")]
        static extern int SetWindowPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);
        [System.Runtime.InteropServices.DllImport("user32")]
        static extern bool DestroyWindow(int hndw);
        [System.Runtime.InteropServices.DllImport("avicap32.dll")]
        static extern int capCreateCaptureWindowA(string lpszWindowName, int dwStyle, int x, int y, int nWidth, short nHeight, int hWndParent, int nID);
        [System.Runtime.InteropServices.DllImport("avicap32.dll")]
        static extern bool capGetDriverDescriptionA(short wDriver, string lpszName, int cbName, string lpszVer, int cbVer);
        #endregion

        public int Width
        {
            get { return iWidth; }
            set { iWidth = value; }
        }
        public int Height
        {
            get { return iHeight; }
            set { iHeight = value; }
        }

        public int Device
        {
            get { return iDevice; }
            set { iDevice = value; }
        }

        public int Hwnd
        {
            get { return hHwnd; }
            set { hHwnd = value; }
        }

        public Rtp(string ipaddr, int port, int w, int h, PMF pmf)
        {
            ep = new IPEndPoint(IPAddress.Parse(ipaddr), port);
            iWidth = w;
            iHeight = h;
            pmfForm = pmf;
        }

        public Rtp(int w,int h,PMF pmf)
        {
            iWidth=w;
            iHeight=h;
            pmfForm=pmf;
        }
        public void OpenPreviewWindow()
        {

            // 
            //  Open Preview window in picturebox
            // 
            hHwnd = capCreateCaptureWindowA(iDevice.ToString(), (WS_VISIBLE | WS_CHILD), 0, 0, 640, 480,pmfForm.pictureBox_Receive.Handle.ToInt32(), 0);
            // 
            //  Connect to device
            // 
            if (SendMessage(hHwnd, WM_CAP_DRIVER_CONNECT, iDevice, 0) == 1)
            {
                // 
                // Set the preview scale
                // 
                SendMessage(hHwnd, WM_CAP_SET_SCALE, 1, 0);
                // 
                // Set the preview rate in milliseconds
                // 
                SendMessage(hHwnd, WM_CAP_SET_PREVIEWRATE, 66, 0);
                // 
                // Start previewing the ima ge from the camera
                // 
                SendMessage(hHwnd, WM_CAP_SET_PREVIEW, 1, 0);
                // 
                //  Resize window to fit in picturebox
                // 
                SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, iWidth, iHeight, (SWP_NOMOVE | SWP_NOZORDER));
            }
            else
            {
                // 
                //  Error connecting to device close window
                //  
                DestroyWindow(hHwnd);
            }
        }

        public void SetwndPos(int hwnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags)
        {
            SetWindowPos(hHwnd, HWND_BOTTOM, 0, 0, iWidth, iHeight, (SWP_NOMOVE | SWP_NOZORDER));
        }
        public void ClosePreviewWindow()
        {
            // 
            //  Disconnect from device
            // 
            SendMessage(hHwnd, WM_CAP_DRIVER_DISCONNECT, iDevice, 0);
            // 
            //  close window
            // 
            DestroyWindow(hHwnd);
        }


        public void HookRtpEvents()
        {
            // RtpEvents.RtpParticipantAdded += new RtpEvents.RtpParticipantAddedEventHandler(RtpParticipantAdded);
            // RtpEvents.RtpParticipantRemoved += new RtpEvents.RtpParticipantRemovedEventHandler(RtpParticipantRemoved);
            RtpEvents.RtpStreamAdded += new RtpEvents.RtpStreamAddedEventHandler(RtpStreamAdded);
            RtpEvents.RtpStreamRemoved += new RtpEvents.RtpStreamRemovedEventHandler(RtpStreamRemoved);
        }

        public void JoinRtpSession(string name)
        {
            rtpSession = new RtpSession(ep, new RtpParticipant(name, name), true, true);
            //rtpSender = rtpSession.CreateRtpSenderFec(name, PayloadType.Chat, null, 0, 200);
        }

       private void send_img()
       {
           try
           {

               ms = new MemoryStream();// Store it in Binary Array as Stream


               IDataObject data;
               Image bmap;

               //  Copy image to clipboard
               SendMessage(hHwnd, WM_CAP_EDIT_COPY, 0, 0);

               //  Get image from clipboard and convert it to a bitmap
               data = Clipboard.GetDataObject();

               if (data.GetDataPresent(typeof(System.Drawing.Bitmap)))
               {
                   bmap = ((Image)(data.GetData(typeof(System.Drawing.Bitmap))));
                   bmap.Save(ms, ImageFormat.Jpeg);
               }

               pmfForm.pictureBox_Receive.Image.Save(ms, ImageFormat.Jpeg);
               rtpSender.Send(ms.ToArray ());
           }
           catch (Exception) { //timer1.Enabled = false; }
           }
       }
       

        // CF5 Receive data from network
        private void RtpParticipantAdded(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            // ShowMessage(string.Format("{0} has joined", ea.RtpParticipant.Name));
        }

        private void RtpParticipantRemoved(object sender, RtpEvents.RtpParticipantEventArgs ea)
        {
            //  ShowMessage(string.Format("{0} has left", ea.RtpParticipant.Name));
        }

        private void RtpStreamAdded(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            ea.RtpStream.FrameReceived += new RtpStream.FrameReceivedEventHandler(FrameReceived);
        }

        private void RtpStreamRemoved(object sender, RtpEvents.RtpStreamEventArgs ea)
        {
            ea.RtpStream.FrameReceived -= new RtpStream.FrameReceivedEventHandler(FrameReceived);
        }

        private void FrameReceived(object sender, RtpStream.FrameReceivedEventArgs ea)
        {
            System.IO.MemoryStream ms = new MemoryStream(ea.Frame.Buffer);
            pmfForm.pictureBox_Receive.Image = Image.FromStream(ms);
        }



        public void Cleanup()
        {
            UnhookRtpEvents();
            //LeaveRtpSession();
        }

        public void UnhookRtpEvents()
        {
            //RtpEvents.RtpParticipantAdded -= new RtpEvents.RtpParticipantAddedEventHandler(RtpParticipantAdded);
            //RtpEvents.RtpParticipantRemoved -= new RtpEvents.RtpParticipantRemovedEventHandler(RtpParticipantRemoved);
            RtpEvents.RtpStreamAdded -= new RtpEvents.RtpStreamAddedEventHandler(RtpStreamAdded);
            RtpEvents.RtpStreamRemoved -= new RtpEvents.RtpStreamRemovedEventHandler(RtpStreamRemoved);
        }

        public void LeaveRtpSession()
        {
            if (rtpSession != null)
            {
                // Clean up all outstanding objects owned by the RtpSession
                rtpSession.Dispose();
                rtpSession = null;
                //  rtpSender = null;
            }
        }

    }
}
