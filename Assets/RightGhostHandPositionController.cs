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

    private SkinnedMeshRenderer _renderer;
    
    void Start()
    {
        _renderer = handSynthetic.transform.GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        if (root is null || !hand.GetRootPose(out var handRootPose)) return;
        
        root.transform.rotation = handRootPose.rotation;

        var isHit = CalcHandPosition(out var position);
        root.transform.position = position;
        
        _renderer.SetMaterials(isHit ? moreTransparentShaders : originalShaders);
    }

    private bool CalcHandPosition(out Vector3 position)
    {
        hand.GetJointPose(HandJointId.HandWristRoot, out var wristPose);
        hand.GetJointPose(HandJointId.HandMiddle1, out var middlePose);
        var handPosition = new Vector3(middlePose.position.x, middlePose.position.y, wristPose.position.z) + new Vector3(-0.04f, 0.04f, 0);
        var centerEyePosition = centerEyeAnchor.position;

        var direction = handPosition - centerEyePosition;
        var centerEyeRay = new Ray(handPosition + direction, direction);
        var isHit = Physics.Raycast(centerEyeRay, out var centerRaycastHit, 200);

        position = isHit
            ? centerRaycastHit.point + new Vector3(0, 0, -0.08f)
            : new Vector3(0, -1, 0);
        
        return isHit;
    }
}
