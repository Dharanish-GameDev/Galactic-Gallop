using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    #region Private Variables
    [SerializeField] GameObject[] RoadsPrefab;
    private float zSpawn = 0f;
    [SerializeField] float TileLength = 56f;
    [SerializeField] int NoOfTiles = 5;
    [SerializeField] Transform PlayerTransforn;
    List<GameObject> ActiveRoads = new List<GameObject>();
    [SerializeField] float RunSpeedIncrease = 0.2f;
    PlayerStateContext P_Controller;
    private int HowManyTilesPlaced;
    [SerializeField] bool RunS_Increased=false;
    [SerializeField] float MaxSpeed;
    [SerializeField] Transform Jimmy;
    [SerializeField] Transform Clarie;
    [SerializeField] Transform Zombie;
    [SerializeField] Transform Boss;
    #endregion

    void Start()
    {
        if (Jimmy.gameObject.activeInHierarchy)
        {
            PlayerTransforn = Jimmy.transform;
        }
        else if (Clarie.gameObject.activeInHierarchy)
        {
            PlayerTransforn = Clarie.transform;
           
        }
        else if (Zombie.gameObject.activeInHierarchy)
        {
            PlayerTransforn = Zombie.transform;
           
        }
        else if(Boss.gameObject.activeInHierarchy)
        {
            PlayerTransforn = Boss.transform;
            
        }
        for(int i =0;i < NoOfTiles;i++)
        {
            if(i == 0)
            {
                SpawnTile(0);
            }
            else
            {
                SpawnTile(Random.Range(1, RoadsPrefab.Length));
            }
        }
        P_Controller = PlayerStateContext.Instance;
    }
    void Update()
    {
        SpawningAndDeletingRoads();
        RunSpeed(HowManyTilesPlaced);
    }

    private void SpawningAndDeletingRoads()
    {
        if (PlayerTransforn.position.z - 60 > zSpawn - (NoOfTiles * TileLength))
        {
            SpawnTile(Random.Range(1, RoadsPrefab.Length));
            DeleteTile();
        }
    }

    private void SpawnTile(int TileIndex)
    {
        HowManyTilesPlaced++;
        GameObject Go = Instantiate(RoadsPrefab[TileIndex], transform.forward * zSpawn, transform.rotation);
        ActiveRoads.Add(Go);
        zSpawn += TileLength;
    }
    private void DeleteTile()
    {
        Destroy(ActiveRoads[0]);
        ActiveRoads.RemoveAt(0);
    }
    private void RunSpeed(int n)
    {
        if(n%10 == 0&& !RunS_Increased && P_Controller.forwardSpeed < MaxSpeed)
        {  
            P_Controller.forwardSpeed = Mathf.MoveTowards(P_Controller.forwardSpeed, P_Controller.forwardSpeed + RunSpeedIncrease + (P_Controller.isSliding? 50f : 0), 100f);
            HowManyTilesPlaced++;
            RunS_Increased = true;
        }
        else RunS_Increased = false;
    }
}
