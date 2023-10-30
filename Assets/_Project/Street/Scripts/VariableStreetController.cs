using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace upx.Game
{
    public class VariableStreetController : StreetController
    {
       [SerializeField] private float calculatedSpeed = 2;

        public class OnClickOnBlockEventArgs : EventArgs 
        {
            public VariableStreetController variableStreetController;
            public float currentSpeed;
        }

        public static event EventHandler<OnClickOnBlockEventArgs> OnClickOnBlock;

        public void SetCalculatedSpeed(float speed)
        {
            this.calculatedSpeed = Map(speed, 30, 120, 0.5f, 4);
        }

        public float GetCalculatedSpeed()
        {
            return this.calculatedSpeed;
        }

        private void OnMouseOver()
        {
            if(Input.GetMouseButtonDown(0) && !IsPointerOverUI())
            {
                OnClickOnBlock?.Invoke(this, new OnClickOnBlockEventArgs
                {
                    variableStreetController = this,
                    currentSpeed = Map(this.calculatedSpeed, 0.5f, 4, 30, 120)
                });
            }
        }

        private bool IsPointerOverUI()
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);
            List<RectTransform> uiResults = new List<RectTransform>();

            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject.TryGetComponent<RectTransform>(out RectTransform rect))
                {
                    uiResults.Add(rect);
                }
            }

            return uiResults.Count > 0;
        }

        private float Map(float input, float inputMin, float inputMax, float min, float max)
        {
            return min + (input - inputMin) * (max - min) / (inputMax - inputMin);
        }
    }
}
