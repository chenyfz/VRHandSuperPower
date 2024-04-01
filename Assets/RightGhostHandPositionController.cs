using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.Input;
using UnityEngine;

public class RightGhostHandPositionController : MonoBehaviour
{
    public Hand hand;
    public Transform root;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (root is null || !hand.GetRootPose(out var handRootPose)) return;
        
        root.rotation = handRootPose.rotation;
        
        // root.position = handRootPose.position;
    }
}
