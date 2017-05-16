using UnityEngine;

public enum AutoFacingOptions { None, Up, Down };

// Container has to manage Container field in Movement component

public interface IContainer
{
    AutoFacingOptions AutoFacing { get; set; }
    bool ForceFacing { get; set; }

    bool AddEntity(Entity entity, Vector3 hitPos);
    void RemoveEntity(Entity entity);
}