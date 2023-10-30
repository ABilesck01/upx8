using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using upx.ui;

namespace upx.Game
{
    public class EndGameView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtTittle;
        [SerializeField] private TextMeshProUGUI txtScore;
        [SerializeField] private Button btnContinue;
        [SerializeField] private Button btnRetry;
        [SerializeField] private Toggle[] stars;
        [Space]
        [SerializeField] private Animator animator;

        private void Awake()
        {
            btnContinue.onClick.AddListener(BtnContinueClick);
            btnRetry.onClick.AddListener(BtnRetryClick);
        }

        public void ShowEndScreen(bool reachedDestination, bool isOnDesiredSpeed)
        {
            bool win = reachedDestination && isOnDesiredSpeed; //asure both conditions to win
            animator.SetBool("isOpen", true);
            btnRetry.gameObject.SetActive(!win);
            txtTittle.text = "Level 1";
            StartCoroutine(WriteIn("15200", txtScore));
        }

        private void BtnRetryClick()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void BtnContinueClick()
        {
            SceneManager.LoadScene(MainMenuController.SceneName);
            SceneManager.LoadScene(LevelSelectionController.sceneName, LoadSceneMode.Additive);
        }

        private IEnumerator WriteIn(string text, TextMeshProUGUI textMeshProUGUI)
        {
            textMeshProUGUI.text = "";
            yield return new WaitForSeconds(0.5f);

            foreach (char letter in text.ToCharArray())
            {
                textMeshProUGUI.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            StartCoroutine(EnableStars());
        }

        private IEnumerator EnableStars()
        {
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].isOn = true;
                yield return new WaitForSeconds(0.45f);
            }
        }
    }
}
