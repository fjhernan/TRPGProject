using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    struct Position {
        private int index_x;
        private int index_y;

        public Position(int x, int y) {
            index_x = y;
            index_y = x;
        }

        public void SetPosition(int x, int y) {
            index_x = x;
            index_y = y;
        }

        public int GetPositionX() {
            return index_x;
        }

        public int GetPositionY() {
            return index_y;
        }

        public void UpdatePositionX(int value) {
            index_x = value;
        }

        public void UpdatePositionY(int value) {
            index_y = value;
        }
    };

    public GameObject[,] AllTiles;
    public GameObject CombatManager;
    public int hp, atk, def;
    private Dictionary<string, Position> AllPlayers = new Dictionary<string, Position>();
    private Position starting, current;
    private int remainingMovement = 3;
    private bool start = false;
    private bool control = false;
    //private string targetName;
    //private bool found = false;
    private Dictionary<Position, string> Occupied = new Dictionary<Position, string>();
    List<Position> Pathing = new List<Position>();
    private Position CurrentTarget;
    private int PathingIndex = 0;

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
                //Target player 2
                CurrentTarget.SetPosition(x1, y1);
        //        targetName = "Player2";
                //Debug.Log("Current target is " + Occupied[CurrentTarget]);
            }
            else if(d1 > d2){
                //Target player 1
      //          targetName = "Player1";
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
        control = true;
        SetCurrentTarget();
        SearchBestPath();
    }

    private void Update(){
        if(control == true){
            if (remainingMovement > 0){
                Debug.Log("Remaining movement is " + remainingMovement);
                //if (PathingIndex < Pathing.Count)
                if(Pathing.Count != 0)
                {
                    Debug.Log("PathingIndex" + PathingIndex + " PathingSize " + Pathing.Count);
                    Vector3 temp = AllTiles[Pathing[0].GetPositionX(), Pathing[0].GetPositionY()].transform.position;
                    temp = new Vector3(temp.x, temp.y, -1.0f);
                    if (transform.position != temp)
                    {
                        //Debug.Log("Moving to Position");
                        transform.position = Vector3.MoveTowards(transform.position, temp, 0.5f * Time.deltaTime);
                    }
                    else if (transform.position == temp)
                    {
                        //Debug.Log("Position reached");
                        current.SetPosition(Pathing[PathingIndex].GetPositionX(), Pathing[PathingIndex].GetPositionY());
                        remainingMovement--;
                        //PathingIndex++;
                        Pathing.RemoveAt(0);
                    }
                }
                else if(Pathing.Count == 0){
                    if (control == true)
                    {
                        //Reached the end of the array. Target found.
                        remainingMovement = 3;
                        if (Occupied.ContainsKey(CurrentTarget)){
                            Debug.Log("Reached Target. Initiating Combat");
                            CombatManager.GetComponent<CombatManager>().CombatBegin(Occupied[CurrentTarget], gameObject.name, false);
                            Debug.Log("Combat has finished");
                        }

                        UpdatePosition();
                        Pathing.Clear();
                        control = false;
                        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().UpdateCurrentCharacterControl();
                    }
                }
            }
            else{
                if (control == true)
                {
                    if(Pathing.Count == 0){
                        //Debug.Log("Target found");
                        Debug.Log("Reached Target. Initiating Combat");
                        CombatManager.GetComponent<CombatManager>().CombatBegin(Occupied[CurrentTarget], gameObject.name, false);
                        Debug.Log("Combat has finished");
                    }
                    Debug.Log("Does the turn end? Yes");
                    remainingMovement = 3;
                    UpdatePosition();
                    Pathing.Clear();
                    control = false;
                    GameObject.Find("EnemyManager").GetComponent<EnemyManager>().UpdateCurrentCharacterControl();
                }
            }
        }
    }

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

        //int i = 6;
        while(true){
            if (CheckAdjacentTiles(p) != 0){
                Pathing.RemoveAt(Pathing.Count - 1);
                return;
            }
            if (CurrentTarget.GetPositionY() < p.GetPositionY() && CheckAdjacentTiles(p) != 5)
            {
                Debug.Log("Up");
                t.SetPosition(p.GetPositionX(), p.GetPositionY() - 1);
                Pathing.Add(t);
                p.SetPosition(t.GetPositionX(), t.GetPositionY());
            }
            if (CurrentTarget.GetPositionY() > p.GetPositionY() && CheckAdjacentTiles(p) != 4)
            {
                //Debug.Log("Down");
                t.SetPosition(p.GetPositionX(), p.GetPositionY() + 1);
                Pathing.Add(t);
                p.SetPosition(t.GetPositionX(), t.GetPositionY());
            }
            if (p.GetPositionX() > CurrentTarget.GetPositionX() && CheckAdjacentTiles(p) != 3)
            {
                //Debug.Log("Right");
                t.SetPosition(p.GetPositionX() - 1, p.GetPositionY());
                Pathing.Add(t);
                p.SetPosition(p.GetPositionX() - 1, p.GetPositionY());
            }
            if (p.GetPositionX() < CurrentTarget.GetPositionX() && CheckAdjacentTiles(p) != 2)
            {
                //Debug.Log("Left");
                t.SetPosition(p.GetPositionX() + 1, p.GetPositionY());
                Pathing.Add(t);
                p.SetPosition(t.GetPositionX(), t.GetPositionY());
            }
        };
    }

    private int CheckAdjacentTiles(Position p){
        Position temp = new Position();
        //Checks current tile
        if(Occupied.ContainsKey(p) == true){
            return 1;
        }

        //Checks left
        temp.SetPosition(p.GetPositionX() + 1, p.GetPositionY());
        if(Occupied.ContainsKey(p) == true){
            return 2;
        }
        //Checks right
        temp.SetPosition(p.GetPositionX() - 1, p.GetPositionY());
        if(Occupied.ContainsKey(p) == true){
            return 3;
        }
        //Checks below
        temp.SetPosition(p.GetPositionX(), p.GetPositionY() + 1);
        if(Occupied.ContainsKey(p) == true){
            return 4;
        }
        //Checks above
        temp.SetPosition(p.GetPositionX(), p.GetPositionY() - 1);
        if(Occupied.ContainsKey(p) == true){
            return 5;
        }

        return 0;
    }

    //Updates the position of the enemy. Used when the enemy control is being switched.
    private void UpdatePosition()
    {
        starting.SetPosition(current.GetPositionX(), current.GetPositionY());
    }

    public void TookDamage(int NewHP){
        hp = NewHP;
        if (hp <= 0){
            Debug.Log(gameObject.name + " has died.");
        }
        else{
            Debug.Log(gameObject.name + " is still alive.");
        }
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

    //Gets the stats
    public int GetHP(){
        return hp;
    }

    public int GetAtk(){
        return atk;
    }

    public int GetDef(){
        return def;
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
