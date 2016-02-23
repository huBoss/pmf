#include "stdafx.h"
#include "FFTWFilter.h"

#define _USE_MATH_DEFINES
#include <math.h>
#include <iostream>

#pragma comment(lib, ".\\lib\\libfftw3-3.lib")

/**
@input:
	- N, the length of signal
*/
FFTWFilter::FFTWFilter(size_t N)
{
	this->N = N;
	in = (fftw_complex*)fftw_malloc(sizeof(fftw_complex)* N);
	out = (fftw_complex*)fftw_malloc(sizeof(fftw_complex)* N);
	//freq = new double[N];
	direction = FFTW_FORWARD;
	plan = fftw_plan_dft_1d(N, in, out, direction, FFTW_ESTIMATE); // FFTW_ESTIMATE can be replaced with FFTW_MEASURE
}


FFTWFilter::~FFTWFilter()
{
	fftw_destroy_plan(plan);
	fftw_free(in);
	fftw_free(out);
	//delete freq;
}

//double *
//FFTWFilter::GetFreqPtr()
//{
//	return freq;
//}

void
FFTWFilter::exe_fft(double *signal, double *freq)
{
	if (direction != FFTW_FORWARD)
	{
		direction = FFTW_FORWARD;
		plan = fftw_plan_dft_1d(N, in, out, direction, FFTW_ESTIMATE);
	}
	/* set input data */
	for (size_t i = 0; i < N; i++)
	{
		in[i][0] = in[i][1] = signal[i];
	}

	fftw_execute(plan);

	/* get output data */
	for (size_t i = 0; i < N; i++)
	{
		freq[i] = hypot(out[i][0], out[i][1]) / N;
	}
}

/*
You'd better call exe_fft before call this method, since the (complex *)out has been used
here. The out contain the fft result with complex frequncy domain data.
*/
void
FFTWFilter::exe_ifft(double *freq, double *signal)
{
	if (direction != FFTW_BACKWARD)
	{
		direction = FFTW_BACKWARD;
		plan = fftw_plan_dft_1d(N, in, out, direction, FFTW_ESTIMATE);
	}
	double tempHytop;
	/* set input data */
	for (size_t i = 0; i < N; i++)
	{
		tempHytop = hypot(out[i][0], out[i][1]);
		if (tempHytop)
		{
			in[i][0] = freq[i] * out[i][0] / tempHytop;
			in[i][1] = freq[i] * out[i][1] / tempHytop;
		}
	}
	fftw_execute(plan);

	/* get input data */
	for (size_t i = 0; i < N; i++)
	{
		signal[i] = out[i][0];
	}
}

/**
@input:
	- signal, N length double array contain the input signal, and it's also 
		used for return the value of filted signal
	- freq, N length double array for return the freq domain data
	- minFreq, the min frequency of bandpass filter
	- maxFreq, the max frequency of bandpass filter
	- seconds, the time length of input signal
*/
void 
FFTWFilter::BandpassFilt(double *signal, double *freq, int minFreq, int maxFreq, double seconds)
{
	if (!signal || !freq)
	{
		std::cerr << "ERROR: signal or freq is NULL pointer! exit..." << std::endl;
		exit(-1);
	}

	minFreq = (int)(minFreq*seconds);
	maxFreq = (int)(maxFreq*seconds);

	exe_fft(signal, freq);

	freq[0] *= 1e-5;
	for (int i = 1; i < minFreq; i++)
	{
		freq[i] *= 1e-5;
		freq[N - i] *= 1e-5;
	}
	for (size_t i = maxFreq+1; i < N/2; i++)
	{
		freq[i] *= 1e-5;
		freq[N - i] *= 1e-5;
	}

	exe_ifft(freq, signal);
}

#ifdef _DEFINE_TEST_METHOD_
/*
This method will create a signal including 50Hz, 101Hz and 0Hz sin-signals.
Using the filter, the signal will be filter to a band frequency.
Output the signal and frequency to the file "testFilter.txt"
*/
bool
FFTWFilter::test()
{
	std::cout << "** filter test **" << std::endl;
	FILE *fp;
	fopen_s(&fp, "testFilter.txt", "w");
	if (!fp)
	{
		std::cout << "ERROR: open file failed! exit..." << std::endl;
		exit(-1);
	}
	size_t N = 10240;
	FFTWFilter filter(N);
	double *signal = new double[N];
	double *freq = new double[N];

	double f = 50;
	for (size_t i = 0; i < N; i++)
	{
		signal[i] = 0.5 + sin(2*M_PI*f * i/N) + sin(2*M_PI*101*i/N);
	}

	filter.BandpassFilt(signal, freq, 0, 100, 1.0);

	fprintf(fp, "signal\tfreq\n");
	for (size_t i = 0; i < N; i++)
	{
		fprintf(fp, "%lf\t%lf\n", signal[i], freq[i]);
	}
	fclose(fp);

	delete[] signal;
	delete[] freq;

	return true;
}
#endif // _DEFINE_TEST_METHOD_
