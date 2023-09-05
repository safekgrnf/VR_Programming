using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using static UnityVR.LibraryForVRTextbook;



namespace UnityVR
{
    [RequireComponent(typeof(XRGrabInteractable))]
    public class GrabbableObjectManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI displayMessage;

        bool isReady;
        XRGrabInteractable grabInteractable;
        MeshRenderer meshRenderer;
        Color normalColor;
        static readonly Color ColorOnActivated = Color.red;

        void Awake()
        {
            if (displayMessage is null) { Application.Quit(); }

            grabInteractable = GetComponent<XRGrabInteractable>();
            meshRenderer = GetComponent<MeshRenderer>();

            if (grabInteractable is null || meshRenderer is null || !meshRenderer.enabled)
            {
                isReady = false;
                var errorMessage = "#grabInteractable or meshRenderer";
                displayMessage.text = $"{GetSourceFileName()}\r\nError: {errorMessage}";
                return;
            }

            isReady = true;
        }

        void OnEnable()
        {
            if (!isReady) { return; }

            grabInteractable.activated.AddListener(OnActivated);
            grabInteractable.deactivated.AddListener(OnDeactivated);
            grabInteractable.selectEntered.AddListener(OnSelectEntered);
            grabInteractable.selectExited.AddListener(OnSelectExited);
        }

        void OnDisable()
        {
            if (!isReady) { return; }

            grabInteractable.activated.RemoveListener(OnActivated);
            grabInteractable.deactivated.RemoveListener(OnDeactivated);
            grabInteractable.selectEntered.RemoveListener(OnSelectEntered);
            grabInteractable.selectExited.RemoveListener(OnSelectExited);
        }


        void OnSelectEntered(SelectEnterEventArgs args) => displayMessage.text = $"{GetCallerMember()}\r\n";

        void OnSelectExited(SelectExitEventArgs args)
        {
            displayMessage.text = displayMessage.text = $"{GetCallerMember()}\r\n";
            meshRenderer.material.color = normalColor;
        }

        void OnActivated(ActivateEventArgs args)
        {
            displayMessage.text = displayMessage.text = $"{GetCallerMember()}\r\n";
            meshRenderer.material.color = ColorOnActivated;
        }

        void OnDeactivated(DeactivateEventArgs args)
        {
            displayMessage.text = displayMessage.text = $"{GetCallerMember()}\r\n";
            meshRenderer.material.color = normalColor;
        }
    }
}

