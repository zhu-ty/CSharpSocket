// CSharpSocketCppExport.cpp : Defines the exported functions for the DLL application.
//

#include "CSharpSocketCppExport.h"
#include <vector>
#using "../CSharpSocket/bin/Debug/CSharpSocket.dll"

#define MAX_SOCKET 50

ref class SKref {
public:
	// static CSharpSocket::SKTcpSocket ^tcpSocket_;
	// static vector<CSharpSocket::SKTcpSocket^> tcpSocket_list;
	static cli::array<CSharpSocket::SKTcpSocket^>^ tcpSocket_array;
};

// This is the constructor of a class that has been exported.
// see CSharpSocketCppExport.h for the class definition
SKSocket::SKSocket()
{
	if (SKref::tcpSocket_array == nullptr)
		SKref::tcpSocket_array = gcnew cli::array<CSharpSocket::SKTcpSocket^>(MAX_SOCKET);
	int i = 0;
	for (i = 0; i < MAX_SOCKET; i++)
	{
		if (SKref::tcpSocket_array[i] == nullptr)
			break;
	}
	usage_i = i;
	printf("[SK Socket]using socket array num.%d, maximum %d\n", usage_i, MAX_SOCKET);
	//tcpSocket_ = (void*)(gcnew SKref());
	SKref::tcpSocket_array[usage_i] = gcnew CSharpSocket::SKTcpSocket();
	return;
}

bool SKSocket::state()
{
	return SKref::tcpSocket_array[usage_i]->state();
}

bool SKSocket::abort()
{
	SKref::tcpSocket_array[usage_i]->abort();
	return true;
}

bool SKSocket::connectToHost(unsigned char * ip, int ip_len, int port, int exceed_time)
{
	return SKref::tcpSocket_array[usage_i]->connectToHost(ip, ip_len, port, exceed_time);
}

bool SKSocket::write(unsigned char * data, int data_len)
{
	return SKref::tcpSocket_array[usage_i]->write(data, data_len);
}

bool SKSocket::waitForReadyRead(int exceed_time)
{
	return SKref::tcpSocket_array[usage_i]->waitForReadyRead(exceed_time);
}

int SKSocket::read(unsigned char * data, int len)
{
	return SKref::tcpSocket_array[usage_i]->read(data, len);
}
