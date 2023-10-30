using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static upx.Game.VariableStreetController;

namespace upx.Game
{
    public class VariableStreetView : MonoBehaviour
    {
        [SerializeField] private Animator screenAnimator;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private TextMeshProUGUI txtSpeed;
        [SerializeField] private Button btnClose;

        private VariableStreetController controller;

        private void Awake()
        {
            speedSlider.onValueChanged.AddListener(OnSliderChange);
            btnClose.onClick.AddListener(OnButtonClick);
        }

        private void OnEnable()
        {
            VariableStreetController.OnClickOnBlock += VariableStreetController_OnClickOnBlock;
        }

        private void OnDisable()
        {
            VariableStreetController.OnClickOnBlock -= VariableStreetController_OnClickOnBlock;
        }

        private void VariableStreetController_OnClickOnBlock(object sender, OnClickOnBlockEventArgs e)
        {
            this.controller = e.variableStreetController;
            speedSlider.value = e.currentSpeed;
            screenAnimator.SetBool("isOpen", true);
        }

        private void OnSliderChange(float value)
        {
            if (controller == null) return;

            txtSpeed.text = $"{value} km/h";
            controller.SetCalculatedSpeed(value);
        }

        private void OnButtonClick()
        {
            screenAnimator.SetBool("isOpen", false);
        }
    }
}
