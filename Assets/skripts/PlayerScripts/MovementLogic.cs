using UnityEngine;

public class MovementLogic : MonoBehaviour
{
    Vector3 moveInput;
    [SerializeField] float Speed;    
    [SerializeField] Transform marker;
    public float checkDistance;
    public LayerMask interactableLayer;

    private GameObject highlightedObject;
    private Vector3 facingDirection = Vector3.down; // стартовий напрямок

    

    void Update()
    {
        moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveInput != Vector3.zero)
            facingDirection = moveInput.normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, facingDirection, checkDistance, interactableLayer);

        if (hit.collider != null)
        {
            GameObject obj = hit.collider.gameObject;

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
    public Material outlineMaterial;
    public Material standartMaterial;


    void Highlight(GameObject obj)
    {
        highlightedObject = obj;
        marker.gameObject.SetActive(true);
        marker.position = obj.transform.position + obj.gameObject.GetComponent<Renderer>().bounds.extents.y * Vector3.up;
    }

    void ClearHighlight()
    {
        if (highlightedObject != null)
        {
            marker.gameObject.SetActive(false);
            highlightedObject = null;
        }
    }

}

