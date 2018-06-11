#include<iostream>
using namespace std;

#include "../CSharpSocketCppExport/CSharpSocketCppExport.h"
#pragma comment(lib, "../x64/Debug/CSharpSocketCppExport")

int main()
{
	SKSocket *tcpsocket = new SKSocket();
	char p[] = "10.8.5.188";
	char b[100];
	memset(b, 0, sizeof(b));
	int send = 1;
	//tcpsocket->abort();
	tcpsocket->connectToHost((unsigned char*)p, sizeof(p) - 1, 54321, 10000);
	tcpsocket->write((unsigned char*)(&send), 4);
	if (tcpsocket->waitForReadyRead(1000))
	{
		int readbytes = tcpsocket->read((unsigned char*)b, 4);
		printf("%d %d", readbytes, b[0]);
	}
	system("pause");
	return 0;
}