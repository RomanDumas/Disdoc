using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    private float _y_move;
    private float _x_move;
    [SerializeField] float Speed;

    void Update()
    {
        _y_move = Input.GetAxisRaw("Vertical");
        _x_move = Input.GetAxisRaw("Horizontal");
        
    }
    private void FixedUpdate()
    {
        transform.position += new Vector3(_x_move, _y_move, 0) * Time.fixedDeltaTime * Speed;
    }
}
