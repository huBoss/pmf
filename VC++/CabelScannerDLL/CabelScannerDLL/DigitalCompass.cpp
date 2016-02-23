#include "stdafx.h"
#include <iostream>
using namespace std;

#pragma comment(lib, "lib/USBIOX.LIB")
#include "USBIOX.H"

#include "DigitalCompass.h"

DigitalCompass::DigitalCompass() :
x(0), y(0), z(0)
{
	if (!Init())
		exit(-1);
}

DigitalCompass::~DigitalCompass()
{

}

bool
DigitalCompass::Init()
{
	ULONG mIndex = 0;
	ULONG mwlen = 3, mrlen = 0;
	UCHAR wbuffer[3] = "";
	UCHAR rbuffer[3] = "";
	wbuffer[0] = 0x3C;
	wbuffer[1] = 0x02;
	wbuffer[2] = 0x00;//修改闲置模式为连续测量模式
	if (USBIO_OpenDevice(mIndex) != INVALID_HANDLE_VALUE)	{
		BOOL a = USBIO_StreamI2C(mIndex, mwlen, &wbuffer[0], mrlen, &rbuffer[0]);
		cout << "Initialize OK" << endl;
		return true;
	}
	else{
		cout << "Initialize Error\r\n" << endl;
		return false;
	}
}

void DigitalCompass::Update(){
	ULONG mIndex = 0;
	ULONG mwlen = 3, mrlen = 9;
	UCHAR wbuffer[3] = "";
	UCHAR rbuffer[9] = "";
	wbuffer[0] = 0x3C;
	wbuffer[1] = 0x0C;
	wbuffer[2] = 0x3D;//顺序读出前九个数据，前三个为状态，后六个为xzy
	BOOL a = USBIO_StreamI2C(mIndex, mwlen, &wbuffer[0], mrlen, &rbuffer[0]);
	x = (rbuffer[3] << 8) | rbuffer[4];//x转化为十六位补码表示的数字
	z = (rbuffer[5] << 8) | rbuffer[6];//z转化为十六位补码表示的数字
	y = (rbuffer[7] << 8) | rbuffer[8];//y转化为十六位补码表示的数字
	cout << endl;
	for (unsigned int i = 0; i<mrlen; i++){
		printf("%2x,", rbuffer[i]);//显示读出的九个数据
	}
	cout << endl;
}

int DigitalCompass::getX(){
	return x;
}

int DigitalCompass::getY(){
	return y;
}

int DigitalCompass::getZ(){
	return z;
}

double DigitalCompass::getAngle(){
	double Angle = atan2((double)y, (double)x)*(180 / 3.14159265) + 180;
	return Angle;
}
