using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;

namespace PMF
{
    public class SearchCable
    {

        public SearchCable(){
        

        }
        [DllImport("CableScannerDLL.dll", EntryPoint = "fnCableScanInit", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fnCableScanInit();
        [DllImport("CableScannerDLL.dll", EntryPoint = "fnTestPrint", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fnTestPrint();
        [DllImport("CableScannerDLL.dll", EntryPoint = "fnMPSInitX", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fnMPSInitX(int bulks, int sampleMode, double[] sig);

        [DllImport("CableScannerDLL.dll", EntryPoint = "fnFreqPtrMalloc", CallingConvention = CallingConvention.Cdecl)]
        public static extern double[] fnFreqPtrMalloc(ref int _cableScannerPoint, ref int _mpsPoint);

        [DllImport("CableScannerDLL.dll", EntryPoint = "fnGetDataPtr", CallingConvention = CallingConvention.Cdecl)]
        public static extern double[] fnGetDataPtr(int _mpsPoint);

        [DllImport("CableScannerDLL.dll", EntryPoint = "fnGetDataLength", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fnGetDataLength(int _mpsPoint);

        [DllImport("CableScannerDLL.dll", EntryPoint = "fnFreqPtrLen", CallingConvention = CallingConvention.Cdecl)]
        public static extern int fnFreqPtrLen(int _cableScannerPoint, int _mpsPoint);

        [DllImport("CableScannerDLL.dll", EntryPoint = "fnCableScan", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool fnCableScan(int _cableScannerPoint, int _mpsPoint, double[] map,
                                                 ref double y, ref double z, double[] freq, double[]volt,double[]param);

        [DllImport("CableScannerDLL.dll", EntryPoint = "fnCableScanCloseAll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void fnCableScanCloseAll(int _cableScannerPoint, int _mpsPoint);

        [DllImport("CableScannerDLL.dll", EntryPoint = "fnGetTimeLen", CallingConvention = CallingConvention.Cdecl)]
        public static extern double fnGetTimeLen(int _mpsPoint);

        [DllImport("CableScannerDLL.dll", EntryPoint = "fnGetData", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool fnGetData(int _mpsPoint);

        public int cableScannerPoint = 0;
        public int mpsPoint = 0;

        static int bulks = 10;
        static int sampleMode = 8;
        static double[] sig = new double[bulks * 8 * 1024];

        static double timelen=0;
       
        static int freqPtrLen = bulks * 8 * 1024;
        static double[] freq = new double[freqPtrLen];

        static double[] volt = new double[8];

        static double current = 100;
        static double ymin = -2, ymax = 2, zmin = -2, zmax = 0;
        static int ynum = 35, znum = 35;
        static double[] map = new double[ynum * znum];
        static double y = 0, z = 0;
        static bool value_get = false;
        static double []param={current,ymin,ymax,zmin,zmax,ynum,znum};

        static bool status;

        public bool Status
        {
            get { return status; }
            set { status = value; }
        }

        public int CableScannerPoint
        {
            get { return cableScannerPoint; }
            set { cableScannerPoint = value; }
        }

        public int MpsPoint
        {
            get { return mpsPoint; }
            set { mpsPoint = value; }
        }
        public double Current
        {
            get { return current; }
            set { current = value;}
        }

        public double Y
        {
            get { return y; }
            set { y = value;}
        }

        public double Z
        {
            get { return z; }
            set { z = value;}
        }
        public double TimeLen
        {
            get { return timelen; }
            set { timelen = value;}
        }

        public double[] Sig
        {
            get { return sig; }
            set { sig = value;}
        }

        public double[] Freq
        {
            get { return freq; }
            set { freq = value;}
        }

        public double[] Map
        {
            get { return map; }
            set { map = value;}
        }

        public double[] Volt
        {
            get { return volt; }
            set { volt = value; }
        }

        public bool Value_get
        {
            get { return value_get; }
            set { value_get = value; }
        }

     
        public  void SearchCables()
        {
           // fnTestPrint();

            
            cableScannerPoint = fnCableScanInit();
            
           
          //  Console.WriteLine("MPSInit...");
            mpsPoint = fnMPSInitX(bulks, sampleMode, sig);
          //  Console.WriteLine("MPSInit...ok");

            timelen = fnGetTimeLen(mpsPoint);
            status = fnGetData(mpsPoint);
          
            status=fnCableScan(cableScannerPoint, mpsPoint, map, ref y, ref z, freq, volt, param);
           
            value_get = true;
            
            
        }

        public void closeDevice()
        {
            fnCableScanCloseAll(cableScannerPoint, mpsPoint);
        }
    }
}
