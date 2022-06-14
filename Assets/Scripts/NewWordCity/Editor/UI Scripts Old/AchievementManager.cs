using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewWordCity.Editor.UI_Scripts_Old
{
    public class AchievementManager : MonoBehaviour
    {
        #region Inspector

        /// <summary>
        /// Delay in seconds until the animation starts
        /// </summary>
        [SerializeField]
        private float animationStartDelay;

        /// <summary>
        /// Speed of animation, the higher this is the faster the animation
        /// </summary>
        [SerializeField]
        private float animationSpeed;

        /// <summary>
        /// Speed of fading in, the higher this is the faster the animation
        /// </summary>
        [SerializeField]
        private float fadingSpeed;

        [SerializeField]
        private TMP_Text meaningText;

        [SerializeField]
        private TMP_Text constantText;

        [SerializeField]
        private Color textColor = Color.black;

        #endregion

        #region Private Properties

        /// <summary>
        /// Current interpolation value
        /// </summary>
        private float t = 0;

        /// <summary>
        /// Starting scale of the achievement gameobject
        /// </summary>
        private Vector3 statringScale;

        /// <summary>
        /// Starting position of the achievement gameobject
        /// </summary>
        private Vector3 startingPosition;

        /// <summary>
        /// Script of the Question Marks Maker
        /// </summary>
        private QuestionMarksMaker marksManager;

        /// <summary>
        /// Animation will start only if this is true
        /// </summary>
        private bool startAnimation = false;

        /// <summary>
        /// Transform of the achievement gameobject
        /// </summary>
        private RectTransform myTransform;

        /// <summary>
        /// Transform of the question mark related to the achievement gameobject
        /// </summary>
        private RectTransform otherTransform;

        /// <summary>
        /// Image of the achievement UI object
        /// </summary>
        private Image myImage;

        /// <summary>
        /// Current transparency of the image and text of the achievement
        /// </summary>
        private float transparency = 0;
        private Image otherImage;

        #endregion

        public int Index { get; set; }

        #region Private Methods

        private void Start()
        {
            GameObject marksHolder = GameObject.Find("QuestionMarksHolder");
            marksManager = marksHolder.GetComponent<QuestionMarksMaker>();
            myTransform = gameObject.GetComponent<RectTransform>();
            otherImage = marksManager.questionMarksList[Index].GetComponentInChildren<Image>();
            otherTransform = otherImage.GetComponent<RectTransform>();
            // otherTransform = marksManager.questionMarksList[Index].GetComponent<RectTransform>();
            statringScale = myTransform.sizeDelta;
            startingPosition = myTransform.position;
            myImage = gameObject.GetComponent<Image>();
            // myText = GetComponentInChildren<TMP_Text>();
            // otherImage.enabled = false;
        }

        private void OnEnable()
        {
            StartCoroutine(AnimationDelay());
        }

        private void Update()
        {
            // Fading:
            if (t < 1f && !startAnimation)
            {
                t += Time.deltaTime * fadingSpeed;
                transparency = Mathf.Lerp(0f, 255f, t);
                myImage.color = new Color32(255, 255, 255, (byte) transparency);
                meaningText.color = new Color(textColor.r, textColor.g, textColor.b, transparency);
                constantText.color = new Color(textColor.r, textColor.g, textColor.b, transparency);
            }

            // Animation to the left bottom of the screen
            if (t < 1f && startAnimation)
            {
                t += Time.deltaTime * animationSpeed;
                myTransform.sizeDelta = Vector3.Lerp(statringScale, otherTransform.sizeDelta, t);
                myTransform.position = Vector3.Lerp(startingPosition, otherTransform.position, t);
            }

            if (t >= 1 && startAnimation)
            {
                // otherImage.enabled = true;
                otherImage.sprite = myImage.sprite;
                // marksManager.questionMarksList[Index].GetComponentInChildren<TMP_Text>().text = meaningText.text;
                Destroy(gameObject);
            }
        }

        IEnumerator AnimationDelay()
        {
            yield return new WaitForSeconds(animationStartDelay);

            meaningText.enabled = false;
            constantText.enabled = false;
            startAnimation = true;
            t = 0f;
        }

        #endregion
    }
}