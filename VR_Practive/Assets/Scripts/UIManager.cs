using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;
using static UnityVR.LibraryForVRTextbook;
using UnityEngine.UI;
using System.Net.Sockets;
using Unity.VisualScripting;

namespace UnityVR
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI displayMessage;
        [SerializeField] GameObject targetObject;
        [SerializeField] Button buttonForStart;
        [SerializeField] Slider sliderForSpeed;
        [SerializeField] TMP_Dropdown dropdownForSpeedMode;
        [SerializeField] Toggle toggleForReverse;
        [SerializeField] TMP_InputField inputFieldForSpeed;

        bool isReady;
        bool hasStarted;
        static readonly IReadOnlyList<(string modeName, float maxSpeed)> SpeedModeData = new[]
        {
            ("Normal Mode", 90f),("High Speed Mode", 720f),
        };
        float rotationSpeed = SpeedModeData[0].maxSpeed;
        int rotationSign = 1;

        void Awake()
        {
            if (displayMessage is null) { Application.Quit(); }

            if (targetObject is null || buttonForStart is null || sliderForSpeed is null ||
                dropdownForSpeedMode is null || toggleForReverse is null || inputFieldForSpeed is null)
            {
                isReady = false;
                var errorMessage = "#targetObject or #UI Objects";
                displayMessage.text = $"{GetSourceFileName()}\r\nError: {errorMessage}";
                return;
            }

            isReady = true;
        }
        void OnEnable()
        {
            if (!isReady) { return; }

            buttonForStart.onClick.AddListener(OnButtonClicked);

            sliderForSpeed.maxValue = SpeedModeData[0].maxSpeed;
            sliderForSpeed.minValue = 0;
            sliderForSpeed.value = rotationSpeed;
            sliderForSpeed.onValueChanged.AddListener(OnSliderValueChanged);

            dropdownForSpeedMode.ClearOptions();
            foreach (var (modeName, _) in SpeedModeData)
            {
                dropdownForSpeedMode.options.Add(new TMP_Dropdown.OptionData(modeName));
            }

            dropdownForSpeedMode.value = 0;
            dropdownForSpeedMode.RefreshShownValue();
            dropdownForSpeedMode.onValueChanged.AddListener(OnDropdownValueChanged);

            toggleForReverse.isOn = false;
            toggleForReverse.onValueChanged.AddListener(OnToggleValueChanged);

            inputFieldForSpeed.contentType = TMP_InputField.ContentType.DecimalNumber;
            inputFieldForSpeed.onSelect.AddListener(OnInputFieldSelect);
            inputFieldForSpeed.onEndEdit.AddListener(OnInputFieldEndEdit);

            hasStarted = true;
            OnButtonClicked();
        }

        void OnDisable()
        {
            if (!isReady) { return; }

            buttonForStart.onClick.RemoveListener(OnButtonClicked);
            sliderForSpeed.onValueChanged.RemoveListener(OnSliderValueChanged);
            dropdownForSpeedMode.onValueChanged.RemoveListener(OnDropdownValueChanged);
            toggleForReverse.onValueChanged.RemoveListener(OnToggleValueChanged);
            inputFieldForSpeed.onSelect.RemoveListener(OnInputFieldSelect);
            inputFieldForSpeed.onEndEdit.RemoveListener(OnInputFieldEndEdit);
        }

        void Update()
        {
            if(!isReady || !hasStarted) { return; }

            var angularVelocity = rotationSign * rotationSpeed * Vector3.up;
            targetObject.transform.Rotate(angularVelocity * Time.deltaTime);
            displayMessage.text = $"Rotation Speed: {rotationSign * rotationSpeed:F1} [deg/s]";
        }

        void OnButtonClicked()
        {
            hasStarted = !hasStarted;
            targetObject.SetActive(hasStarted);
            sliderForSpeed.interactable = hasStarted;
            dropdownForSpeedMode.interactable = hasStarted;
            toggleForReverse.interactable = hasStarted;
            inputFieldForSpeed.interactable = hasStarted;
            displayMessage.text = "";
        }

        void OnSliderValueChanged(float value) => rotationSpeed = value;
        
        void OnDropdownValueChanged(int index) => sliderForSpeed.maxValue = SpeedModeData[index].maxSpeed;

        void OnToggleValueChanged(bool isOn) => rotationSign = isOn ? -1 : 1;

        void OnInputFieldSelect(string text) => inputFieldForSpeed.text = "";

        void OnInputFieldEndEdit(string text) => rotationSpeed = float.TryParse(text, out var num) ? num : rotationSpeed;
    }
}