using UnityEngine;
public interface IItem
{
    bool IsTaken { get; }
    void OnTake(Transform hand);
    void OnDrop(Vector3 dropPosition);
    void OnThrow(Vector3 dropPosition, Vector3 directionToThrow);
    int hp { get; }
    void decreaseHp();
    void die();
}