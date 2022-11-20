using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyController : MonoBehaviour
{
    public Transform playerMovePoint;
    public float moveSpeed = 1000f;
    public Transform movePoint;
    public LayerMask Obstacles;
    public TurnState turnState;
    public GameObject battery;
    public GameObject bullet;
    public GameObject bulletNew;
    public int reloadTime=0;
    public int reloadCount=3;
    public EnemySpawner enemySpawner;
    private BulletController bulletController;
    float xDiff = 0;
    float yDiff = 0;
    // Start is called before the first frame update
    public void movement()

    {
        if (reloadTime == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, movePoint.position) < 0.1f)
            {
                xDiff = playerMovePoint.position.x - movePoint.position.x;
                yDiff = playerMovePoint.position.y - movePoint.position.y;
                if (Mathf.Abs(xDiff) > 0.5 && Mathf.Abs(yDiff) > 0.5)
                {
                    if (xDiff > 0.5f)
                        movePoint.position += new Vector3(1f, 0f, 0f);
                    else if (xDiff < -0.5f)
                        movePoint.position += new Vector3(-1f, 0f, 0f);
                    else if (yDiff > 0.5f)
                        movePoint.position += new Vector3(0f, 1f, 0f);
                    else if (yDiff < -0.5f)
                        movePoint.position += new Vector3(0f, -1f, 0f);
                }
                else if (xDiff <= 5f && xDiff > 0.5f)
                {
                    shoot("Right");
                }
                else if (xDiff >= -5f && xDiff < -0.5f)
                {
                    shoot("Left");
                }
                else if (yDiff <= 5f && yDiff > 0.5f)
                {
                    shoot("Up");

                }
                else if (yDiff >= -5f && yDiff < -0.5f)
                {
                    shoot("Down");
                }


            }
        }
        else
        {
            reloadTime++;
            reloadTime %= reloadCount;
        }
    }

    void OnDestroy()
    {
        battery.transform.position = transform.position;
        GameObject pickup = Instantiate(battery);
    }
    void Start()
    {
        movePoint.parent = null;
    }

    public void shoot(string direction)
    {
        bullet = Instantiate(bulletNew);
        bullet.transform.position = transform.position;
        bulletController = bullet.GetComponent<BulletController>();
        bulletController.movePoint.position = transform.position;
        bulletController.direction = direction;
        bulletController.turnState = turnState;
        turnState.bullets.Add(bullet);
        reloadTime = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
