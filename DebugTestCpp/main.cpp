#include<iostream>
using namespace std;
#using "../CSharpSocket/bin/Debug/CSharpSocket.dll"

int main()
{
	using namespace CSharpSocket;
	SKTcpSocket ^tcpsocket;
	tcpsocket->debug_test("Hello world!");
	unsigned char *a;
	cli::array<unsigned char>^ inData = gcnew cli::array<unsigned char>(10);
	//inData->Add('1');
	inData[0] = '1';
	//inData[0] = '1';
	//a = (unsigned char *)(void *)System::Runtime::InteropServices::Marshal
	auto aa = tcpsocket->send_receive(inData);
	system("pause");
	return 0;
}