using UnityEngine;
using UnityEngine.Events;

public class AnimEvents_E : MonoBehaviour
{
    public UnityAction Swing = null;

    public void OnSwing()
    {
        Swing?.Invoke();
    }
}
