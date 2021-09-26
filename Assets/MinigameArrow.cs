using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameArrow : MonoBehaviour
{
    public enum Direction { up, down, left, right };
    public Direction moveDir;
    public List<Sprite> dirSprite;
    public SpriteRenderer spriteRenderer;

 
    public float beatDuration;    //How long the object lasts before turning itself off

    public PlayerController player;

    public int stepNumber;

    //Arrow movement
    public float moveSpeedX;
    public float moveSpeedY;
    public float targetX;
    public float targetY;


    public float targetTime;
    public float currentTime;
    public float inputBuffer; //maybe use this if beat duration is too easy
    public Vector3 beatPosition;

    public bool targetReached;

    // Start is called before the first frame update
    void Start()
    {
        targetReached = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (beatDuration <= 0)
        {
            //Increment the target beat so the player can readjust, but take stamina drain
            if (stepNumber >= player.targetBeat)
            {
                player.targetBeat += 1;
            }
            this.gameObject.SetActive(false);

        }

        //If target is moving up it stops when it reaches the designated height
        if (moveSpeedX > 0)
        {
            if (transform.localPosition.x <= targetX)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + (moveSpeedX * Time.deltaTime), transform.localPosition.y, - 1);
            }
            else
            {
                targetReached = true;
            }
        }
        //If target is moving down it stops when it reaches the designated depth
        if (moveSpeedX < 0)
        {
            if (transform.localPosition.x >= targetX)
            {
                transform.localPosition = new Vector3(transform.localPosition.x + (moveSpeedX * Time.deltaTime), transform.localPosition.y, - 1);
            }
            else
            {
                targetReached = true;
            }
        }

        //If target is moving up it stops when it reaches the designated height
        if (moveSpeedY > 0)
        {
            if (transform.localPosition.y <= targetY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (moveSpeedY * Time.deltaTime), -1);
            }
            else
            {
                targetReached = true;
            }
        }

        //If target is moving left it stops when it reaches the designated area
        if (moveSpeedY < 0)
        {
            if (transform.localPosition.y >= targetY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + (moveSpeedY * Time.deltaTime), -1);
            }
            else
            {
                targetReached = true;
            }
        }

        //If you're within the window for keeping the beat, move the player and relieve some stress. Else, you take stress :(

        //Check to make sure this is the correct target
        if (player.targetBeat == stepNumber)
        {

            if (beatDuration > 0 && (player.targetBeat == stepNumber) )
            {
                if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
                {
                    if (moveDir == Direction.up && targetReached)
                    {
                        player.targetBeat += 1;
                        this.gameObject.SetActive(false);
                    }
                    
                }
               
                if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
                {
                    if (moveDir == Direction.down && targetReached)
                    {
                        player.targetBeat += 1;
                        this.gameObject.SetActive(false);
                    }
                }
                
                if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
                {
                    if (moveDir == Direction.left && targetReached)
                    {
                        player.targetBeat += 1;
                        this.gameObject.SetActive(false);
                    }
                }
                
                if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
                {
                    if (moveDir == Direction.right && targetReached)
                    {
                        player.targetBeat += 1;
                        this.gameObject.SetActive(false);
                    }
                }
              
            }
        }
    }
    void FixedUpdate()
    {
        currentTime = Time.time;

        beatDuration -= Time.deltaTime;


        //selects correct directional sprite and affects which input is required
        if (moveDir == Direction.up)
        {
            spriteRenderer.sprite = dirSprite[0];
        }

        if (moveDir == Direction.down)
        {
            spriteRenderer.sprite = dirSprite[1];
        }

        if (moveDir == Direction.left)
        {
            spriteRenderer.sprite = dirSprite[2];
        }
        if (moveDir == Direction.right)
        {
            spriteRenderer.sprite = dirSprite[3];
        }




    }

}

