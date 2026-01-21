using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public sealed class FastHeap<T> where T : class, IHeapItem<T>
{
    private const int D = 4; 
    private T[] items;
    private int count;

    public FastHeap(int initialCapacity = 32)
    {
        if (initialCapacity <= 0) initialCapacity = 32;
        items = new T[initialCapacity];
        count = 0;
    }

    public int Count => count;
    public bool IsEmpty => count == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Add(T item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        EnsureCapacity(count + 1);

        int idx = count;
        items[idx] = item;
        item.HeapIndex = idx;
        SiftUp(item);
        count++;
    }
    public T RemoveFirst()
    {
        if (count == 0) throw new InvalidOperationException("Heap is empty.");

        T root = items[0];
        int last = --count;

        if (last >= 0)
        {
            T tail = items[last];
            items[last] = null;
            if (last > 0)
            {
                items[0] = tail;
                tail.HeapIndex = 0;
                SiftDown(tail);
            }
            else
            {
                items[0] = null;
            }
        }

        root.HeapIndex = -1;
        return root;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryRemoveFirst(out T item)
    {
        if (count == 0) { item = null; return false; }
        item = RemoveFirst();
        return true;
    }

    /// <summary>
    /// Repositions an item after its priority changed.
    /// Works for both increase-key and decrease-key.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void UpdateItem(T item)
    {
        // Try both directions—one of them will exit immediately.
        SiftUp(item);
        SiftDown(item);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Contains(T item)
    {
        int idx = item?.HeapIndex ?? -1;
        return idx >= 0 && idx < count && ReferenceEquals(items[idx], item);
    }

    // ----------------- Core heap ops -----------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SiftUp(T item)
    {
        int idx = item.HeapIndex;
        while (idx > 0)
        {
            int parent = (idx - 1) / D;
            T p = items[parent];
            if (item.CompareTo(p) > 0)
            {
                Swap(item, p);
                idx = item.HeapIndex;
            }
            else break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void SiftDown(T item)
    {
        int idx = item.HeapIndex;

        while (true)
        {
            int firstChild = idx * D + 1;
            if (firstChild >= count) break;

            // Find best child among up to D children
            int best = firstChild;
            T bestItem = items[best];

            // Unroll small loop for D=4
            int c2 = firstChild + 1;
            if (c2 < count)
            {
                T i2 = items[c2];
                if (bestItem.CompareTo(i2) < 0) { best = c2; bestItem = i2; }
            }
            int c3 = firstChild + 2;
            if (c3 < count)
            {
                T i3 = items[c3];
                if (bestItem.CompareTo(i3) < 0) { best = c3; bestItem = i3; }
            }
            int c4 = firstChild + 3;
            if (c4 < count)
            {
                T i4 = items[c4];
                if (bestItem.CompareTo(i4) < 0) { best = c4; bestItem = i4; }
            }

            if (item.CompareTo(bestItem) < 0)
            {
                Swap(item, bestItem);
                idx = item.HeapIndex;
            }
            else break;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void Swap(T a, T b)
    {
        int ia = a.HeapIndex;
        int ib = b.HeapIndex;

        items[ia] = b;
        items[ib] = a;

        a.HeapIndex = ib;
        b.HeapIndex = ia;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureCapacity(int needed)
    {
        if (needed <= items.Length) return;
        int newCap = items.Length < 32 ? 64 : items.Length * 2;
        if (newCap < needed) newCap = needed;
        Array.Resize(ref items, newCap);
    }
}

