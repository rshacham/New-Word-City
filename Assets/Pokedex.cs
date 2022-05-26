using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pokedex : MonoBehaviour
{

    [SerializeField] private float rotatingSpeed;
    private bool isOpen = false;
    private float angle = 0;
    private Vector3 zAxis = new Vector3(0, 0, 1);
    private RectTransform pokedexTransform;
    
    

    [SerializeField] private RectTransform pivot;
    // Start is called before the first frame update
    void Start()
    {
        pokedexTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        CanvasManager._canvasManager.ActiveCanvas = gameObject.GetComponent<Pokedex>();
    }

    // Update is called once per frame
    void Update()
    {
        if (angle > -180f && !isOpen)
        {
            pokedexTransform.RotateAround(pivot.transform.position, zAxis, Time.deltaTime * rotatingSpeed);
            angle -= rotatingSpeed * Time.deltaTime;
        }

        if (angle < 0 && isOpen)
        {
            pokedexTransform.RotateAround(pivot.transform.position, zAxis, Time.deltaTime * -rotatingSpeed);
            angle += rotatingSpeed * Time.deltaTime;
        }
    }

    public void OpenClose()
    {
        isOpen = !isOpen;
    }
}
