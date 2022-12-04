using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform playerMovePoint;
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask Obstacles;
    public GameObject battery;
    float xDiff = 0;
    float yDiff = 0;

    // Start is called before the first frame update
    public void movement()
    {
        //transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) < 0.1f)
        {
                xDiff = playerMovePoint.position.x - movePoint.position.x;
                yDiff = playerMovePoint.position.y - movePoint.position.y;
                if (xDiff > 0.5f && !Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f, 0f), 0.2f, Obstacles))
                {
                    movePoint.position += new Vector3(1f, 0f, 0f);
                }
                else if(xDiff < -0.5f && !Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f, 0f), 0.2f, Obstacles))
                {
                    movePoint.position += new Vector3(-1f, 0f, 0f);
                }



                else if (yDiff > 0.5f && !Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f, 0f), 0.2f, Obstacles))
                {
                    movePoint.position += new Vector3(0f, 1f, 0f);
                }
                else if (yDiff < -0.5f && !Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f, 0f), 0.2f, Obstacles))
                {
                    movePoint.position += new Vector3(0f, -1f, 0f);
                }
            
        }
    }

    void OnDestroy()
    {
        battery.transform.position = this.transform.position;
        GameObject pickup = Instantiate(battery);
        Destroy(movePoint);
    }
    void Start()
    {
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }
}
