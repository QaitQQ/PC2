using Network;

using System;
using System.Collections.Generic;

namespace MGSol
{
    [Serializable]
    public class BaseInfoPrice
    {
        private UserPass logPass;
        private AddressPort addressPort;
        private List<PriceIDPair> priceIDPairList;
        public UserPass LogPass
        {
            get => logPass;
            set => logPass = value;
        }
        public BaseInfoPrice()
        {
            logPass = new UserPass();
            priceIDPairList = new List<PriceIDPair>();
            addressPort = new AddressPort();
        }
        public List<PriceIDPair> PriceIDPair
        {
            get => priceIDPairList;
            set => priceIDPairList = value;
        }
        public AddressPort AddressPort
        {
            get => addressPort;
            set => addressPort = value;
        }
    }
    [Serializable]
    public class PriceIDPair
    {
        public int ID { get; set; }
        public double Price { get; set; }
    }
    [Serializable]
    public class UserPass
    {
        public string User { get; set; }
        public string Pass { get; set; }
    }
    [Serializable]
    public class AddressPort
    {
        public string Address { get; set; }
        public string Port { get; set; }
    }
    public class WrapNetClient : INetClient
    {
        private readonly string IP;
        private readonly int Port;
        private readonly string Token;
        public WrapNetClient(string iP, string port, string token)
        {
            IP = iP;
            Port = Convert.ToInt32(port);
            Token = token;
        }

        public TCPMessage Messaging(TCPMessage Data)
        {
            try
            {
                using ClientTCP Client = new(IP, Port, Token);
                return Client.Messaging(Data);
            }
            catch 
            {
                return null;
            }
        }
    }
}
