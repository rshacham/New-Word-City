using System;
using Avrahamy;
using Managers;
using Managers.UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pokedex : MonoBehaviour
{
    [SerializeField]
    private MeaningCanvasHolder[] holders;

    [SerializeField] private float rotatingSpeed;
    [SerializeField] private float targetAngle;
    private bool isOpening = false; 
    private bool isOpen = false;

    public bool IsOpen
    {
        get => isOpen;
        set => isOpen = value;
    }
    private float angle = 0;

    public float Angle
    {
        get => angle;
        set => angle = value;
    }
    private Vector3 zAxis = new Vector3(0, 0, 1);
    private RectTransform pokedexTransform;

    [SerializeField]
    private RectTransform pivot;

    // Start is called before the first frame update
    void Start()
    {
        pokedexTransform = GetComponent<RectTransform>();
        WordsGameManager.OnMeaningFound += MeaningFound;
        CanvasManager._canvasManager.ActiveCanvas = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (angle > targetAngle && !isOpening)
        {
            pokedexTransform.RotateAround(pivot.transform.position, zAxis, Time.deltaTime * rotatingSpeed);
            angle -= rotatingSpeed * Time.deltaTime;
        }

        if (angle < 0 && isOpening)
        {
            pokedexTransform.RotateAround(pivot.transform.position, zAxis, Time.deltaTime * -rotatingSpeed);
            angle += rotatingSpeed * Time.deltaTime;
        }


    }

    public void OpenClose()
    {
        isOpening = !isOpening;
    }

    public void MeaningFound(object sender, MeaningDescriptor e)
    {
        // TODO: getting meaning index:
        var index = WordsGameManager.Current.Meanings.IndexOf(e);
        DebugLog.Log($"{e} at index {index}");
    }
}