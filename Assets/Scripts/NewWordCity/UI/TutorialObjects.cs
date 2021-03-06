using System;
using Avrahamy.Collections;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Class to create and activate tutorial sprites
    /// </summary> TODO: probably static of singleton - to allow access from movements
    [Serializable]
    public class TutorialObjects
    {
        #region Inspector

        [SerializeField]
        [Tooltip("The offset to create the sprite from the pivot of the object")]
        private Vector3 offset = new Vector3(1, 1);

        [Space(2)]
        [Header("Schemes")]
        [SerializeField]
        private TutorialScheme controller;

        [SerializeField]
        private TutorialScheme kbm;

        [Space]
        [SerializeField]
        [Tooltip("Pool to use for TutorialSprite game object")]
        private GameObjectPool pool;

        #endregion

        #region Private Fields

        private GameObject _tutorialInstance;

        #endregion

        #region Public Properties

        public enum Schemes
        {
            KBM,
            Controller
        }

        public Vector3 Offset
        {
            get => offset;
            set => offset = value;
        }

        #endregion

        #region Public Methods

        public Sprite GetForScheme(TutorialScheme.Tutorials type, Schemes scheme)
        {
            var tutorialScheme = scheme switch
            {
                Schemes.KBM => kbm,
                Schemes.Controller => controller,
                _ => throw new ArgumentOutOfRangeException(nameof(scheme), scheme, null)
            };
            return type switch
            {
                TutorialScheme.Tutorials.Interact => tutorialScheme.interact,
                TutorialScheme.Tutorials.SecondaryInteract => tutorialScheme.secondaryInteract,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        // TODO: add animation slight shake, custom images, pop in/popout, multi tutorials support
        public ref GameObject CreateTutorial(Vector3 position, TutorialScheme.Tutorials type,
            Schemes scheme)
        {
            // if (_tutorialInstance == null)
            // {
            //     _tutorialInstance = Object.Instantiate(tutorialSprite);
            // }

            _tutorialInstance = pool.BorrowGameObject();

            var tutorialScheme = scheme switch
            {
                Schemes.KBM => kbm,
                Schemes.Controller => controller,
                _ => throw new ArgumentOutOfRangeException(nameof(scheme), scheme, null)
            };
            _tutorialInstance.GetComponentInChildren<SpriteRenderer>().sprite = type switch
            {
                TutorialScheme.Tutorials.Interact => tutorialScheme.interact,
                TutorialScheme.Tutorials.SecondaryInteract => tutorialScheme.secondaryInteract,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };

            _tutorialInstance.SetActive(true);

            _tutorialInstance.transform.parent = null;
            _tutorialInstance.transform.rotation = Quaternion.identity;
            _tutorialInstance.transform.localScale = Vector3.one;
            _tutorialInstance.transform.position = position;
            return ref _tutorialInstance;
        }

        public void RemoveTutorial(GameObject toRemove)
        {
            toRemove.transform.parent = null;
            toRemove.SetActive(false);
        }

        #endregion
    }

    /// <summary>
    ///  Hold sprite for single scheme
    /// </summary>
    [Serializable]
    public struct TutorialScheme
    {
        public enum Tutorials
        {
            Interact,
            SecondaryInteract
        }

        [SerializeField]
        public Sprite interact;
        [SerializeField]
        public Sprite secondaryInteract;
    }
}