using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace upx.ui
{
    public class SettingsController : MonoBehaviour
    {
        [SerializeField] private Button btnSettings;
        [SerializeField] private Animator animator;
        [SerializeField] private Toggle audioToggle;
        [SerializeField] private AudioMixer audioMixer;

        private bool isOpen = false;

        private void Awake()
        {
            btnSettings.onClick.AddListener(BtnSettingsClick);
            audioToggle.onValueChanged.AddListener(OnAudioChange);
        }

        private void BtnSettingsClick()
        {
            isOpen = !isOpen;
            animator.SetBool("open", isOpen);
        }

        private void OnAudioChange(bool value)
        {
            float volume = value ? 0f : -80f;
            audioMixer.SetFloat("volume", volume);
        }
    }
}
