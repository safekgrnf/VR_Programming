using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using static UnityVR.LibraryForVRTextbook;

namespace UnityVR
{
    public class SocketManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI displayMessage;
        [SerializeField] XRSocketInteractor socket;

        bool isReady;
        Color normalColor;
        static readonly Color ColorOnSockedSelect = Color.blue;

        void Awake()
        {
            if (displayMessage is null) { Application.Quit(); }

            if (socket is null)
            {
                isReady = false;
                var errorMessage = "#socket";
                displayMessage.text = $"{GetSourceFileName()}\r\nError: {errorMessage}";
                return;
            }

            isReady = true;
        }

        void OnEnable()
        {
            if (!isReady) { return; }

            socket.selectEntered.AddListener(OnSelectEntered);
            socket.selectExited.AddListener(OnSelectExited);
        }

        void OnDisable()
        {
            if (!isReady) { return; }

            socket.selectEntered.RemoveListener(OnSelectEntered);
            socket.selectExited.RemoveListener(OnSelectExited);
        }


        void OnSelectEntered(SelectEnterEventArgs args)
        { 
            var objectInSocket = args.interactableObject as XRGrabInteractable;
            if (objectInSocket is null) { return; }

            var message = $"Object in Socket: {objectInSocket.name}";
            displayMessage.text = $"{GetCallerMember()}\r\n{message}\r\n";
            var meshRenderer = objectInSocket.GetComponent<MeshRenderer>();
            if (meshRenderer != null )
            {
                meshRenderer.material.color = ColorOnSockedSelect;
            }
        }

        void OnSelectExited(SelectExitEventArgs args)
        {
            var objectInSocket = args.interactableObject as XRGrabInteractable;
            if (objectInSocket is null) { return; }

            displayMessage.text = $"{GetCallerMember()}\r\n";
            var meshRenderer = objectInSocket.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material.color = normalColor;
            }
        }
    }
}


