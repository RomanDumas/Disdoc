using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class MovementLogic : MonoBehaviour
{
    Vector3 moveInput;
    [SerializeField] float _speed;
    [SerializeField] float _hunger;
    [SerializeField] Transform _marker;
    [SerializeField] Hand _leftHand;
    [SerializeField] Hand _rightHand;
    public float checkDistance;
    public LayerMask interactableLayer;

    private float _actualHunger;
    private float _actualSpeed;

    private GameObject _highlightedObject;
    private Vector3 _facingDirection = Vector3.down; // стартовий напрямок

    void Awake()
    {
        _actualHunger = _hunger;
        _actualSpeed = _speed;
    }
    void Update()
    {  
        
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput != Vector3.zero)
            _facingDirection = moveInput.normalized;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            _facingDirection, 
            checkDistance, 
            interactableLayer);

        if (Input.GetKeyDown(KeyCode.T))
        {
            ThrowItem(_facingDirection);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            EatItem();
        }

        if (hit.collider != null)
        {
            GameObject obj = hit.collider.gameObject;

            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeItem(obj);
            }

            if (_highlightedObject != obj)
            {
                ClearHighlight();
                Highlight(obj);
            }
        }
        else
        {
            ClearHighlight();
        }


    }
    private void FixedUpdate()
    {
        
        ProcessHungerMechanik();
        
        transform.position += moveInput * Time.fixedDeltaTime * _actualSpeed;
        
    }

    void Highlight(GameObject obj)
    {
        _highlightedObject = obj;
        _marker.gameObject.SetActive(true);
        _marker.position = obj.transform.position + obj.gameObject.GetComponent<Renderer>().bounds.extents.y * Vector3.up;
    }

    void ClearHighlight()
    {
        if (_highlightedObject != null)
        {
            _marker.gameObject.SetActive(false);
            _highlightedObject = null;
        }
    }
    private void EatItem()  //якщо в майбутньому буде більше предметів які можна з'їсти, 
                            // то зробити інтерфейс IEatable і замінити Apple на нього
    {

        // if(!_leftHand.IsEmpty() && _leftHand.getItem().TryGetComponent<Apple>(out var apple))
        if(!_leftHand.IsEmpty())
        {
            _actualHunger += _leftHand.EatItem();
        }
        // else if(!_rightHand.IsEmpty() && _rightHand.getItem().TryGetComponent<Apple>(out var apple))
        else if(!_rightHand.IsEmpty())
        {
            _actualHunger += _rightHand.EatItem();
        }
    }
    private void TakeItem(GameObject obj)
    {
        if(obj.TryGetComponent<IItem>(out var item) && !item.IsTaken)
        {
            if (_rightHand.IsEmpty())
            {
                _rightHand.TakeItem(item);
            }
            else if (_leftHand.IsEmpty())
            {
                _leftHand.TakeItem(item);
            }
        }
    }
    private void DropItem()
    {
        if (!_leftHand.IsEmpty())
        {
            _leftHand.DropItem();
        }
        else if (!_rightHand.IsEmpty())
        {
            _rightHand.DropItem();
        }
    }
    private void ThrowItem(Vector3 directionToThrow)
    {
        if (!_leftHand.IsEmpty())
        {
            _leftHand.ThrowItem(directionToThrow);
        }
        else if (!_rightHand.IsEmpty())
        {
            _rightHand.ThrowItem(directionToThrow);
        }
    }
    private void ProcessHungerMechanik() //потім коли буде спрінт, зменшувати швидкість спрінта
    {   
        //зміна голоду
        if(!(_actualHunger < 0))
        {
            _actualHunger -= 0.1f;  // потім змінити на пдекватне значення, 
                                    //зараз коофіцієнт спеціально великий, 
                                    // для того щоб бачити різницю       
        }

        //зміна швидкості від голоду
        if(_actualHunger < 0)
        {
            _actualSpeed = _speed * 0.2f;
        }
        else if(_actualHunger < 20)
        {
            _actualSpeed = _speed * 0.4f;
        }
        else if(_actualHunger < 40)
        {
            _actualSpeed = _speed * 0.6f;
        }
        else if(_actualHunger < 60)
        {
            _actualSpeed = _speed * 0.8f; 
        }
        else
        {
            _actualSpeed = _speed;
        }
    }
}

