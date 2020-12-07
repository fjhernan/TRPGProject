using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> Enemies;
    private int currentMember = 0;
    private bool currentTurn = false;
    private bool gameOver = false;
    private bool paused = false;

    public void GameStart(){
        for(int i = 0; i < Enemies.Count; i++){
            //Debug.Log("Tiles should be occupied");
            Enemies[i].GetComponent<Enemy>().UpdateOccupiedTiles();
        }
    }

    public void UpdateTurn(bool value)
    {
        if (gameOver == false)
        {
            currentTurn = value;
            if (value == true)
            {
                if (paused == false)
                {
                    //Debug.Log("It should be enemies turn by now");
                    Enemies[currentMember].GetComponent<Enemy>().Movement();
                }
            }
        }
    }

    public void Pause()
    {
        paused = true;
    }

    public void UnPause()
    {
        paused = false;
        if(currentTurn == true)
        {
            Enemies[currentMember].GetComponent<Enemy>().Movement();
        }
    }



    public void UpdateCurrentCharacterControl(){
        currentMember++;
        if (currentMember >= Enemies.Count)
        {
            GameObject.Find("TurnManager").GetComponent<TurnManager>().UpdateCurrentTurn(true);
            GameObject.Find("GameManager").GetComponent<GameManager>().UpdateEveryonesTiles();
            currentMember = 0;
            currentTurn = false;
        }
        else
        {
            if (paused == false)
            {
                Enemies[currentMember].GetComponent<Enemy>().Movement();
            }
        }
    }

    public void FillEnemyTiles(GameObject[,] arr2d){
        for(int i = 0; i < Enemies.Count; i++){
            Enemies[i].GetComponent<Enemy>().SetAllTiles(arr2d);
        }
    }

    public void UpdateEnemiesOccupiedTiles()
    {
        for (int i = 0; i < Enemies.Count; i++){
            Enemies[i].GetComponent<Enemy>().UpdateOccupiedTiles();
        }
    }

    public void EnemyHasDied(GameObject enemy){
        Enemies.Remove(enemy);
        if(Enemies.Count == 0){
            GameObject.Find("GameManager").GetComponent<GameManager>().AllEnemiesDead();
        }
    }

    public void GameOver(){
        gameOver = true;
    }

    /*
    public void NewTargetAdded(GameObject Player){
        for(int i = 0; i < Enemies.Count; i++){
            Enemies[i].GetComponent<Enemy>().AddNewTarget(Player);
        }
        //Debug.Log("New target added should be called twice");
    }*/
}
