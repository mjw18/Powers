using UnityEngine;
using System.Collections;

public abstract class VisualEffect : MonoBehaviour
{
    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public virtual void PlayEffect()
    {

    }
}

public enum VisualEffectPlacement
{
    CenteredOnPlayer,
    CenteredAtTarget,
    OffsetPlayer,
    OffsetTarget
}