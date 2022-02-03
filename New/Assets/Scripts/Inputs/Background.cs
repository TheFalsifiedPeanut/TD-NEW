using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField] List<GameObject> UIElements;

    private void OnMouseDown()
    {
        for (int i = 0; i < UIElements.Count; i++)
        {
            UIElements[i].SetActive(false);
        }
    }
}
