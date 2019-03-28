using System;

using Settlers.Toolbox.Infrastructure.Cabinet.Interfaces;

namespace Settlers.Toolbox.Infrastructure.Cabinet
{
    /// <summary>
    /// Represents a single file inside a <see cref="ICabContainer"/> as data transfer object.
    /// </summary>
    public class CabFile
    {
        public int Index { get; }

        public string Name { get; }

        public CabFile(int index, string name)
        {
            if (index < 0) throw new IndexOutOfRangeException($"{nameof(CabFile)} {nameof(index)} is negative ({index}).");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            
            Index = index;
            Name = name;
        }
    }
}