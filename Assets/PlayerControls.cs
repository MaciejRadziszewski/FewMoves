using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask Obstacles;
    public Text movesText;
    public int moves = 100;
    public string collisionSource;
    private float xCollDiff;
    private float yCollDiff;
    public TurnState turnState;
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public LevelSpawner enemySpawner;
    public float attackRange = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
        turnState.state = State.PLAYERTURN;
        movesText.text = moves.ToString();
    }

    void attack(string direction)
    {
        Collider2D[] hit;
        switch(direction)
        {
            case "UP":
                attackPoint.position = movePoint.position + new Vector3(0f, 1f, 0f);
                hit = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(0.5f, 0.5f), 0f, enemyLayer);
                break;
            case "DOWN":
                attackPoint.position = movePoint.position + new Vector3(0f, -1f, 0f);
                hit = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(0.5f, 0.5f), 0f, enemyLayer);
                break;
            case "LEFT":
                attackPoint.position = movePoint.position + new Vector3(-1f, 0f, 0f);
                hit = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(0.5f, 0.5f), 0f, enemyLayer);
                break;
            case "RIGHT":
                attackPoint.position = movePoint.position + new Vector3(1f, 0f, 0f);
                hit = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(0.5f, 0.5f), 0f, enemyLayer);
                break;
            default:
                hit = Physics2D.OverlapBoxAll(attackPoint.position, new Vector2(0.5f, 0.5f), 0f, enemyLayer);
                break;

        }
        
        foreach(Collider2D enemy in hit)
        {
            enemySpawner.enemies.Remove(enemy.gameObject);
            Destroy(enemy.gameObject);
        }
        attackPoint.position = movePoint.position;
        moves -= 5;
        movesText.text = moves.ToString();
        turnState.state = State.ENEMYTURN;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Collectible":
                Destroy(collision.gameObject);
                moves += 20;
                movesText.text = moves.ToString();
                break;
            case "Enemy":
                moves -= 30;
                movesText.text = moves.ToString();
                xCollDiff = transform.position.x - collision.transform.position.x;
                yCollDiff = transform.position.y - collision.transform.position.y;

                if (Mathf.Abs(xCollDiff) > Mathf.Abs(yCollDiff) && xCollDiff < 0f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-2f, 0f, 0f), 0.2f, Obstacles))
                    {
                        movePoint.position += new Vector3(-2f, 0f, 0f);
                    }
                }
                else if (Mathf.Abs(xCollDiff) > Mathf.Abs(yCollDiff) && xCollDiff > 0f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(2f, 0f, 0f), 0.2f, Obstacles))
                    {
                        movePoint.position += new Vector3(2f, 0f, 0f);
                    }
                }
                else if (Mathf.Abs(xCollDiff) < Mathf.Abs(yCollDiff) && yCollDiff > 0f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 2f, 0f), 0.2f, Obstacles))
                    {
                        movePoint.position += new Vector3(0f, 2f, 0f);
                    }
                }
                else if (Mathf.Abs(xCollDiff) < Mathf.Abs(yCollDiff) && yCollDiff < 0f)
                {
                    if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -2f, 0f), 0.2f, Obstacles))
                    {
                        movePoint.position += new Vector3(0f, -2f, 0f);
                    }
                }
                break;
            default:
                break;
        }

    }

    public void movement()
    {
        Debug.Log("XD");
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, movePoint.position) < 0.1f)
        {
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.5f)
            {

                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, Obstacles))
                {
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f);
                    moves -= 1;
                    movesText.text = moves.ToString();
                    turnState.state = State.ENEMYTURN;

                }
            }
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0.5f)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, Obstacles))
                {
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                    moves -= 1;
                    movesText.text = moves.ToString();
                    turnState.state = State.ENEMYTURN;
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (moves > 0)
        {
            if (turnState.state == State.PLAYERTURN)
            {
                if (Input.GetKeyDown(KeyCode.I))
                {
                    attack("UP");
                }
                else if (Input.GetKeyDown(KeyCode.K))
                {
                    attack("DOWN");
                }
                else if (Input.GetKeyDown(KeyCode.L))
                {
                    attack("RIGHT");
                }
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    attack("LEFT");
                }
                movement();
            }
        }
        else
        {
            moves = 0;
            movesText.text = "Game Over";
            Destroy(this.gameObject);
        }

    }
}
