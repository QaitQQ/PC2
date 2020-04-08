using StructLibs;

using System;
using System.Collections.Generic;
using System.Linq;

using static NetEnum.Selector.SecondSelector;
using static Object_Description.Access_Struct;

namespace Server.Class.Net.NetServer
{
    internal class Аuthorization : AbstractNetClass
    {
        private string Token;
        public Аuthorization(TCP_CS_Obj Data)
        {
            this.Data = Data;
            if (Program.Cash.UserName == null) { Program.Cash.UserName = new Class.Query.NameCash().GetCashUserName(); }

            switch (this.Data.Code[1])
            {
                case EnumАuthorization.UserNames:
                    this.Data.Obj = Program.Cash.UserName;
                    break;
                case EnumАuthorization.GetToken:
                    this.Data.Obj = GetToken((string[])Data.Obj);
                    Program.Cash.Tokens.Add(Token);
                    break;
                default:
                    break;
            }
        }

        private string GetToken(string[] NamePass)
        {
            string Name = NamePass[0];
            string Pass = NamePass[1];
            string QweryResult = null;

            if (Program.Cash.UserName.Contains(Name))
            {
                using ApplicationContext db = new ApplicationContext();
                List<string> Qwery = (from User in db.User where User.Name == Name select User.Pass).ToList();
                QweryResult = Qwery[0];
            }

            if (QweryResult == Pass)
            {
                Random rnd = new Random();
                Token = null;

                for (int i = 0; i < 12; i++)
                {
                    int value = rnd.Next(50, 114);
                    Token += (char)value;
                }
            }

            return Token;
        }
    }
    public static class СheckToken
    {
        public static bool Сheck(string Token)
        {
            if (Token == null)
            {
                return false;
            }
            else
            {
                return Program.Cash.Tokens.Contains(Token);
            }
        }
    }


}
