using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    public int rooms = 0;
    public int[,] taken = new int[16, 10];
    public bool[] spawnedDoors = new bool[5];
    public GameObject roomLayoutInstance;
    public GameObject block;
    public GameObject shootingEnemy;
    public GameObject meleeEnemy;
    public GameObject battery;
    public GameObject door;
    public GameObject entryDoor;
    public GameObject trophy;
    public List<GameObject> doors = new List<GameObject>();
    public GameObject[,] layouts = new GameObject[50, 50];
    public int positionX = 20;
    public int positionY = 20;
    public List<GameObject> enemies = new List<GameObject>();
    public List<GameObject> roomItems = new List<GameObject> ();
    public int blockSpawnRatio = 30;
    public int shootingEnemySpawnRatio = 30;
    public int meleeEnemySpawnRatio = 30;
    public int batterySpawnRatio = 10;
    public int entry = 0;
    int treshold = 0;
    public GameObject spawnedObject;
    public RoomLayout roomLayout;
    public ShootingEnemyController shootingEnemyController;
    public EnemyController meleeEnemyController;
    public DoorController doorController;
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
        //Debug.Log(randomValue.ToString());
        if (randomValue >= treshold && randomValue <= treshold + blockSpawnRatio)
        {
            //Debug.Log("Blok" + randomValue.ToString());
            element = 1;
        }
        treshold += blockSpawnRatio;

        if (randomValue > treshold && randomValue <= treshold + shootingEnemySpawnRatio)
        {
            //Debug.Log("Szuter" + randomValue.ToString());
            element = 2;
            treshold += shootingEnemySpawnRatio;
        }
        treshold += shootingEnemySpawnRatio;

        if (randomValue > treshold && randomValue <= treshold + meleeEnemySpawnRatio)
        {
            //Debug.Log("Melak" + randomValue.ToString());
            element = 3;
        }
        treshold += meleeEnemySpawnRatio;

        if (randomValue >= treshold && randomValue <= treshold + batterySpawnRatio)
        {
            //Debug.Log("Bateria" + randomValue.ToString());
            element = 4;
        }
        treshold += batterySpawnRatio;
        //Debug.Log(randomX.ToString() + ", " + randomY.ToString() + element);
        switch (element)
        {
            //1 - blok
            case 1:
                spawnedObject = Instantiate(block);
                break;
            //2 - Strzelaj¹cy przeciwnik
            case 2:
                spawnedObject = Instantiate(shootingEnemy);
                shootingEnemyController = spawnedObject.GetComponent<ShootingEnemyController>();
                shootingEnemyController.playerMovePoint = player.movePoint;
                shootingEnemyController.turnState = turnState;
                enemies.Add(spawnedObject);
                break;
            //3 - Przeciwnik walcz¹cy w zwarciu
            case 3:
                spawnedObject = Instantiate(meleeEnemy);
                meleeEnemyController = spawnedObject.GetComponent<EnemyController>();
                meleeEnemyController.playerMovePoint = player.movePoint;
                enemies.Add(spawnedObject);
                break;
            //4 - Bateria
            case 4:
                spawnedObject = Instantiate(battery);
                break;
            default:
                break;
        }
        spawnedObject.transform.position = transform.position + new Vector3(randomX, randomY, 0f);
        roomItems.Add(spawnedObject);
        taken[randomX, randomY] = element;
        treshold = 0;

    }
    public void spawnDoor(int side)
    {
        spawnedObject = Instantiate(door);
        doorController = spawnedObject.GetComponent<DoorController>();
        doorController.levelSpawner = this;
        doorController.isVisible = false;
        doorController.playerControls = player;
        doorController.turnState = turnState;
        doorController.roomLayoutLeft = layouts[positionX, positionY].GetComponent<RoomLayout>();
        doorController.side = side;
        switch (side)
        {
            case 1:
                spawnedObject.transform.position = new Vector3(-9f, -0.5f, 0f);
                spawnedObject.transform.Rotate(0f, 0f, -90f);
                break;
            case 2:
                spawnedObject.transform.position = new Vector3(-1f, 4.8f, 0f);
                spawnedObject.transform.Rotate(0f, 0f, 180f);
                break;
            case 3:
                spawnedObject.transform.position = new Vector3(8.4f, -0.5f, 0f);
                spawnedObject.transform.Rotate(0f, 0f, 90f);
                break;
            case 4:
                spawnedObject.transform.position = new Vector3(-1f, -5.8f, 0f);
                spawnedObject.transform.Rotate(0f, 0f, 0f);
                break;
            default:
                break;
        }
        doors.Add(spawnedObject);
        Debug.Log("drzwiSa");
    }

    public void spawnRoom()
    {
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                taken[i, j] = 0;
            }
        }
        int spawnedObjects = Random.Range(2, 6);
        for (int i = 0; i < spawnedObjects; i++)
        {
            randomSpawn();
        }
        rooms += 1;
        spawnedObject = Instantiate(roomLayoutInstance);
        spawnedObject.transform.position = transform.position;
        spawnedObject.name = rooms.ToString();
        roomLayout = spawnedObject.GetComponent<RoomLayout>();
        roomLayout.layoutMap = taken.Clone() as int[,];
        roomLayout.turnState = turnState;
        roomLayout.roomNumber = rooms;
        roomLayout.player = player;
        roomLayout.levelSpawner = this;
        //if (rooms > 1)
        //{
        //    for (int i = 0; i < 4; i++)
        //    {
        //        roomLayout.roomLayouts[i] = doors[i].GetComponent<DoorController>().roomLayoutEnter;
        //    }
        //}
        foreach (GameObject door in doors.ToList())
        {
            if(door.GetComponent<DoorController>().side != entry)
            {
                door.GetComponent<DoorController>().roomLayoutEnter = null;
            }
        }
        layouts[positionX, positionY] = spawnedObject;
        Debug.Log(positionX.ToString() + " " + positionY.ToString());
        int side = Random.Range(1, 5);
        for (int i = 0; i < 1; i++)
        {
            while (spawnedDoors[side] == true)
            {
                side = Random.Range(1, 5);
            }
            spawnedDoors[side] = true;
        }
        //if(entryDoor != null)
        //{
        //    spawnedObject = Instantiate(entryDoor);
        //    doors.Add(spawnedObject);
        //}
        //else
        //{
        //    spawnDoor(0);
        //}
        //spawnDoor(0);
        //spawnDoor(0);
        roomLayout.spawnedDoors = spawnedDoors;
        for (int i = 0; i < 15; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                //Debug.Log(i.ToString() + ", " + j.ToString() + ", " + taken[i, j]);
                taken[i, j] = 0;
            }
        }
        int randomValue = Random.Range(2, 100);
        if(randomValue < rooms || rooms == 40)
        {
            spawnedObject = Instantiate(trophy);
        }
        DoorSetup();
    }
    public void DoorSetup()
    {
        foreach (GameObject door in doors)
        {
            doorController = door.GetComponent<DoorController>();
            doorController.roomLayoutLeft = layouts[positionX, positionY].GetComponent<RoomLayout>();
            switch (doorController.side)
            {
                case 1:
                    if(layouts[positionX - 1, positionY]?.TryGetComponent<RoomLayout>(out roomLayout) == true)
                    {
                        doorController.roomLayoutEnter = layouts[positionX - 1, positionY].GetComponent<RoomLayout>();
                    }
                    else
                    {
                        doorController.roomLayoutEnter = null;
                    }
                    break;
                case 2:
                    if (layouts[positionX, positionY - 1]?.TryGetComponent<RoomLayout>(out roomLayout) == true)
                    {
                        doorController.roomLayoutEnter = layouts[positionX, positionY - 1].GetComponent<RoomLayout>();
                    }
                    else
                    {
                        doorController.roomLayoutEnter = null;
                    }
                    break;
                case 3:
                    if (layouts[positionX + 1, positionY]?.TryGetComponent<RoomLayout>(out roomLayout) == true)
                    {
                        doorController.roomLayoutEnter = layouts[positionX + 1, positionY].GetComponent<RoomLayout>();
                    }
                    else
                    {
                        doorController.roomLayoutEnter = null;
                    }
                    break;
                case 4:
                    if (layouts[positionX, positionY + 1]?.TryGetComponent<RoomLayout>(out roomLayout) == true)
                    {
                        doorController.roomLayoutEnter = layouts[positionX, positionY + 1].GetComponent<RoomLayout>();
                    }
                    else
                    {
                        doorController.roomLayoutEnter = null;
                    }
                    break;
                default:
                    break;
            }
        }
    }
    public void ClearRoom()
    {
        foreach (GameObject item in roomItems.ToList())
        {
            Destroy(item);
            roomItems.Remove(item);
        }
        foreach (GameObject enemy in enemies.ToList())
        {
            Destroy(enemy);
            enemies.Remove(enemy);
        }
        foreach (GameObject bullet in turnState.bullets.ToList())
        {
            //Debug.Log("121123123");
            Destroy(bullet);
            turnState.bullets.Remove(bullet);
        }
        foreach (GameObject door in doors.ToList())
        {
            door.GetComponent<DoorController>().isVisible = false;
        }

    }

    void Start()
    {
        spawnRoom();
        for (int i = 1; i <= 4; i++)
        {
            spawnDoor(i);
        }
    }

    public void moveEnemies()
    {
        foreach (GameObject enemy in enemies.ToArray())
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

    public void OnDestroy()
    {
        roomItems.Clear();
    }
    // Update is called once per frame
    void Update()
    {
        if(enemies.Count == 0)
        {
                foreach (GameObject door in doors.ToList())
                {
                    if(spawnedDoors[door.GetComponent<DoorController>().side] == true)
                    {
                        door.GetComponent<DoorController>().isVisible = true;
                    }
                }
        }
        else
        {
            foreach (GameObject door in doors.ToList())
            {
                door.GetComponent<DoorController>().isVisible = false;
            }
        }
    }
}
