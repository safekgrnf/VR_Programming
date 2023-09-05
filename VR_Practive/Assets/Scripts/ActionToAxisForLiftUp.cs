using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using static UnityVR.LibraryForVRTextbook;

namespace UnityVR
{
    public class ActionToAxisForLiftUp : ActionToControl
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
            var liftUpValue = ctx.ReadValue<float>();
            var pos = targetObject.transform.position;
            pos.y = initPos.y + liftUpValue;
            targetObject.transform.position = pos;
            displayMessage.text = $"Lift Up: {liftUpValue:F2}";
        }

    }
}

