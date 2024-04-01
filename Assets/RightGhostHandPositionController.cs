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
    public GameObject ghostHand;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (root is null || !hand.GetRootPose(out var handRootPose)) return;
        
        root.transform.rotation = handRootPose.rotation;

        CalcHandPosition(out var position);
        root.transform.position = position;

    }

    private bool CalcHandPosition(out Vector3 position)
    {
        hand.GetJointPose(HandJointId.HandIndex1, out var jointPose);
        hand.GetRootPose(out var handRootPose);
        var centerEyePosition = centerEyeAnchor.position;

        var direction = jointPose.position - centerEyePosition;
        var centerEyeRay = new Ray(jointPose.position + direction, direction);
        var isHit = Physics.Raycast(centerEyeRay, out var centerRaycastHit, 200);

        position = isHit ? new Vector3(centerRaycastHit.point.x, centerRaycastHit.point.y, centerRaycastHit.point.z - 0.1f) : new Vector3(0, 0, -1);
        
        // header.transform.GetComponent<TextMeshProUGUI>().text = centerRaycastHit.colliderInstanceID + " ";

        return isHit;
    }
}
