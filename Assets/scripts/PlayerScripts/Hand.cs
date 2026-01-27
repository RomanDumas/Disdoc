using System.ComponentModel;
using System.Dynamic;
using System.Collections.Generic;
using UnityEngine;
public class Hand : MonoBehaviour
{   
    [SerializeField] public static int maxCapacity = 5;
    private List<IItem> _itemList = new List<IItem>(maxCapacity);
    public void AddItemToList(IItem item)
    {
        if(_itemList.Count <= maxCapacity)
            _itemList.Add(item);
        else
            Debug.Log("Max capacity of hand");
    }
    public IItem GetLastItem()
    {
        return _itemList[_itemList.Count-1];
    }
    public void RemoveLastItem()
    {
        _itemList.RemoveAt(_itemList.Count-1);
    }
    public void TakeItem(IItem item)
    {
        AddItemToList(item);
        item.OnTake(transform);
    }
    public void DropItem()
    {
        IItem itemToDrop = GetLastItem();
        RemoveLastItem();

        Vector3 dropPosition = transform.position;
        itemToDrop.OnDrop(dropPosition);
    }
    public void ThrowItem(Vector3 directionToThrow)
    {
        IItem itemToThrow = GetLastItem();
        RemoveLastItem();

        Vector3 dropPosition = transform.position;
        itemToThrow.OnThrow(dropPosition, directionToThrow);
    }
    public bool IsEmpty()
    {
        return _itemList.Count == 0;
    }
    public List<IItem> GetItemList()
    {
        return _itemList;
    }
    public void SetItemList(List<IItem> itemList)
    {
        _itemList = itemList;
    }
    public void clearItemList()
    {
        _itemList.Clear();
    }
    public bool isFull()
    {
        return _itemList.Count == maxCapacity;
    }
    public bool CanBeTaken(IItem newItem)
    {
        if (_itemList.Count == 0)
            return true;

        IItem existingItem = _itemList[0];

        return existingItem.GetType() == newItem.GetType();
    }
}