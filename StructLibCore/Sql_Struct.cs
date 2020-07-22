using System;

namespace Pricecona
{
    public struct Sqlstruct : IEquatable<PriceStruct>
    {
        private readonly int Id;
        private readonly string Name;
        private readonly double Price;
        private readonly string Description;
        private readonly DateTime DateСhange;
        public Sqlstruct(int Id, string Name, double Price, string Description, DateTime DateСhange)
        {
            this.Id = Id;
            this.Name = Name;
            this.Price = Price;
            this.Description = Description;
            this.DateСhange = DateСhange;
        }
        public string GetUpdateSql()
        {
            string Fin = $"UPDATE `oc_product` SET `base_price` = '{Price.ToString().Replace(",", ".")}' WHERE `oc_product`.`product_id` = {Id.ToString()};\n";
            return Fin;
        }
        public bool Equals(PriceStruct other)
        {
            if (Name == null)
            {
                return false;
            }
            else
            {
                return Name.Equals(other.Name);
            }
        }
        public override int GetHashCode() => Name.GetHashCode();
    }

}