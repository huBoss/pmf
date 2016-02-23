#include "stdafx.h"
#include "GPS.h"
#include <iostream>


GPS::GPS(unsigned port):
started(false)
{
	serialPort.InitPort(port);
}


GPS::~GPS()
{
	cout << "Goodbye from GPS" << endl;
	if (started)
		serialPort.CloseListenTread();
}

bool
GPS::Start()
{
	started = serialPort.OpenListenThread();
	return started;
}

bool
GPS::Stop()
{
	started = !serialPort.CloseListenTread();
	return started;
}

bool
GPS::GetLocation(double &latitude, char &NS, double &longtitude, char &EW)
{
	string locstr = serialPort.getLocation(); // Get 'GPRMC' format lcation
	this->split(locstr, ",");
	if (locvec.size() < 13 || locvec[12].find("N") != string::npos)
	{
		cout << " * not valid" << endl;
		return false;
	}
	if (locvec.size() < 3 || locvec[2].find("A")==string::npos)
	{
		cout << " * not located" << endl;
		return false;
	}
	cout << locstr << endl;
	latitude = str2degree(locvec[3]);
	NS = locvec[4].c_str()[0];
	longtitude = str2degree(locvec[5]);
	EW = locvec[6].c_str()[0];
	//cout << latitude << NS << "," << longtitude << EW << endl;
	return true;
}

void GPS::split(string str, string pattern)
{
	string::size_type pos;
	/*vector<string> result;
	result.clear()*/
	locvec.clear();
	str += pattern;//扩展字符串以方便操作
	string::size_type size = str.size();

	for (unsigned i = 0; i<size; i++)
	{
		pos = str.find(pattern, i);
		if (pos<size)
		{
			string s = str.substr(i, pos - i);
			locvec.push_back(s);
			i = pos + pattern.size() - 1;
		}
	}
}

double GPS::str2degree(string str)
{
	double degree = atof(str.c_str());
	degree /= 100;
	degree = (int)degree + (degree - (int)degree)*100/60;
	return degree;
}
