using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    private int index_x;
    private int index_y;
    private bool outside = false;
    /*
    public void SetIndexPosition(int x, int y) {
        index_x = x;
        index_y = y;
        Debug.Log("Index x is " + index_x + "\nIndex y is " + index_y);
        CheckIfOutside();
    }

    public void UpdateIndexPosition(int x, int y){
        index_x += x;
        index_y += y;
        CheckIfOutside();
    }

    private void CheckIfOutside()
    {
        if (index_x < 3 || index_x > 13 || index_y < 0 || index_y > 9){
            GetComponent<SpriteRenderer>().enabled = false;
            outside = false;
        }
        else if((index_x >= 3 && index_x <= 17) && (index_y >= 0 && index_y <= 9)){
            GetComponent<SpriteRenderer>().enabled = true;
            outside = true;
        }
    }

    public bool OutsideStatus(){
        //if(outside == true){
        //    return true;
        //}
        return outside;//false;
    }

    public int GetIndexX(){
        return index_x;
    }

    public int GetIndexY(){
        return index_y;    
    }*/
}
