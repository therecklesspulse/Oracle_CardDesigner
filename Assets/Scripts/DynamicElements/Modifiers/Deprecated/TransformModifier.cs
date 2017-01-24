using UnityEngine;
using System.Collections;

public class TransformModifier{

    protected Transform transform;

    public TransformModifier(Transform parentTransform)
    {
        transform = parentTransform;
    }

    virtual protected void ModifyTransform()
    {

    }

    virtual public Transform GetModifiedTransform()
    {
        ModifyTransform();
        return transform;
    }
}
