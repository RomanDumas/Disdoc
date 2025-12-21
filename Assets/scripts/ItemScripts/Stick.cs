using UnityEngine;
public class Stick : MonoBehaviour, IItem
{
    public bool IsTaken { get; private set; }
    private Collider2D _collider;
    private Rigidbody2D _rb;
    public int hp { get; private set;}

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        hp = 3;
    }
    public void OnTake(Transform hand)
    {
        IsTaken = true;

        transform.SetParent(hand);
        transform.localPosition = Vector3.zero;

        GetComponent<Collider2D>().enabled = false;

        if (_collider != null)
            _collider.enabled = false;

        if (_rb != null)
            _rb.simulated = false;        
    }
    public void OnDrop(Vector3 dropPosition)
    {
        IsTaken = false;

        transform.position = dropPosition;
        transform.SetParent(null);

        if (_collider != null)
            _collider.enabled = true;

        if (_rb != null)
            _rb.simulated = true;
    }
    public void OnThrow(Vector3 dropPosition, Vector3 directionToThrow)
    {
        OnDrop(dropPosition);
        _rb.velocity = directionToThrow * 10;
    }
    public void decreaseHp()
    {
        hp -= 1;
    }
    public void die()
    {
        // SetActive(false); // потім замінити на метод для телепортації в рандомну точку на карті
    }
}