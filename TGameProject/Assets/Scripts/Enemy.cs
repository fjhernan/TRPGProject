using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    struct Position{
        private int index_x;
        private int index_y;

        public Position(int x, int y){
            index_x = y;
            index_y = x;
        }

        public void SetPosition(int x, int y){
            index_x = x;
            index_y = y;
        }

        public int GetPositionX(){
            return index_x;
        }

        public int GetPositionY(){
            return index_y;
        }

        public void UpdatePositionX(int value){
            index_x = value;
        }

        public void UpdatePositionY(int value){
            index_y = value;
        }
    };

    public GameObject MoveTile;
    public GameObject[,] AllTiles;
    private Dictionary<string, Position> AllPlayers = new Dictionary<string, Position>();
    private Position starting, current;
    //private int remainingMovement = 3;
    //private const int movement = 4;
    //private const int size_x = 15, size_y = 9;
    private bool start = false;
    //private bool control = false;
    //private bool found = false;
    private Dictionary<Position, string> Occupied = new Dictionary<Position, string>();
    List<Position> Pathing = new List<Position>();
    //private List<Position> Occupied = new List<Position>();
    private Position CurrentTarget;
    //private const float offset_z = -0.5f;

    //Enemy now has a reference for movement
    public void SetAllTiles(GameObject[,] grid){
        AllTiles = grid;
    }

    private void SetCurrentTarget(){
        List<Position> PlayerPositions = new List<Position>();
        
        foreach(KeyValuePair<Position, string> p in Occupied){
            PlayerPositions.Add(p.Key);
        }
        
        CurrentTarget.SetPosition(PlayerPositions[0].GetPositionX(), PlayerPositions[0].GetPositionY());
        
        int x1, y1, x2, y2, compare_x, compare_y, d1, d2;
        
        for(int i = 0; i < PlayerPositions.Count - 1; i++){
            x1 = PlayerPositions[i].GetPositionX();
            y1 = PlayerPositions[i].GetPositionY();

            compare_x = Mathf.Abs(x1 - starting.GetPositionX());
            compare_y = Mathf.Abs(y1 - starting.GetPositionY());
            d1 = compare_x + compare_y;

            x2 = PlayerPositions[i + 1].GetPositionX();
            y2 = PlayerPositions[i + 1].GetPositionY();
            
            compare_x = Mathf.Abs(x2 - starting.GetPositionX());
            compare_y = Mathf.Abs(y2 - starting.GetPositionY());
            d2 = compare_x + compare_y;
            Debug.Log("Player 1 dis " + d1 + " Player 2 dis" + d2 ); 
            if (d2 > d1){
                CurrentTarget.SetPosition(x1, y1);
                //Debug.Log("Current target is " + Occupied[CurrentTarget]);
            }
            else if(d1 > d2){
                CurrentTarget.SetPosition(x2, y2);
                //Debug.Log("Current target is " + Occupied[CurrentTarget]);
            }
        }
        //SearchBestPath();
    }

    public void AddNewTarget(GameObject Player){
        Position temp = new Position();
        temp.SetPosition(Player.GetComponent<Player>().GetCurrentX(), Player.GetComponent<Player>().GetCurrentY());
        AllPlayers.Add(Player.name, temp);
    }

    //Enemy now knows what Tiles are occupied.
    public void UpdateOccupiedTiles(){
        Occupied.Clear();
        //Pathing.Clear();

        //Debug.Log("Pathing is empty now " + Pathing.Count);

        Position temp = new Position();
        Player tPlayer = GameObject.Find("Player").GetComponent<Player>();

        int x = tPlayer.GetCurrentX();
        int y = tPlayer.GetCurrentY();

        temp.SetPosition(x, y);
        //Debug.Log("Player 1 pos x " + x + " y " + y);
        Occupied.Add(temp, "Player");
        AllPlayers[tPlayer.name] = temp;

        tPlayer = GameObject.Find("Player2").GetComponent<Player>();
        x = tPlayer.GetCurrentX();
        y = tPlayer.GetCurrentY();
        //Debug.Log("Player 2 pos x " + x + " y " + y);
        temp.SetPosition(x, y);
        
        Occupied.Add(temp, "Player2");
        AllPlayers[tPlayer.name] = temp;
        //SetCurrentTarget();
    }

    public void Movement(){
        SetCurrentTarget();
        SearchBestPath();
        Debug.Log("Current Target is " + Occupied[CurrentTarget]);
        Debug.Log("Pathing is " + Pathing.Count);
        Pathing.Clear();

        UpdatePosition();
        //remainingMovement = 3;
        //Debug.Log("Movement");
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().UpdateCurrentCharacterControl();
        //Debug.Log("Enemy manager called");
    }

    //Calls the proper methods based on the status of the Enemy
    /*
            Position p = new Position();
            
            while(Pathing.Count > 0){
                p = Pathing[0];
                Pathing.RemoveAt(0);
                Vector3 temp = AllTiles[p.GetPositionX(), p.GetPositionY()].transform.position;
                transform.position = new Vector3(temp.x, temp.y, -0.5f);
                current.SetPosition(p.GetPositionX(), p.GetPositionY());
                remainingMovement--;
                if (remainingMovement == 0){
                    break;
               }
            }
            if(Pathing.Count == 0)
                remainingMovement = 0;

            if(remainingMovement == 0){
                UpdatePosition();
                GameObject.Find("EnemyManager").GetComponent<EnemyManager>().UpdateCurrentCharacterControl();
    }*/
    
    private void SearchBestPath(){
        Pathing.Clear();
        Position p = new Position();
        Position t = new Position();
        
        p.SetPosition(starting.GetPositionX(), starting.GetPositionY());
        Debug.Log("Starting x " + starting.GetPositionX() + " y " + starting.GetPositionY());
        Debug.Log("Target x " + CurrentTarget.GetPositionX() + " y " + CurrentTarget.GetPositionY());
        if(CheckAdjacentTiles(p) != 0){
            return;
        }

        int i = 6;
        while(true){
            if (CheckAdjacentTiles(p) != 0){
                //Debug.Log("Target has been found. loop broken");
                return;
            }
            if (CurrentTarget.GetPositionY() < p.GetPositionY())
            {
                Debug.Log("Up");
                t.SetPosition(p.GetPositionX(), p.GetPositionY() - 1);
                Pathing.Add(t);
                p.SetPosition(t.GetPositionX(), t.GetPositionY());
            }
            if (CurrentTarget.GetPositionY() > p.GetPositionY()){
                //Debug.Log("Down");
                t.SetPosition(p.GetPositionX(), p.GetPositionY() + 1);
                Pathing.Add(t);
                p.SetPosition(t.GetPositionX(), t.GetPositionY());
            }
            if (p.GetPositionX() > CurrentTarget.GetPositionX())
            {
                Debug.Log("Right");
                Debug.Log("Moving to space " + (p.GetPositionX() - 1) + " " + p.GetPositionY());
                t.SetPosition(p.GetPositionX() - 1, p.GetPositionY());
                Pathing.Add(t);
                p.SetPosition(p.GetPositionX() - 1, p.GetPositionY());
            }
            if (p.GetPositionX() < CurrentTarget.GetPositionX())
            {
                //Debug.Log("Left");
                t.SetPosition(p.GetPositionX() + 1, p.GetPositionY());
                Pathing.Add(t);
                p.SetPosition(t.GetPositionX(), t.GetPositionY());
            }


            i++;
            //Debug.Log("Loop continues");
            if(i > 14){
              //  Debug.Log("Breaking out of infinite loop");
                return;
            }
        };

    }

    private int CheckAdjacentTiles(Position p){
        Position temp = new Position();
        //Checks left
        if(Occupied.ContainsKey(p) == true){
            return 5;
        }

        temp.SetPosition(p.GetPositionX() + 1, p.GetPositionY());
        if(Occupied.ContainsKey(p) == true){
            return 1;
        }
        //Checks right
        temp.SetPosition(p.GetPositionX() - 1, p.GetPositionY());
        if(Occupied.ContainsKey(p) == true){
            return 2;
        }
        //Checks below
        temp.SetPosition(p.GetPositionX(), p.GetPositionY() + 1);
        if(Occupied.ContainsKey(p) == true){
            return 3;
        }
        //Checks above
        temp.SetPosition(p.GetPositionX(), p.GetPositionY() - 1);
        if(Occupied.ContainsKey(p) == true){
            return 4;
        }

        return 0;
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

        transform.position = new Vector3(other.transform.position.x, other.transform.position.y, -1.0f);
        if (start == false)
        {
            starting.SetPosition(x, y);
            start = true;
            current.SetPosition(x, y);
        }
    }

}
