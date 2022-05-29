using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    private Animator airplaneAnimator;
    [SerializeField] private int airplaneAnimation;
    // Start is called before the first frame update
    void Start()
    {
        airplaneAnimator = GetComponent<Animator>();
        airplaneAnimator.SetInteger("Animation", airplaneAnimation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
