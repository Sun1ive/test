using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

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

       // const int Silence = 0x00E6A85C;

        static void Main(string[] args)
        {
            ProcessAccessType access = ProcessAccessType.PROCESS_QUERY_INFORMATION | ProcessAccessType.PROCESS_VM_READ | ProcessAccessType.PROCESS_VM_WRITE | ProcessAccessType.PROCESS_VM_OPERATION;

            Process process = Process.GetProcessesByName("revoexe").ToList().FirstOrDefault();
            Console.WriteLine("Process: " + process);
            if (process == null)
            {
                Console.WriteLine("Error");
                throw new Exception("Error has occured, looks like there no process with this name");
            }

            IntPtr processHandle = OpenProcess((int)access, false, process.Id);

            //SendMessage(process, 0x000C, 0, "HELLO WORLD");

            Console.WriteLine("Process pointer: " + processHandle);

            const int MaxHpAddress = 0x00E6E838;
            const int CurHpAddress = 0x00E6E834;
            const int MaxSpAddress = 0x00E6E840;
            const int CurSpAddress = 0x00E6E83C;


            int bytesRead = 0;

            byte[] bufferCurHp = new byte[4];
            byte[] bufferMaxHp = new byte[4];
            byte[] bufferCurSp = new byte[4];
            byte[] bufferMaxSp = new byte[4];

            ReadProcessMemory((int)processHandle, MaxHpAddress, bufferMaxHp, bufferMaxHp.Length, ref bytesRead);
            ReadProcessMemory((int)processHandle, CurHpAddress, bufferCurHp, bufferCurHp.Length, ref bytesRead);
            ReadProcessMemory((int)processHandle, MaxSpAddress, bufferMaxSp, bufferMaxSp.Length, ref bytesRead);
            ReadProcessMemory((int)processHandle, CurSpAddress, bufferCurSp, bufferCurSp.Length, ref bytesRead);

            int CurHpValue = 0;
            int MaxHpValue = 0;
            int MaxSpValue = 0;
            int CurSpValue = 0;


            MaxHpValue = BitConverter.ToInt32(bufferMaxHp, 0);
            CurHpValue = BitConverter.ToInt32(bufferCurHp, 0);
            CurSpValue = BitConverter.ToInt32(bufferCurSp, 0);
            MaxSpValue = BitConverter.ToInt32(bufferMaxSp, 0);

            Console.WriteLine("Cur Hp int: {0}", CurHpValue);
            Console.WriteLine("Max Hp int: {0}", MaxHpValue);
            Console.WriteLine("Cur Sp int: {0}", CurSpValue);
            Console.WriteLine("Max Sp int: {0}", MaxSpValue);

             while (CurHpValue < MaxHpValue) {
              Console.WriteLine("Current hp less than maxhp");
              Thread.Sleep(2000);
             }

            Console.ReadKey();
        }
    }
}
