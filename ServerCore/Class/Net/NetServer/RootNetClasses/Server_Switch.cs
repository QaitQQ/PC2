using StructLibs;

using static NetEnum.Selector;


namespace Server.Class.Net.NetServer
{
    internal class Server_Switch : AbstractNetClass
    {
        public Server_Switch(TCP_CS_Obj Data) => this.Data = Data;
        public TCP_CS_Obj DoSwitch()
        {
            if ((FirstSelector)Data.Code[0] == FirstSelector.Аuthorization || СheckToken.Сheck(Data.Token) || (FirstSelector)Data.Code[0] == FirstSelector.Ok_code)
            {
                switch (Data.Code[0])
                {
                    case FirstSelector.Ok_code: // код ОК
                        Data.Code[0] = 0;
                        Data.Obj = null;
                        return Data;
                    case FirstSelector.Аuthorization: // авторизация
                        Data.Code[0] = 0;
                        Data = new Аuthorization(Data).Result();
                        return Data;
                    case FirstSelector.ItemNetClass: //  Работа с позициями
                        Data.Code[0] = 0;
                        Data = new Server_ItemNetClass(Data).Result();
                        return Data;
                    case FirstSelector.CrmNetClass: // CRM               
                        Data = new CrmNetClass(Data).Result();
                        Data.Code[0] = 0;
                        return Data;
                    case FirstSelector.BaseNetClass:
                        Data = new BaseNetClass(Data).Result();
                        Data.Code[0] = 0;
                        return Data;
                    default:
                        Data.Obj = "Error";
                        return Data;
                }
            }
            else { Data.Obj = "Error Token"; return Data; }
        }
    }
}
