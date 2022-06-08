using Avrahamy;
using Avrahamy.Math;
using BitStrap;
using UnityEngine;
using FloatRange = Avrahamy.Math.FloatRange;

public class StartAnimationRandomTime : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private TriggerAnimationParameter startTrigger;

    [SerializeField]
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