using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

  private float[,] heights;
  private int fieldSizeX, fieldSizeY;

  public Board(int sizeX, int sizeY) {

    // Initialize fields
    heights = new float[sizeX, sizeY];

    // Generate tile heights
    fieldSizeX = Mathf.RoundToInt(Mathf.Pow(2, 3)) + 1;
    fieldSizeY = Mathf.RoundToInt(Mathf.Pow(2, 3)) + 1;
    float[,] fractalField = GenerateFractalField();

    for (int i = 0; i < sizeX; i++) {
      for (int j = 0; j < sizeY; j++) {
        heights[i, j] = SampleFractalField(fractalField, i, j, sizeX, sizeY);
      }
    }
  }

  // Get vector3 world position of given coords
  public Vector3 GetPosition(CoOrds coords) {
    return new Vector3(coords.x, heights[coords.x, coords.y], coords.y);
  }

  // Generates a fractal landscape of size fieldSizeX by fieldSizeY
  public float[,] GenerateFractalField() {

    // Initialize points
    float[,] field = new float[fieldSizeX, fieldSizeY];
    field[0, 0] = 8f;
    field[0, fieldSizeY - 1] = 4f;
    field[fieldSizeX - 1, 0] = 0f;
    field[fieldSizeX - 1, fieldSizeY - 1] = 0f;

    int stepSize = fieldSizeX - 1;
    int halfStepSize;
    float randomSize = 6f;

    while (stepSize > 1) {
      halfStepSize = (stepSize / 2);
      randomSize = (randomSize / 2);

      for (int i = 0; i < fieldSizeX - 1; i += stepSize) {
        for (int j = 0; j < fieldSizeY - 1; j += stepSize) {
          // Diamond Square Alg

          // Diamond step
          float avg = 0f;
          avg += field[i, j];
          avg += field[i, j + stepSize];
          avg += field[i + stepSize, j];
          avg += field[i + stepSize, j + stepSize];
          field[i + halfStepSize, j + halfStepSize] = (avg / 4f) + Random.Range(-randomSize, randomSize);

          // Square step
          int[][] cardinals = new int[4][] {
            new int[2] {0, 1},
            new int[2] {0, -1},
            new int[2] {1, 0},
            new int[2] { -1, 0}
          };

          foreach (int[] xy in cardinals) {
            int x = i + halfStepSize + (xy[0] * halfStepSize);
            int y = j + halfStepSize + (xy[1] * halfStepSize);
            avg = 0f;
            int total = 0;

            foreach (int[] ab in cardinals) {
              int a = x + (ab[0] * halfStepSize);
              int b = y + (ab[1] * halfStepSize);

              // If out of range, skip
              if (a < 0 || b < 0 || a >= fieldSizeX || b >= fieldSizeY)
                continue;

              total += 1;
              avg += field[a, b];
            }

            field[x, y] = (avg / total) + Random.Range(-randomSize, randomSize);
          }
        }
      }

      stepSize = stepSize / 2;
    }

    return field;
  }

  // Samples from an fieldSizeX by fieldSizeY fractal field
  public int SampleFractalField(float[,] field, int x, int y, int sizeX, int sizeY) {
    float xf = (x * 1f) * (fieldSizeX - 1f) / (sizeX - 1f);
    float yf = (y * 1f) * (fieldSizeY - 1f) / (sizeY - 1f);
    return Mathf.RoundToInt(field[Mathf.RoundToInt(xf), Mathf.RoundToInt(yf)]);
  }

  // Gets 3d perlin noise, offset to center on 0.5f, and clamped to [0,1]
  /*public static int GetHeightPerlin(int x, int y) {

    float noise = Perlin.Noise(x * 10f, y * 10f, 10f);
    Debug.Log(noise);
    noise *= 0.5f;
    noise += 0.5f;
    noise = Mathf.Clamp(noise, 0f, 1f);
    return (int)(noise * 10f);
  }*/
}