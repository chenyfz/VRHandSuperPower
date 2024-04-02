using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class ModeController : MonoBehaviour
{
    public List<GameObject> interactors;
    public GameObject handRoot;
    public GameObject handVisual;

    private void Start()
    {
        SetToNativeMode();
    }

    public void SetToNativeMode()
    {
        SetInterators(true);
        handRoot.SetActive(false);
    }

    public void SetToDistanceTapMode()
    {
        SetInterators(false);
        handRoot.SetActive(true);
        handVisual.GetComponent<GhostHandTapController>().enabled = true;
        handVisual.GetComponent<GhostHandPinchController>().enabled = false;
    }

    public void SetToDistancePinchMode()
    {
        SetInterators(false);
        handRoot.SetActive(true);
        handVisual.GetComponent<GhostHandTapController>().enabled = false;
        handVisual.GetComponent<GhostHandPinchController>().enabled = true;
    }

    private void SetInterators(bool value)
    {
        foreach (var interactor in interactors)
        {
            interactor.SetActive(value);
        }
    }
}
