#pragma once
#include "SerialPort.h"
#include <string>


class GPS
{
private:
	vector<string> locvec;
	CSerialPort serialPort;
	void split(string str, string pattern); // split string by pattern
	double str2degree(string); // translate numeric string to double degree
public:
	bool started; // true if the GPS Thread is started
public:
	GPS(unsigned port);
	~GPS();
	bool Start(); // start GPS Thread
	bool Stop();  // stop GPS Thread
	bool GetLocation(double &latitude, char &NS, double &longtitude, char &EW);
};

