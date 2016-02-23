#pragma once

#ifndef __EIGEN__
#define __EIGEN__
#include "Eigen\Dense"

using namespace Eigen;
#endif //~__EIGEN__

#include <iostream>

/*Define the Class of coil containing the normal and position of coil.*/
class Coil
{
public:
	Vector3d normal;
	Vector3d position;

public:
	Coil(Vector3d, Vector3d);
	Coil();
	~Coil();
	int set(Vector3d, Vector3d);
	//void Show()const;
	bool isZero();
	friend std::ostream & operator<< (std::ostream &out, Coil &c);
};

