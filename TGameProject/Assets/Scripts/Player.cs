using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    struct Position{
        private int index_x;
        private int index_y;
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
    };

    public GameObject MoveTile; //To be instantiated
    public GameObject[,] AllTiles; //The grid tiles
    public List<GameObject> AllSceneEnemies = new List<GameObject>();
    public List<GameObject> AllOtherPlayers = new List<GameObject>();
    public GameObject UIManager;
    public GameObject CombatManager;
    public GameObject PartyManager;
    public int hp, atk, def;
    private ArrayList HighlightTiles = new ArrayList();//Array of MoveTile
    //private List<Position> Occupied = new List<Position>();//Array that holds the positions of all occupied tiles
    private Dictionary<Position, string> Occupied = new Dictionary<Position, string>();
    private Position starting, current; //Hold indexes
    private Position target;
    private int remainingMovement = 3;
    private const int movement = 4;//Total movement. Should not be changed at all
    private const int min_size_x = 3, size_x = 15, size_y = 9;//Max movable areas in the grid
    private bool start = false; //Used by functions that should only work once
    private bool control = false;//Determines if the player can be controlled
    private const float offset_z = -0.5f;//Used when instantiating MoveTile
    private bool combatAllowed = false;

    //Player now has a reference for movement
    public void SetAllTiles(GameObject[,] grid){
        AllTiles = grid;
        //Debug.Log("This should play twice");
    }
    
    //PlayerObject now knows what Tiles are occupied.
    public void UpdateOccupiedTiles(){
        Occupied.Clear();
        Position temp = new Position();
        int x;
        int y;
        
        //Get the position of all other players currently in the grid
        foreach(GameObject play in AllOtherPlayers){
            if (play.activeSelf == true)
            {
                x = play.GetComponent<Player>().GetCurrentX();
                y = play.GetComponent<Player>().GetCurrentY();
                temp.SetPosition(x, y);
                Occupied.Add(temp, play.gameObject.name);
            }
        }

        //Get the position of all other enemies currently in the grid
        foreach(GameObject enem in AllSceneEnemies){
            if (enem.activeSelf == true)
            {
                x = enem.GetComponent<Enemy>().GetCurrentX();
                y = enem.GetComponent<Enemy>().GetCurrentY();
                temp.SetPosition(x, y);
                Occupied.Add(temp, enem.gameObject.name);
            }
        }
    }

    //Calls the proper methods based on the status of the player
    public void SetControlStatus(bool value){
        if (gameObject.activeSelf == true)
        {
            control = value;
            if (value == false)
            {
                //Player turn ends
                Debug.Log(gameObject.name + " ends turn with " + hp + " hp left");
                UpdateMovementHighlight();
                UpdatePosition();
                DisableHighlight();
                combatAllowed = true;
            }
            else
            {
                //Player turn begins
                UpdateMovementHighlight();
                UpdatePosition();
                EnableHighlight();
                UIManager.GetComponent<UIManager>().PlayerTurn();
                Debug.Log(gameObject.name + " has " + hp + " hp left");
                UIManager.GetComponent<UIManager>().PlayerInfo(gameObject.name, hp, atk, def);
                combatAllowed = true;
            }
        }
    }

    void Update()
    {
        if (control == true) {
            if(CheckForAdjacentEnemy() != 0){
                UIManager.GetComponent<UIManager>().ShowEnterKey();
            }
            else{
                UIManager.GetComponent<UIManager>().HideEnterKey();
                target.SetPosition(0,0);
            }

            if (target.GetPositionX() != 0 && target.GetPositionY() != 0){
                if (Input.GetKeyDown(KeyCode.Return)){
                    Debug.Log("Starting combat");
                    if(combatAllowed == true)    
                        CombatManager.GetComponent<CombatManager>().CombatBegin(gameObject.name, Occupied[target], true);

                    //PartyManager.GetComponent<PartyManager>().FinishedCharacterTurn();
                    UIManager.GetComponent<UIManager>().PlayerInfo(gameObject.name, hp, atk, def);
                    UIManager.GetComponent<UIManager>().HideArrowKeys();
                    UIManager.GetComponent<UIManager>().HideEnterKey();
                    control = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.RightArrow)) {
                int compare_x = (current.GetPositionX() - 1) - starting.GetPositionX();
                int compare_y = Mathf.Abs(current.GetPositionY() - starting.GetPositionY());
                remainingMovement = movement - (Mathf.Abs(compare_x) + compare_y);
                if(compare_x > 0){
                    remainingMovement += compare_x;
                }
                if (remainingMovement != 0) {
                    if (current.GetPositionX() > min_size_x) {
                        int x = current.GetPositionX() - 1;
                        int y = current.GetPositionY();
                        Position p = new Position();
                        p.SetPosition(x, y);

                        if (!(Occupied.ContainsKey(p))){
                            Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                            transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                        }
                        else if(Occupied.ContainsKey(p) == true){
                            if(remainingMovement > 1){
                                if (current.GetPositionX() > min_size_x + 1)
                                {
                                    x--;
                                    p.SetPosition(x, y);
                                    if (!(Occupied.ContainsKey(p))){
                                        Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                        transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                                    }
                                    else if (Occupied.ContainsKey(p) == true){
                                        if (remainingMovement > 2){
                                            if (current.GetPositionX() > min_size_x + 2){
                                                x--;
                                                p.SetPosition(x, y);
                                                if (!(Occupied.ContainsKey(p))){
                                                    Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                                    transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                                                }
                                                else if (Occupied.ContainsKey(p) == true){
                                                    if (current.GetPositionX() > min_size_x + 3){
                                                        x--;
                                                        p.SetPosition(x, y);
                                                        if (!(Occupied.ContainsKey(p))){
                                                            Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                                            transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                int compare_x = (current.GetPositionX() + 1) - starting.GetPositionX();
                int compare_y = Mathf.Abs(current.GetPositionY() - starting.GetPositionY());
                remainingMovement = movement - (compare_x + compare_y);
                if (remainingMovement != 0){
                    if (current.GetPositionX() < size_x){
                        int x = current.GetPositionX() + 1;
                        int y = current.GetPositionY();
                        Position p = new Position();
                        p.SetPosition(x, y);
                        
                        if (!(Occupied.ContainsKey(p))) { 
                            Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                            transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                        }
                        else if(Occupied.ContainsKey(p) == true){
                            if(remainingMovement > 1){
                                if (current.GetPositionX() < size_x - 1) {
                                    x++;
                                    p.SetPosition(x, y);
                                    if (!(Occupied.ContainsKey(p))){
                                        Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                        transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                                    }
                                    else if (Occupied.ContainsKey(p) == true){
                                        if (remainingMovement > 2){
                                            if (current.GetPositionX() < size_x - 2){
                                                x++;
                                                p.SetPosition(x, y);
                                                if (!(Occupied.ContainsKey(p))){
                                                    Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                                    transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                                                }
                                                else if (!(Occupied.ContainsKey(p))){
                                                    if (current.GetPositionX() < size_x - 3){
                                                        x++;
                                                        p.SetPosition(x, y);
                                                        if (!(Occupied.ContainsKey(p))){
                                                            Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                                            transform.position = new Vector3(temp.x, transform.position.y, transform.position.z);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.UpArrow)){
                int compare_x = Mathf.Abs(current.GetPositionX() - starting.GetPositionX());
                int compare_y = Mathf.Abs((current.GetPositionY() - 1) - starting.GetPositionY());
                remainingMovement = movement - (compare_x + compare_y);
                
                if (((current.GetPositionY() - 1) - starting.GetPositionY())  > 0){
                    remainingMovement += compare_y;
                }

                if (remainingMovement != 0){
                    if (current.GetPositionY() > 0){

                        int x = current.GetPositionX();
                        int y = current.GetPositionY() - 1;
                        Position p = new Position();
                        p.SetPosition(x, y);

                        if (!(Occupied.ContainsKey(p))){
                            Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                            transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                            
                        }
                        else if (Occupied.ContainsKey(p) == true){
                            if (remainingMovement > 1){
                                if (current.GetPositionY() > 1){
                                    y--;
                                    p.SetPosition(x, y);
                                    if (!(Occupied.ContainsKey(p))){
                                        Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                        transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                                    }
                                    else if(Occupied.ContainsKey(p) == true){
                                        if (remainingMovement > 2) {
                                            if (current.GetPositionY() > 2) {
                                                y--;
                                                p.SetPosition(x, y);
                                                if (!(Occupied.ContainsKey(p))){
                                                    Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                                    transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.DownArrow)){
                int compare_x = Mathf.Abs(current.GetPositionX() - starting.GetPositionX());
                int compare_y = (current.GetPositionY() + 1) - starting.GetPositionY(); 
                remainingMovement = movement - (compare_x + compare_y);
                if (remainingMovement != 0){
                    if (current.GetPositionY() < size_y) {
                        int x = current.GetPositionX();
                        int y = current.GetPositionY() + 1;
                        Position p = new Position();
                        p.SetPosition(x, y);

                        if (!(Occupied.ContainsKey(p))){
                            Vector3 temp = AllTiles[current.GetPositionX(), current.GetPositionY() + 1].GetComponent<Grid>().transform.position;
                            transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                        }
                        else if (Occupied.ContainsKey(p) == true){
                            if (remainingMovement > 1){
                                if (current.GetPositionY() < (size_y - 1)){
                                    y++;
                                    p.SetPosition(x, y);
                                    if (!(Occupied.ContainsKey(p))){
                                        Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                        transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                                    }
                                    else if (Occupied.ContainsKey(p) == true){
                                        if (remainingMovement > 2){
                                            if (current.GetPositionY() < size_y - 2){
                                                y++;
                                                p.SetPosition(x, y);
                                                if (!(Occupied.ContainsKey(p))){
                                                    Vector3 temp = AllTiles[x, y].GetComponent<Grid>().transform.position;
                                                    transform.position = new Vector3(transform.position.x, temp.y, transform.position.z);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private int CheckForAdjacentEnemy(){
        Position temp = new Position();

        //Checks left
        temp.SetPosition(current.GetPositionX() + 1, current.GetPositionY());
        if (Occupied.ContainsKey(temp) == true){
            if (Occupied[temp] == "Enemy1"){
                target.SetPosition(temp.GetPositionX(), temp.GetPositionY());
                return 1;
            }
        }
        //Checks right
        temp.SetPosition(current.GetPositionX() - 1, current.GetPositionY());
        if (Occupied.ContainsKey(temp) == true){
            if (Occupied[temp] == "Enemy1"){
                target.SetPosition(temp.GetPositionX(), temp.GetPositionY());
                return 1;
            }
        }
        //Checks below
        temp.SetPosition(current.GetPositionX(), current.GetPositionY() + 1);
        if (Occupied.ContainsKey(temp) == true){
            if (Occupied[temp] == "Enemy1"){
                target.SetPosition(temp.GetPositionX(), temp.GetPositionY());
                return 1;
            }
        }
        //Checks above
        temp.SetPosition(current.GetPositionX(), current.GetPositionY() - 1);
        if (Occupied.ContainsKey(temp) == true){
            if (Occupied[temp] == "Enemy1"){
                target.SetPosition(temp.GetPositionX(), temp.GetPositionY());
                return 1;
            }
        }

        return 0;
    }

    //Updates the position of the player. Used when the player control is being switched.
    private void UpdatePosition(){
        starting.SetPosition(current.GetPositionX(), current.GetPositionY());
    }

    public void TookDamage(int NewHP){
        hp = NewHP;
        if(hp <= 0){
            Debug.Log(gameObject.name + " has died.");
            SetControlStatus(false);
            gameObject.SetActive(false);
            PartyManager.GetComponent<PartyManager>().PlayerHasDied(gameObject);
        }
        else{
            hp = NewHP;
            Debug.Log(gameObject.name + " is still alive with " + hp + " hp");
        }
    }

    //Gets the Starting X Position value
    public int GetStartingX(){
        return starting.GetPositionX();
    }

    //Gets the Starting Y Position value
    public int GetStartingY(){
        return starting.GetPositionY();
    }

    //Gets the Current X Position value
    public int GetCurrentX(){
        return current.GetPositionX();
    }

    //Gets the Current Y Position value
    public int GetCurrentY(){
        return current.GetPositionY();
    }

    //Gets stats
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
    private void OnTriggerEnter(Collider other){
        int x = other.gameObject.GetComponent<Grid>().GetIndexX();
        int y = other.gameObject.GetComponent<Grid>().GetIndexY();

        current.SetPosition(x, y);

        transform.position = new Vector3(other.transform.position.x, other.transform.position.y, -1.0f);
        if (start == false){
            starting.SetPosition(x, y);
            CreateMovementHighlight();
            start = true;
        }
    }


    //Tile Code
    private void CreateMovementHighlight(){//Creates Movement Tiles 
        GameObject temp;
        for (int i = 0; i < movement; i++){
            for (int k = 0; k < movement - i; k++){
                temp = GameObject.Instantiate(MoveTile, AllTiles[7 + i, 4 + k].transform);
                temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, offset_z);
                HighlightTiles.Add(temp);
                temp = GameObject.Instantiate(MoveTile, AllTiles[7 - i, 4 - k].transform);
                temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, offset_z);
                HighlightTiles.Add(temp);
            }
        }
        for (int i = (movement - 1) * -1; i < 0; i++){
            for (int k = (movement - 1) + i; k >= 0; k--){
                temp = GameObject.Instantiate(MoveTile, AllTiles[7 + i, 4 + k].transform);
                temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, offset_z);
                HighlightTiles.Add(temp);
                temp = GameObject.Instantiate(MoveTile, AllTiles[7 - i, 4 - k].transform);
                temp.transform.position = new Vector3(temp.transform.position.x, temp.transform.position.y, offset_z);
                HighlightTiles.Add(temp);
            }
        }

        //Moves Tiles to Player Position. Cannot use UpdateMovementHighlight function as values used are different
        int x = current.GetPositionX() - 7;
        int y = current.GetPositionY() - 4;

        foreach (GameObject Tile in HighlightTiles){
            Tile.transform.position = new Vector3(Tile.transform.position.x - x, Tile.transform.position.y - y, offset_z);
        }

        GameObject.Find("GameManager").GetComponent<GameManager>().Countdown();//Tells the GameManager that this has finished
    }

    //Update HighlightTiles position
    private void UpdateMovementHighlight(){
        int x = current.GetPositionX() - starting.GetPositionX();
        int y = current.GetPositionY() - starting.GetPositionY();
        foreach (GameObject Tile in HighlightTiles){
            Tile.transform.position = new Vector3(Tile.transform.position.x - x, Tile.transform.position.y - y, Tile.transform.position.z);
        }
    }

    //If player is not in control, removes tiles. Signifies that player is not being controlled.
    public void DisableHighlight(){
        foreach (GameObject Tile in HighlightTiles){
            Tile.SetActive(false);
        }
    }

    //If player is in control, displays tiles. Signifies that player is being controlled.
    public void EnableHighlight(){
        foreach (GameObject Tile in HighlightTiles){
            Tile.SetActive(true);
        }
    }
}
