namespace Mallos.Networking
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public readonly struct NetConnectionParameters
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

        public NetConnectionParameters(SerializationInfo info, StreamingContext context)
        {
            this.Username = info.GetString("Username");
            this.Password = string.Empty;

            this.Address = info.GetString("Address");
            this.TcpPort = info.GetInt32("TcpPort");
            this.UdpPort = info.GetInt32("UdpPort");
            this.UdpLocalPort = info.GetInt32("UdpLocalPort");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Username", Username);

            info.AddValue("Address", Address);
            info.AddValue("TcpPort", TcpPort);
            info.AddValue("UdpPort", UdpPort);
            info.AddValue("UdpLocalPort", UdpLocalPort);
        }
    }
}