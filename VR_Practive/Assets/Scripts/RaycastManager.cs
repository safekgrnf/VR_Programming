using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using static UnityVR.LibraryForVRTextbook;

namespace UnityVR
{
    public class RaycastManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI displayMessage;
        [SerializeField] XRRayInteractor rightRay;
        [SerializeField] AudioClip soundEffect;

        bool isReady;

        void Awake()
        {
            if(displayMessage is null) { Application.Quit(); }

            if(rightRay is null || soundEffect is null)
            {
                isReady = false;
                var errorMessage = "#rightRay or soundEffect";
                displayMessage.text = $"{GetSourceFileName()}\r\nError: {errorMessage}";
                return;
            }

            isReady = true;
        }

        void OnEnable()
        {
            if (!isReady) { return; }

            rightRay.useForceGrab = false;
            rightRay.hitClosestOnly = true;

            rightRay.hoverEntered.AddListener(OnHoverEntered);
            rightRay.hoverExited.AddListener(OnHoverExited);
            rightRay.selectEntered.AddListener(OnSelectEntered);
            rightRay.selectExited.AddListener(OnSelectExited);

            rightRay.playHapticsOnSelectEntered = true;
            rightRay.hapticSelectEnterIntensity = 1f;
            rightRay.hapticSelectEnterDuration = 0.5f;

            rightRay.playAudioClipOnSelectEntered = true;
            rightRay.audioClipForOnSelectEntered = soundEffect;
        }

        void OnDisable()
        {
            if(!isReady) { return; }

            rightRay.hoverEntered.RemoveListener(OnHoverEntered);
            rightRay.hoverExited.RemoveListener(OnHoverExited);
            rightRay.selectEntered.RemoveListener(OnSelectEntered);
            rightRay.selectExited.RemoveListener(OnSelectExited);
        }

        void DisplayInteraction<T>(T args, string EventListenerName) where T : BaseInteractionEventArgs 
        {
            displayMessage.text = $"{EventListenerName}\r\n";

            if (rightRay.hasHover)
            {
                var grabInteractable = args.interactableObject as XRGrabInteractable;
                var grabInteractableName = grabInteractable != null ? grabInteractable.name : "UnKnown";
                displayMessage.text += $"> Interactor: {rightRay.name}\r\n" + $"> Interactable: {grabInteractableName}\r\n";
            }

            if(rightRay.hasSelection && rightRay.TryGetCurrent3DRaycastHit(out var hit))
            {
                displayMessage.text += $"> Hit Position: {hit.point}\r\n";
            }
        }
        void OnHoverEntered(HoverEnterEventArgs args) => DisplayInteraction(args, GetCallerMember());

        void OnHoverExited(HoverExitEventArgs args) => DisplayInteraction(args, GetCallerMember());

        void OnSelectEntered(SelectEnterEventArgs args) => DisplayInteraction(args, GetCallerMember());

        void OnSelectExited(SelectExitEventArgs args) => DisplayInteraction(args, GetCallerMember());
    }
}
