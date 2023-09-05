using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using static UnityVR.LibraryForVRTextbook;

namespace UnityVR
{
    public class ActionToVector2ForMove : ActionToControl
    {
        [SerializeField] GameObject targetObject;

        Vector3 initPos;


        void Start()
        {
            if (targetObject == null)
            {
                isReady = false;
                errorMessage += "#targetObject";
            }

            if (!isReady)
            {
                displayMessage.text = $"{GetSourceFileName()}\r\nError: {errorMessage}";
                return;
            }

            initPos = targetObject.transform.position;
        }

        protected override void OnActionPerformed(InputAction.CallbackContext ctx) => UpdateValue(ctx);

        protected override void OnActionCanceled(InputAction.CallbackContext ctx) => UpdateValue(ctx);

        void UpdateValue(InputAction.CallbackContext ctx)
        {
            var moveValue = ctx.ReadValue<Vector2>();
            var pos = targetObject.transform.position;
            pos.x = moveValue.x + initPos.x;
            pos.y = moveValue.y + initPos.y;
            targetObject.transform.position = pos;  
            displayMessage.text = $"Move: {moveValue}";
        }
    }
}


