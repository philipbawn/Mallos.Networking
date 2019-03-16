namespace Mallos.Networking
{
    public struct NetConnectionParameters
    {
        public readonly string Username;
        public readonly string Password;

        public readonly string Address;
        public readonly int TcpPort;
        public readonly int UdpPort;
        public readonly int UdpLocalPort;

        public NetConnectionParameters(string username, string password, string address, int tcpPort = 0, int udpPort = 0, int udpLocalPort = 0)
        {
            this.Username = username;
            this.Password = password;
            this.Address = address;
            this.TcpPort = tcpPort;
            this.UdpPort = udpPort;
            this.UdpLocalPort = udpLocalPort;
        }
    }
}