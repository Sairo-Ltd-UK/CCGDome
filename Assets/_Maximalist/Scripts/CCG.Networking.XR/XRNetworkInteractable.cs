using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.XR.Interaction.Toolkit;

public class XRNetworkInteractable : NetworkBehaviour
{
    private Rigidbody rb;

    private void Start()
    {
        // we should technically make them public and set in inspector for less startup calls, but im feeling lazy.
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    public void EventPickup()
    {
        //if (isOwned == false)
        //{
            ResetInteractableVelocity();
            CmdPickup(XRStaticVariables.handValue);
        //}
    }


    public void EventDrop()
    {
        // technically dont need to pass auth when dropping, only remove auth when another player grabs, or current grabber disconnects
        // doing it this way stops jitter when passing auth of moving objects (due to ping difference of positions)
        ///*
        if (isOwned == true)
        {
   
        }
        //*/
    }

    [Command(requiresAuthority = false)]
    public void CmdPickup(int _hand, NetworkConnectionToClient sender = null)
    {
        //Debug.Log("Mirror CmdPickup owner set to: " + sender.identity);

        ResetInteractableVelocity();

        if (sender != netIdentity.connectionToClient)
        {
            netIdentity.RemoveClientAuthority();
            netIdentity.AssignClientAuthority(sender);
        }
    }

    ///*
    [Command(requiresAuthority = false)]
    public void CmdDrop(int _hand, NetworkConnectionToClient sender = null) //Vector3 _velocity, Vector3 _angualarVelocity,
    {

    }
    //*/

    private void ResetInteractableVelocity()
    {
        // Unitys interactable types need some adjustments to stop them behaving weird over network
        // Without this you may notice some pickups rapidly fall through the floor
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // we can use this check apply different behaviour depending on interactable type
        // if (xRGrabInteractable.movementType == XRBaseInteractable.MovementType.VelocityTracking) { }
    }

    public XRNetworkPlayerScript vrNetworkPlayerScript;

    private void Update()
    {
        if (vrNetworkPlayerScript == null)
            return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            EventPickup();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            EventDrop();
        }
    }
}
