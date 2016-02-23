/*This is used for test the function of CableScanner and MPS and FFTWFilter
*/
#include <Windows.h>
#include <iostream>
#include <fstream>
#include <time.h>

//#include "DataAcquire.h"

//#define _TEST_MSG_


#define _CABLESCANNER_DLL_TEST_
//#define _CABLESCANNER_CLASS_DLL_TEST_
#define _CABLESCANNER_FUNC_DLL_TEST_

#ifdef _CABLESCANNER_DLL_TEST_
//#include "..\Release\CabelScannerDLL.lib"
#pragma comment(lib, "..\\Release\\CableScannerDLL.lib")
//#pragma comment(lib, "lib\\CableScannerDLL.lib") // not used anymore unless it's updated
#include "..\CabelScannerDLL\CableScannerDLL.h"
#endif // _CABLESCANNER_DLL_TEST_

using namespace std;

//#undef _DLL_BUILD_TEST_

int SearchCable();

int init();

int startDataAcquire(int);

int main()
{
	///////////// Cable Search Test ///////////////
	////char filename[128];
	////* use mps get data and scan
	//for (size_t i = 0; i < 100; i++)
	//{
	//	SearchCable("data/test.txt", 10);
	//}/*/ test scan from spec data
	//CableScanner cs;
	//cs.setCoilsNumber(8);
	//cs.addCoils("coils_ascii.mat");
	//cs.test(); */

	/////////////// GPS Test //////////////////
	//unsigned port = 3;
	//double latitude;
	//char NS;
	//double longtitude;
	//char EW;

	//GPS *gps = fnInitGPS(port);
	//fnGPSStart(gps); // start
	////gps->Start();
	//Sleep(2000);
	//while (true) {
	//	if (fnGPSGetLocation(gps, &latitude, &NS, &longtitude, &EW))
	//		cout << latitude << NS << "," << longtitude << EW << endl;
	//	Sleep(1000);
	//}
	//fnGPSStop(gps); // stop

	///////////// data acquire ////////////////
	init();
	startDataAcquire(100);

	return 0;
}


int SearchCable(char *sigfile, char *freqfile, char *phafile)
{
#ifdef _CABLESCANNER_FUNC_DLL_TEST_
	CableScanner *cs;
	CableScanner **csptr = &cs;
	cs = (CableScanner *)fnCableScanInit();
	MPS *mps;
	MPS **mpsptr = &mps;
	int bulks = 10;
	int sampleMode = 8; //k
#ifdef _TEST_MSG_
	cout << "MPSInit..." << endl;
#endif
	mps = (MPS *)fnMPSInit(bulks, sampleMode);
#ifdef _TEST_MSG_
	cout << "MPSInit...ok" << endl;
#endif

#ifdef _TEST_MSG_
	cout << "Ptr Init...1" << endl;
#endif
	double *freq = fnFreqPtrMalloc(cs, mps);
#ifdef _TEST_MSG_
	cout << "Ptr Init...2" << endl;
#endif
	double *sig = fnGetDataPtr(mps);
#ifdef _TEST_MSG_
	cout << "Ptr Init...3" << endl;
#endif
	int dataLen = fnGetDataLength(mps);
#ifdef _TEST_MSG_
	cout << "Ptr Init...ok" << endl;
#endif

	double current = 100;
	double ymin = -2, ymax = 2, zmin = -2, zmax = 0;
	int ynum = 100, znum = 100;
	double *map = new double[ynum*znum];
	double y, z;
	ofstream outfile;
	double *e0 = new double[mps->channels];
	/*for (int i = 0; i < loops; i++)
	{*/
		cout << "------------------------" << endl;
#ifdef _TEST_MSG_
		cout << "CableScan test..." << endl;
#endif
		fnGetData(mps);
		double params[] = {current, ymin, ymax, zmin, zmax, ynum, znum };
		fnCableScan(cs, mps, map, &y, &z, freq, e0, params);
#ifdef _TEST_MSG_
		cout << "CableScan test...ok" << endl;
#endif

		/* output data to file */
		/*map*/
		cout << "trying output data to file" << endl;
		outfile.open("data/map.txt");
#ifdef _TEST_MSG_
		cout << "opening data/map.txt" << endl;
#endif
		if (outfile.is_open())
		{
			double *ptr = map;
			for (int i = 0; i < ynum; i++, ptr += znum)
			{
				for (int j = 0; j < znum; j++)
				{
					outfile << ptr[j] << "\t";
				}
				outfile << endl;
			}
		}
		else {
			cout << "file 'data/map.txt' can not be open. exit..." << endl;
			exit(-1);
		}
		outfile.close();
		/*signal*/
		if (sigfile == NULL)
			sigfile = "data/sig.txt";
		outfile.open(sigfile);
#ifdef _TEST_MSG_
		cout << "opening data/sig.txt" << endl;
#endif
		if (outfile.is_open())
		{
			double *ptr = sig;
			int bufLen = dataLen / 8;
			for (int i = 0; i < 8; i++, ptr += bufLen) // HERE is a bug, replace sig with ptr
			{
				for (int i = 0; i < bufLen; i++)
				{
					outfile << ptr[i] << "\t";
				}
				outfile << endl;
			}
		}
		else {
			cout << "file '" << sigfile << "' can not be open. exit..." << endl;
			exit(-1);
		}
		outfile.close();
		/*freq*/
		if (freqfile==NULL)
			freqfile = "data/freq.txt";
		outfile.open(freqfile);
#ifdef _TEST_MSG_
		cout << "opening data/freq.txt" << endl;
#endif
		if (outfile.is_open())
		{
			double *ptr = freq;
			int bufLen = dataLen / 8;
			for (int i = 0; i < 8; i++, sig += bufLen)
			{
				for (int i = 0; i < bufLen; i++)
				{
					outfile << ptr[i] << "\t";
				}
				outfile << endl;
			}
		}
		else {
			cout << "file '" << freqfile << "' can not be open. exit..." << endl;
			exit(-1);
		}
		outfile.close();
	//}

	fnFreqPtrFree(freq);
	fnCableScanCloseAll(cs, mps);

#endif //_CABLESCANNER_FUNC_DLL_TEST_

#ifdef _CABLESCANNER_CLASS_DLL_TEST_
	CableScanner cs;
	cs.setCoilsNumber(8);
	if (cs.addCoils("coils_ascii.mat"))
		return -1;

	// CableScanner test
	if (cs.test())
	{
		cout << "Cable Scanner test success!" << endl;
	}
#endif // _CABLESCANNER_DLL_TEST_

#ifdef _DLL_BUILD_TEST_
	clock_t t1 = clock();
	fnCableScanGeneralTest();
	cout << "fnCableScanGeneralTest	used time: " << (clock() - t1) << " ms" << endl;
#endif // _DLL_BUILD_TEST_
	return 0;

}

int init() {
	return 0;
}

int startDataAcquire(int loop) {
	for (int i = 0; i < loop; i++)
	{
		cout << "**** " << i << " ****" << endl;
	}
	return 0;
}