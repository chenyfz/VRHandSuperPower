using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using Oculus.Interaction.Input;
using Unity.VisualScripting;
using UnityEngine;

public class RightGhostHandPositionController : MonoBehaviour
{
    public Hand hand;
    public Hand handSynthetic;
    public GameObject root;
    public Transform centerEyeAnchor;
    public GameObject ghostHand;

    public List<Material> moreTransparentShaders;
    public List<Material> originalShaders;
    
    public bool isRightHand = true;
    
    
    void Start()
    {
    }

    private void Update()
    {
        if (root is null || !hand.GetRootPose(out var handRootPose)) return;
        
        root.transform.rotation = handRootPose.rotation;

        var isHit = CalcHandPosition(out var position);
        root.transform.position = position;

        handSynthetic.transform.GetComponentInChildren<SkinnedMeshRenderer>()
            .SetMaterials(isHit ? moreTransparentShaders : originalShaders);
    }

    private bool CalcHandPosition(out Vector3 position)
    {
        hand.GetJointPose(HandJointId.HandWristRoot, out var jointPose);
        var handPosition = isRightHand
                ? jointPose.position + new Vector3(-0.1f, 0.15f, 0)
                : jointPose.position + new Vector3(0.1f, 0.15f, 0);
        var centerEyePosition = centerEyeAnchor.position;

        var direction = handPosition - centerEyePosition;
        var centerEyeRay = new Ray(handPosition + direction, direction);
        var isHit = Physics.Raycast(centerEyeRay, out var centerRaycastHit, 200);

        position = isHit
            ? new Vector3(centerRaycastHit.point.x, centerRaycastHit.point.y, centerRaycastHit.point.z - 0.05f)
            : new Vector3(0, -1, 0);
        
        return isHit;
    }
}
