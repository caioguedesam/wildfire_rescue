using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start() {
        grid = new Grid<GameObject>(width, height, cellSize, originPosition, () => GameObject.Instantiate(holderPrefab));
        PopulateGridHolders();

        StartCoroutine(SpawnBucketHandler());
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
                Vector2 neighborPos = NeighborPos(new Vector2(i, j));
                holder.nextPosX = (int)neighborPos.x;
                holder.nextPosY = (int)neighborPos.y;
                grid.gridArray[i, j].transform.position = grid.GetWorldPosition(i, j);
            }
        }
    }

    public void SpawnNewBucket() {
        // Raise receive event to first element of grid
        receiveEvent.Raise(new Vector2(0, 0));
        Debug.Log("Receive event with " + new Vector2(0, 0));
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

    public Vector2 NeighborPos(Vector2 pastHolder) {
        if (pastHolder.x < grid.gridArray.GetLength(0) && pastHolder.y < grid.gridArray.GetLength(1)) {
            // End of grid neighbor is 0,0 by default, may change later
            if (pastHolder.x == grid.gridArray.GetLength(0) - 1 && pastHolder.y == grid.gridArray.GetLength(1) - 1)
                return new Vector2(0, 0);


            if (pastHolder.y % 2 == 0) {
                // go towards right
                if (pastHolder.x == grid.gridArray.GetLength(0) - 1) {
                    // go up
                    return new Vector2(pastHolder.x, pastHolder.y + 1);
                }
                else {
                    // go right
                    return new Vector2(pastHolder.x + 1, pastHolder.y);
                }
            }
            else {
                // go towards left
                if (pastHolder.x == 0) {
                    // go up
                    return new Vector2(pastHolder.x, pastHolder.y + 1);
                }
                else {
                    // go left
                    return new Vector2(pastHolder.x - 1, pastHolder.y);
                }
            }
        }
        else {
            return new Vector2(0, 0);
        }
    }

    public void CheckForNeighborHolding(Vector2 neighborPos) {
        if (neighborPos == Vector2.zero) sendNeighborHoldingEvent.Raise(false);
        else {
            // Checks if holder in given grid position is holding a bucket
            sendNeighborHoldingEvent.Raise(grid.gridArray[(int)neighborPos.x, (int)neighborPos.y].GetComponent<Holder>().isHoldingBucket);
        }
    }

    public void CheckForEndReached(Vector2 gridPos) {
        if (gridPos.x == grid.gridArray.GetLength(0) - 1 && gridPos.y == grid.gridArray.GetLength(1) - 1) {
            endReached++;
            Debug.Log("End reached " + endReached + " times");
            bucketReachedEndEvent.Raise();
            if (endReached == bucketsToEnd) completedLevelEvent.Raise();
        }
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
