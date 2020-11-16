using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private int index_x = 0;
    private int index_y = 0;

    public void SetIndex(int x, int y){
        index_x = x;
        index_y = y;
    }

    private void OnTriggerEnter(Collider other){
        Debug.Log("Index x: " + index_x + "\nIndex y: " + index_y);
    }
}
