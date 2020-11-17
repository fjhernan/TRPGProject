using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject MoveTile;
    public GameObject[,] AllTiles;
    //private ArrayList Tiles = new ArrayList();
    private int start_x = 0, start_y = 0, current_x = 0, current_y = 0;
    private int remainingMovement = 3;
    private const int movement = 4;
    //private int size_x = 13, size_y = 9;
    private bool start = false;

    public void SetAllTiles(GameObject[,] grid){
        AllTiles = grid;
    }

    void Update()
    {   
        if(Input.GetKeyDown(KeyCode.RightArrow)){
            int compare_x = Mathf.Abs((current_x-1) - start_x);
            int compare_y = Mathf.Abs(current_y - start_y);
            remainingMovement = movement - (compare_x + compare_y);
            if(remainingMovement > 0){
                if (current_x > 1)
                {
                    Vector3 temp = AllTiles[current_x - 1, current_y].GetComponent<Grid>().transform.position;
                    transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                    current_x--;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            int compare_x = Mathf.Abs((current_x + 1) - start_x);
            int compare_y = Mathf.Abs(current_y - start_y);
            remainingMovement = movement - (compare_x + compare_y);
            if (remainingMovement > 0)
            {
                if (current_x < 13)
                {
                    Vector3 temp = AllTiles[current_x + 1, current_y].GetComponent<Grid>().transform.position;
                    transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                    current_x++;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            int compare_x = Mathf.Abs(current_x - start_x);
            int compare_y = Mathf.Abs((current_y - 1) - start_y);
            remainingMovement = movement - (compare_x + compare_y);
            if (remainingMovement > 0)
            {
                if (current_y > 0)
                {
                    Vector3 temp = AllTiles[current_x, current_y - 1].GetComponent<Grid>().transform.position;
                    transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                    current_y--;
                }
            }    
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            int compare_x = Mathf.Abs(current_x - start_x);
            int compare_y = Mathf.Abs((current_y + 1) - start_y);
            remainingMovement = movement - (compare_x + compare_y);
            if (remainingMovement > 0)
            {
                if (current_y < 9) { 
                Vector3 temp = AllTiles[current_x, current_y + 1].GetComponent<Grid>().transform.position;
                transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                current_y++;
                }
            }
        }
    }

    private void CreateMovementHighlight(){
        GameObject temp;
        int x = start_x + movement;
        int y = start_y + movement;
        //int left_x = movement;
        int left_y = movement;
        if(x > 13){
            //left_x = (x - start_x) - movement;
            //Debug.Log(x);
        }
        if(y > 9){
            left_y = y - 9;
            //Debug.Log(y);
        }
        //Debug.Log(y);
        //Debug.Log("x" + left_x);
        Debug.Log("y" + left_y);
        for(int i = 0; i <= left_y; i++){
            temp = GameObject.Instantiate(MoveTile, AllTiles[start_x, start_y + i].transform);
            temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, -0.5f);
        }
    }

    private void OnTriggerEnter(Collider other){
        int x = other.gameObject.GetComponent<Grid>().GetIndexX();
        int y = other.gameObject.GetComponent<Grid>().GetIndexY();

        if(start_x == 0 && start_y == 0){
            start_x = x;
            start_y = y;
            current_x = x;
            current_y = y;
        }

        transform.position = new Vector3(other.transform.position.x, other.transform.position.y, -1.0f);
        if (start == false){
            CreateMovementHighlight();
            start = true;
        }
    }
}
