#include "stdafx.h"

#include "Coil.h"
#include <iostream>

using namespace std;

Coil::Coil(Vector3d position, Vector3d normal)
{
	set(position, normal);
}


Coil::~Coil()
{
}

Coil::Coil()
{
}

int
Coil::set(Vector3d position, Vector3d normal)
{
	this->normal = normal;
	this->position = position;

	return 0;
}

bool
Coil::isZero()
{
	for (int i = 0; i < 3; i++)
	{
		if (normal[i])
		{
			return false;
		}
	}
	return true;
}

ostream & operator <<(ostream &out, Coil &c)
{
	out << "position " << c.position.adjoint() <<
		"\tnormal " << c.normal.adjoint() << endl;
	return out;
};