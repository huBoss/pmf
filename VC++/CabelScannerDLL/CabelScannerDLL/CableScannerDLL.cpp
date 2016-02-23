// CabelScannerDLL.cpp : 定义 DLL 应用程序的导出函数。
//
#include "stdafx.h"
#include "CableScannerDLL.h"
#include "FFTWFilter.h"

//#define _WRITE_DATA_TO_FILE // write measured voltage of 8 coils to file "data/CableScnnerDLL.txt"

//// 这是导出变量的一个示例
//CABELSCANNERDLL_API int nCabelScannerDLL=0;
//
//// 这是导出函数的一个示例。
//CABELSCANNERDLL_API int fnCabelScannerDLL(void)
//{
//	return 42;
//}
//
//// 这是已导出类的构造函数。
//// 有关类定义的信息，请参阅 CabelScannerDLL.h
//CCabelScannerDLL::CCabelScannerDLL()
//{
//	return;
//}

//#define _TEST_MSG_


#include <iostream>
//#include "CableScanner.h"

#define _USE_MATH_DEFINES
#include <math.h>
#include <fstream>
#include <time.h>

using namespace std;


CableScanner::CableScanner() :
coils(NULL)
{
	cout << WelcomeMsg << endl;

	int N = 2000;
	float f = 50;
	double mu_0 = 4 * M_PI*1e-7;
	double a = 0.03;
	k = N*f*mu_0*M_PI*a*a;
	cout << " k = " << k << endl;
}

CableScanner::~CableScanner()
{
	cout << "Delet CableScanner" << endl;
}

/**
* add coils from file
*/
int
CableScanner::addCoils(char *filename)
{
	cout << "* loading parameters from " << filename << endl;
	FILE *f;
	fopen_s(&f, filename, "r");
	if (!f)
	{
		cout << "open file failed:" << filename << endl;
		//return -1;
		exit(-1);
	}
	fscanf_s(f, "%d", &n);
	this->coils = new Coil[n];
	int notZeroCoilNum = 0;
	for (int i = 0; i < n; i++)
	{
		for (size_t j = 0; j < 3; j++)
		{
			fscanf_s(f, "%lf", &(this->coils[i].position(j)));
		}
		for (size_t j = 0; j < 3; j++)
		{
			fscanf_s(f, "%lf", &(this->coils[i].normal(j)));
		}
		//cout << "  " << coils[i];
		if (!coils[i].isZero())
			notZeroCoilNum++;
	}
	cout << "* load coils data success:" << n << " Valid Coils:" << notZeroCoilNum << endl;

	fclose(f);
	return 0;
}

/**
* @Input
*	- pos: the position of cable in (x,y,z)
*	- cur: the vector of current in (ix,iy,iz)
* @return
*	VectorXd double n length vector include the emf of n coils.
*/
VectorXd
CableScanner::emf(Vector3d pos, Vector3d cur)
{
	VectorXd e = VectorXd::Zero(n);
	// 在Matlab中pos = cb.p'; cur = cb.i';
	// % ---- 计算径向向量r ----
	MatrixXd R = MatrixXd::Zero(n, 3);		//R = zeros(n, 3);
	MatrixXd r = MatrixXd::Zero(n, 3);		//r = zeros(n, 3);
	MatrixXd rr = MatrixXd::Zero(n, 1);		//rr = zeros(n, 1);
	for (int i = 0; i < n; i++)				//for i = 1:n
	{
		R.row(i) = pos - coils[i].position;	//	R(i, :) = cb.p(:) - co(i).p(:); % R_0
	} //	end
	double curNorm = cur.norm();			//	ii = cb.i*cb.i'; maybe sould be changed to cur.norm()?
	MatrixXd v = cur / curNorm;				//	v = cb.i. / sqrt(ii); % 电流的单位向量 v_cable
	MatrixXd r1 = R*v;						//	r1 = R*v'; 
	for (int i = 0; i < n; i++)				//for i = 1:n
	{
		r.row(i) = R.row(i) - r1(i)*v.adjoint();		//	r(i, :) = R(i, :) - r1(i)*v; % r
		//	%         r(i, :) = r(i, :). / (r(i, :)*r(i, :)'); % vector(r)/r^2
		rr(i) = r.row(i).squaredNorm();		//	rr(i) = r(i, :)*r(i, :)';
	}										//	end

	//	% ----计算径向向量r----END----

	//	% ----计算电动势e----
	//	grad = zeros(n, 6);
	Vector3d vt;
	MatrixXd temp;
	for (int i = 0; i < n; i++) //for i = 1:n
	{
		//	% ----计算电动势e----
		vt = R.row(i);
		temp = k *(cur.cross(vt).adjoint() * coils[i].normal) / rr(i);
		e(i) = temp(0); //	e(i) = k*(cross(cb.i, R(i, :))*co(i).n')/rr(i); % (V)
		//	% ----计算电动势对cb.p的偏导dR----
		//	grad(i, 1:3) = (k*cross(co(i).n, cb.i) - 2 * e(i)*r(i, :));
		//% ----计算电动势对cb.i的偏导dR----
		//	grad(i, 4:6) = -(k*cross(co(i).n, R(i, :)) - 2 * (R(i, :)*cb.i')*e(i)*r(i,:)/ii );
		//	grad(i, :) = sign(e(i)) * grad(i, :) / rr(i);
		e(i) = abs(e(i));//e(i) = abs(e(i));
		//%         e(i) = e(i) * 1000; % mV 如果使用mV作为单位的话能在MinFunc的使用中获得更高的精度
	}					//	end
	//	% ----计算电动势e----END----

	return e;
}

