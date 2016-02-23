#include "stdafx.h"

#include "mps.h"

#include <iostream>

using namespace std;

MPS::MPS(
	int bulks,
	int sampleRateSelect,
	double *data):
	samples(1024),
	channels(MPS_CHANNELS),
	MPSdllname("lib/MPS-140801.dll"),
	malloced_data(false)
{
	init(bulks, sampleRateSelect, data);
}

MPS::MPS(
	int bulks,
	int sampleRateSelect):
	samples(1024),
	channels(MPS_CHANNELS),
	MPSdllname("lib/MPS-140801.dll"),
	malloced_data(true)
{
	/* malloc memory for data */
	//data = (double *)malloc(channels*samples*bulks * sizeof(double));
	data = new double[channels*samples*bulks];

	init(bulks, sampleRateSelect, data);

}

void MPS::init(
	int bulks,
	int sampleRateSelect,
	double *data)
{
	this->data = data;
	this->bulks = bulks;

	sampleRate = sampleRateSelect;
	switch (sampleRate) {
	case 128:
		sampleRate = SampleRate128k;
		break;
	case 64:
		sampleRate = SampleRate64k;
		break;
	case 32:
		sampleRate = SampleRate32k;
		break;
	case 16:
		sampleRate = SampleRate16k;
		break;
	case 8:
		sampleRate = SampleRate8k;
		break;
	default:
		printf("Error sample rate: %d(should be 128,64,32,16,or 8), set default valuE\n",
			sampleRate);
		sampleRate = DEFAULT_SAMPLE_RATE;
	}
	printf("set sample rate as %d", sampleRate);

	/* init function pointers */
	int flag = initFuncPointers();
	if (flag == -1)
	{
		cout << "exit..." << endl;
		return;
	}
	else if (flag != 0)
	{
		cout << "error numbers: [" << flag << "]" << endl;
		return;
	}

	/* init device */
	int DeviceNumber = 0; // default single device

	Device_H = MPS_OpenDevice(DeviceNumber);
	if (NULL == Device_H)
	{
		printf("can't open device id: %d\n", DeviceNumber);
		//return;
		exit(-1);
	}
	int DeviceID = MPS_GetDeviceID(Device_H);
	if (0 == DeviceID)
	{
		cout << "ERROR: Get device id failed, please plug in the MPS-device, exit..." << endl;
		exit(-1);
		// return;
	}
	cout << "open device: " << DeviceNumber << "(" << DeviceID << ") success" << endl;

	timeLen = samples*bulks / sampleRate;
}

MPS::~MPS()
{
	if (malloced_data)
	{
		delete[] data;
	}
	MPS_Stop(Device_H);
	if (MPS_CloseDevice(Device_H))
		cout << "close device success" << endl;
	else
	{
		cout << "close device failed" << endl;
		if (!Device_H)
			cerr << "WARN: DEVICE HANDLE is 0" << endl;
	}

}

/**
 verb == false, not display the values 
*/
double * 
MPS::GetData(bool verb){

	/* data acquire */
	int Buffer[1024 * 8] = { 0 };
	// const int loops = bulks;
	//double Voltage[channels][samples];
	//double *data = (double *)mxMalloc(channels*samples*bulks * sizeof(double));
	int flag = 1; // 函数执行成功标志

	/* configure */
	MPS_Configure(sampleRate, Device_H);

	MPS_Start(Device_H);

	int j, k;
	double *ptr[MPS_CHANNELS];
	for (int i = 0; i < MPS_CHANNELS; i++)
	{
		ptr[i] = data + i*bulks*samples;
	}
	// = data;
	/* data in loop */
	for (int i = 0; i<bulks; ++i/*, ptr += samples*channels*/) {
		/* data in */
		flag = MPS_DataIn(Buffer, samples * channels, Device_H);
		if (flag != 0) {
			for (j = 0; j<samples/**channels*/; ++j)
			{
				for (k = 0; k < MPS_CHANNELS; ++k)
				{
					ptr[k][j] = ((double)Buffer[j*MPS_CHANNELS + k] / (65536 * 128)) * 10.218;
				}
			}
			if (verb)
				cout << "data: " << i << " " << ptr[10] << endl;
		}
		else {
			cout << "ERROR: MPS_DataIn return 0, data error index: " << i << endl;
			break;
			//exit(-1);
		}
		for (int i = 0; i < MPS_CHANNELS; i++)
		{
			ptr[i] += samples;
		}
	}

	MPS_Stop(Device_H); // stop device

	return data;
}


int 
MPS::initFuncPointers(){

	int flag = 0; // record error numbers

	/* load dll */
	HINSTANCE MPSdll_H;
	MPSdll_H = LoadLibraryA(MPSdllname);

	if (NULL == MPSdll_H)
	{
		cout << "can not load dll \"" << MPSdllname << "\"" << endl;
		return --flag;
	}
	cout << "load dll \"" << MPSdllname << "\" success" << endl;

	/* init func pointers */
	MPS_OpenDevice = (Handle(*)(int))GetProcAddress(MPSdll_H, "MPS_OpenDevice");
	if (NULL == MPS_OpenDevice)
	{
		printf("can't find function MPS_OpenDevice\n");
		flag++;
	}

	MPS_GetDeviceID = (int(*)(Handle))GetProcAddress(MPSdll_H, "MPS_GetDeviceID");
	if (NULL == MPS_GetDeviceID){
		printf("can't find function MPS_OpenDevice\n");
		flag++;
	}

	// configure
	MPS_Configure = (int(*)(int, Handle))GetProcAddress(MPSdll_H, "MPS_Configure");
	if (NULL == MPS_Configure){
		printf("can't find function MPS_OpenDevice\n");
		flag++;
	}
	// start
	MPS_Start = (int(*)(Handle))GetProcAddress(MPSdll_H, "MPS_Start");
	if (NULL == MPS_Start){
		printf("can't find function MPS_OpenDevice\n");
		flag++;
	}
	// stop
	MPS_Stop = (int(*)(Handle))GetProcAddress(MPSdll_H, "MPS_Stop");
	if (NULL == MPS_Stop){
		printf("can't find function MPS_OpenDevice\n");
		flag++;
	}
	// datain
	MPS_DataIn = (int(*)(int*, int, Handle))GetProcAddress(MPSdll_H, "MPS_DataIn");
	if (NULL == MPS_DataIn){
		printf("can't find function MPS_OpenDevice\n");
		flag++;
	}

	MPS_CloseDevice = (int(*)(Handle))GetProcAddress(MPSdll_H, "MPS_CloseDevice");
	if (NULL == MPS_CloseDevice){
		printf("can't find function MPS_OpenDevice\n");
		flag++;
	}

	return flag;
}