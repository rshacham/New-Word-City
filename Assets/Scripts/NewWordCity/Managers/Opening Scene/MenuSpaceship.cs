using System.Collections;
using Avrahamy;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    // TODO: merge all opening scene scripts
    public class MenuSpaceship : MonoBehaviour
    {
        private Animator _myAnimator;
        private OpeningSceneMenu _openingSceneMenu;
        private bool _next;

        // TODO: use AnimatorParameter
        private static readonly int Menu = Animator.StringToHash("Menu");

        #region MonoBehaviour

        private void Start()
        {
            _myAnimator = GetComponent<Animator>();
            _myAnimator.SetBool(Menu, true);
            _openingSceneMenu = FindObjectOfType<OpeningSceneMenu>();
            StartCoroutine(LoadScene(1));
        }

        #endregion

        #region Public Mehtods

        public void MainMenuSound(int sound)
        {
            _openingSceneMenu.PlaySound(sound);
        }

        public void GoNext()
        {
            _next = true;
        }

        #endregion

        #region Coroutines

        private IEnumerator LoadScene(int scene)
        {
            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scene);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            DebugLog.Log(LogTag.Default, "Start async load", this);

            while (!asyncOperation.isDone)
            {

                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    // DebugLog.Log(LogTag.Default, "Ready to load", this);
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

        #endregion
    }
}