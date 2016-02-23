#pragma once

#include "fftw3.h"
/*
This Macro will build the test method in FFTWFilter
This method will create a signal including 50Hz, 101Hz and 0Hz sin-signals.
Using the filter, the signal will be filter to a band frequency.
*/
#define _DEFINE_TEST_METHOD_

/*
FFTWFilter use the FFTW to achieve the feature of Digital Filter.
You have to place the libfftw3-3.lib in 'lib' folder under the project folder.
Place the 'libfftw3-3.dll' file in the project folder.
*/
class FFTWFilter
{
private:
	fftw_complex *in, *out;
	fftw_plan plan;
	//double *freq;
public:
	/*the length of signal to initialize the FFT.*/
	size_t N;
	/*The direction of FFT, FORWARD(+1), BACKWARD(-1)*/
	int    direction;

private:
	void exe_fft(double *signal, double *freq);
	void exe_ifft(double *freq, double *signal);
public:
	/**
	@input:
	- signal, N length double array contain the input signal, and it's also used for return the value of filted signal
	- freq, N length double array for return the freq domain data
	- minFreq, the min frequency of bandpass filter
	- maxFreq, the max frequency of bandpass filter
	- seconds, the time length of input signal
	*/
	void BandpassFilt(double *signal, double *freq, int minFreq, int maxFreq, double seconds);
	/*Use the length of signal to initialize the FFT.*/
	FFTWFilter(size_t N);
	~FFTWFilter();
	//double *GetFreqPtr();
#ifdef _DEFINE_TEST_METHOD_
	/*
	This method will create a signal including 50Hz, 101Hz and 0Hz sin-signals.
	Using the filter, the signal will be filter to a band frequency.
	Output the signal and frequency to the file "testFilter.txt"
	*/
	static bool test();
#endif // _DEFINE_TEST_METHOD_
};


