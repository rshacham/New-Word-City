using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

public class HonkInteraction : EventInteractable
{
    #region Inspector

    [SerializeField] private AudioSource firstSound;
    [SerializeField] private AudioSource secondSound;
    
    #endregion
    
    
    #region Private Fields

    private static int counter;
    
    #endregion
    protected override void ScriptInteract()
    {
        if (firstSound.isPlaying && secondSound.isPlaying)
        {
            return;
        }
        
        switch (++counter % 2)
        {
            case 0:
                firstSound.PlayOneShot(firstSound.clip);
                return;
            case 1:
                secondSound.PlayOneShot(secondSound.clip);
                return;
        }
    }
}
