/* 
* @Author: Dell
* @Date:   2014-09-20 14:34:19
* @Last Modified by:   Administrator
* @Last Modified time: 2014-12-26 11:06:35
*/

#include "mps_mex86.h"

/**
 * data = mps_mex86(bulks, verb, sampleRate); if verb is 0 will print error msg.
 * @param nlhs the number of ouput param
 * @param plhs the list of output param
 * @param nrhs the number of input param
 * @param prhs the list of input param
 *
 * @Todo 注意内存释放的问题，特别是非Matlab方式申请的内存空间�?
 * @Todo 尝试返回设备Handle，避免设备的反复Open
 */
void mexFunction(int nlhs, mxArray *plhs[], int nrhs, const mxArray *prhs[]){

	/* check for matlab parameters */
	if (nrhs < 1)
		mexErrMsgTxt("Require least 1 parameters (the number of bulks).");
	else if (nrhs > 3)
		mexErrMsgTxt("Too many input parameters.");
	if (nlhs > 1)
		mexErrMsgTxt("Too many output parameters");

	/* input params */
	int paraSizeM, paraSizeN;
	paraSizeM = mxGetM(prhs[0]);
	paraSizeN = mxGetN(prhs[0]);
	if (paraSizeM != 1 || paraSizeN != 1)
		mexErrMsgTxt("Wrong dim of the 1 parameter, should be 1x1.");
	if (nrhs > 1)
	{
		paraSizeM = mxGetM(prhs[1]);
		paraSizeN = mxGetN(prhs[1]);
		if (paraSizeM != 1 || paraSizeN != 1)
			mexErrMsgTxt("Wrong dim of the 2 parameter, should be 1x1.");
	}
	// if (nlhs > 1)
	// {
		
	// }

	/* get input params */
	double *inputPr = mxGetPr(prhs[0]);
	int bulks = (int)inputPr[0];
	int verb = 0; // 1 : 打印错误信息
	int sampleRate = SampleRateNow;
	if (nrhs >1)
	{
		inputPr = mxGetPr(prhs[1]);
		verb = (int)inputPr[0];
	}
	if (nrhs>2)
	{
		inputPr = mxGetPr(prhs[2]);
		sampleRate = (int)inputPr[0];
		switch (sampleRate) {
			case 128:
				sampleRate = SampleRate128k;
				printf("set sample rate as %d", sampleRate);
				break;
			case 64: 
				sampleRate = SampleRate64k ;
				printf("set sample rate as %d", sampleRate);
				break;
			case 32: 
				sampleRate = SampleRate32k ;
				printf("set sample rate as %d", sampleRate);
				break;
			case 16: 
				sampleRate = SampleRate16k ;
				printf("set sample rate as %d", sampleRate);
				break;
			case 8:  
				sampleRate = SampleRate8k  ;
				printf("set sample rate as %d", sampleRate);
				break;
			default: 
				printf("Error sample rate: %d(should be 128,64,32,16,or 8), set default value:%d\n", 
					sampleRate, SampleRateNow);
				sampleRate = SampleRateNow;
		}
	}
	// if (channels > 8)
	// {
	// 	channels = 8;
	// } else if (channels < 1)
	// {
	// 	channels = 1;
	// }
	plhs[0] = mxCreateDoubleMatrix(0, 0, mxREAL);

	/* init function pointers */
    cout << "init func pointers..." << endl;
//     return;
	int flag = initFuncPointers();
	if (flag==-1)
	{
		cout << "exit..." << endl;
        mexErrMsgTxt("flag is -1, return []");
		return;
	}
	else if (flag!=0)
	{
		cout << "error numbers: [" << flag << "]" <<endl;
        mexErrMsgTxt("flag is 0, return []");
		return;
	}
    
	Handle Device_H;
	int DeviceNumber = 0; // default single device

	Device_H = MPS_OpenDevice(DeviceNumber);
	if (NULL == Device_H)
	{
		printf("can't open device id: %d\n", DeviceNumber);
		return;
	}
	int DeviceID = MPS_GetDeviceID(Device_H);
	if (0 == DeviceID)
	{
		cout  << "get device id failed, please plug in the MPS-device, exit..." << endl;
		return;
	}
	cout << "open device: "<<DeviceNumber<<"("<<DeviceID<<") success"<<endl;

	/* data acquire */
	int Buffer[1024 * 8] = { 0 };
	const int channels = 8;
	const int samples =1024;
	// const int loops = bulks;
	double Voltage[channels][samples];
	double *data = (double *) mxMalloc(channels*samples*bulks * sizeof(double));
	flag = 1; // 函数执行成功标志

	/* configure */
	MPS_Configure(sampleRate, Device_H);

	MPS_Start(Device_H);

	int j;
	int chs;
	double *ptr = data;
	/* data in loop */
	for (int i = 0; i<bulks; ++i,ptr+=samples*channels) {
		/* data in */
		flag = MPS_DataIn(Buffer, samples * channels, Device_H);
		if (flag != 0) {
			for (j = 0; j<samples*channels; ++j)
			{
				ptr[j] = ((double)Buffer[j] / (65536 * 128)) * 10.218; // matlab的矩阵式列主序的，�?Buffer也是
			}
			if (verb)
				cout << "data: " << i << " " << ptr[10] << endl;
		}
		else {
			cout << "data error: " << i << endl;
			break;
		}
	}

	MPS_Stop(Device_H);

	/* close device(@BUG : will cause Matlab 2013b crash) */
	// if (MPS_CloseDevice(Device_H))
	// 	cout << "close device success" << endl;
	// else
	// 	cout << "close device failed" << endl;

	/* set output data */
	plhs[0] = mxCreateDoubleMatrix(channels, samples * bulks, mxREAL);
	/* send data */
// 	cout << "nlhs: " << nlhs <<endl;
	mxSetPr(plhs[0], data);


    return;
}


int initFuncPointers(){

	int flag = 0; // record error numbers

	/* load dll */
	HINSTANCE MPSdll_H;
	const char *MPSdllname = "./MPS-140801.dll";
	MPSdll_H = LoadLibraryA(MPSdllname);

	if (NULL == MPSdll_H)
	{
		cout << "can not load dll \"" << MPSdllname << "\"" << endl;
        mexErrMsgTxt("can not load dll");
		return --flag;
	}
	cout << "load dll \"" << MPSdllname <<"\" success" <<endl;

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

	int(*MPS_CloseDevice)(Handle) = (int(*)(Handle))GetProcAddress(MPSdll_H, "MPS_CloseDevice");
	if (NULL == MPS_CloseDevice){
		printf("can't find function MPS_OpenDevice\n");
		flag++;
	}

	return flag;
}