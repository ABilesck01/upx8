using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace upx.ui
{
    public class MainMenuController : MonoBehaviour
    {
        public static readonly string SceneName = "Main";

        [SerializeField] private Button btnPlay;

        private void Awake()
        {
            btnPlay.onClick.AddListener(BtnPlayGame);
        }

        public void BtnPlayGame()
        {
            SceneManager.LoadScene(LevelSelectionController.sceneName, LoadSceneMode.Additive);
        }
    }
}
