using Server.Class.IntegrationSiteApi;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForms
{
    static class Program
    {

        public static Server.Class.IntegrationSiteApi.StructSite.product_attribute[] product_attribute;
        public static StructSite.attribute_description[] attribute_description;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            attribute_description = new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").GetTable<Server.Class.IntegrationSiteApi.StructSite.attribute_description>("attribute_description");

            //      ПоискПоИмени(attribute_description);

            product_attribute = new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").GetTable<Server.Class.IntegrationSiteApi.StructSite.product_attribute>("product_attribute");

            //    УдалениеПустых(product_attribute, attribute_description);


            foreach (var item in attribute_description)
            {
                item.name = ИсправитьТекст(item.name);

                if (item.name == null)
                {
                    item.name = "null";
                }
            }
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());

        }

        public static void УдалениеПустых(Server.Class.IntegrationSiteApi.StructSite.product_attribute[] product_Attributes, StructSite.attribute_description[] attribute_description)
        {
            foreach (var item in attribute_description)
            {
                var X = product_Attributes.Where(x => x.attribute_id == item.attribute_id);

                if (X.Count() <= 1)
                {
                    new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").DeleteAttribute(Convert.ToInt32(item.attribute_id));
                    Console.WriteLine(item.attribute_id);
                    Thread.Sleep(1000);
                }
            }
        }
        public static void ПоискПоЗначению(Server.Class.IntegrationSiteApi.StructSite.product_attribute[] product_Attributes, StructSite.attribute_description[] attribute_description)
        {
            IEnumerable<IGrouping<string, StructSite.product_attribute>> R = product_Attributes.GroupBy(x => x.text);
            foreach (IGrouping<string, StructSite.product_attribute> item in R)
            {
                if (item.Count() > 2)
                {

                    List<StructSite.product_attribute> itemList = item.ToList();

                    List<IGrouping<string, StructSite.product_attribute>> GroupAttId = itemList.GroupBy(x => x.attribute_id).ToList();

                    if (GroupAttId.Count() > 2)
                    {
                        for (int i = 1; i < GroupAttId.Count(); i++)
                        {
                            Console.WriteLine((attribute_description.Where(x => x.attribute_id == GroupAttId[0].Key)).First().name);
                            var FullList = product_Attributes.Where(x => x.attribute_id == GroupAttId[i].Key);

                            foreach (var Item in FullList)
                            {
                                Console.WriteLine(Item.text);
                            }

                            Console.ReadKey();
                            Console.Clear();
                        }
                    }

                }
                else
                {
                }
            }
        }
        public static void ПоискПоИмени(StructSite.attribute_description[] attribute_Descriptions, Action<string> action)
        {

            var attribute_description_List = attribute_description.ToList();



            for (int i = 0; i < attribute_description_List.Count(); i++)
            {



                var Name = attribute_description_List[i].name.ToLower();
                var Find = attribute_description_List.Where(X => X.name.ToLower() == Name).ToList();


                if (Find.Count() > 1)
                {
                    for (int y = 1; y < Find.Count(); y++)
                    {
                        action(Find[y].attribute_id + " => " + Find[0].attribute_id);
                        new Server.Class.IntegrationSiteApi.StructSite("https://salessab.su" + "/index.php?route=api").Attribute_replacement(Convert.ToInt32(Find[y].attribute_id), Convert.ToInt32(Find[0].attribute_id));

                        attribute_description_List.Remove(Find[y]);
                    }
                    Thread.Sleep(250);
                 
                }
            }
        }
        public static string ИсправитьТекст(string Текст) 
        {

            string НовыйТекст = null;

            foreach (var item in Текст)
            {

                if (Regex.Match(item.ToString(), @"[а-яА-Я]|[a-zA-Z]").Success)
                {
                    НовыйТекст +=item;
                }

            }




            return НовыйТекст;
        
        
        }
    }
}
