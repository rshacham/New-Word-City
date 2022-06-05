using System;
using Avrahamy.EditorGadgets;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Managers
{
    /// <summary>
    /// Game Manager to hold all Events that can be static - MessageHub.
    /// </summary>
    public static class StaticEventsGameManager
    {
        #region Player Collision

        public static event EventHandler<bool> PlayerShouldCollide;

        public static void OnPlayerShouldCollide(object sender, bool e)
        {
            PlayerShouldCollide?.Invoke(sender, e);
        }

        #endregion

        #region PlayerInteraction

        public static event EventHandler<bool> PlayerShouldInteract;

        public static void OnPlayerShouldInteract(object sender, bool e)
        {
            PlayerShouldInteract?.Invoke(sender, e);
        }

        #endregion

        #region Particle Emit

        public static event EventHandler<Vector2> EmitParticles;

        #endregion

        public static void OnEmitParticles(object sender, Vector2 pos)
        {
            EmitParticles?.Invoke(sender, pos);
        }
    }
}