using System;

namespace SaltyLibrary.Saltybet
{
    public class Fighter : IFighter
    {
        public Fighter(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public bool Equals(IFighter other)
        {
            if (other == null)
            {
                return false;
            }
            return this.Name.Equals(other.Name);
        }
    }
}
