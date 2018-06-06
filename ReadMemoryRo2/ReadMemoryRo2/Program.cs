using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace ReadMemoryRo2
{
    class Program
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
        [Flags]
        public enum ProcessAccessType
        {
            PROCESS_TERMINATE = (0x0001),
            PROCESS_CREATE_THREAD = (0x0002),
            PROCESS_SET_SESSIONID = (0x0004),
            PROCESS_VM_OPERATION = (0x0008),
            PROCESS_VM_READ = (0x0010),
            PROCESS_VM_WRITE = (0x0020),
            PROCESS_DUP_HANDLE = (0x0040),
            PROCESS_CREATE_PROCESS = (0x0080),
            PROCESS_SET_QUOTA = (0x0100),
            PROCESS_SET_INFORMATION = (0x0200),
            PROCESS_QUERY_INFORMATION = (0x0400),
            PROCESS_QUERY_LIMITED_INFORMATION = (0x1000)
        }



        //const int PROCESS_WM_READ = 0x0010;
        //const int PROCESS_VM_OPERATION = 0x0008;


        static void Main(string[] args)
        {
            ProcessAccessType access = ProcessAccessType.PROCESS_QUERY_INFORMATION | ProcessAccessType.PROCESS_VM_READ | ProcessAccessType.PROCESS_VM_WRITE | ProcessAccessType.PROCESS_VM_OPERATION;
            Process process = Process.GetProcessesByName("revoexe").ToList().FirstOrDefault();
            Console.WriteLine(process);
            IntPtr processHandle = OpenProcess((int)access, true, process.Id);
            Console.WriteLine(processHandle);

            int bytesRead = 0;
            byte[] buffer = new byte[4];

            //ReadProcessMemory((int)processHandle, 0x00E6E834, buffer, buffer.Length, ref bytesRead);
            ReadProcessMemory((int)processHandle, 0xE6E834, buffer, buffer.Length, ref bytesRead);

            Console.WriteLine(bytesRead.ToString() + " bytes");
            //foreach (var value in buffer) {
            //   Console.WriteLine(value, value.GetType().Name);
            //}
            Console.WriteLine(System.Text.Encoding.ASCII.GetString(buffer));
            Console.WriteLine(System.Text.Encoding.BigEndianUnicode.GetString(buffer));
            Console.WriteLine(System.Text.Encoding.UTF32.GetString(buffer));
            Console.WriteLine(System.Text.Encoding.UTF7.GetString(buffer));
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(buffer));
            Console.ReadLine();
        }
    }
}
