using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using upx.level;

namespace upx.ui
{
    public class LevelSelectionController : MonoBehaviour
    {
        [System.Serializable]
        public struct LevelView
        {
            public TextMeshProUGUI txtCount;
            public TextMeshProUGUI txtName;
            public Image levelImage;
        }

        public static string sceneName = "levelSelection";

        [SerializeField] private Button btnBack;
        [SerializeField] private Button btnPreviousLevel;
        [SerializeField] private Button btnNextLevel;
        [SerializeField] private LevelView levelView;
        [Space]
        [SerializeField] private Button btnPlay;
        [Space]
        [SerializeField] private LevelData[] levels;

        private int levelIndex = 0;
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();

            btnBack.onClick.AddListener(BtnBackClick);
            btnPreviousLevel.onClick.AddListener(BtnPreviousClick);
            btnNextLevel.onClick.AddListener(BtnNextClick);
            btnPlay.onClick.AddListener(btnPlayClick);
        }

        private void Start()
        {
            animator.SetBool("open", true);
            CheckButtons();
            SetLevelData();
        }

        private void BtnBackClick()
        {
            animator.SetBool("open", false);
            Invoke(nameof(UnloadScene), 1f);
        }

        private void BtnPreviousClick()
        {
            levelIndex--;
            if (levelIndex < 0)
            {
                levelIndex = 0;
                return;
            }
            CheckButtons();
            SetLevelData();
        }

        private void BtnNextClick()
        {
            levelIndex++;
            if(levelIndex >= levels.Length)
            {
                levelIndex = levels.Length - 1;
                return;
            }
            CheckButtons();
            SetLevelData();
        }

        private void CheckButtons()
        {
            if(levelIndex == 0)
            {
                btnPreviousLevel.gameObject.SetActive(false);
                btnNextLevel.gameObject.SetActive(true);
            }
            else if(levelIndex == levels.Length - 1)
            {
                btnPreviousLevel.gameObject.SetActive(true);
                btnNextLevel.gameObject.SetActive(false);
            }
            else
            {
                btnPreviousLevel.gameObject.SetActive(true);
                btnNextLevel.gameObject.SetActive(true);
            }
        }

        private void SetLevelData()
        {
            levelView.txtCount.text = $"Level {levelIndex + 1}";

            levelView.txtName.text = levels[levelIndex].levelName;
            levelView.levelImage.sprite = levels[levelIndex].levelImage;
        }

        private void UnloadScene()
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        private void btnPlayClick()
        {
            UnloadScene();
            SceneManager.LoadScene("Game");
        }
    }
}
