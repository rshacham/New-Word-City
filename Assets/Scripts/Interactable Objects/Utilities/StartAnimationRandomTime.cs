using Avrahamy;
using Avrahamy.Math;
using BitStrap;
using UnityEngine;
using FloatRange = Avrahamy.Math.FloatRange;

namespace Interactable_Objects
{
    /// <summary>
    /// Start the animation in random time
    /// </summary> TODO: use stateMachine instead
    public class StartAnimationRandomTime : MonoBehaviour
    {
        #region Inspector

        [SerializeField]
        [Tooltip("The trigger to move into the initial animation state")]
        private TriggerAnimationParameter startTrigger;

        [SerializeField]
        [Tooltip("Time range to start in")]
        private FloatRange range;

        #endregion

        private Animator _animator;

        #region MonoBehaviour

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            var delay = RandomUtils.Range(range);
            PureCoroutines.DelaySeconds(() => startTrigger.Set(_animator), delay);
            // DebugLog.Log(LogTag.LowPriority, $"delay: {delay}", this);
        }

        #endregion
    }
}