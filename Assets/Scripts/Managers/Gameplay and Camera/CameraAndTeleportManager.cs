using System.Collections;
using Avrahamy;
using Cinemachine;
using Interactable_Objects;
using Player_Control;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class CameraAndTeleportManager : MonoBehaviour
    {
        public static CameraAndTeleportManager Shared;

        #region Inspector

        [SerializeField]
        [Tooltip("Camera Confiner")]
        private CinemachineConfiner cameraConfiner;


        [SerializeField]
        [Tooltip("Camera Virtual Camera")]
        private CinemachineVirtualCamera virtualCamera;


        [SerializeField]
        [Tooltip("First collider is the tutorial collider, second one is the world collider")]
        private PolygonCollider2D[] cameraColliders;


        // TODO: struct or another solution.
        [SerializeField]
        [Tooltip("First is the tutorial ending position." +
                 "Second is the new world starting position. Third is added on awake, and is the player transform. ")]
        private Transform[] transformsForCamera;


        [FormerlySerializedAs("endingScene")]
        [SerializeField]
        [Tooltip("Ending Canvas")]
        private Pokedex endingCanvas;

        #endregion

        #region Private Fields

        private Movement _playerMovement;

        private CartoonHoleManager _holeManager;
        
        // TODO: use AnimatorParameter
        private static readonly int Dance = Animator.StringToHash("Dance");

        #endregion

        #region Public Fields

        public bool EndScenePlayed { get; set; }

        public bool EndSceneIsOn { get; set; }

        public Pokedex EndingCanvas
        {
            get => endingCanvas;
            set => endingCanvas = value;
        }

        #endregion

        #region MonoBehaviour

        private void Awake()
        {
            if (Shared != null)
            {
                Destroy(gameObject);
                return;
            }

            Shared = this;
            // TODO:  _playerMovement = Tutorial.PlayerMovement;
            _playerMovement =  FindObjectOfType<Movement>();
            // TODO: make CartoonHoleManager singleton instead
            _holeManager = FindObjectOfType<CartoonHoleManager>();
            FindObjectOfType<Camera>(); // TODO: ????
        }

        #endregion

        #region Public Methods

        public void ChangeCamera(int cameraNum)
        {
            cameraConfiner.m_BoundingShape2D = cameraColliders[cameraNum];
            cameraConfiner.InvalidatePathCache();
        }

        public void ChangeFollowPlayer(int newTransform)
        {
            virtualCamera.m_Follow = transformsForCamera[newTransform];
        }

        public void EndScene()
        {
            _holeManager.ChangeMinMax(2, 30);
            _holeManager.CloseCircle();
            EndSceneIsOn = true;
            EndScenePlayed = true;
            _playerMovement.EnableMovement = false;
            _playerMovement.DesiredVelocity = Vector2.zero;
            _playerMovement.GetComponent<Animator>().SetBool(Dance, true);
            // CanvasManager.ActiveCanvas.gameObject.SetActive(false);
            // CanvasManager.ActiveCanvas = endingCanvas;
            // CanvasManager.ActiveCanvas.gameObject.SetActive(true);
            //
            Pokedex.TutorialHolder.TutorialContinue();
            StartCoroutine(ContinueAfterEndingScene());
        }

        #endregion

        #region Coroutines

        public IEnumerator ThrowPlayerOnWorld()
        {
            Vector3 worldPosition = transformsForCamera[1].position;
            Vector3 fallPosition = new Vector3(transformsForCamera[1].position.x,
                transformsForCamera[1].position.y + 20,
                transformsForCamera[1].position.z);
            yield return new WaitForSeconds(1.5f);
            _playerMovement.GetComponent<SpriteRenderer>().enabled = false;
            _playerMovement.EnableMovement = false;
            _playerMovement.TeleportPlayer(transformsForCamera[0].position);
            _holeManager.ChangeDuration(5f);
            _holeManager.CloseCircle();
            yield return new WaitForSeconds(2.5f);
            _playerMovement.FellToWorld = true; // TODO: just call interact here instead...
            var trampoline = FindObjectOfType<TrampolineInteractable>();
            trampoline.UseOnEnd = true;
            trampoline.Interact();
            _playerMovement.TeleportPlayer(worldPosition);
            if (!CanvasManager.ActiveCanvas.IsOpen)
            {
                CanvasManager.ActiveCanvas.OpenClose();
            }

            ChangeCamera(1);
            ChangeFollowPlayer(1);
            yield return new WaitForSeconds(6f);
            _playerMovement.TeleportPlayer(fallPosition);
            _playerMovement.GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(_playerMovement.ChangePosition(worldPosition, 1f));
            yield return new WaitForSeconds(1.5f);
            ChangeFollowPlayer(2);
            // _gameCamera.transform.position = worldStartingPosition;
            // StartCoroutine(_playerMovement.ChangePosition(worldStartingPosition, 2f));
            // // yield return new WaitForSeconds(3f);
            // GameManager._shared.ChangeFollowPlayer();
            // yield return new WaitForSeconds(2.5f);
        }


        private IEnumerator ContinueAfterEndingScene()
        {
            while (Tutorial.CurrentTutorial != Tutorial.Instance.TutorialsTexts.Length - 1)
            {
                DebugLog.Log(Tutorial.CurrentTutorial);
                DebugLog.Log(Tutorial.Instance.TutorialsTexts.Length);
                yield return new WaitForSeconds(0.2f);
            }

            _playerMovement.GetComponent<Animator>().SetBool(Dance, false);
            EndSceneIsOn = false;
            _holeManager.ChangeHole = true;
        }


        public IEnumerator ChangeBoolWithDelay(bool state, bool newBoolean, float delay)
        {
            yield return new WaitForSeconds(delay);
            state = newBoolean;
        }

        #endregion
    }
}