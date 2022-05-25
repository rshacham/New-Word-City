using System;
using UnityEngine;

namespace Managers
{
    public class BoundMonoBehaviour : MonoBehaviour, IBound
    {
        private Collider2D _myCollider;

        private void Awake()
        {
            _myCollider = GetComponent<Collider2D>();
            BoundsGameManager.PlayerShouldCollide += ShouldBound;
        }

        public void ShouldBound(object sender, bool bound)
        {
            _myCollider.enabled = bound;
        }
    }
}