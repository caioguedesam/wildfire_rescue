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
    // Debug color for holding bucket
    public Color holdingColor;
    // Debug color for completed level
    public Color completedColor;
    // Debug color for failed level
    public Color failedColor;
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
    // is holders neighbor occupied
    public bool isNeighborHolding = false;
    // neighbor verify event
    public Vector2Event checkNeighborHoldingEvent;
    // animator reference
    private Animator animator;


    public Holder(Vector2Event passBucketEvent, Vector2Event receiveBucketEvent, Transform parent) {
        this.posX = 0;
        this.posY = 0;
        this.passBucketEvent = passBucketEvent;
        this.receiveBucketEvent = receiveBucketEvent;
        transform.parent = parent;
    }

    private void Start() {
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
            passBucketEvent.Raise(new Vector2(posX, posY));
            if(!hasFinished) {
                isHoldingBucket = canPassBucket = false;
                Debug.Log("Raised pass event from " + new Vector2(posX, posY));
                key = "";

                GameObject keyObj = transform.GetChild(0).gameObject;
                keyObj.SetActive(false);

                this.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    public void ReceiveBucket(Vector2 gridPos) {
        if(!hasFinished && !isHoldingBucket
            && gridPos.x == posX && gridPos.y == posY) {
            StartCoroutine(HoldBucket());
        }
    }

    public IEnumerator HoldBucket() {
        if(!isHoldingBucket) {
            isHoldingBucket = true;
            this.GetComponent<SpriteRenderer>().color = holdingColor;

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

        this.GetComponent<SpriteRenderer>().color = completedColor;
    }

    public void Failed() {
        hasFinished = true;
        isHoldingBucket = canPassBucket = false;
        key = "";

        GameObject keyObj = transform.GetChild(0).gameObject;
        keyObj.SetActive(false);

        this.GetComponent<SpriteRenderer>().color = failedColor;
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
