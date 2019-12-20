using System.Net.NetworkInformation;

namespace DHCPClient_TZ
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.GetIPProperties().DhcpServerAddresses.Count > 0)
                {
                    DHCP.GetOption(nic.Id, 100);
                    DHCP.GetOption(nic.Id, 101);
                }
            }
        }
    }
}
