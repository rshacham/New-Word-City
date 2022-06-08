using System.Collections;
using UnityEngine;
using Cinemachine;
using Interactable_Objects;
using Managers;
using Player_Control;
using UI;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Shared;
    
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
    

    [SerializeField] 
    [Tooltip("The world position where the player will fall after the tutorial")]
    private Vector3 worldStartingPosition;

    
    [SerializeField] 
    [Tooltip("First is the tutorial ending position." +
             "Second is the new world starting position. Third is added on awake, and is the player transform. ")]
    Transform[] transformsForCamera;


    [FormerlySerializedAs("endingScene")] [SerializeField] [Tooltip("Ending Canvas")]
    private Pokedex endingCanvas;


    #endregion
    
    #region Private Fields
    
    private Movement _playerMovement;

    private Camera _gameCamera;
    
    private CartoonHoleManager _holeManager;



    #endregion
    
    #region Public Fields

    public bool EndScenePlayed { get; set; } = false;

    public bool EndSceneIsOn { get; set; } = false;

    public Pokedex EndingCanvas
    {
        get => endingCanvas;
        set => endingCanvas = value;
    }

    #endregion
    private void Awake()
    {
        Shared = this;
        _playerMovement = FindObjectOfType<Movement>();
        _gameCamera = FindObjectOfType<Camera>();
        _holeManager = FindObjectOfType<CartoonHoleManager>();
    }
    
    public void ChangeCamera(int cameraNum)
    {
        cameraConfiner.m_BoundingShape2D = cameraColliders[cameraNum];
        cameraConfiner.InvalidatePathCache();
    }

    public void ChangeFollowPlayer(int newTransform)
    {
        virtualCamera.m_Follow = transformsForCamera[newTransform];
    }

    public IEnumerator ThrowPlayerOnWorld()
    {
        Vector3 worldPosition = transformsForCamera[1].position;
        Vector3 fallPosition = new Vector3(transformsForCamera[1].position.x,
            transformsForCamera[1].position.y + 20, transformsForCamera[1].position.z);
        yield return new WaitForSeconds(1.5f);
        _playerMovement.GetComponent<SpriteRenderer>().enabled = false;
        _playerMovement.EnableMovement = false;
        _playerMovement.TeleportPlayer(transformsForCamera[0].position);
        Debug.Log("player on trampoline");
        _holeManager.CloseCircle();
        yield return new WaitForSeconds(2.5f);
        _playerMovement.TeleportPlayer(worldPosition);
        ChangeCamera(1);
        ChangeFollowPlayer(1);
        yield return new WaitForSeconds(4f);
        _playerMovement.TeleportPlayer(fallPosition);
        _playerMovement.GetComponent<SpriteRenderer>().enabled = true;
        StartCoroutine(_playerMovement.ChangePosition(worldPosition, 1f, false));
        yield return new WaitForSeconds(1.5f);
        ChangeFollowPlayer(2);
        // _gameCamera.transform.position = worldStartingPosition;
        // StartCoroutine(_playerMovement.ChangePosition(worldStartingPosition, 2f));
        // // yield return new WaitForSeconds(3f);
        // GameManager._shared.ChangeFollowPlayer();
        // yield return new WaitForSeconds(2.5f);
        _playerMovement.FellToWorld = true; // TODO: just call interact here instead...
        var trampolina = FindObjectOfType<Trampolina>();
        trampolina.UseOnEnd = true;
        trampolina.Interact();
    }


    public void EndScene()
    {
        EndSceneIsOn = true;
        EndScenePlayed = true;
        _holeManager.EndingCircleMinMax();
        _holeManager.CloseCircle();
        _playerMovement.EnableMovement = false;
        // CanvasManager.ActiveCanvas.gameObject.SetActive(false);
        // CanvasManager.ActiveCanvas = endingCanvas;
        // CanvasManager.ActiveCanvas.gameObject.SetActive(true);
        //
        endingCanvas.TutorialHolder.TutorialContinue();
    }


    public IEnumerator ChangeBoolWithDelay(bool state, bool newBoolean, float delay)
    {
        yield return new WaitForSeconds(delay);
        state = newBoolean;
    }
    
}
