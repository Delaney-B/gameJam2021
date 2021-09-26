using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timescale : MonoBehaviour
{
    // Start is called before the first frame update

    //time information
    public string songPart; //this is just to help know which portion of the song you're editing
    public float trackTime; //time song has been playing since this script began
    public float endTrackTime; //time you switch to next beat
    public float beatStartTime; //what time (in the track) do you want to start the current playable sequence
    public float nextBeat; //set this to the rate you want beats to appear
    public int currentStep; //moves through the lists of steps and preset directions
    public int currentArrowObject; //moves through the arrow objects
    
    //Arrow object alterations
    public enum Direction { up, down, left, right };
    public List<Direction> nextSteps; //list of directions to appear throughout this song part
    public List<Vector2> directionPos;
    public List<GameObject> movementDirections;
    public float inputBuffer;

    //Changes info in the next beat object
    public MoveOnBeat beatInfo;
    public PlayerController player;

    public GameObject nextPart;


    void Start()
    {
        trackTime = 0;
        endTrackTime = (nextSteps.Count * nextBeat) + beatStartTime;
        player.targetBeat = 0;

         for (int i = 0; i < (nextSteps.Count - 1); i++)
        {
            directionPos.Add(new Vector2 (0,0));
        }
        for (int i = 0; i < (nextSteps.Count - 1); i++)
        {

            if (nextSteps[i + 1] == Direction.up)
            {
                directionPos[i + 1] = new Vector2(directionPos[i].x, directionPos[i].y + .25f);
            }
            if (nextSteps[i + 1] == Direction.down)
            {
                directionPos[i + 1] = new Vector2(directionPos[i].x, directionPos[i].y - .25f);
            }
            if (nextSteps[i + 1] == Direction.left)
            {
                directionPos[i + 1] = new Vector2(directionPos[i].x - .25f, directionPos[i].y);
            }

            if (nextSteps[i + 1] == Direction.right)
            {
                directionPos[i + 1] = new Vector2(directionPos[i].x + .25f, directionPos[i].y);
            }
        }
    }
    private void Update()
    {

        if (player.targetBeat < (currentStep - movementDirections.Count))
        {
            player.stress = 100;
        }
        //Track the time
        trackTime = Time.time;

        //End this part
        if (trackTime > endTrackTime)
        {
            nextPart.SetActive(true);
            this.gameObject.SetActive(false);
        }

        //Restart arrow object pool
        if (currentArrowObject >= movementDirections.Count)
        {
            currentArrowObject = 0;
        }


        if (currentStep < nextSteps.Count)
        {
            
            if (trackTime > beatStartTime)
            {
                //grabs the next arrow object and sets it's direction to the next beat in the list and sets it's duration timer
                beatInfo = movementDirections[currentArrowObject].GetComponent<MoveOnBeat>();
                beatInfo.beatDuration = (nextBeat * movementDirections.Count - inputBuffer);

                //changes arrow image direction then sets them active
                if (nextSteps[currentStep] == Direction.up)
                {
                    beatInfo.moveDir = MoveOnBeat.Direction.up;
                }
                else if (nextSteps[currentStep] == Direction.down)
                {
                    beatInfo.moveDir = MoveOnBeat.Direction.down;
                }
                else if (nextSteps[currentStep] == Direction.left)
                {
                    beatInfo.moveDir = MoveOnBeat.Direction.left;
                }
                else
                {
                    beatInfo.moveDir = MoveOnBeat.Direction.right;
                }
                movementDirections[currentArrowObject].transform.position = new Vector2(directionPos[currentStep].x, directionPos[currentStep].y);
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

