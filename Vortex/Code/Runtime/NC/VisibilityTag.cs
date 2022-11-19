using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVisibilityEventReceiver
{
    void OnAppearToCamera();
    void OnDisappearFromCamera();
}

public class VisibilityTag : MonoBehaviour
{
    IVisibilityEventReceiver receiver = null;
    private void Awake()
    {
        receiver = GetComponentInParent<IVisibilityEventReceiver>();
    }
    void OnBecameVisible()
    {
        if (receiver == null) 
        {
            receiver = GetComponentInParent<IVisibilityEventReceiver>();
        }
        if (receiver != null)
        {
            receiver.OnAppearToCamera();
        }
    }
    void OnBecameInvisible()
    {
        if (receiver == null)
        {
            receiver = GetComponentInParent<IVisibilityEventReceiver>();
        }
        if (receiver != null)
        {
            receiver.OnDisappearFromCamera();
        }
    }
}