﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    struct Stats{
        private int hp;
        private int atk;
        private int def;
        public void SetStats(int h, int a, int d){
            hp = h;
            atk = a;
            def = d;
        }

        public int GetHealth(){
            return hp;
        }
        public int GetAttack(){
            return atk;
        }

        public int GetDefense(){
            return def;
        }
    }

    //private bool combatStart = false, whoStarted;
    private Player pComponent;
    private Enemy eComponent;
    private Stats player = new Stats(), enemy = new Stats();


    private void Update(){
        /*
       if(combatStart == true){
            if(whoStarted == false){
                //Enemy started combat
                Debug.Log("Enemy Attacks!");
                int damage = enemy.GetAttack() - player.GetDefense();
                int newHp = player.GetHealth() - damage;
                if (newHp <= 0){
                    Debug.Log("Player has died");
                    GameObject.Find(pComponent.gameObject.name).GetComponent<Player>().TookDamage(newHp);
                }
                else{
                    Debug.Log("Player survives with " + newHp);
                    GameObject.Find(pComponent.gameObject.name).GetComponent<Player>().TookDamage(newHp);
                }
                combatStart = false;
            }
            else if(whoStarted == true){
                //Player started combat
                Debug.Log("Player Attacks!");
                int damage = player.GetAttack() - enemy.GetDefense();
                int newHp = enemy.GetHealth() - damage;
                if (newHp <= 0)
                {
                    Debug.Log("enemy hp " + enemy.GetHealth());
                    Debug.Log("dmg" + damage + " newHP" + newHp);
                    Debug.Log("Enemy has died");
                    GameObject.Find(eComponent.gameObject.name).GetComponent<Enemy>().TookDamage(newHp);
                }
                else
                {
                    Debug.Log("Enemy survives");
                    GameObject.Find(eComponent.gameObject.name).GetComponent<Enemy>().TookDamage(newHp);
                }

                combatStart = false;
            }
       }*/
    }

    public void CombatBegin(string playerName, string enemyName, bool initiate){
        pComponent = GameObject.Find(playerName).GetComponent<Player>();
        eComponent = GameObject.Find(enemyName).GetComponent<Enemy>();
        player.SetStats(pComponent.GetHP(), pComponent.GetAtk(), pComponent.GetDef());
        enemy.SetStats(eComponent.GetHP(), eComponent.GetAtk(), eComponent.GetDef());
        //whoStarted = initiate;
        //combatStart = true;
        if(initiate == true){
            //Player started combat
            Debug.Log("Player Attacks!");
            int damage = player.GetAttack() - enemy.GetDefense();
            int newHp = enemy.GetHealth() - damage;
            if (newHp <= 0)
            {
                Debug.Log("enemy hp " + enemy.GetHealth());
                Debug.Log("dmg" + damage + " newHP" + newHp);
                Debug.Log("Enemy has died");
                GameObject.Find(eComponent.gameObject.name).GetComponent<Enemy>().TookDamage(newHp);
            }
            else
            {
                Debug.Log("Enemy survives");
                GameObject.Find(eComponent.gameObject.name).GetComponent<Enemy>().TookDamage(newHp);
            }
        }
        else if(initiate == false){
            //Enemy started combat
            Debug.Log("Enemy Attacks!");
            int damage = enemy.GetAttack() - player.GetDefense();
            int newHp = player.GetHealth() - damage;
            if (newHp <= 0)
            {
                Debug.Log("player hp " + player.GetHealth());
                Debug.Log("dmg" + damage + " newHP" + newHp);
                Debug.Log("Player has died");
                GameObject.Find(pComponent.gameObject.name).GetComponent<Player>().TookDamage(newHp);
            }
            else
            {
                Debug.Log("Player survives");
                GameObject.Find(pComponent.gameObject.name).GetComponent<Player>().TookDamage(newHp);
            }
        }
    
    }
}
