//using Devart.Data.MySql;
//using System;
//using System.Collections.Generic;
//using System.Windows.Forms;
//namespace Pricecona
//{
//    public class SQL
//    {
//         private static MySqlCommand cmd = new MySqlCommand();
//         public MySqlConnection Connect(string[] npsdb)
//        {
//             string ServConString = $"User Id={npsdb[0]};Password={npsdb[1]};Host={npsdb[2]};Database={npsdb[3]};";
//            MySqlConnection conn = new MySqlConnection(ServConString);
//            cmd.Connection = conn;
//            try { conn.Open(); }
//            catch (Exception e)
//            { MessageBox.Show(Convert.ToString(e)); }
//             return conn;
//         }
//         public List<PriceStruct> Select(string command)
//        {
//            List<PriceStruct> vs = new List<PriceStruct>();
//            try
//            {
//                command = "SET NAMES `UTF8`;" + command;
//                cmd.CommandText = command;
//                cmd.ExecuteNonQuery();
//                MySqlDataReader reader = cmd.ExecuteReader();

//                while (reader.Read())
//                {
//                    vs.Add(new PriceStruct()
//                    {
//                        Id = Convert.ToInt32(reader[0]),
//                        Name = Convert.ToString(reader[1]),
//                        Description = Convert.ToString(reader[2]),
//                        PriceRC = Convert.ToDouble(reader[3]),
//                    });
//                }
//            }
//            catch (Exception)
//            {

//            }

//            return vs;
//        }
//        public string Update(string command) {
//            string answer = null;
//            cmd.CommandText = command;
//            cmd.ExecuteNonQuery();
//            MySqlDataReader reader = cmd.ExecuteReader();
//            while (reader.Read())
//            {
//                answer = answer + reader.ToString();
//            }
//              return answer;
//        }
//         public void CloseCon(MySqlConnection conn) { conn.Close(); }
//     }
//}
