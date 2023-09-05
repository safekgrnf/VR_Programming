using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using static UnityVR.LibraryForVRTextbook;

namespace UnityVR
{
    public class ActionToButtonForChangeColor : ActionToControl

    {
        [SerializeField] GameObject targetObject;

        Renderer meshRenderer;
        Color normalColor;
        static readonly Color ColorOnpressed = Color.red;

        void Start()
        {
            if (targetObject == null || (meshRenderer = targetObject.GetComponent<MeshRenderer>()) is null)
            {
                isReady = false;
                errorMessage += "#targetObject";
            }

            if (!isReady)
            {
                displayMessage.text = $"{GetSourceFileName()}\r\nError: {errorMessage}";
                return;
            }

            normalColor = meshRenderer.material.color;

        }

        protected override void OnActionPerformed(InputAction.CallbackContext ctx) => UpdateValue(ctx);

        protected override void OnActionCanceled(InputAction.CallbackContext ctx) => UpdateValue(ctx);

        void UpdateValue(InputAction.CallbackContext ctx)
        {
            var isOn = ctx.ReadValueAsButton();
            meshRenderer.material.color = isOn ? ColorOnpressed : normalColor; 
            displayMessage.text = $"Change Color: {isOn}";
        }
    }
}