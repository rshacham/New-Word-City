using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void GoNext()
    {
        SceneManager.LoadScene(0);
    }
}
