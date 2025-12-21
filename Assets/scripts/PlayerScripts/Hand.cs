using System.ComponentModel;
using System.Dynamic;
using UnityEngine;
public class Hand : MonoBehaviour
{
    private IItem _item;
    public int EatItem()
    {
        _item.die();
        _item = null;
        return 20;  //коли появляться нова їжа то додати перевірку
    }
    public void TakeItem(IItem item)
    {
        if(!IsEmpty()) return;

        this._item = item;
        item.OnTake(transform);
    }
    public void DropItem()
    {
        if(_item == null) return;

        Vector3 dropPosition = transform.position;
        _item.OnDrop(dropPosition);
        _item = null;
    }
    public void ThrowItem(Vector3 directionToThrow)
    {
        if(_item == null) return;

        Vector3 dropPosition = transform.position;
        _item.OnThrow(dropPosition, directionToThrow);
        _item = null;

    }
    public IItem GetItem()
    {
        return _item;
    }
    public bool IsEmpty()
    {
        return _item == null;
    }
}