using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPresence : MonoBehaviour
{
    [SerializeField] private InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField] private List<GameObject> controllerPrefabs;
    [SerializeField] private GameObject handModelPrefab;

    private bool HandIsActive
    {
        get => handIsActive;
        set
        {
            handIsActive = value;
            UpdateActivePrefab();
        }
    }

    private void UpdateActivePrefab()
    {
        if (HandIsActive)
        {
            controllerInstance.SetActive(false);
            handInstance.SetActive(true);

            return;
        }
        
        controllerInstance.SetActive(true);
        handInstance.SetActive(false);
    }

    private bool handIsActive; 
    private InputDevice targetDevice;
    private GameObject controllerInstance;
    private GameObject handInstance;
    private Animator handAnimator;

    private bool TargetDeviceIsValid => targetDevice.isValid;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        if (TargetDeviceIsValid == false)
        {
            Init();
            return;
        }
        
        if (handIsActive) UpdateHandAnimation();
    }

    private void Init()
    {
        var devices = GetDevices();
        
        if (devices.Count <= 0) return;
        targetDevice = devices[0];

        InitControllerInstance();
        InitHandInstance();
        handAnimator = handInstance.GetComponent<Animator>();
        
        HandIsActive = true;
    }

    private void InitHandInstance()
    {
        handInstance = Instantiate(handModelPrefab, transform);
    }

    private void InitControllerInstance()
    {
        var controllerPrefab = GetControllerPrefab();
        
        controllerInstance = GetPrefabInstance(controllerPrefab);
    }

    private GameObject GetPrefabInstance(GameObject prefab) => Instantiate(prefab, transform);

    private GameObject GetControllerPrefab()
    {
        var prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);

        return prefab ? prefab : controllerPrefabs[0];
    }
    
    private List<InputDevice> GetDevices()
    {
        var devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
        
        return devices;
    }

    private void UpdateHandAnimation()
    {
        UpdateTriggerValue();
        UpdateGripValue();
    }

    private void UpdateGripValue()
    {
        var isGripping = targetDevice.TryGetFeatureValue(CommonUsages.grip, out var gripValue);
        var gripAmount = isGripping ? gripValue : 0f;

        handAnimator.SetFloat("Grip", gripAmount);
    }

    private void UpdateTriggerValue()
    {
        var isTriggering = targetDevice.TryGetFeatureValue(CommonUsages.trigger, out var triggerValue);
        var triggerAmount = isTriggering ? triggerValue : 0f;
        
        
        handAnimator.SetFloat("Trigger", triggerAmount);
    }
}