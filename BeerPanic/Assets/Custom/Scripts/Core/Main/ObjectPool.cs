using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    private readonly Func<T> createFunc;
    private readonly Stack<T> pool;
    private readonly int maxSize;
    private readonly Action<T> onGetFromPool;
    private readonly Action<T> onReturnToPool;
    private readonly Action<T> onDestroyElement;

    public ObjectPool(Func<T> createFunc, int maxSize, Action<T> onGetFromPool = null, 
                     Action<T> onReturnToPool = null, Action<T> onDestroyElement = null)
    {
        this.createFunc = createFunc;
        this.maxSize = maxSize;
        this.onGetFromPool = onGetFromPool;
        this.onReturnToPool = onReturnToPool;
        this.onDestroyElement = onDestroyElement;
        pool = new Stack<T>(maxSize);
        
        for (int i = 0; i < maxSize; i++)
        {
            pool.Push(createFunc());
        }
    }

    public T Get()
    {
        T item = pool.Count > 0 ? pool.Pop() : createFunc();
        onGetFromPool?.Invoke(item);
        return item;
    }
    
    public void Return(T item)
    {
        if (pool.Count < maxSize)
        {
            onReturnToPool?.Invoke(item);
            pool.Push(item);
        }
        else
        {
            onDestroyElement?.Invoke(item);
        }
    }
}