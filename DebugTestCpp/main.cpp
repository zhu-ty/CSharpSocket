#include<iostream>
using namespace std;
#using "../CSharpSocket/bin/Debug/CSharpSocket.dll"



int main()
{
	using namespace CSharpSocket;
	SKTcpSocket ^tcpsocket = gcnew SKTcpSocket();
	//tcpsocket->debug_test("Hello world!");
	//unsigned char *a;
	//cli::array<unsigned char>^ inData = gcnew cli::array<unsigned char>(10);
	////inData->Add('1');
	//inData[0] = '1';
	////inData[0] = '1';
	////a = (unsigned char *)(void *)System::Runtime::InteropServices::Marshal
	//auto aa = tcpsocket->send_receive(inData);
	////tcpsocket->test((unsigned char*)"Hi man",sizeof("Hi man"));
	//tcpsocket->connected;

	char p[] = "10.8.5.188";
	char b[100];
	memset(b, 0, sizeof(b));
	int send = 1;
	tcpsocket->abort();
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