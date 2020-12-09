using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int starting;
    public GameObject PartyManager;
    public GameObject EnemyManager;
    public GameObject TurnManager;
    public GameObject UIManager;
    //private List<GameObject> CurrentPlayers = new List<GameObject>();
    //private List<GameObject> CurrentEnemies = new List<GameObject>();

    /*
    public void AddPlayer(GameObject Player){
        CurrentPlayers.Add(Player);
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().NewTargetAdded(Player);
    }*/

    /*
    public void AddEnemy(GameObject Enemy){
        CurrentEnemies.Add(Enemy);
    }*/

    private void Start(){
        starting = PartyManager.GetComponent<PartyManager>().GetPartySize();
    }

    //Called when the GridManager creates its grid
    public void GridCreated(GameObject[,] grid){
        PartyManager.GetComponent<PartyManager>().FillPlayerTiles(grid);
        EnemyManager.GetComponent<EnemyManager>().FillEnemyTiles(grid);
        //GameObject.Find("PartyManager").GetComponent<PartyManager>().FillPlayerTiles(grid);
        //GameObject.Find("EnemyManager").GetComponent<EnemyManager>().FillEnemyTiles(grid);
    }

    //Called when each player has finished creating everything they needed
    public void Countdown(){
        starting--;
        if(starting == 0){
            //GameObject.Find("PartyManager").GetComponent<PartyManager>().GameStart();
            //GameObject.Find("EnemyManager").GetComponent<EnemyManager>().GameStart();
            PartyManager.GetComponent<PartyManager>().GameStart();
            EnemyManager.GetComponent<EnemyManager>().GameStart();
        }
    }

    public void AllPlayersDead(){
        Debug.Log("Game Over");
        PartyManager.GetComponent<PartyManager>().GameOver();
        EnemyManager.GetComponent<EnemyManager>().GameOver();
        //GameObject.Find("TurnManager").GetComponent<TurnManager>().GameOver();
        TurnManager.GetComponent<TurnManager>().GameOver();
        UIManager.GetComponent<UIManager>().GameOver();
    }

    public void AllEnemiesDead(){
        Debug.Log("You Win!");
        PartyManager.GetComponent<PartyManager>().GameOver();
        EnemyManager.GetComponent<EnemyManager>().GameOver();
        //GameObject.Find("TurnManager").GetComponent<TurnManager>().GameOver();
        TurnManager.GetComponent<TurnManager>().GameOver();
        UIManager.GetComponent<UIManager>().CombatMessage();
        UIManager.GetComponent<UIManager>().GameOver();
        UIManager.GetComponent<UIManager>().LevelBeaten();
    }

    //Called whenever a player or an enemy finishes moving
    public void UpdateEveryonesTiles(){
        //GameObject.Find("PartyManager").GetComponent<PartyManager>().UpdatePlayersOccupiedTiles();
        //GameObject.Find("EnemyManager").GetComponent<EnemyManager>().UpdateEnemiesOccupiedTiles();   
        PartyManager.GetComponent<PartyManager>().UpdatePlayersOccupiedTiles();
        EnemyManager.GetComponent<EnemyManager>().UpdateEnemiesOccupiedTiles();
    }

    public void Pause(){
        PartyManager.GetComponent<PartyManager>().Pause();
        EnemyManager.GetComponent<EnemyManager>().Pause();
    }

    public void UnPause(){
        PartyManager.GetComponent<PartyManager>().UnPause();
        EnemyManager.GetComponent<EnemyManager>().UnPause();
    }

    public void Level1Beaten(){
        SceneManager.LoadScene(2);
    }

    public void Level2Beaten(){
        SceneManager.LoadScene(3);
    }

    public void GameBeaten(){
        SceneManager.LoadScene(0);
    }
}