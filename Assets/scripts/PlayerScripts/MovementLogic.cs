using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class MovementLogic : MonoBehaviour
{
    Vector3 moveInput;
    [SerializeField] float Speed;    
    [SerializeField] Transform marker;
    [SerializeField] Hand leftHand;
    [SerializeField] Hand rightHand;
    public float checkDistance;
    public float checkSize;
    public LayerMask interactableLayer;

    private GameObject highlightedObject;
    private Vector3 facingDirection = Vector3.down; // стартовий напрямок

    void Update()
    {
        Camera.main.transform.position = transform.position + new Vector3(0, 0, -10);
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput != Vector3.zero)
            facingDirection = moveInput.normalized;

        // RaycastHit2D hit = Physics2D.Raycast(
        //     transform.position, 
        //     facingDirection, 
        //     checkDistance, 
        //     interactableLayer);

        RaycastHit2D hit = Physics2D.CircleCast(
            transform.position,
            checkSize, 
            facingDirection, 
            checkDistance, 
            interactableLayer);

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropItem();
        }  

        if (hit.collider != null)
        {
            GameObject obj = hit.collider.gameObject;

            if (Input.GetKeyDown(KeyCode.E))
            {
                TakeItem(obj);
            }

            if (highlightedObject != obj)
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
        transform.position += moveInput * Time.fixedDeltaTime * Speed;
    }

    void Highlight(GameObject obj)
    {
        highlightedObject = obj;
        marker.position = obj.transform.position + obj.gameObject.GetComponent<Renderer>().bounds.extents.y * Vector3.up;
        marker.gameObject.SetActive(true);
    }

    void ClearHighlight()
    {
        if (highlightedObject != null)
        {
            marker.gameObject.SetActive(false);
            highlightedObject = null;
        }
    }
    
    private void TakeItem(GameObject obj)
    {
        if(obj.TryGetComponent<IItem>(out var item) && !item.IsTaken) // перевірка чи obj IItem
        {
            if (rightHand.IsEmpty())
            {
                rightHand.TakeItem(item); //дописати логіку слідкування предмету за рукою
            }
            else if (leftHand.IsEmpty()) //дописати логіку слідкування предмету за рукою
            {
                leftHand.TakeItem(item);
            }
        }
    }
    private void DropItem()
    {
        if (!leftHand.IsEmpty())
        {
            leftHand.DropItem();
        }
        else if (!rightHand.IsEmpty())
        {
            rightHand.DropItem();
        }
    }
}

