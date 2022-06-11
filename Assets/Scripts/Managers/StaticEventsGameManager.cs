using System;
using Avrahamy;
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

        /// <summary>
        /// Called when player collision status should change
        /// </summary>
        public static event EventHandler<bool> PlayerShouldCollide;

        public static void OnPlayerShouldCollide(object sender, bool e)
        {
            PlayerShouldCollide?.Invoke(sender, e);
        }

        #endregion

        #region PlayerInteraction

        /// <summary>
        /// Called when player interaction status should change
        /// </summary>
        public static event EventHandler<bool> PlayerShouldInteract;

        public static void OnPlayerShouldInteract(object sender, bool e)
        {
            DebugLog.Log(LogTag.LowPriority, $"Player Interact: {e}");
            PlayerShouldInteract?.Invoke(sender, e);
        }

        #endregion

        #region Particle Emit

        /// <summary>
        /// Called when particles should be emitted at location
        /// </summary>
        public static event EventHandler<Vector2> EmitParticles;

        public static void OnEmitParticles(object sender, Vector2 pos)
        {
            EmitParticles?.Invoke(sender, pos);
        }

        #endregion
    }
}