int
CableScanner::setCoilsNumber(int num)
{
	if (coils)
		delete[] coils;
	this->n = num;
	this->coils = new Coil[num];

	return this->n;
}

int
CableScanner::addCoil(Coil c, int i)
{
	if (i < 0 || i >= this->n)
	{
		cerr << "ERROR:index out of range: " << i << endl;
		return -1;
	}
	this->coils[i] = c;
	//this->coils[i].Show();
	cout << "added coil:" << coils[i] << endl;
	return 0;
}

bool
CableScanner::test()
{
	Vector3d pos(0, 0, -0.2);
	Vector3d cur(100, 0, 0);
	VectorXd e0 = VectorXd::Zero(n);
	e0 << 0.000107550000000000,	
		0.000894042000000000	,
		0.000414297000000000	,
		0.000376473000000000	,
		0.000840930000000000	,
		0.00105442000000000	,
		0.00103013000000000	,
		0;//-1m
		//0,
		//0.000473595000000000	,
		//0.000915808000000000	,
		//0.00158678000000000	,
		//0.000510788000000000	,
		//0.000413732000000000	,
		//0.000841858000000000	,
		//0; // 0.5m
		//0,
		//0.000485222000000000,
		//0.000469724000000000,
		//0.000602988000000000,
		//0.000218011000000000,
		//0.000144855000000000,
		//0.000447740000000000,
		//0;// 1m
	VectorXd cK = VectorXd::Zero(n);
	//cK << 0.7578,
	//	378.6616,
	//	124.7508,
	//	75.3842,
	//	216.4860,
	//	249.0531,
	//	2.5615,
	//	1.0000;
	/*
	cK << 0.9985,
		1.0385,
		1.0069,
		0.9691,
		1.0242,
		1.0061,
		1.0273,
		1.0000;
	this->k = k*(-0.024384);/*/
	cK << 0.7783,
		41.6144,
		36.5788,
		28.0842,
		43.4910,
		36.4128,
		38.9824,
		1.0000;/*/
		/*0.3269,
		36.3118,
		29.1515,
		15.4095,
		47.2785,
		37.4407,
		0.0026,
		1.0000;*/
	//ofstream file("D:\\Docs\\code\\gitlab\\pmf\\MPS_Acquire_matlab_package\\errmap.txt");
	ofstream file("errmap.txt");

	clock_t t1 = clock();
	MatrixXd MXd = scan(pos, cur, cK.cwiseProduct(e0), -2, 2, -2, 0.2, 100, 100);
	cout << (clock() - t1) << " ms" << endl;

	if (file.is_open())
	{
		file << MXd << endl;
	}
	else
		cout << MXd << endl;
	cout << "(" << maxy << "," << maxz << ")" << endl;
	return true;
}

