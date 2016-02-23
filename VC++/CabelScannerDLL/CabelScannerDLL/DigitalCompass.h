#pragma once

class DigitalCompass
{
private:
	short x; // 16bit补码表示
	short y;
	short z;

public:
	DigitalCompass();
	~DigitalCompass();
	bool Init();
	void Update();
	double getAngle();//由xy计算并返回角度
	int getX();//返回x,short int转化为int,无损失
	int getY();//返回y,short int转化为int,无损失
	int getZ();//返回z,short int转化为int,无损失
};
