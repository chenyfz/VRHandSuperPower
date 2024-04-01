using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Oculus.Interaction.Input;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

public class RightGhostHandPositionController : MonoBehaviour
{
    public Hand hand;
    public GameObject root;
    public Transform centerEyeAnchor;
    public GameObject header;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (root is null || !hand.GetRootPose(out var handRootPose)) return;
        
        root.transform.rotation = handRootPose.rotation;

        if (CalcHandPosition(out var position))
        {
            root.transform.position = position;
            ShowHand();
        }
        else
        {
            HideHand();
        }
    }

    private bool CalcHandPosition(out Vector3 position)
    {
        hand.GetJointPose(HandJointId.HandIndexTip, out var pose);
        var indexTipPosition = pose.position;
        var centerEyePosition = centerEyeAnchor.position;
        
        var centerEyeRay = new Ray(centerEyePosition, indexTipPosition);
        Physics.Raycast(centerEyeRay, out var centerRaycastHit);

        position = new Vector3(centerRaycastHit.point.x, centerEyeAnchor.position.y, centerRaycastHit.point.z);

        var isHit = centerRaycastHit.colliderInstanceID != 0;
        if (isHit)
        {
            header.transform.GetComponent<TextMeshProUGUI>().text = centerRaycastHit.point + " ";
        }
        return isHit;
    }

    private void HideHand()
    {
        if (root.activeSelf) root.SetActive(false);
    }

    private void ShowHand()
    {
        if (!root.activeSelf) root.SetActive(true);
    }
}
