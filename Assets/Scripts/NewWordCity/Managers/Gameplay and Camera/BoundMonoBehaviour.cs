using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Player Utility to control when the player should collide
    /// </summary>
    public class BoundMonoBehaviour : MonoBehaviour, IBound
    {
        private Collider2D _myCollider;

        private void Awake()
        {
            _myCollider = GetComponent<Collider2D>();
            StaticEventsGameManager.PlayerShouldCollide += ShouldBound;
        }

        /// <summary>
        /// Callback to set this objects collider to bound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="bound"></param>
        public void ShouldBound(object sender, bool bound)
        {
            _myCollider.enabled = bound;
        }
    }

    /// <summary>
    /// Interface in case other objects should call this type - UNUSED
    /// </summary>
    public interface IBound
    {
        public void ShouldBound(object sender, bool bound);
    }
}