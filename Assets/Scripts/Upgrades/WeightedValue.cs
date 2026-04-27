using System;

namespace Assets.Scripts.Upgrades
{
    [Serializable]
    public struct WeightedValue<T>
    {
        public T value;
        public float weight;
    }
}
