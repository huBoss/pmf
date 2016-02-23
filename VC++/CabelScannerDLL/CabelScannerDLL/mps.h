#pragma once

#include <Windows.h>

#define SampleRate128k 128000
#define SampleRate64k  64000
#define SampleRate32k  32000
#define SampleRate16k  16000
#define SampleRate8k   8000
#define SampleRate4k   4000

#define DEFAULT_SAMPLE_RATE  SampleRate8k

typedef int Handle;

#define MPS_CHANNELS 8

/*
MPS is used for read data from MPS-1 device.
*/
class MPS {
private:
	Handle Device_H;
	const char *MPSdllname; // default as "lib/MPS-140801.dll"
	bool malloced_data;

public:
	int bulks;
	int sampleRate;
	const int channels;
	double timeLen;
	double *data;
	int samples; // the number of samples of one bulk

private:
	Handle(*MPS_OpenDevice)(int DeviceNumber);
	int(*MPS_GetDeviceID)(Handle);
	int(*MPS_Configure)(int SampleRate, Handle DeviceHandle);
	int(*MPS_Start)(Handle DeviceHandle);
	int(*MPS_Stop)(Handle DeviceHandle);
	int(*MPS_DataIn)(int *DataArray, int SampleNum, Handle DeviceHandle);
	int(*MPS_CloseDevice)(Handle HDevice);

	int initFuncPointers(); // return error numbers
	void init(int bulks, int sampleRateSelect, double *data);

public:
	/*This will malloc the memory for reading data.
	@Input:
		- bulks, the number of buffers, the length of data will be (samples*bulks*channels).
		- sampleRateSelect, would be 128(k), 64(k), 32(k), 16(k), 8(k)
	*/
	MPS(int bulks, int sampleRateSelect); // this will init the memory
	MPS(int bulks, int sampleRateSelect, double *data);
	~MPS();
	/*Read data to 'data' pointer in class MPS, and return the pointer.*/
	double *GetData(bool verb); // read data to 'data' pointer
};