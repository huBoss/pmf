#ifndef _MPS_MEX86_H_
#define _MPS_MEX86_H_


#include <iostream>

using namespace std;

#include <Windows.h>

#include "mex.h"


#define SampleRate128k 128000
#define SampleRate64k  64000
#define SampleRate32k  32000
#define SampleRate16k  16000
#define SampleRate8k   8000
#define SampleRate4k   4000

#define SampleRateNow  SampleRate8k


typedef int Handle;

Handle (*MPS_OpenDevice)(int DeviceNumber);
int (*MPS_GetDeviceID)(Handle);
int (*MPS_Configure)(int SampleRate, Handle DeviceHandle);
int (*MPS_Start)(Handle DeviceHandle);
int (*MPS_Stop)(Handle DeviceHandle);
int (*MPS_DataIn)(int *DataArray, int SampleNum, Handle DeviceHandle);
int (*MPS_CloseDevice)(Handle HDevice);

int initFuncPointers(); // return error numbers


#endif //~_MPS_MEX86_H_