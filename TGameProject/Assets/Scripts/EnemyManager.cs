using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> Enemies;
    private int currentMember = 0;
    private bool currentTurn = false;

    public void GameStart(){
        for(int i = 0; i < Enemies.Count; i++){
            //Debug.Log("Tiles should be occupied");
            Enemies[i].GetComponent<Enemy>().UpdateOccupiedTiles();
        }
    }

    private void Update(){
        /*
        if(currentTurn == true){
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (currentMember < Enemies.Count)
                {
                    Debug.Log("Enemies size is " + Enemies.Count);
                    Enemies[currentMember].GetComponent<Enemy>().SetControlStatus(false);
                    currentMember++;
                    GameObject.Find("GameManager").GetComponent<GameManager>().UpdateEveryonesTiles();

                    if (currentMember < Enemies.Count)
                    {
                        Enemies[currentMember].GetComponent<Enemy>().SetControlStatus(true);
                    }
                    else
                    {
                        GameObject.Find("TurnManager").GetComponent<TurnManager>().UpdateCurrentTurn(true);
                        currentMember = 0;
                    }
                }
            }
        }*/
    }

    public void UpdateTurn(bool value)
    {
        currentTurn = value;
        if (value == true)
        {
            //Debug.Log("It should be enemies turn by now");
            Enemies[currentMember].GetComponent<Enemy>().SetControlStatus(true);
        }
    }

    public void UpdateCurrentCharacterControl(){
        Enemies[currentMember].GetComponent<Enemy>().SetControlStatus(false);
        currentMember++;
        if (currentMember >= Enemies.Count){
            GameObject.Find("TurnManager").GetComponent<TurnManager>().UpdateCurrentTurn(true);
            currentMember = 0;   
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

    public void NewTargetAdded(GameObject Player){
        for(int i = 0; i < Enemies.Count; i++){
            Enemies[i].GetComponent<Enemy>().AddNewTarget(Player);
        }
        //Debug.Log("New target added should be called twice");
    }
}
