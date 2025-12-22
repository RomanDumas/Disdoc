using UnityEngine;
public interface IItem
{
    bool IsTaken { get; }
    void OnTake(Transform hand);
    void OnDrop(Vector3 dropPosition);
}