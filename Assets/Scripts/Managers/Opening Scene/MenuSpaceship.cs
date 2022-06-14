using System;
using System.Collections;
using System.Collections.Generic;
using Avrahamy;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSpaceship : MonoBehaviour
{
    private Animator _myAnimator;
    private MainMenu _mainMenu;
    private bool _next;

    private void Start()
    {
        _myAnimator = GetComponent<Animator>();
        _myAnimator.SetBool("Menu", true);
        _mainMenu = FindObjectOfType<MainMenu>();
        StartCoroutine(LoadScene(1));
    }

    public void MainMenuSound(int sound)
    {
        _mainMenu.PlaySound(sound);
    }

    public void GoNext()
    {
        _next = true;
    }

    private IEnumerator LoadScene(int scene)
    {
        yield return null;

        //Begin to load the Scene you specify
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
        //Don't let the Scene activate until you allow it to
        asyncOperation.allowSceneActivation = false;
        DebugLog.Log(LogTag.Default, "Start async load", this);
        //When the load is still in progress, output the Text and progress bar
        while (!asyncOperation.isDone)
        {
            //Output the current progress
            // m_Text.text = "Loading progress: " + (asyncOperation.progress * 100) + "%";

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                // DebugLog.Log(LogTag.Default, "Ready to load", this);
                //Change the Text to show the Scene is ready
                // m_Text.text = "Press the space bar to continue";
                //Wait to you press the space key to activate the Scene
                if (_next)
                {
                    //Activate the Scene
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}