using System;
using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

public class DropFromSink : EventInteractable
{
    private Animator myAnimator;
    private AudioSource audioSource;
    private bool interactionActive = false;
    private void Start()
    {
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        print(interactionActive);
    }

    protected override void ScriptInteract()
    {
        if (!interactionActive)
        {
            interactionActive = true;
            myAnimator.SetTrigger("Drop");
        }
        
    }
    
    public void interactionOff()
    {
        interactionActive = false;
    }

    public void PlayDropSound()
    {
        audioSource.PlayOneShot(audioSource.clip);
    }

}
