using System;
using Avrahamy.EditorGadgets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Managers
{
    public static class BoundsGameManager
    {
        public static event EventHandler<bool> PlayerShouldCollide;
        
        public static void OnPlayerShouldCollide(object sender, bool e)
        {
            PlayerShouldCollide?.Invoke(sender, e);
        }
    }
}