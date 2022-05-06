using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Mechanics.Cursor_Change
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField]
        private Texture2D cursorNormal;

        [SerializeField]
        private Texture2D cursorHover;

        [SerializeField]
        private InputActionReference mouseHover;

        public static void HoverInteractable()
        {
            if (_currentType == CurrentType.Normal)
            {
                ChangeCursor(_instance.cursorHover);
                _currentType = CurrentType.Hover;
            }
        }

        public static void UnHoverInteractable()
        {
            if (_currentType == CurrentType.Hover)
            {
                ChangeCursor(_instance.cursorNormal);
                _currentType = CurrentType.Normal;
            }
        }
        
        private static void ChangeCursor(Texture2D cursor)
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }

        private enum CurrentType
        {
            Normal,
            Hover
        }

        private static CurrentType _currentType = CurrentType.Normal;


        private static CursorController _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            _instance = this;
            DontDestroyOnLoad(gameObject);
            ChangeCursor(cursorNormal);
            // Cursor.lockState = CursorLockMode.Confined;
        }

        private void OnEnable()
        {
            mouseHover.action.Enable();
            mouseHover.action.started += OnMouseHover;
            mouseHover.action.performed += OnMouseHover;
            mouseHover.action.canceled += OnMouseHover;
        }

        private void OnDisable()
        {
            mouseHover.action.started -= OnMouseHover;
            mouseHover.action.performed -= OnMouseHover;
            mouseHover.action.canceled -= OnMouseHover;
        }

        public void OnMouseHover(InputAction.CallbackContext context)
        {
            Vector2 mousePos = Camera.main!.ScreenToWorldPoint(context.ReadValue<Vector2>());
            print(mousePos);
            var hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (hit && hit.collider.CompareTag("Interactable"))
            {
                HoverInteractable();
                return;
            }
            UnHoverInteractable();
        }
    }
}