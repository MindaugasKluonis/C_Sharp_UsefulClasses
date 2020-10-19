using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static bool _destroyCheck = false;

    protected static T _instance;
    public static T Instance
    {

        get
        {
            if (_destroyCheck)
            {
                Debug.LogWarning("Singleton " + typeof(T) + " is destroyed!");
                return null;
            }

            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    _instance = obj.AddComponent<T>();
                    obj.name = typeof(T).ToString() + " (Singleton)";

                    DontDestroyOnLoad(obj);
                }
            }

            return _instance;
        }

    }

    private void OnApplicationQuit()
    {
        _destroyCheck = true;
    }


    private void OnDestroy()
    {
        _destroyCheck = true;
    }
}