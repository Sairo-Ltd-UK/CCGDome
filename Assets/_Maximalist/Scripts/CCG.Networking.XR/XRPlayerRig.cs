using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPlayerRig : MonoBehaviour
{
    public Transform rHandTransform;
    public Transform lHandTransform;
    public Transform headTransform;

    public Transform canvasUIPosition;

    public GameObject damageTriggerR;
    public GameObject damageTriggerL;

    public XRNetworkPlayerScript localVRNetworkPlayerScript;
    public AudioSource audioCue;

    // switch to Late/Fixed Update if weirdness happens
    private void Update()
    {
        if (localVRNetworkPlayerScript)
        {
            // presuming you want a head object to sync, optional, same as hands.
            localVRNetworkPlayerScript.headTransform.position = this.headTransform.position;
            localVRNetworkPlayerScript.headTransform.rotation = this.headTransform.rotation;
            localVRNetworkPlayerScript.rHandTransform.position = this.rHandTransform.position;
            localVRNetworkPlayerScript.rHandTransform.rotation = this.rHandTransform.rotation;
            localVRNetworkPlayerScript.lHandTransform.position = this.lHandTransform.position;
            localVRNetworkPlayerScript.lHandTransform.rotation = this.lHandTransform.rotation;
        }
    }

    // Simple movement for testing on PC/Editor/Controller joystick
    // helps if you cannot use headset directly in Unity Editor  (W A S D)
    private void FixedUpdate()
    {
        HandleMovement();
        HandleInput();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 100.0f;
        float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

        transform.Rotate(0, moveX, 0);
        transform.Translate(0, 0, moveZ);
    }

    private void HandleInput()
    {
        // take input from focused window only
        if (!Application.isFocused)
            return;

        // input for local player
        if (localVRNetworkPlayerScript && localVRNetworkPlayerScript.isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
               // audioCue.Play();
                localVRNetworkPlayerScript.Fire(0);
            }
        }


        if (localVRNetworkPlayerScript && localVRNetworkPlayerScript.isLocalPlayer)
        {

            if (rightHand.GetComponent<ActionBasedController>().activateAction.action.ReadValue<float>() > 0.5f)
            {
                localVRNetworkPlayerScript.Fire(1);
            }
            if (leftHand.GetComponent<ActionBasedController>().activateAction.action.ReadValue<float>() > 0.5f)
            {
                localVRNetworkPlayerScript.Fire(2);
            }
        }
    }
    public XRBaseController rightHand;
    public XRBaseController leftHand;
    public InputHelpers.Button buttonR;
    public InputHelpers.Button buttonL;

    public InputActionReference shootButtonRightHand;
    public InputActionReference shootButtonLeftHand;



    public InputActionReference testReference = null;

 



 
}