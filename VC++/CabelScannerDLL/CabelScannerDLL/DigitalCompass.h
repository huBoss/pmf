#pragma once

class DigitalCompass
{
private:
	short x; // 16bit�����ʾ
	short y;
	short z;

public:
	DigitalCompass();
	~DigitalCompass();
	bool Init();
	void Update();
	double getAngle();//��xy���㲢���ؽǶ�
	int getX();//����x,short intת��Ϊint,����ʧ
	int getY();//����y,short intת��Ϊint,����ʧ
	int getZ();//����z,short intת��Ϊint,����ʧ
};
