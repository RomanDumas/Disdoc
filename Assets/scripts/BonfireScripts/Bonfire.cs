using UnityEngine;

public class Bonfire : MonoBehaviour
{
    [SerializeField] float _fireLevel;
    private float _actualFireLevel;

    void Awake()
    {
        _actualFireLevel = _fireLevel;
    }
    void FixedUpdate()
    {
        _actualFireLevel -= 0.01f;
    }

    public void BurnStick()
    {
        _actualFireLevel += 20f;
    }
}