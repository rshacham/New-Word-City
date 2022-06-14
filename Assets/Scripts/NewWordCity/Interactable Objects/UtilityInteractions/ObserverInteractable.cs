using Avrahamy;
using Interactable_Objects.Utilities;
using UnityEngine;

namespace Interactable_Objects
{
    // TODO: Remove: replaced by PairedAnimationInteractable
    class ObserverInteractable : EventInteractable
    {
        [Space]
        [Header("Observer")]
        [SerializeField]
        private EventFromScriptTest.RegisterToEvents events;

        [Space]
        [SerializeField]
        private bool unregisterWhenTargetHit = true;

        [SerializeField]
        private int target = 1;

        private int _received;

        public int Target
        {
            get => target;
            set => target = value;
        }

        public int Received => _received;

        protected override void Awake()
        {
            base.Awake();
            events.Register(CallbackEvent);
        }

        private void OnDestroy()
        {
            events.UnRegister(CallbackEvent);
        }

        private void CallbackEvent(object sender, InteractableObject interactableObject)
        {
            _received++;
            DebugLog.Log($"<color=red>Callback received</color> {Received}", this);
            if (_received < target)
            {
                return;
            }

            CanInteract = true;
            if (unregisterWhenTargetHit)
            {
                events.UnRegister(CallbackEvent);
            }
        }

        protected override void ScriptInteract()
        {
            base.ScriptInteract();
            DebugLog.Log(_received, this);
        }
    }
}