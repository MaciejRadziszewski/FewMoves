using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RoomLayout : MonoBehaviour
{
    public int[,] layoutMap = new int[16, 10];
    public int roomNumber;
    public bool[] spawnedDoors = new bool[5];
    public GameObject spawnedObject;
    public GameObject block;
    public GameObject shootingEnemy;
    public GameObject meleeEnemy;
    public GameObject battery;
    public PlayerControls player;
    public TurnState turnState;
    public DoorController doorController;
    public ShootingEnemyController shootingEnemyController;
    public EnemyController meleeEnemyController;
    public int type;
    public LevelSpawner levelSpawner;
    // Start is called before the first frame update
    public void spawnFromLayout()
    {
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    type = layoutMap[i, j];
                    //Debug.Log(i.ToString() + " " + j.ToString() + " " + type.ToString());
                    switch (type)
                    {
                        case 1:
                            spawnedObject = Instantiate(block);
                            spawnedObject.transform.position = transform.position + new Vector3(i, j, 0f);
                            break;
                        //case 2:
                        //    spawnedObject = Instantiate(shootingEnemy);
                        //    shootingEnemyController = spawnedObject.GetComponent<ShootingEnemyController>();
                        //    shootingEnemyController.playerMovePoint = player.movePoint;
                        //    shootingEnemyController.turnState = turnState;
                        //    shootingEnemy.transform.position = transform.position + new Vector3(i, j, 0f);
                        //    levelSpawner.enemies.Add(spawnedObject);
                        //    break;
                        //case 3:
                        //    spawnedObject = Instantiate(meleeEnemy);
                        //    meleeEnemyController = spawnedObject.GetComponent<EnemyController>();
                        //    meleeEnemyController.playerMovePoint = player.movePoint;
                        //    meleeEnemy.transform.position = transform.position + new Vector3(i, j, 0f);
                        //    levelSpawner.enemies.Add(spawnedObject);
                        //    break;
                        case 4:
                            spawnedObject = Instantiate(battery);
                            spawnedObject.transform.position = transform.position + new Vector3(i, j, 0f);
                            break;
                        default:
                            break;
                    }
                    levelSpawner.roomItems.Add(spawnedObject);
                }
            }
        levelSpawner.spawnedDoors = spawnedDoors;
        levelSpawner.DoorSetup();
    }
    void Start()
    {
        //Debug.Log(layoutMap);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
