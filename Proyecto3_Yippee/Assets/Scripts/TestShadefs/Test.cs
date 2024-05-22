using UnityEngine;

public class Test : MonoBehaviour
{    
    void Update()
    {
        Debug.Log(Camera.main.WorldToScreenPoint(transform.position));
    }
}
