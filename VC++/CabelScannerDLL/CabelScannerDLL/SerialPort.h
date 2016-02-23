#pragma once
 
#include<Windows.h>  
#include<string>
#include<vector>


using namespace std;

class CSerialPort  
{  
public:
    CSerialPort(void);  
    ~CSerialPort(void);  
 
public:  
    void InitPort( UINT  portNo = 1,UINT  baud = CBR_9600,char  parity = 'N',UINT  databits = 8, UINT  stopsbits = 1,DWORD dwCommEvents = EV_RXCHAR);  
    void InitPort( UINT  portNo ,const LPDCB& plDCB );  
    bool OpenListenThread();  
    bool CloseListenTread();  
    bool WriteData(unsigned char* pData, unsigned int length);  
    UINT GetBytesInCOM();  
    bool ReadChar(char &cRecved);  
    bool openPort( UINT  portNo );  
    void ClosePort();  
    static UINT WINAPI ListenThread(void* pParam);  
    HANDLE  m_hComm;  
    static bool s_bExit;  
    volatile HANDLE    m_hListenThread;  
    CRITICAL_SECTION   m_csCommunicationSync;

	static string location;

	string getLocation();
 
};  


