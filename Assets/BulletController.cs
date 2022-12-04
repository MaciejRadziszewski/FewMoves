using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public Transform movePoint;
    public float moveSpeed = 5f;
    public string direction;
    public TurnState turnState;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.position = transform.position;
        movePoint.parent = null;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Obstacle"))
        {
            Destroy(this.gameObject);
        }
    }
    public void movement()
    {
        //transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        switch (direction)
        {
            case "Right":
                movePoint.position += new Vector3(1f, 0f, 0f);
                break;
            case "Left":
                movePoint.position += new Vector3(-1f, 0f, 0f);
                break;
            case "Up":
                movePoint.position += new Vector3(0f, 1f, 0f);
                break;
            case "Down":
                movePoint.position += new Vector3(0f, -1f, 0f);
                break;
            default:
                break;
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    public void OnDestroy()
    {
        Destroy(movePoint);
    }

}
