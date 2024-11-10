using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
{
    private readonly Func<T> createFunc;
    private readonly Stack<T> pool;
    private readonly int maxSize;
    private Func<GameObject> createPooledElement;
    private Action<GameObject> onGetElementFromPool;
    private Action<GameObject> onReturnElementToPool;
    private Action<GameObject> onDestroyPoolElement;
    private int poolSize;

    public ObjectPool(Func<T> create, int size, Action<UnityEngine.GameObject> onReturnElementToPool, Action<UnityEngine.GameObject> onDestroyPoolElement, int poolSize)
    {
        createFunc = create;
        maxSize = size;
        pool = new Stack<T>(size);
        
        for (int i = 0; i < size; i++)
        {
            pool.Push(createFunc());
        }
    }

    public ObjectPool(Func<GameObject> createPooledElement, Action<GameObject> onGetElementFromPool, Action<GameObject> onReturnElementToPool, Action<GameObject> onDestroyPoolElement, int poolSize)
    {
        this.createPooledElement = createPooledElement;
        this.onGetElementFromPool = onGetElementFromPool;
        this.onReturnElementToPool = onReturnElementToPool;
        this.onDestroyPoolElement = onDestroyPoolElement;
        this.poolSize = poolSize;
    }

    public T Get()
    {
        if (pool.Count > 0)
        {
            return pool.Pop();
        }
        return createFunc();
    }
    
    public void Return(T item)
    {
        if (pool.Count < maxSize)
        {
            pool.Push(item);
        }
    }
}