using System.Collections;
using System.Collections.Generic;
using Interactable_Objects;
using UnityEngine;

public class JumpingObject : MonoBehaviour
{

    private DropFromTree _dropTree;
    // Start is called before the first frame update
    void Start()
    {
        _dropTree = GetComponent<DropFromTree>();
    }

    // Update is called once per frame
    public void InteractionEnd()
    {
        _dropTree.EndJump();
    }
}
