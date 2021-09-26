using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveOnBeat : MonoBehaviour
{
    public enum Direction { up, down, left, right};
    public Direction moveDir;
    public List<Sprite> dirSprite;
    public SpriteRenderer spriteRenderer;

    //How long the object lasts before turning itself off
    public float beatDuration;

    public PlayerController player;
    public int playerTargetBeat;

    public int stepNumber;


    public float moveSpeed;
    public float targetTime;
    public float currentTime;
    public float inputBuffer; //maybe use this if beat duration is too easy
    public Vector3 beatPosition;

    public float stressReduce;
    public float stressAdd;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        playerTargetBeat = player.targetBeat;
        //If you're within the window for keeping the beat, move the player and relieve some stress. Else, you take stress :(

        //Check to make sure this is the correct target
        if (stepNumber == playerTargetBeat)
        {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)))
            {
                if (moveDir == Direction.up)
                {
                    MovePlayer();
                    player.playerDirection = PlayerController.Direction.up;
                }
                else
                {
                    player.stress += stressAdd;
                }
            }
            if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)))
            {
                if (moveDir == Direction.down)
                {
                    

                    MovePlayer();
                    player.playerDirection = PlayerController.Direction.down;
                }
                else
                {
                    player.stress += stressAdd;
                }
            }
            if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)))
            {
                if (moveDir == Direction.left)
                {
                    MovePlayer();
                    player.playerDirection = PlayerController.Direction.left;
                }
                else
                {
                    player.stress += stressAdd;
                }
            }
            if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)))
            {
                if (moveDir == Direction.right)
                {
                    MovePlayer();
                    player.playerDirection = PlayerController.Direction.right;
                }
                else
                {
                    player.stress += stressAdd;
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

    public void MovePlayer()
    {
        Debug.Log(player.targetBeat);
        player.stress -= stressReduce;
        player.beatDuration = beatDuration;
        player.targetPos = this.transform.position;
        player.StartCoroutine("Walk");
        this.gameObject.SetActive(false);
    }
    
}
