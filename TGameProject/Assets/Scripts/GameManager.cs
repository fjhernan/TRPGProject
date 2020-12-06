﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int starting = 2;   
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

    //Called when the GridManager creates its grid
    public void GridCreated(GameObject[,] grid){
        GameObject.Find("PartyManager").GetComponent<PartyManager>().FillPlayerTiles(grid);
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().FillEnemyTiles(grid);
    }

    //Called when each player has finished creating everything they needed
    public void Countdown(){
        starting--;
        if(starting == 0){
            GameObject.Find("PartyManager").GetComponent<PartyManager>().GameStart();
            GameObject.Find("EnemyManager").GetComponent<EnemyManager>().GameStart();
        }
    }

    public void AllPlayersDead(){
        
        Debug.Log("Game Over");
    }

    public void AllEnemiesDead(){
        Debug.Log("You Win!");
    }

    //Called whenever a player or an enemy finishes moving
    public void UpdateEveryonesTiles(){
        GameObject.Find("PartyManager").GetComponent<PartyManager>().UpdatePlayersOccupiedTiles();
        GameObject.Find("EnemyManager").GetComponent<EnemyManager>().UpdateEnemiesOccupiedTiles();   
    }
}