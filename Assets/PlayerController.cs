using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator playerAnim;
    public enum Direction { up, down, left, right };
    public Direction playerDirection;
    public bool walk;
    public float moveSpeed;

    public float beatDuration;
    public Vector3 targetPos;
    public int targetBeat;

    public float stress;
    public Image stressMeter;
    void Start()
    {
       
    }


    // Update is called once per frame
    void Update()
    {
        stressMeter.fillAmount = stress / 100;
        stress = Mathf.Clamp(stress, 0, 100);

        if (beatDuration > 0)
        {
            moveSpeed = .1f / beatDuration;
        }

        if (transform.position != targetPos)
        {
            walk = true;
        } else
        {
            walk = false;
        }

        if (playerDirection == Direction.up)
        {
            playerAnim.SetBool("facingUp", true);
        } else 
        {playerAnim.SetBool("facingUp", false);}
        if (playerDirection == Direction.down)
        {
            playerAnim.SetBool("facingDown", true);
        }
        else
        { playerAnim.SetBool("facingDown", false); }
        if (playerDirection == Direction.left)
        {
            playerAnim.SetBool("facingLeft", true);
        }
        else
        { playerAnim.SetBool("facingLeft", false); }

        if (playerDirection == Direction.right)
        {
            playerAnim.SetBool("facingRight", true);
        }
        else
        { playerAnim.SetBool("facingRight", false); }

        

        if (walk)
        {
            playerAnim.SetBool("walking", true);
        } else
        {
            playerAnim.SetBool("walking", false);
        }
    }

    IEnumerator Walk()
    {
        yield return new WaitForSeconds(0);
        transform.position = Vector3.MoveTowards(transform.position, targetPos, 1);
        Debug.Log(targetPos);
        if (transform.position != transform.position)
        {
            StartCoroutine("Walk");
        } else
        {
            targetBeat += 1;
        }
    }
}
