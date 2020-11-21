using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    public List<GameObject> PartyMembers;
    private int currentMember = 0;
    private int starting = 0;

    public void GameStart(){
        starting++;
        if (starting >= PartyMembers.Count){
            int i = 0;
            foreach (GameObject Member in PartyMembers){
                if (i != currentMember){
                    Member.GetComponent<Player>().DisableHighlight();
                }
                i++;
            }       
            for(i = 0; i < PartyMembers.Count; i++){
                PartyMembers[i].GetComponent<Player>().FillOccupiedTiles();
            }

        }
        PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);
    }

    private void Update(){
        if (Input.GetKeyDown(KeyCode.Space)){
            PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(false);
            
            currentMember++;
           
            if (currentMember >= PartyMembers.Count)
                currentMember = 0;

            PartyMembers[currentMember].GetComponent<Player>().SetControlStatus(true);

            for(int i = 0; i < PartyMembers.Count; i++){
                PartyMembers[i].GetComponent<Player>().UpdateOccupiedTiles();
            }
        }
    }
}