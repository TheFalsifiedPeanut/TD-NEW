using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DetectWorker : MonoBehaviour
{
    Action<Worker> detectWorker;
    

    public void SubscribeToDetectWorker(Action<Worker> detectWorker)
    {
        this.detectWorker += detectWorker;
    }
    public void UnsubscribeToDetectWorker(Action<Worker> detectWorker)
    {
        this.detectWorker -= detectWorker;
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Worker>())
        {
            detectWorker?.Invoke(collision.GetComponent<Worker>());          
        }   
    }
}
