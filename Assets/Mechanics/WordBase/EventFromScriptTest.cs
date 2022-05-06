using System;
using System.Linq;
using Mechanics.Object_Interactions.InteractionScripts;
using UnityEngine;

namespace Mechanics.WordBase
{
    /// <summary>
    /// Activate scripts for events from prefab - doesnt require the object to be in the scene
    /// </summary>
    public class EventFromScriptTest : MonoBehaviour
    {
        [Serializable]
        public struct RegisterToEvents
        {
            //TODO: add possible observer events here as some sort of factory class
            [SerializeField]
            private bool informObjects;

            public void Register(EventHandler<InteractableObject> e)
            {
                if (informObjects)
                {
                    RegisterInformObjects(e);
                }
            }
            public void UnRegister(EventHandler<InteractableObject> e)
            {
                if (informObjects)
                {
                    UnRegisterInformObjects(e);
                }
            }
        }
        #region Static methods For Events

        // TODO: create this option for different times somehow?
        private static event EventHandler<InteractableObject> InformObjects;

        public static void RegisterInformObjects(EventHandler<InteractableObject> e)
        {
            InformObjects += e;
        }
        public static void UnRegisterInformObjects(EventHandler<InteractableObject> e)
        {
            InformObjects -= e;
        }
        public static void OnInformObjects(InteractableObject e)
        {
            InformObjects?.Invoke(null, e);
        }

        /// <summary>
        /// Debug: display all completed words.
        /// </summary>
        public static void DisplayAllCompleted()
        {
            var words = WordsGameManager.Completed.Aggregate("", (current, word) => current + $"{word}, ");
            Debug.Log(words);
        }

        #endregion

        private EventFromScriptTest _instance;

        // public EventFromScriptTest Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            Destroy(gameObject);
        }
    }
}