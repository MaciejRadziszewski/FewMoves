using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public int rooms = 0;
    public int[,] taken = new int[16, 10];
    public GameObject roomLayoutInstance;
    public GameObject block;
    public GameObject shootingEnemy;
    public GameObject meleeEnemy;
    public GameObject battery;
    public GameObject door;
    public List<GameObject> layouts = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public int blockSpawnRatio = 30;
    public int shootingEnemySpawnRatio = 30;
    public int meleeEnemySpawnRatio = 30;
    public int batterySpawnRatio = 10;
    int treshold = 0;
    public GameObject spawnedObject;
    public RoomLayout roomLayout;
    public ShootingEnemyController shootingEnemyController;
    public EnemyController meleeEnemyController;
    public TurnState turnState;
    public PlayerControls player;

    public void randomSpawn()
    {
        int randomX = Random.Range(0, 15);
        int randomY = Random.Range(0, 8);
        while(taken[randomX,randomY] != 0)
        {
            randomX = Random.Range(0, 15);
            randomY = Random.Range(0, 8);
        }
        
        int element = 0;
        int randomValue = Random.Range(0, 100);
        Debug.Log(randomValue.ToString());
        if (randomValue >= treshold && randomValue <= treshold + blockSpawnRatio)
        {
            Debug.Log("Blok" + randomValue.ToString());
            element = 1;
        }
        treshold += blockSpawnRatio;

        if (randomValue > treshold && randomValue <= treshold + shootingEnemySpawnRatio)
        {
            Debug.Log("Szuter" + randomValue.ToString());
            element = 2;
            treshold += shootingEnemySpawnRatio;
        }
        treshold += shootingEnemySpawnRatio;

        if (randomValue > treshold && randomValue <= treshold + meleeEnemySpawnRatio)
        {
            Debug.Log("Melak" + randomValue.ToString());
            element = 3;
        }
        treshold += meleeEnemySpawnRatio;

        if (randomValue >= treshold && randomValue <= treshold + batterySpawnRatio)
        {
            Debug.Log("Bateria" + randomValue.ToString());
            element = 4;
        }
        treshold += batterySpawnRatio;
        Debug.Log(randomX.ToString() + ", " + randomY.ToString() + element);

        switch (element)
        {
            case 1:
                spawnedObject = Instantiate(block);
                spawnedObject.transform.position = transform.position + new Vector3(randomX, randomY, 0f);
                break;
            case 2:
                spawnedObject = Instantiate(shootingEnemy);
                shootingEnemyController = spawnedObject.GetComponent<ShootingEnemyController>();
                shootingEnemyController.playerMovePoint = player.movePoint;
                shootingEnemyController.turnState = turnState;
                shootingEnemy.transform.position = transform.position + new Vector3(randomX, randomY, 0f);
                enemies.Add(spawnedObject);
                break;
            case 3:
                spawnedObject = Instantiate(meleeEnemy);
                meleeEnemyController = spawnedObject.GetComponent<EnemyController>();
                meleeEnemyController.playerMovePoint = player.movePoint;
                meleeEnemy.transform.position = transform.position + new Vector3(randomX, randomY, 0f);
                enemies.Add(spawnedObject);
                break;
            case 4:
                spawnedObject = Instantiate(battery);
                spawnedObject.transform.position = transform.position + new Vector3(randomX, randomY, 0f);
                break;
            default:
                break;
        }
        taken[randomX, randomY] = element;
        treshold = 0;
        rooms += 1;

    }

    void Start()
    {
        for(int i = 0; i < 15; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                taken[i, j] = 0;
            }
        }
        int spawnedObjects = Random.Range(5, 15);
        for(int i = 0; i < spawnedObjects; i++)
        {
            randomSpawn();
        }
        spawnedObject = Instantiate(roomLayoutInstance);
        spawnedObject.transform.position = transform.position;
        roomLayout = spawnedObject.GetComponent<RoomLayout>();
        roomLayout.layoutMap = taken.Clone() as int[,];
        roomLayout.turnState = turnState;
        roomLayout.roomNumber = rooms;
        roomLayout.player = player;
        roomLayout.levelSpawner = this;
        layouts.Add(spawnedObject);

        //for (int i = 0; i < 15; i++)
        //{
        //    for (int j = 0; j < 8; j++)
        //    {
        //        //Debug.Log(i.ToString() + ", " + j.ToString() + ", " + taken[i, j]);
        //        taken[i, j] = 0;
        //    }
        //}
    }

    public void moveEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.TryGetComponent<EnemyController>(out meleeEnemyController) == true)
            {
                meleeEnemyController = enemy.GetComponent<EnemyController>();
                meleeEnemyController.movement();
            }
            else
            {
                shootingEnemyController = enemy.GetComponent<ShootingEnemyController>();
                shootingEnemyController.movement();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
