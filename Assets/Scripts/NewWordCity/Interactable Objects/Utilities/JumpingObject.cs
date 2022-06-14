using UnityEngine;

namespace Interactable_Objects.Utilities
{
    public class JumpingObject : MonoBehaviour
    {

        private DropFromTreeInteractable _dropTreeInteractable;
        // Start is called before the first frame update
        void Start()
        {
            _dropTreeInteractable = GetComponentInParent<DropFromTreeInteractable>();
        }

        // Update is called once per frame
        public void InteractionEnd()
        {
            _dropTreeInteractable.EndJump();
        }
    }
}
