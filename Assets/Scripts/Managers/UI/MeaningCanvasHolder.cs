using Avrahamy;
using Interactable_Objects;
using UnityEngine;

namespace Managers.UI
{
    public class MeaningCanvasHolder : MonoBehaviour
    {
        public void FoundMeaning(MeaningDescriptor sender, InteractableObject e)
        {
            DebugLog.Log($"<color=magenta>Meaning Found: </color> {sender}", e);
        }
    }
}