using UnityEngine;
public class Apple : MonoBehaviour, IItem, IEatable
{
    [SerializeField] private int _energy = 20;
    public int energy => _energy;
    public bool IsTaken { get; private set; }
    private Collider2D _collider;
    private Rigidbody2D _rb;
    public int hp { get; private set;}

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        hp = 1;
    }
    private void FixedUpdate()
    {
        _rb.velocity *= 0.98f;

        if (_rb.velocity.magnitude < 0.01f)
        {
            _rb.velocity = Vector3.zero;
            _rb.angularVelocity = 0f;
        } 
    }
    
    public void OnEat()
    {
        die();

        if (_collider != null)
            _collider.enabled = true;

        if (_rb != null)
            _rb.simulated = true;
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
        //добавити перевірку чи предмет зіткнувся з чимось ще, якщо так, то decreaseHp()
    }
    public void decreaseHp()
    {
        hp -= 1;
    }
    public void die()
    {
        IsTaken = false;
        transform.SetParent(null);
        transform.position = new Vector3(10,10,10);// потім замінити на метод для телепортації в рандомну точку на карті
    }
    public int GetEnergy()
    {
        return energy;
    }
}