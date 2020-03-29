using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GridManager : MonoBehaviour
{
    // Grid variables
    public int width = 1, height = 1;
    public float cellSize = 1f;
    // origin position for level grid
    public Vector3 originPosition = Vector3.zero;

    private Grid<GameObject> grid;
    // pass and receive bucket events
    public Vector2Event passEvent, receiveEvent;
    public BaseEvent bucketReachedEndEvent;
    public StringEvent keyPressEvent;
    // bucket hold time for this level
    public float bucketHoldTime = 2f;
    // spawn new bucket every x seconds
    public float bucketRespawnTime = 10f;
    // spawn first bucket after how many seconds
    public float firstBucketSpawnTime = 2f;
    // Holder prefab
    public GameObject holderPrefab;
    // Available keys for this level
    public List<string> keys = new List<string>();
    // How many times end has been reached
    private int endReached = 0;
    // How many times do you need to reach end to win
    public int bucketsToEnd = 5;
    // Completed Level Event
    public BaseEvent completedLevelEvent;
    // neighbor verify event
    public BoolEvent sendNeighborHoldingEvent;
    // text for buckets remaining
    public TextMeshProUGUI bucketText;

    private void Start() {
        grid = new Grid<GameObject>(width, height, cellSize, originPosition, () => GameObject.Instantiate(holderPrefab));
        PopulateGridHolders();

        StartCoroutine(SpawnBucketHandler());

        bucketText.text = (bucketsToEnd - endReached).ToString() + " buckets remaining";
        if(bucketsToEnd == 1) bucketText.text = (bucketsToEnd - endReached).ToString() + " bucket remaining";
    }

    private void RaiseKeyEvent(KeyCode key) {
        if(key != KeyCode.None && keys.Contains(key.ToString())) {
            keyPressEvent.Raise(key.ToString());
            Debug.Log("Raised event with key " + key.ToString());
        }
    }

    private void OnGUI() {
        UnityEngine.Event e = UnityEngine.Event.current;
        if(e != null && e.isKey && Input.GetKeyDown(e.keyCode)) {
            RaiseKeyEvent(e.keyCode);
        }
    }

    private void PopulateGridHolders() {
        for (int i = 0; i < grid.gridArray.GetLength(0); i++) {
            for (int j = 0; j < grid.gridArray.GetLength(1); j++) {
                Holder holder = grid.gridArray[i, j].GetComponent<Holder>();
                holder.SetHolderData(i, j, bucketHoldTime, passEvent, receiveEvent, keyPressEvent, keys, transform);
                Vector2 nextPos = NextNeighborPos(new Vector2(i, j));
                holder.nextPosX = (int)nextPos.x;
                holder.nextPosY = (int)nextPos.y;
                Vector2 pastPos = pastNeighborPos(new Vector2(i, j));
                holder.pastPosX = (int)pastPos.x;
                holder.pastPosY = (int)pastPos.y;

                holder.receiveDirection = holder.CalculateReceiveDirection();
                holder.passDirection = holder.CalculatePassDirection();

                grid.gridArray[i, j].transform.position = grid.GetWorldPosition(i, j);
            }
        }
    }

    public void SpawnNewBucket() {
        // Raise receive event to first element of grid
        if(!grid.gridArray[0,0].GetComponent<Holder>().isHoldingBucket) {
            receiveEvent.Raise(new Vector2(0, 0));
            Debug.Log("Receive event with " + new Vector2(0, 0));
        }
    }

    public IEnumerator SpawnBucketHandler() {
        yield return new WaitForSeconds(firstBucketSpawnTime);

        while (true) {
            SpawnNewBucket();

            yield return new WaitForSeconds(bucketRespawnTime);
        }
    }

    public void GoToNextHolder(Vector2 pastHolder) {
        if(pastHolder.x < grid.gridArray.GetLength(0) && pastHolder.y < grid.gridArray.GetLength(1)) {
            if (pastHolder.y % 2 == 0) {
                // go towards right
                if (pastHolder.x == grid.gridArray.GetLength(0) - 1) {
                    // go up
                    receiveEvent.Raise(new Vector2(pastHolder.x, pastHolder.y + 1));
                    Debug.Log("Receive event with " + new Vector2(pastHolder.x, pastHolder.y + 1));
                }
                else {
                    // go right
                    receiveEvent.Raise(new Vector2(pastHolder.x + 1, pastHolder.y));
                    Debug.Log("Receive event with " + new Vector2(pastHolder.x + 1, pastHolder.y));
                }
            }
            else {
                // go towards left
                if (pastHolder.x == 0) {
                    // go up
                    receiveEvent.Raise(new Vector2(pastHolder.x, pastHolder.y + 1));
                    Debug.Log("Receive event with " + new Vector2(pastHolder.x, pastHolder.y + 1));
                }
                else {
                    // go left
                    receiveEvent.Raise(new Vector2(pastHolder.x - 1, pastHolder.y));
                    Debug.Log("Receive event with " + new Vector2(pastHolder.x - 1, pastHolder.y));
                }
            }
        }
    }

    public Vector2 NextNeighborPos(Vector2 holderPos) {
        if (holderPos.x < grid.gridArray.GetLength(0) && holderPos.y < grid.gridArray.GetLength(1)) {
            // End of grid neighbor is -1,-1 by default, may change later
            if (holderPos.x == grid.gridArray.GetLength(0) - 1 && holderPos.y == grid.gridArray.GetLength(1) - 1)
                return new Vector2(-1, -1);


            if (holderPos.y % 2 == 0) {
                // go towards right
                if (holderPos.x == grid.gridArray.GetLength(0) - 1) {
                    // go up
                    return new Vector2(holderPos.x, holderPos.y + 1);
                }
                else {
                    // go right
                    return new Vector2(holderPos.x + 1, holderPos.y);
                }
            }
            else {
                // go towards left
                if (holderPos.x == 0) {
                    // go up
                    return new Vector2(holderPos.x, holderPos.y + 1);
                }
                else {
                    // go left
                    return new Vector2(holderPos.x - 1, holderPos.y);
                }
            }
        }
        else {
            return new Vector2(0, 0);
        }
    }

    // redo later because this is horrible but works
    private Vector2 pastNeighborPos(Vector2 holderPos) {
        for(int i = 0; i < grid.gridArray.GetLength(0); i++) {
            for(int j = 0; j < grid.gridArray.GetLength(1); j++) {
                Vector2 nextPos = NextNeighborPos(new Vector2(i, j));
                if(nextPos == holderPos) {
                    return new Vector2(i, j);
                }
            }
        }
        return new Vector2(-1, -1);
    }

    public void CheckForNeighborHolding(Vector2 neighborPos) {
        if (neighborPos == -Vector2.one) sendNeighborHoldingEvent.Raise(false);
        else {
            // Checks if holder in given grid position is holding a bucket
            sendNeighborHoldingEvent.Raise(grid.gridArray[(int)neighborPos.x, (int)neighborPos.y].GetComponent<Holder>().isHoldingBucket);
        }
    }

    public void CheckForEndReached(Vector2 gridPos) {
        if (gridPos.x == grid.gridArray.GetLength(0) - 1 && gridPos.y == grid.gridArray.GetLength(1) - 1) {
            endReached++;
            bucketText.text = (bucketsToEnd - endReached).ToString() + " buckets remaining";
            if(endReached == bucketsToEnd - 1) bucketText.text = (bucketsToEnd - endReached).ToString() + " bucket remaining";
            Debug.Log("End reached " + endReached + " times");
            bucketReachedEndEvent.Raise();
            if (endReached == bucketsToEnd) {
                completedLevelEvent.Raise();
                bucketText.gameObject.SetActive(false);
            }
        }
    }

    public void ClearBucketText() {
        bucketText.gameObject.SetActive(false);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(originPosition, .5f);
    }

    // LOOP LIKE THIS:
    /*
    for(int i = 0; i < gridArray.GetLength(1); i++) {
        if(i % 2 == 0) {
            for (int j = 0; j < gridArray.GetLength(0); j++) {
                Debug.Log(i + "," + j);
            }
        }
        else {
            for (int j = gridArray.GetLength(0) - 1; j >= 0; j--) {
                Debug.Log(i + "," + j);
            }
        }
    }*/
}
