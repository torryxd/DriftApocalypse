using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyInSeconds : MonoBehaviour
{
    public float timeToDestroyObject = 0;
    public Component component;
    public float timeToDestroyComponent = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(component, timeToDestroyComponent);
        Destroy(this.gameObject, timeToDestroyObject);
    }
}
