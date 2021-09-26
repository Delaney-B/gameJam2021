using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTimescale : MonoBehaviour
{
    // Start is called before the first frame update

    //time information
    public string songPart; //this is just to help know which portion of the song you're editing
    public float trackTime; //time song has been playing since this script began
    public float beatStartTime; //what time (in the track) do you want to start the current playable sequence
    public float nextBeat; //set this to the rate you want beats to appear
    public int currentStep; //moves through the lists of steps and preset directions
    public int currentArrowObject; //moves through the arrow objects

    //Arrow object alterations
    public enum Direction { up, down, left, right };
    public List<Direction> nextSteps;
    public List<Vector2> directionPos;
    public List<GameObject> movementDirections;
    public float inputBuffer;

    //Arrow movement
    public float moveSpeedX;
    public float moveSpeedY;
    public float targetX;
    public float targetY;

    //Changes info in the next beat object
    public MinigameArrow beatInfo;
    public ArrowMovement arrowMovement;
    public PlayerController player;

    void Start()
    {
        trackTime = 0;
        player.targetBeat = 0;
        for (int i = 0; i < movementDirections.Count; i++)
        {

        }

    }
    private void Update()
    {
        if (currentArrowObject >= movementDirections.Count)
        {
            currentArrowObject = 0;

        }

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentStep < nextSteps.Count)
        {
            trackTime = Time.time;
            if (trackTime > beatStartTime)
            {
                //grabs the next arrow object and sets it's direction to the next beat in the list and sets it's duration timer
                beatInfo = movementDirections[currentArrowObject].GetComponent<MinigameArrow>();
                beatInfo.beatDuration = (nextBeat * movementDirections.Count);

                //Sets each arrow to it's target
                arrowMovement = movementDirections[currentArrowObject].GetComponent<ArrowMovement>();
                beatInfo.moveSpeedX = moveSpeedX;
                beatInfo.moveSpeedY = moveSpeedY;
                beatInfo.targetX = targetX;
                beatInfo.targetY = targetY;

                //changes arrow image direction then sets them active
                if (nextSteps[currentStep] == Direction.up)
                {
                    beatInfo.moveDir = MinigameArrow.Direction.up;
                }
                else if (nextSteps[currentStep] == Direction.down)
                {
                    beatInfo.moveDir = MinigameArrow.Direction.down;
                }
                else if (nextSteps[currentStep] == Direction.left)
                {
                    beatInfo.moveDir = MinigameArrow.Direction.left;
                }
                else
                {
                    beatInfo.moveDir = MinigameArrow.Direction.right;
                }
                //Sets each arrow's starting position
                movementDirections[currentArrowObject].transform.localPosition = new Vector2(directionPos[currentStep].x, directionPos[currentStep].y);
                movementDirections[currentArrowObject].SetActive(true);

                //Sends arrow object info that corresponds to player input
                beatInfo.inputBuffer = inputBuffer;
                beatInfo.targetTime = trackTime;
                beatInfo.stepNumber = currentStep;

                //update appear time to impact next arrow object in sequence
                beatStartTime = beatStartTime + nextBeat;
                currentStep += 1;
                currentArrowObject += 1;
            }
        }

    }
}
