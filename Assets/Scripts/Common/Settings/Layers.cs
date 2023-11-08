using UnityEngine;

namespace Common.Settings
{
    public static class Layers
    {
        public static readonly int Ground = LayerMask.NameToLayer("Ground");
        public static readonly int Player = LayerMask.NameToLayer("Player");
    }
}