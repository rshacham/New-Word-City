using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Interactable_Objects;
using Player_Control;
using UnityEditor.IMGUI.Controls;

public class GameManager : MonoBehaviour
{
    public static GameManager _shared;
    
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


    [SerializeField] private Transform startingTransform;
    
    
    #endregion
    
    #region Private Fields
    
    private Movement _playerMovement;

    private Camera _gameCamera;

    
    #endregion
    private void Awake()
    {
        _shared = this;
        _playerMovement = FindObjectOfType<Movement>();
        _gameCamera = FindObjectOfType<Camera>(); 
    }
    
    public void ChangeCamera()
    {
        cameraConfiner.m_BoundingShape2D = cameraColliders[1];
        cameraConfiner.InvalidatePathCache();
    }

    public void ChangeFollowPlayer()
    {
        if (virtualCamera.Follow != startingTransform)
        {
            virtualCamera.m_Follow = startingTransform;
            return;
        }
        
        virtualCamera.m_Follow = _playerMovement.transform;
    }

    public IEnumerator ThrowPlayerOnWorld()
    {
        yield return new WaitForSeconds(3f);
        GameManager._shared.ChangeCamera();
        yield return new WaitForSeconds(1.5f);
        _playerMovement.TeleportPlayer(new Vector3(worldStartingPosition.x, worldStartingPosition.y + 20f, worldStartingPosition.z));
        ChangeFollowPlayer();
        _gameCamera.transform.position = worldStartingPosition;
        StartCoroutine(_playerMovement.ChangePosition(worldStartingPosition, 2f));
        // yield return new WaitForSeconds(3f);
        GameManager._shared.ChangeFollowPlayer();
        yield return new WaitForSeconds(2.5f);
        _playerMovement.FellToWorld = true; // TODO: just call interact here instead...
    }


    public IEnumerator ChangeBoolWithDelay(bool state, bool newBoolean, float delay)
    {
        yield return new WaitForSeconds(delay);
        state = newBoolean;
    }
    
}
