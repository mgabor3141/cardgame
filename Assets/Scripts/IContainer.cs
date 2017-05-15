using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContainer
{
    bool isEntityInContainer(Vector2 pos);

    Vector3 GetTargetPosition(Vector2 screenPos);
}