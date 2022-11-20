using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject enemy;
    public GameObject shootingEnemy;
    private GameObject newEnemy;
    public List<GameObject> enemies = new List<GameObject>();
    private EnemyController enemyController;
    private ShootingEnemyController shootingEnemyController;
    public PlayerControls playerControls;
    public TurnState turnState;
    float yField = 0.612f;
    float xField = 0.511f;

    public void spawn()
    {
        float randomY = yField + Random.Range(-5, 15);
        float randomX = xField + Random.Range(-15, 27);
        //if (Random.Range(0, 2) == 0)
        //{
        //    newEnemy = Instantiate(shootingEnemy);
        //    shootingEnemyController = newEnemy.GetComponent<ShootingEnemyController>();
        //    shootingEnemyController.playerMovePoint = playerControls.movePoint;
        //    shootingEnemyController.turnState = turnState;
            
        //}
        //else
        //{
            newEnemy = Instantiate(enemy);
            enemyController = newEnemy.GetComponent<EnemyController>();
            enemyController.playerMovePoint = playerControls.movePoint;
        //}
        float wall = Random.Range(0, 4);
        switch (wall)
        {
            case 0:
                newEnemy.transform.position = new Vector3(-15.489f, randomY, 0);
                break;
            case 1:
                newEnemy.transform.position = new Vector3(randomX, 18.612f, 0);
                break;
            case 2:
                newEnemy.transform.position = new Vector3(27.511f, randomY, 0);
                break;
            case 3:
                newEnemy.transform.position = new Vector3(randomX, -5.388f, 0);
                break;
            default:
                break;
        }
        enemies.Add(newEnemy);
    }

    public void moveEnemies()
    {
        foreach (GameObject enemy in enemies)
        {
            if (enemy.TryGetComponent<EnemyController>(out enemyController) == true)
            {
                enemyController = enemy.GetComponent<EnemyController>();
                enemyController.movement();
            }
            else
            {
                shootingEnemyController = enemy.GetComponent<ShootingEnemyController>();
                shootingEnemyController.movement();
            }
        }
    }

    void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