MatrixXd
CableScanner::scan(Vector3d pos, Vector3d cur, VectorXd e0, double ymin, double ymax, double zmin, double zmax, int ynum, int znum)
{
	if (!coils)
	{
		cerr << "ERROR: the coils have not been added! exit..." << endl;
		exit(-1);
	}

	MatrixXd probmap = MatrixXd::Zero(ynum, znum);
	VectorXd e;
	pos(1) = ymin;
	pos(2) = zmin;
	double dy = (ymax - ymin) / ynum;
	double dz = (zmax - zmin) / znum;
	maxprob = .0;
	minprob = 1.;
	maxy = ymin;
	maxz = zmin;
	for (int i = 0; i < ynum; i++)
	{
		pos(1) += dy;
		pos(2) = zmin;
		for (int j = 0; j < znum; j++)
		{
			pos(2) += dz;
			e = emf(pos, cur);
			probmap(i, j) = exp(-1000 * (e - e0).norm());
			if (maxprob < probmap(i, j))
			{
				maxprob = probmap(i, j);
				maxy = pos(1);
				maxz = pos(2);
			}
			if (minprob > probmap(i, j))
			{
				minprob = probmap(i, j);
			}
		}
	}
	/*if (maxprob == 0)
	{
		cout << "maxprob is zero: not found cable" << endl;
	}*/
	//cout << "the max probability is " << (maxprob * 100)/*"->" << (log(maxprob)/(-100)) <<*/ << " at (" << maxy << ", " << maxz << ")" << endl;
	return probmap;
}

CableScanner *fnCableScanInit()
{
	CableScanner *cableScannerPtr = new CableScanner();
	cableScannerPtr->setCoilsNumber(8);
	cableScannerPtr->addCoils("coils_ascii.mat");
	return cableScannerPtr;
}

MPS *fnMPSInit(int bulks, int sampleRateSelect)
{
	return new MPS(bulks, sampleRateSelect);
}

MPS *fnMPSInitX(int bulks, int sampleRateSelect, double data[])
{
	return new MPS(bulks, sampleRateSelect, data);
}

/*return the length of data
 */
int fnGetDataLength(MPS *mps)
{
	return mps->samples*mps->bulks*mps->channels;
}

double *fnGetDataPtr(MPS *mps)
{
	return mps->data;
}

int fnFreqPtrLen(CableScanner *cableScanner, MPS *mps)
{
	return mps->samples*mps->bulks*cableScanner->n;
}

double *fnFreqPtrMalloc(CableScanner *cableScanner, MPS *mps) 
{
	int N = mps->samples*mps->bulks;
	double *ptr = new double[N*(cableScanner->n)];
	return ptr;
}

void fnFreqPtrFree(double *freq)
{
	delete[] freq;
}

bool fnGetData(MPS *mps)
{
	if (mps->GetData(false))
		return true;
	return false;
}

