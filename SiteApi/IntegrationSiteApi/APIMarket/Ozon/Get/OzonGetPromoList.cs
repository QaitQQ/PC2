using Newtonsoft.Json;

using StructLibCore.Marketplace;

using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace SiteApi.IntegrationSiteApi.APIMarket.Ozon.Get
{
    public class OzonGetPromoList : OzonGet
    {
        public OzonGetPromoList(APISetting aPISetting) : base(aPISetting)
        {
        }

        public List<Promo> Get()
        {
            var httpWebRequest = GetRequest(@"v1/actions");
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream())) { result = streamReader.ReadToEnd(); }
            Request request = JsonConvert.DeserializeObject<Request>(result);

            List<Promo> X = new List<Promo>();
            foreach (var item in request.Result)
            {
                X.Add(item);

            }

            return X;
        }

        public class Result : Promo
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("date_start")]
            public DateTime DateStartO { get; set; }

            [JsonProperty("date_end")]
            public DateTime DataEndO { get; set; }

            [JsonProperty("potential_products_count")]
            public int PotentialProductsCount { get; set; }

            [JsonProperty("is_participating")]
            public bool IsParticipating { get; set; }

            [JsonProperty("participating_products_count")]
            public int ParticipatingProductsCount { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("action_type")]
            public string ActionType { get; set; }

            [JsonProperty("banned_products_count")]
            public int BannedProductsCount { get; set; }

            [JsonProperty("with_targeting")]
            public bool WithTargeting { get; set; }

            [JsonProperty("discount_type")]
            public string DiscountType { get; set; }

            [JsonProperty("discount_value")]
            public int DiscountValue { get; set; }

            [JsonProperty("order_amount")]
            public int OrderAmount { get; set; }

            [JsonProperty("freeze_date")]
            public string FreezeDate { get; set; }

            [JsonProperty("is_voucher_action")]
            public bool IsVoucherAction { get; set; }
            public string Name { get=> Title; }
            public string DataStart { get => DateStartO.ToString();}
            public string DataEnd { get => DataEndO.ToString();}
        }

        public class Request
        {
            [JsonProperty("result")]
            public List<Result> Result { get; set; }
        }

    }

}


