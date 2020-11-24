using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    struct Position
    {
        private int index_x;
        private int index_y;
        public void SetPosition(int x, int y)
        {
            index_x = x;
            index_y = y;
        }

        public int GetPositionX()
        {
            return index_x;
        }

        public int GetPositionY()
        {
            return index_y;
        }

        public void UpdatePositionX(int value)
        {
            index_x += value;
        }

        public void UpdatePositionY(int value)
        {
            index_y += value;
        }
    };

    public GameObject MoveTile;
    public GameObject[,] AllTiles;
    private Dictionary<Position, bool> Occupied = new Dictionary<Position, bool>();
    private Position starting, current;
    private int remainingMovement = 3;
    private const int movement = 4;
    private const int size_x = 15, size_y = 9;
    private bool start = false;
    private bool control = false;
    private const float offset_z = -0.5f;

    //Enemy now has a reference for movement
    public void SetAllTiles(GameObject[,] grid)
    {
        AllTiles = grid;
        Debug.Log("This should play twice");
    }

    //Enemy now knows what Tiles are occupied.
    public void UpdateOccupiedTiles()
    {
        Occupied.Clear();
        Position temp = new Position();
        int x = GameObject.Find("Player").GetComponent<Player>().GetCurrentX();
        int y = GameObject.Find("Player").GetComponent<Player>().GetCurrentY();
        temp.SetPosition(x, y);
        Occupied.Add(temp, true);;
        x = GameObject.Find("Player2").GetComponent<Player>().GetCurrentX();
        y = GameObject.Find("Player2").GetComponent<Player>().GetCurrentY();
        temp.SetPosition(x, y);
        Occupied.Add(temp, true);
    }

    //Calls the proper methods based on the status of the Enemy
    public void SetControlStatus(bool value){
        control = value;
        if(value == false) {  
            UpdatePosition();
        }
    }

    void Update()
    {
        if (control == true)
        {

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                int compare_x = Mathf.Abs((current.GetPositionX() - 1) - starting.GetPositionX());
                int compare_y = Mathf.Abs(current.GetPositionY() - starting.GetPositionY());
                remainingMovement = movement - (compare_x + compare_y);
                if (remainingMovement > 0)
                {
                    if (current.GetPositionX() > 3)
                    {
                        int x = current.GetPositionX() - 1;
                        int y = current.GetPositionY();
                        Position p = new Position();
                        p.SetPosition(x, y);

                        if (!(Occupied.ContainsKey(p)))
                        {
                            Vector3 temp = AllTiles[current.GetPositionX() - 1, current.GetPositionY()].GetComponent<Grid>().transform.position;
                            transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                            current.UpdatePositionX(-1);
                        }
                        else if (Occupied.ContainsKey(p) == true)
                        {
                            if (remainingMovement > 1)
                            {
                                if (current.GetPositionX() > 4)
                                {

                                    Vector3 temp = AllTiles[current.GetPositionX() - 2, current.GetPositionY()].GetComponent<Grid>().transform.position;
                                    transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                                    current.UpdatePositionX(-2);
                                    remainingMovement--;
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                int compare_x = Mathf.Abs((current.GetPositionX() + 1) - starting.GetPositionX());
                int compare_y = Mathf.Abs(current.GetPositionY() - starting.GetPositionY());

                remainingMovement = movement - (compare_x + compare_y);
                if (remainingMovement > 0)
                {
                    if (current.GetPositionX() < size_x)
                    {
                        int x = current.GetPositionX() + 1;
                        int y = current.GetPositionY();
                        Position p = new Position();
                        p.SetPosition(x, y);

                        if (!(Occupied.ContainsKey(p)))
                        {
                            Vector3 temp = AllTiles[current.GetPositionX() + 1, current.GetPositionY()].GetComponent<Grid>().transform.position;
                            transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                            current.UpdatePositionX(1);
                        }
                        else if (Occupied.ContainsKey(p) == true)
                        {
                            if (remainingMovement > 1)
                            {
                                if (current.GetPositionX() < size_x - 1)
                                {

                                    Vector3 temp = AllTiles[current.GetPositionX() + 2, current.GetPositionY()].GetComponent<Grid>().transform.position;
                                    transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                                    current.UpdatePositionX(2);
                                    remainingMovement--;
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                int compare_x = Mathf.Abs(current.GetPositionX() - starting.GetPositionX());
                int compare_y = Mathf.Abs((current.GetPositionY() - 1) - starting.GetPositionY());
                remainingMovement = movement - (compare_x + compare_y);

                if (remainingMovement > 0)
                {
                    if (current.GetPositionY() > 0)
                    {

                        int x = current.GetPositionX();
                        int y = current.GetPositionY() - 1;
                        Position p = new Position();
                        p.SetPosition(x, y);

                        if (!(Occupied.ContainsKey(p)))
                        {
                            Vector3 temp = AllTiles[current.GetPositionX(), current.GetPositionY() - 1].GetComponent<Grid>().transform.position;
                            transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                            current.UpdatePositionY(-1);
                        }
                        else if (Occupied.ContainsKey(p) == true)
                        {
                            if (remainingMovement > 1)
                            {
                                if (current.GetPositionY() > 1)
                                {
                                    Debug.Log("Current.GetPositionY() is being called");
                                    Vector3 temp = AllTiles[current.GetPositionX(), current.GetPositionY() - 2].GetComponent<Grid>().transform.position;
                                    transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                                    current.UpdatePositionY(-2);
                                    remainingMovement--;
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                int compare_x = Mathf.Abs(current.GetPositionX() - starting.GetPositionX());
                int compare_y = Mathf.Abs((current.GetPositionY() + 1) - starting.GetPositionY());
                remainingMovement = movement - (compare_x + compare_y);
                if (remainingMovement > 0)
                {
                    if (current.GetPositionY() < size_y)
                    {
                        int x = current.GetPositionX();
                        int y = current.GetPositionY() + 1;
                        Position p = new Position();
                        p.SetPosition(x, y);

                        if (Occupied.ContainsKey(p) == false)
                        {
                            Vector3 temp = AllTiles[current.GetPositionX(), current.GetPositionY() + 1].GetComponent<Grid>().transform.position;
                            transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                            current.UpdatePositionY(1);
                        }
                        else if (Occupied.ContainsKey(p) == true)
                        {
                            if (remainingMovement > 1)
                            {
                                if (current.GetPositionY() < (size_y - 1))
                                {
                                    Vector3 temp = AllTiles[current.GetPositionX(), current.GetPositionY() + 2].GetComponent<Grid>().transform.position;
                                    transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                                    current.UpdatePositionY(2);
                                    remainingMovement--;

                                }
                            }
                        }
                    }
                }
            }
        }
    }

    //Updates the position of the enemy. Used when the enemy control is being switched.
    private void UpdatePosition()
    {
        starting.SetPosition(current.GetPositionX(), current.GetPositionY());
    }

    //Gets the Starting X Position value
    public int GetStartingX()
    {
        return starting.GetPositionX();
    }

    //Gets the Starting Y Position value
    public int GetStartingY()
    {
        return starting.GetPositionY();
    }

    //Gets the Current X Position value
    public int GetCurrentX()
    {
        return current.GetPositionX();
    }

    //Gets the Current Y Position value
    public int GetCurrentY()
    {
        return current.GetPositionY();
    }

    //Gets position on the grid using a trigger method.
    private void OnTriggerEnter(Collider other)
    {
        int x = other.gameObject.GetComponent<Grid>().GetIndexX();
        int y = other.gameObject.GetComponent<Grid>().GetIndexY();

        current.SetPosition(x, y);

        transform.position = new Vector3(other.transform.position.x, other.transform.position.y, -1.0f);
        if (start == false)
        {
            starting.SetPosition(x, y);
            start = true;
        }
    }

}
