using System;

namespace SaltyLibrary.Saltybet
{
    public interface IFighter : IEquatable<IFighter>
    {
        public string Name { get; }
        public int Wins { get; }
        public int Losses { get; }

    }
}