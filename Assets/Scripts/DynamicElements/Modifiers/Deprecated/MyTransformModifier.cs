using UnityEngine;
using System.Collections;

public class MyTransformModifier : TransformModifier
{

    public MyTransformModifier(Transform parentTransform) : base(parentTransform)
    {
        
    }

    override protected void ModifyTransform()
    {
        transform.localScale *= 0.5f;
    }

    override public Transform GetModifiedTransform()
    {
        ModifyTransform();
        return transform;
    }
}
