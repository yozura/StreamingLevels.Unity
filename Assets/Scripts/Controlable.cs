using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controlable : MonoBehaviour
{
    public Transform cameraArmSocket;
    public Transform cameraArm;

    public abstract void Move(Vector2 input);

    public abstract void Rotate(Vector2 input);

    public abstract void Interact();

    public abstract void Jump();
}