/*@Input:
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
*/
bool fnCableScan(
	CableScanner *cableScanner, MPS *mps,
	double *Map, double *yPtr, double *zPtr, double *freq, double *measuedVolt,
	double *params)
	//double current, double ymin, double ymax, double zmin, double zmax, int ynum, int znum)
{
	double current = params[0];
	double ymin = params[1];
	double ymax = params[2];
	double zmin = params[3];
	double zmax = params[4];
	int ynum = (int)params[5];
	int znum = (int)params[6];
	//cout << ymin << ymax << zmin << zmax << "ynum " << ynum << ", znum " << znum << endl;
	if (!Map || !yPtr || !zPtr || !freq || !mps->data)
	{
		cout << "NULL pointers" << endl;
#ifdef _TEST_MSG_
		if (!Map)
			cout << "Map is NULL" << endl;
		if (!yPtr)
			cout << "yPtr is NULL" << endl;
		if (!zPtr)
			cout << "zPtr is NULL" << endl;
		if (!freq)
			cout << "freq is NULL" << endl;
		if (!mps->data)
			cout << "mps->data is NULL" << endl;
#endif
		exit(-1);
	}

	VectorXd e0 = VectorXd::Zero(cableScanner->n);
	/* ------ data getter and digital filter ------*/
#ifdef _TEST_MSG_
	cout << "getdata..." << endl;
#endif
	double *signals = mps->data;//mps->GetData(false);  // FIXME:内存不应该这么分配
#ifdef _TEST_MSG_
	cout << "getdata...ok" << endl;
#endif
	int N = mps->samples*mps->bulks;
	double *sig = signals;
	//double *freq;
	if (freq == NULL)
		//freq = new double[N*cableScanner->n]; // FIXME
	{
		cout << "the freq pointer has not been initialized, you may call 'fnFreqPtrMalloc' firstly. exit..." << endl;
		return false;
	}

#ifdef _TEST_MSG_
	cout << "init filter..." << endl;
#endif
	FFTWFilter filter(N);
#ifdef _TEST_MSG_
	cout << "init filter...ok" << endl;
#endif
	int minFreq = 30;
	int maxFreq = 70;
	double time = N / (double)mps->sampleRate;
	double maxvalue;
	int win_min = (int)(0.1*N);
	int win_max = (int)(0.9*N);
	// Here, we use the cableScanner->n but not the mps->channels, since the 
	// former may be smaller than the latter
	for (int i = 0; i < cableScanner->n; i++)
	{
#ifdef _TEST_MSG_
		cout << "BandpassFilt..." << i << endl;
#endif
		filter.BandpassFilt(sig, freq, minFreq, maxFreq, time);

		maxvalue = 0;
		for (int j = win_min; j < win_max; j++)
		{
			if (sig[j] > maxvalue)
				maxvalue = sig[j];
		}
		e0(i) = maxvalue>0.09?maxvalue:0;
		measuedVolt[i] = e0(i)/AMP_SCALE;

		sig += N;
		freq += N;
	}
	cout << "measured volt" << (int)measuedVolt << endl;
	e0 /= AMP_SCALE;
	cout << "measured voltages: " << e0.adjoint() << endl;
#ifdef _WRITE_DATA_TO_FILE
	ofstream outfile;
	outfile.open("data/CableScnnerDLL.txt", ios::app);
	outfile << e0.adjoint() << "  ";
#endif
	/* ------ data getter and digital filter end ------*/


	/* ------- scan probmap --------- */
	Vector3d pos(0, 0, -0.2);
	Vector3d cur(current, 0, 0);
	VectorXd cK = VectorXd::Zero(cableScanner->n);
	cK << 0.7783,
		41.6144,
		36.5788,
		28.0842,
		43.4910,
		36.4128,
		38.9824,
		1.0000; // this should be updated by new train result.

	clock_t t1 = clock();
	MatrixXd MXd = cableScanner->scan(pos, cur, cK.cwiseProduct(e0), ymin, ymax, zmin, zmax, ynum, znum);
	cout << "cable scan used time: " << (clock() - t1) << " ms" << endl;
	cout << "cable scan area :     " << "[" << ymin << ", " << ymax << "]x[" << zmin << ", " << zmax << "]" << endl;

	double *mapPtr = Map;
	int i, j;
	for (/*int */i = 0; i < ynum; i++)
	{
		for (/*int */j = 0; j < znum; j++)
		{
			mapPtr[j] = MXd(i,j);
			//cout << "mapPtr " << mapPtr[j] << endl; // TEST
		}
		mapPtr += znum;
	}

	*yPtr = cableScanner->maxy;
	*zPtr = cableScanner->maxz;
	cout << "cable position: (" << *yPtr << ", " << *zPtr << ")" << "@" << cableScanner->maxprob << endl;
#ifdef _WRITE_DATA_TO_FILE
	outfile << *yPtr << "  " << *zPtr << endl;
	outfile.close();
#endif
#ifdef _TEST_MSG_
	//cout << "i:      " << i << endl;
	//cout << "j:      " << j << endl;
	//cout << "*yPtr = " << *yPtr << endl;
	//cout << "*zPtr = " << *zPtr << endl;

	cout << "return from cable scanner dll" << endl;

#endif // _TEST_MSG_

	if (e0(0) > 1e-4)
	{
		cout << "WARNING: the car may not in correct direction, for coil 1 get value at " << e0(0) << endl;
	}
	// Check if the signals is large enough for finding an cable
	for (i = 0; i < cableScanner->n; i++)
	{
		if (e0(i) > 2e-4)
		{
			cout << "** Found Cable!! **" << endl;
			break;
		}
	}
	if (i == cableScanner->n) {
		cout << "** not found cable **" << endl;
		return false;
	}
	return true;
}

void fnCableScanCloseAll(CableScanner *cableScanner, MPS *mps)
{
	delete cableScanner;
	delete mps;
}

//double *fnMPSGetDataPtr(MPS *mps)
//{
//	return mps->data;
//}

#ifdef _DLL_BUILD_TEST_

void fnCableScanGeneralTest()
{
	FFTWFilter::test();
}

#endif // _DLL_BUILD_TEST_

int fnTestPrint()
{
	cout << "Hello from CableScannerDLL inner!" << endl;
	return 123;
}

double	fnGetTimeLen(MPS *mps)
{
	return mps->timeLen;
}

DigitalCompass *fnInitDigitalCompass()
{
	return new DigitalCompass();
}

double fnGetAngle(DigitalCompass *dc)
{
	return dc->getAngle();
}

GPS *fnInitGPS(unsigned port)
{
	return new GPS(port);
}

bool fnGPSStart(GPS *gps)
{
	return gps->Start();
}
bool fnGPSStop(GPS *gps)
{
	return gps->Stop();
}
bool fnGPSGetLocation(GPS *gps, double *latitude, char *NS, double *longtitude, char *EW)
{
	return gps->GetLocation(*latitude, *NS, *longtitude, *EW);
}