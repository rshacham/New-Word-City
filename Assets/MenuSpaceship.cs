using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpaceship : MonoBehaviour
{
    private Animator _myAnimator;
    private MainMenu _mainMenu;

    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myAnimator.SetBool("Menu", true);
        _mainMenu = FindObjectOfType<MainMenu>();
    }

    public void MainMenuSound(int sound)
    {
        _mainMenu.PlaySound(sound);
    }
}
