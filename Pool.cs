///
/// Simple pooling for Unity.
///   Author: Martin "quill18" Glaude (quill18@quill18.com)
///   Latest Version: https://gist.github.com/quill18/5a7cfffae68892621267
///   License: CC0 (http://creativecommons.org/publicdomain/zero/1.0/)
///   UPDATES:
/// 	2015-04-16: Changed Pool to use a Stack generic.
/// 
/// Usage:
/// 
///   There's no need to do any special setup of any kind.
/// 
///   Instead of calling Instantiate(), use this:
///       SimplePool.Spawn(somePrefab, somePosition, someRotation);
/// 
///   Instead of destroying an object, use this:
///       SimplePool.Despawn(myGameObject);
/// 
///   If desired, you can preload the pool with a number of instances:
///       SimplePool.Preload(somePrefab, 20);
/// 
/// Remember that Awake and Start will only ever be called on the first instantiation
/// and that member variables won't be reset automatically.  You should reset your
/// object yourself after calling Spawn().  (i.e. You'll have to do things like set
/// the object's HPs to max, reset animation states, etc...)
/// 
/// 
/// 

using System.Collections.Generic;
using UnityEngine;

public class Pool<T> where T : MonoBehaviour
{
    private const int DEFAULT_POOL_SIZE = 3;

    private int nextId = 1;
    private Stack<T> inactive;
    private readonly T prefab;

    public Pool(T prefab, int initialQty = DEFAULT_POOL_SIZE)
    {
        this.prefab = prefab;

        // If Stack uses a linked list internally, then this
        // whole initialQty thing is a placebo that we could
        // strip out for more minimal code. But it can't *hurt*.
        inactive = new Stack<T>(initialQty);
    }

    public T Spawn(Vector3 pos, Quaternion rot, Transform parent)
    {
        T obj;
        if (inactive.Count == 0)
        {
            // We don't have an object in our pool, so we
            // instantiate a whole new object.
            obj = (T)GameObject.Instantiate(prefab, pos, rot);
            obj.name = prefab.name + " (" + (nextId++) + ")";

        }
        else
        {
            // Grab the last object in the inactive array
            obj = inactive.Pop();

            if (obj == null)
            {
                // The inactive object we expected to find no longer exists.
                // The most likely causes are:
                //   - Someone calling Destroy() on our object
                //   - A scene change (which will destroy all our objects).
                //     NOTE: This could be prevented with a DontDestroyOnLoad
                //	   if you really don't want this.
                // No worries -- we'll just try the next one in our sequence.

                return Spawn(pos, rot, parent);
            }
        }

        obj.transform.position = pos;
        obj.transform.rotation = rot;
        obj.transform.SetParent(parent);
        obj.gameObject.SetActive(true);
        return obj;

    }

    // Return an object to the inactive pool.
    public void Despawn(T obj)
    {
        obj.gameObject.SetActive(false);

        // Since Stack doesn't have a Capacity member, we can't control
        // the growth factor if it does have to expand an internal array.
        // On the other hand, it might simply be using a linked list 
        // internally.  But then, why does it allow us to specify a size
        // in the constructor? Maybe it's a placebo? Stack is weird.
        inactive.Push(obj);
    }
}
