using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[]items;
    int curentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }
    public void Add(T item)
    {
        item.HeapIndex = curentItemCount;
        items[curentItemCount] = item;
        SortUp(item);
        curentItemCount++;
    }
    /*
    * присвоюєм в перемінну 0 елемент
    * зменшуєм число елементів на один
    * присвоюєм в 0 елемент останній елемент і міняєм йому індекс на 0
    * Сортуєм 0 елемент і вертаємо його
    */
    public T RemoveFirst()
    {
        T FirstItem = items[0];
        curentItemCount--;
        items[0] = items[curentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return FirstItem;
    }
    private void Swap(T objA, T objB)
    {
        items[objA.HeapIndex] = objB;
        items[objB.HeapIndex] = objA;
        int AIndex = objA.HeapIndex;
        objA.HeapIndex = objB.HeapIndex;
        objB.HeapIndex = AIndex;
    }
    public void UpdateItem(T item)
    {
        SortUp(item);
    }
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex],item);
    }
    public int Count{
        get{
            return curentItemCount;
        }
    }
    /*
    * запускаєм цикл вілл
    *   створюємо дві перемінні для лівого і правого нащадка
    *   створюєм перемінну для індекса йкий будем свапати
    *   якщо індекс лівого менший за число елеметів в масиві 
    *     свап індекс = індесу лівого
    *     якщо теж саме зправим нащадком
    *         порівнюємо чи лівий більше правого і якщо так то свайп індек = правий
    *   
    *   порівнюєм чи більший конкретний елемент за вибраний нами дочірній і свайпаєм 
    */
    private void SortDown(T item)
    {
        // перевіряє дітей конкретного елемента і сортує їх по пріоритету.
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex;

            if(childIndexLeft < curentItemCount)
            {
                swapIndex = childIndexLeft;

                if(childIndexRight < curentItemCount)
                {
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                        swapIndex = childIndexRight;
                }

                if(item.CompareTo(items[swapIndex]) < 0)
                    Swap(item,items[swapIndex]);
                else 
                    return;
            }
            else
                return;
            
        }
    }
    private void SortUp(T item)
    {
        //перевіряє батьківський елемент через конкретний дочірній і свайпає

        while (true)
        {
            int parentIndex = (item.HeapIndex - 1) / 2; // перенесення цього за межі цикла робе результат інтереснішим
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else 
                break;
        }
    }
}

public interface IHeapItem<T> :IComparable<T>
{
    int HeapIndex { get; set; }
}
