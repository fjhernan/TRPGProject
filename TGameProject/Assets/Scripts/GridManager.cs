using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject RoughTerrain;
    public GameObject Path;
    public GameObject Block;
    public string filename;
    private GameObject[,] arr2d;

    private void Start(){
        arr2d = new GameObject[15, 10];
        CreateGrid();
        GameObject.Find("Player").GetComponent<Player>().SetAllTiles(arr2d);
    }

    private void CreateGrid(){
        string file = string.Format(Application.dataPath + "/Resources/" + filename);
        //using (StreamReader sr = new StreamReader(string.Format(Application.dataPath + "/Resources/" + filename)))
        using (StreamReader sr = new StreamReader(file))
        {
            string line = "";
            int offsetY = 0;

            while ((line = sr.ReadLine()) != null)
            {
                int offsetX = 0;
                char[] letters = line.ToCharArray();
                foreach (char letter in letters)
                {
                    SpawnGrid(letter, offsetX, offsetY);
                    offsetX++;
                }
                offsetY++;
            }
            sr.Close();
        }
    }

    private void SpawnGrid(char letter, int offsetX, int offsetY){
        GameObject grid = null;
        float offsetZ = 0.0f;

        switch (letter)
        {
            case 't':
                grid = GameObject.Instantiate(RoughTerrain, transform);
                break;
            case 'p':
                grid = GameObject.Instantiate(Path, transform);
                break;
            case 'b':
                offsetZ = -1.0f;
                grid = GameObject.Instantiate(Block, transform);
                break;
            default:
                break;
        }
        grid.transform.position = new Vector3(grid.transform.position.x - ((float)offsetX), grid.transform.position.y - ((float)offsetY), offsetZ);
        grid.GetComponent<Grid>().SetIndex(offsetX, offsetY);
        arr2d[(int)offsetX, (int)offsetY] = grid;
    }
}