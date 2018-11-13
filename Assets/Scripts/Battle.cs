using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour {

  public int xSize = 8;
  public int ySize = 8;

  private Board board;
  public Transform boardContainer;
  public GameObject boardTile;

  public float time = 0;
  public float max = 0;
  public float min = 0;

  void Start() {
    board = new Board(xSize, ySize);

    for (int i = 0; i < xSize; i++) {
      for (int j = 0; j < ySize; j++) {
        CoOrds co = new CoOrds(i, j);
        Vector3 v = board.GetPosition(co);
        GameObject go = Instantiate(boardTile, v, Quaternion.identity, boardContainer);
        go.name = "Tile (" + i + ", " + j + ")";
      }
    }
  }

  void Update() {
    time += Time.deltaTime;
    float n = Perlin.Noise(time);
    max = Mathf.Max(n, max);
    min = Mathf.Min(n, min);
  }
}