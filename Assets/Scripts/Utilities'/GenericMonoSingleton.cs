using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericMonoSingleton<T> : MonoBehaviour where T :GenericMonoSingleton<T>

{
    private static  T  instance;
    public static T Instanec {  get { return instance; } }


    public void Awake()
    {
        if(instance== null)
        {
            instance = this as T;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
