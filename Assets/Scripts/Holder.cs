using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Holder : MonoBehaviour
{
    // Is this holder currently holding a bucket
    public bool isHoldingBucket = false;
    // Has this holder finished the level?
    public bool hasFinished = false;
    // Is this holder ready to release the bucket
    public bool canPassBucket = false;
    // Time to hold bucket
    public float bucketHoldTime = 2f;
    // Position on the grid
    public int posX, posY;
    // Pass bucket event
    public Vector2Event passBucketEvent, receiveBucketEvent;
    // Key press event
    public StringEvent keyPressEvent;
    // key dictionary for this holder
    public List<string> keys = new List<string>();
    // currently assigned key
    public string key = "";
    // Position of next neighbor on grid
    public int nextPosX, nextPosY;
    // Position of past neighbor on grid
    public int pastPosX, pastPosY;
    // Enum for pass and receive direction
    public enum Direction {
        left,
        right,
        up,
        down,
        toss
    }
    public Direction receiveDirection, passDirection;

    // is holders neighbor occupied
    public bool isNeighborHolding = false;
    // neighbor verify event
    public Vector2Event checkNeighborHoldingEvent;
    // animator reference
    private Animator animator;
    // pass animation duration in seconds
    public float passDuration = 1f;
    // event for last holder toss
    public BaseEvent lastHolderTossEvent;


    public Holder(Vector2Event passBucketEvent, Vector2Event receiveBucketEvent, Transform parent) {
        this.posX = 0;
        this.posY = 0;
        this.passBucketEvent = passBucketEvent;
        this.receiveBucketEvent = receiveBucketEvent;
        transform.parent = parent;
    }

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void SetHolderData(int posX, int posY, float bucketHoldTime, Vector2Event passBucketEvent, Vector2Event receiveBucketEvent, 
        StringEvent keyPressEvent, List<string> keys, Transform parent) {
        this.posX = posX;
        this.posY = posY;
        this.bucketHoldTime = bucketHoldTime;
        this.passBucketEvent = passBucketEvent;
        this.receiveBucketEvent = receiveBucketEvent;
        this.keyPressEvent = keyPressEvent;
        this.keys = keys;
        transform.parent = parent;
    }

    public Direction CalculateReceiveDirection() {
        Vector2 current = new Vector2(posX, posY);
        Vector2 past = new Vector2(pastPosX, pastPosY);
        Direction dir;

        // First in line receives from left
        if (past == -Vector2.one) dir = Direction.left;
        else if(current.y == past.y) {
            dir = (current.x > past.x) ? Direction.left : Direction.right;
        }
        else {
            dir = Direction.down;
        }

        animator.SetInteger("receiveDirection", (int)dir);
        return dir;

    }

    public Direction CalculatePassDirection() {
        Vector2 current = new Vector2(posX, posY);
        Vector2 next = new Vector2(nextPosX, nextPosY);
        Direction dir;

        // Last in line passes with a throw
        if (next == -Vector2.one) dir =  Direction.toss;
        else if (current.y == next.y) {
            dir = (current.x < next.x) ? Direction.right : Direction.left;
        }
        else {
            dir = Direction.up;
        }

        animator.SetInteger("passDirection", (int)dir);
        return dir;
    }

    public void PassBucketCheck() {

        if (!hasFinished && canPassBucket && key == "") {
            int randomIndex = Random.Range(0, keys.Count);
            key = keys[randomIndex];
            GameObject keyObj = transform.GetChild(0).gameObject;
            keyObj.SetActive(true);
            keyObj.GetComponentInChildren<TextMeshPro>().text = key;
        }
    }

    public void PassBucket(string keyPressed) {
        CheckNeighborHolding();

        if(key != "" && keyPressed == key && !isNeighborHolding) {
            StartCoroutine(PassBucketHandler());
        }
    }

    public IEnumerator PassBucketHandler() {
        animator.SetTrigger("passEvent");
        Debug.Log("pass animation trigger");
        key = "";

        if(new Vector2(nextPosX, nextPosY) == -Vector2.one) {
            lastHolderTossEvent.Raise();
        }
        if (!hasFinished) {
            isHoldingBucket = canPassBucket = false;

            GameObject keyObj = transform.GetChild(0).gameObject;
            keyObj.SetActive(false);

            //this.GetComponent<SpriteRenderer>().color = Color.white;
        }

        yield return new WaitForSeconds(passDuration);
        passBucketEvent.Raise(new Vector2(posX, posY));
        Debug.Log("Raised pass event from " + new Vector2(posX, posY));
    }

    public void ReceiveBucket(Vector2 gridPos) {
        if(!hasFinished && !isHoldingBucket
            && gridPos.x == posX && gridPos.y == posY) {
            StartCoroutine(HoldBucket());
        }
    }

    public IEnumerator HoldBucket() {
        //yield return new WaitForSeconds(passDuration);

        if (!isHoldingBucket) {
            animator.SetTrigger("receiveEvent");
            Debug.Log("receive animation trigger");
            isHoldingBucket = true;
            //this.GetComponent<SpriteRenderer>().color = holdingColor;

            yield return new WaitForSeconds(bucketHoldTime);

            canPassBucket = true;
        }
    }

    public void Completed() {
        hasFinished = true;
        isHoldingBucket = canPassBucket = false;
        key = "";

        GameObject keyObj = transform.GetChild(0).gameObject;
        keyObj.SetActive(false);

        animator.SetTrigger("win");
        //this.GetComponent<SpriteRenderer>().color = completedColor;
    }

    public void Failed() {
        hasFinished = true;
        isHoldingBucket = canPassBucket = false;
        key = "";

        GameObject keyObj = transform.GetChild(0).gameObject;
        keyObj.SetActive(false);

        animator.SetTrigger("lose");
        //this.GetComponent<SpriteRenderer>().color = failedColor;
    }

    public void CheckNeighborHolding() {
        checkNeighborHoldingEvent.Raise(new Vector2(nextPosX, nextPosY));
    }

    public void SetNeighborHolding(bool value) {
        isNeighborHolding = value;
    }

    private void Update() {
        PassBucketCheck();
    }
}
