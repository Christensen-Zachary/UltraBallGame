using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickFixCollision : MonoBehaviour
{
    [field: SerializeField]
    public ResourceLocator ResourceLocator { get; set; }
    public GameObject _markerPrefab; // reference from editor
    public List<BrickCollision> Bricks { get; private set; } = new List<BrickCollision>();

    //public PolygonCollider2D PolygonCollider2D { get; set; }
    public List<EdgeCollider2D> EdgeCollider2Ds { get; private set; } = new List<EdgeCollider2D>();

    private Grid _grid;


    void Awake()
    {
        ResourceLocator.AddResource("BrickFixCollision", this);

        _grid = ResourceLocator.GetResource<Grid>("Grid");

        //PolygonCollider2D = GetComponent<PolygonCollider2D>();
    }

    private void SetBrickNeighbors()
    {
        foreach (var brick in Bricks)
        {
            brick.HasBeenUsed = false;
            //BrickCollision topLeftNeighbor = Bricks.Find(x => (x.Row == brick.Row - 1 && x.Col == brick.Col - 1));
            BrickCollision topNeighbor = Bricks.Find(x => (x.Row == brick.Row - 1 && x.Col == brick.Col));
            //BrickCollision topRightNeighbor = Bricks.Find(x => (x.Row == brick.Row - 1 && x.Col == brick.Col + 1));
            BrickCollision rightNeighbor = Bricks.Find(x => x.Row == brick.Row && x.Col == brick.Col + 1);
            //BrickCollision bottomRightNeighbor = Bricks.Find(x => (x.Row == brick.Row + 1 && x.Col == brick.Col + 1));
            BrickCollision bottomNeighbor = Bricks.Find(x => (x.Row == brick.Row + 1 && x.Col == brick.Col));
            //BrickCollision bottomLeftNeighbor = Bricks.Find(x => (x.Row == brick.Row + 1 && x.Col == brick.Col - 1));
            BrickCollision leftNeighbor = Bricks.Find(x => x.Row == brick.Row && x.Col == brick.Col - 1);

            brick.Neighbors[0] = leftNeighbor;
            brick.Neighbors[1] = topNeighbor;
            brick.Neighbors[2] = rightNeighbor;
            brick.Neighbors[3] = bottomNeighbor;

            foreach (var key in brick.Neighbors.Keys)
            {
                if (brick.Neighbors[key] != null)
                {
                    //GameObject obj = Instantiate(_markerPrefab);
                    //obj.transform.localScale = Vector3.one * 0.72f;
                    //obj.transform.localPosition = brick.Neighbors[key].transform.position;
                }
            }
        }
    }

    bool advance = false;
    private void Update()
    {
        advance = false;
        if (Input.GetKeyDown(KeyCode.A))
        {
            advance = true;
        }
    }

    public void SetPolygonColliderPaths()
    {
        SetBrickNeighbors();

        //PolygonCollider2D.pathCount = 0;

        int edgeColliderCounter = 0;
        for (int row = 0; row < _grid.NumberOfDivisions; row++)
        {
            for (int col = 0; col < _grid.NumberOfDivisions; col++)
            {
                BrickCollision brickCollision = Bricks.Find(x => x.Row == row && x.Col == col && !x.HasBeenUsed && !(x.Neighbors[0] != null && x.Neighbors[1] != null && x.Neighbors[2] != null && x.Neighbors[3] != null));
                if (brickCollision != null)
                {
                    int i = 0;
                    //for (; i < PolygonCollider2D.pathCount; i++)
                    //{
                    //    if (PolygonCollider2D.GetPath(i).Contains(brickCollision.transform.position))
                    //    {
                    //        i = -1;
                    //        break;
                    //    }
                    //    else
                    //    {
                    //        print($"{PolygonCollider2D.GetPath(i).Length}");
                    //    }
                    //}
                    //if (i == -1) continue;

                    //PolygonCollider2D.pathCount++;

                    int startIndex = 2;
                    List<Vector2> path = new List<Vector2>();
                    //BrickCollision startBrick = brickCollision;
                    print($"StartBrick {brickCollision.name}");
                    Vector2 startPoint = brickCollision.Collider2D.GetPath(0)[0] * brickCollision.transform.localScale.x + (Vector2)brickCollision.transform.position;
                    path.Add(startPoint);
                    bool isPathComplete = false;
                    bool isFirstBrick = true;

                    //Transform marker = Instantiate(_markerPrefab).transform;
                    //marker.localScale *= 0.5f;
                    //marker.GetComponent<SpriteRenderer>().color = Color.red;
                    //marker.GetComponent<SpriteRenderer>().sortingOrder = 10;
                    while (!isPathComplete)
                    {
                        brickCollision.HasBeenUsed = true;
                        List<Vector2> points = new List<Vector2>();
                        for (i = 0; i < 4; i++)
                        {
                            BrickCollision neighbor = brickCollision.Neighbors[(i + startIndex) % 4];

                            Vector2 point = brickCollision.Collider2D.GetPath(0)[(i + startIndex + 3) % 4] * brickCollision.transform.localScale.x + (Vector2)brickCollision.transform.position;

                            //marker.position = point;


                            if (point == startPoint)
                            {
                                path.AddRange(points);
                                isPathComplete = true;
                                break;
                            }

                            if (!((neighbor == null && brickCollision.Neighbors[(i + startIndex + 3) % 4] != null)
                                || (neighbor != null && brickCollision.Neighbors[(i + startIndex + 3) % 4] == null))) // if (not (corner has only one neightbor))
                            {
                                //marker.position = point;
                                points.Add(point);
                            }



                            // if (!path.Contains(point)) path.Add(point);


                            //yield return new WaitUntil(() => advance);
                            //yield return new WaitForSeconds(0.1f);
                            //yield return null;

                            if (neighbor != null)
                            {
                                int tempStartIndex = startIndex;
                                startIndex = ((startIndex + i + 2) % 4) + 1;
                                if (tempStartIndex != startIndex || isFirstBrick)
                                {
                                    isFirstBrick = false;

                                    path.AddRange(points);
                                    //Transform marker1 = Instantiate(_markerPrefab).transform;
                                    //marker1.localScale *= 0.72f;
                                    //marker1.GetComponent<SpriteRenderer>().color = Color.red;
                                    //marker1.GetComponent<SpriteRenderer>().sortingOrder = 11;
                                    //marker1.transform.position = brickCollision.transform.position;
                                    //if (points.Count == 0) path.Add(point);
                                    //else
                                    //{
                                    //    points.RemoveAt(0);
                                    //    path.AddRange(points);
                                    //}
                                }
                                brickCollision = neighbor;

                                //print($"si {startIndex} from {brickCollision.name} to {neighbor.name}");
                                break;
                            }
                            else
                            {
                                //if (brickCollision.Neighbors[(startIndex + i + 1) % 4] == null) 
                            }
                        }
                    }

                    for (i = 0; i < path.Count; i++)
                    {
                        //marker = Instantiate(_markerPrefab).transform;
                        //marker.localScale *= 0.1f;
                        //marker.GetComponent<SpriteRenderer>().color = Color.green;
                        //marker.GetComponent<SpriteRenderer>().sortingOrder = 11;
                        //marker.transform.position = path[i];
                        //if ((path[i].x == path[i + 1].x && path[i].x == path[i + 2].x) || (path[i].y == path[i + 1].y && path[i].y == path[i + 2].y))
                        //{
                        //    // path.RemoveAt(i + 1);

                        //}
                        //else
                        //{
                        //    tempPath.Add(path[i]);
                        //    tempPath.Add(path[i + 2]);
                        //}
                    }

                    path.Add(startPoint);
                    if (edgeColliderCounter++ < EdgeCollider2Ds.Count)
                    {
                        EdgeCollider2Ds[edgeColliderCounter - 1].SetPoints(path);
                    }
                    else
                    {
                        EdgeCollider2D edgeCollider2D = CreateEdgeCollider2D();
                        edgeCollider2D.SetPoints(path);
                        edgeCollider2D.sharedMaterial = Resources.Load<PhysicsMaterial2D>("Materials/AllBounceNoFriction");
                        EdgeCollider2Ds.Add(edgeCollider2D);
                    }
                    //PolygonCollider2D.SetPath(PolygonCollider2D.pathCount - 1, path.ToArray());
                }
            }
        }

        //if (edgeColliderCounter < EdgeCollider2Ds.Count)
        //{
        //    for (int i = edgeColliderCounter; i < EdgeCollider2Ds.Count; i++)
        //    {
        //        Destroy(EdgeCollider2Ds[i].gameObject);
        //    }

        //    EdgeCollider2Ds.RemoveRange(edgeColliderCounter, EdgeCollider2Ds.Count - edgeColliderCounter);
        //}
    }

    public EdgeCollider2D CreateEdgeCollider2D()
    {
        GameObject obj = new GameObject($"Edge Collider {Guid.NewGuid()}");
        obj.transform.SetParent(transform);
        return obj.AddComponent<EdgeCollider2D>();
    }

    //public void SetProblemCorners()
    //{
    //    for (int i = 0; i < Bricks.Count; i++)
    //    {
    //        BrickCollision currentBrick = Bricks[i];
    //        BrickCollision topNeighbor = Bricks.Find(x => (x.Row == currentBrick.Row - 1 && x.Col == currentBrick.Col));
    //        BrickCollision bottomNeighbor = Bricks.Find(x => (x.Row == currentBrick.Row + 1 && x.Col == currentBrick.Col));
    //        BrickCollision leftNeighbor = Bricks.Find(x => x.Row == currentBrick.Row && x.Col == currentBrick.Col - 1);
    //        BrickCollision rightNeighbor = Bricks.Find(x => x.Row == currentBrick.Row && x.Col == currentBrick.Col + 1);
    //        bool doDrawDot = true;
    //        for (int j = 0; j < currentBrick.Collider2D.GetPath(0).Length; j++)
    //        {
    //            switch (j)
    //            {
    //                case 0:
    //                    if (!(topNeighbor != null && leftNeighbor != null))
    //                    {
    //                        currentBrick.IsProblemCorner |= 1 << j; // turn on

    //                        if (topNeighbor != null)
    //                        {
    //                            currentBrick.NewNormals[j] = new Vector2(-1, 0);
    //                        }
    //                        else if (leftNeighbor != null)
    //                        {
    //                            currentBrick.NewNormals[j] = new Vector2(0, 1);
    //                        }
    //                        else
    //                        {
    //                            currentBrick.IsProblemCorner &= ~(1 << j);  // turn off
    //                            doDrawDot = false;
    //                        }
    //                    }
    //                    break;
    //                case 1:
    //                    if (!(topNeighbor != null && rightNeighbor != null))
    //                    {
    //                        currentBrick.IsProblemCorner |= 1 << j;

    //                        if (topNeighbor != null)
    //                        {
    //                            currentBrick.NewNormals[j] = new Vector2(1, 0);
    //                        }
    //                        else if (rightNeighbor != null)
    //                        {
    //                            currentBrick.NewNormals[j] = new Vector2(0, 1);
    //                        }
    //                        else
    //                        {
    //                            currentBrick.IsProblemCorner &= ~(1 << j);
    //                            doDrawDot = false;
    //                        }
    //                    }
    //                    break;
    //                case 2:
    //                    if (!(rightNeighbor != null && bottomNeighbor != null))
    //                    {
    //                        currentBrick.IsProblemCorner |= 1 << j;

    //                        if (rightNeighbor != null)
    //                        {
    //                            currentBrick.NewNormals[j] = new Vector2(0, -1);
    //                        }
    //                        else if (bottomNeighbor != null)
    //                        {
    //                            currentBrick.NewNormals[j] = new Vector2(1, 0);
    //                        }
    //                        else
    //                        {
    //                            currentBrick.IsProblemCorner &= ~(1 << j);
    //                            doDrawDot = false;
    //                        }
    //                    }
    //                    break;
    //                case 3:
    //                    if (!(bottomNeighbor != null && leftNeighbor != null)) 
    //                    {
    //                        currentBrick.IsProblemCorner |= 1 << j;

    //                        if (bottomNeighbor != null) 
    //                        {
    //                            currentBrick.NewNormals[j] = new Vector2(-1, 0);
    //                        }
    //                        else if (leftNeighbor != null)
    //                        {
    //                            currentBrick.NewNormals[j] = new Vector2(0, -1);
    //                        }
    //                        else
    //                        {
    //                            currentBrick.IsProblemCorner &= ~(1 << j);
    //                            doDrawDot = false;
    //                        }
    //                    }
    //                    break;
    //            }

    //            if (doDrawDot)
    //            {
    //                GameObject obj = Instantiate(_markerPrefab);
    //                obj.transform.localScale = Vector3.one * 0.2f;
    //                obj.transform.localPosition = (Vector2)currentBrick.transform.position + currentBrick.Collider2D.GetPath(0)[j] * currentBrick.transform.localScale;

    //                Debug.DrawLine((Vector2)currentBrick.transform.position + currentBrick.Collider2D.GetPath(0)[j] * currentBrick.transform.localScale, (Vector2)currentBrick.transform.position + currentBrick.Collider2D.GetPath(0)[j] * currentBrick.transform.localScale + currentBrick.NewNormals[j] * 0.15f, Color.red, 60);
    //            }
    //        }
    //    }
    //}

}
