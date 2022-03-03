
using System;
using System.Collections.Generic;

namespace Server
{
    [Serializable]
    public class PriceStorage
    {
        public PriceStorage() { ReceivingData = DateTime.Now; Attributes = new List<string>(); }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public string Link { get; set; }
        public DateTime ReceivingData { get; set; }
        public bool DefaultReading { get; set; }
        public List<string> Attributes { get; set; }
        public bool PlanedRead { get; set; }
        public DateTime PlanedTime { get; set; }

    }
}

