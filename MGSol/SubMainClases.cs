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
    [Serializable]
    public class ShipmentOrder
    {
        public string ID { get; set; }
        public string Nomber { get; set; }
        public string Date { get; set; }
        public string DateShipment { get; set; }
        public List<string> Items { get; set; }

        public string GetItemString
        {
            get
            {
                string result = null; foreach (var item in Items)
                {
                    result = result + item + "\r\n";
                }
                return result;
            }
        }

    }
    public class SyncShipment
    {
        private DateTime LastUp;
        private List<ShipmentOrder> OrderList;

        private MainModel mainModel;

        public SyncShipment(MainModel model)
        {
            mainModel = model;
            OrderList = model.ShipmentOrders;

        }
        public bool Sync() 
        {
            try
            {
                var SaveDate = GetLestUp();
                var NewShipmentOrders = new List<ShipmentOrder>();
                if (LastUp < SaveDate)
                {
                    NewShipmentOrders = СompareShipmentOrdersWithLocal(GetShipmentOrdersFromLastUp(LastUp));

                    foreach (var item in NewShipmentOrders)
                    {
                        OrderList.Add(item);
                    }
                }
                mainModel.ShipmentOrders = OrderList;
                LastUp = DateTime.Now;
                mainModel.OptionMarketPlace.LastUpTime = LastUp;


                return true;
            }
            catch 
            {

                return false;
            }

        }


        public void SetLastUpTime(DateTime time) { LastUp = time; }

        private DateTime GetLestUp() { return  DateTime.Now; }
        private List<ShipmentOrder> GetShipmentOrdersFromLastUp(DateTime dateTime) { return new List<ShipmentOrder>(); }
        private List<ShipmentOrder> СompareShipmentOrdersWithLocal(List<ShipmentOrder> shipments) { return new List<ShipmentOrder>(); }
        public bool Save(ShipmentOrder order) { return true; }
    }
}
