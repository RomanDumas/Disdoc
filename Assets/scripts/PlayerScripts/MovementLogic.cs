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
    public float checkSize;
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            SwapItems();
        }

        if (hit.collider != null)
        {
            GameObject obj = hit.collider.gameObject;
            
            if (Input.GetKeyDown(KeyCode.F))
            {
                BurnItem(obj);
            }
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
    private void BurnItem(GameObject obj)
    {
        if (!obj.TryGetComponent<Bonfire>(out var bonfire))
            return;

        if (!_leftHand.IsEmpty() && _leftHand.GetItem() is IBurnable burnableL)
        {
            burnableL.Burn(bonfire);
            _leftHand.DeleteItem();
        }            
        else if (!_rightHand.IsEmpty() && _rightHand.GetItem() is IBurnable burnableR)
        {
            burnableR.Burn(bonfire);
            _rightHand.DeleteItem();
        }
    }

    private void EatItem()
    {
        if (!_leftHand.IsEmpty() && _leftHand.GetItem() is IEatable eatableL)
        {
            _actualHunger += eatableL.energy;
            eatableL.OnEat();
            _leftHand.DeleteItem();
        }
        else if (!_rightHand.IsEmpty() && _rightHand.GetItem() is IEatable eatableR)
        {
            _actualHunger += eatableR.energy;
            eatableR.OnEat();
            _rightHand.DeleteItem();
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
    private void SwapItems()
    {
        IItem temp = _leftHand.GetItem();
        _leftHand.TakeItem(_rightHand.GetItem());
        _rightHand.TakeItem(temp);
    }
    private void ProcessHungerMechanik() //потім коли буде спрінт, зменшувати швидкість спрінта
    {   
        //зміна голоду
        if(!(_actualHunger < 0))
        {
            _actualHunger -= 0.01f;      
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

