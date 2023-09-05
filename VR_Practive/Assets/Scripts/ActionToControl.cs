using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace UnityVR
{
    public class ActionToControl : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI displayMessage;
        [SerializeField] InputActionReference actionReference;

        protected bool isReady = true;
        protected string errorMessage;
        InputAction action;

        void Awake()
        {
            if(displayMessage is null) { Application.Quit(); }

            if((action = actionReference.action) is null || actionReference is null)
            {
                isReady = false;
                errorMessage = "#actionReference";
            }
        }

        void OnEnable()
        {
            if(!isReady){ return; }

            action.started += OnActionStarted;
            action.performed += OnActionPerformed;
            action.canceled += OnActionCanceled;
            action.Enable();
        }

        void OnDisable()
        {
            if (!isReady) { return; }

            action.Disable();
            action.started -= OnActionStarted;
            action.performed -= OnActionPerformed;
            action.canceled -= OnActionCanceled;
        }

        protected virtual void OnActionStarted(InputAction.CallbackContext ctx) { }
        protected virtual void OnActionPerformed(InputAction.CallbackContext ctx) { }
        protected virtual void OnActionCanceled(InputAction.CallbackContext ctx) { }
    }
}

