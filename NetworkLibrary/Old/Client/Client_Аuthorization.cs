//using System.Collections.Generic;

//using static NetEnum.Selector;

//namespace Client.Class.Net
//{
//    public class Аuthorization
//    {
//        private readonly FirstSelector _FirstSelector;

//        public Аuthorization() => _FirstSelector = FirstSelector.Аuthorization;
//        public List<string> GetUsersList() => (List<string>)new TCP_Client_GetObj(_FirstSelector).Get(SecondSelector.EnumАuthorization.UserNames);

//        public bool SetToken(string[] NamePass)
//        {
//            string Token = null;

//            try

//            {
//                var D = new TCP_Client(new object[] { _FirstSelector, SecondSelector.EnumАuthorization.GetToken }, Token: Data.Token, SendObj: NamePass).Data.Obj;

//                if (D != null)
//                {
//                    Token = D.ToString();
//                }
//            }
//            catch { }

//            if (Token != null)
//            {
//                Data.Token = Token;
//                return true;
//            }
//            return false;

//        }

//        public bool ConnectStatus()
//        {
//            if ((FirstSelector)new TCP_Client(new object[] { FirstSelector.Ok_code }).Data.Code[0] == FirstSelector.Ok_code)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }
//    }
//}
