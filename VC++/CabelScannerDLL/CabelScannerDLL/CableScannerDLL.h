// ���� ifdef ���Ǵ���ʹ�� DLL �������򵥵�
// ��ı�׼�������� DLL �е������ļ��������������϶���� CABELSCANNERDLL_EXPORTS
// ���ű���ġ���ʹ�ô� DLL ��
// �κ�������Ŀ�ϲ�Ӧ����˷��š�������Դ�ļ��а������ļ����κ�������Ŀ���Ὣ
// CABELSCANNERDLL_API ������Ϊ�Ǵ� DLL ����ģ����� DLL ���ô˺궨���
// ������Ϊ�Ǳ������ġ�
#ifdef CABLESCANNERDLL_EXPORTS
#define CABLESCANNERDLL_API extern "C" __declspec(dllexport)
#define DIGITALCOMPASS_API __declspec(dllexport)
#else
#define CABLESCANNERDLL_API extern "C" __declspec(dllimport)
#define DIGITALCOMPASS_API __declspec(dllimport)
#endif

//////////////////////////////////
/*fnCableScanGeneralTest*/
//#define _DLL_BUILD_TEST_


#ifndef __EIGEN__
#define __EIGEN__
#include "Eigen\Dense"

using namespace Eigen;
#endif //~__EIGEN__
#include "Coil.h"
#include "MPS.h"

#define AMP_SCALE 1000

class /*CABLESCANNERDLL_API*/ CableScanner
{
private:
	char *WelcomeMsg = "** Cabel Scanner **\n"
		"scan the plan area for the most possible cable position\n"
		;
	VectorXd emf(Vector3d, Vector3d);
	double k; // coeficient
	Coil  *coils;
public:
	int    n; // the number of coils
	double maxprob;
	double minprob;
	double maxy, maxz;

public:
	CableScanner();
	~CableScanner();
public:
	int addCoil(Coil, int);
	int addCoils(char *);
	int setCoilsNumber(int);

	MatrixXd scan(Vector3d, Vector3d, VectorXd, double, double, double, double, int, int);

	bool test();
};

CABLESCANNERDLL_API CableScanner   *fnCableScanInit();//CableScanner *&cableScanner); // init class pointers
CABLESCANNERDLL_API MPS   *fnMPSInit(int bulks, int sampleRateSelect); // init class pointers
CABLESCANNERDLL_API MPS   *fnMPSInitX(int bulks, int sampleRateSelect, double *data); // init class pointers
CABLESCANNERDLL_API double *fnFreqPtrMalloc(CableScanner *cableScanner, MPS *mps); // free with fnFreqPtrFree
CABLESCANNERDLL_API int		fnFreqPtrLen(CableScanner *cableScanner, MPS *mps);
CABLESCANNERDLL_API void    fnFreqPtrFree(double *freq); // if you used fnFreqPtrMalloc, you need to free it.
CABLESCANNERDLL_API double *fnGetDataPtr(MPS *mps);
//CABLESCANNERDLL_API double *fnMPSGetDataPtr(MPS *mps);
CABLESCANNERDLL_API int     fnGetDataLength(MPS *); // return the length of data
CABLESCANNERDLL_API bool	fnGetData(MPS *); // Read Data to MPS->data memory
/*You need to call fnGetData before call this.
@Input:
- cableScanner, the Pointers of CableScanner
- mps,          the Pointers of MPS
- Map,          the [ynum]x[znum] map data will be return through the memory of this pointer.
should be pre-malloc memory.
- yPtr zPtr,    will be returned including the estimated y, z distance.
should be pre-malloc memory.
- freq,         the memory for returning frequency domain data.
- current,      the value of current
- [ymin, ymax]x[zmin, zmax], define the area to be scanned.
- ynum znum,    the sample num of area
@Return:
if existing cable, return true
*/
CABLESCANNERDLL_API bool fnCableScan(
	CableScanner *cableScanner, MPS *mps,
	double *Map, double *yPtr, double *zPtr, double *freq, double *measuedVolt,
	double *params);
	//double current, double ymin, double ymax, double zmin, double zmax, int ynum, int znum); // Return data throught Map yPtr and zPtr
CABLESCANNERDLL_API void    fnCableScanCloseAll(CableScanner *, MPS *);
CABLESCANNERDLL_API double	fnGetTimeLen(MPS *);

CABLESCANNERDLL_API int fnTestPrint(); // For test if the dll import success

/////////// Sensors /////////////
// I2C Digital Compass
#include "DigitalCompass.h"
extern "C" DIGITALCOMPASS_API DigitalCompass *fnInitDigitalCompass(); // init DigitalCompass class pointer
extern "C" DIGITALCOMPASS_API double fnGetAngle(DigitalCompass *); // get the current angle
// com GPS
#include "GPS.h"
CABLESCANNERDLL_API GPS *fnInitGPS(unsigned port);
CABLESCANNERDLL_API bool fnGPSStart(GPS *gps);
CABLESCANNERDLL_API bool fnGPSStop(GPS *gps);
CABLESCANNERDLL_API bool fnGPSGetLocation(GPS *gps, double *latitude, char *NS, double *longtitude, char *EW);


#ifdef _DLL_BUILD_TEST_
CABLESCANNERDLL_API void	fnCableScanGeneralTest();
#endif

