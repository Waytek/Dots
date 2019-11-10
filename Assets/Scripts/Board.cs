using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
  public bool IsReady {
    get
    {
      bool isReady = true;
      for (int i = 0; i < dots.GetLength(0); i++)
      {
        for (int j = 0; j < dots.GetLength(1); j++)
        {
          if (dots[i, j] == null)
          {
            isReady = false;
          }
          else
          {
            isReady &= dots[i, j].IsReady;
          }
        }
      }
      return isReady;
    }
  }

  [SerializeField]
  private Dot dotPrefab;
  [SerializeField]
  private List<DotTypes> dotTypes;

  private Dot[,] dots;
  private int width;
  private int height;

  private void Start()
  {
    GenerateBoard(6, 6);
  }

  public void GenerateBoard(int width, int height)
  {
    dots = new Dot[width, height];
    this.width = width;
    this.height = height;
    for (int x = 0; x < width; x++)
    {
      for (int y = 0; y < height; y++)
      {
        CreateDot(x, y);
      }
    }
  }

  public Dot CreateDot(int x, int y)
  {
    if(dots[x,y] != null)
    {
      dots[x, y].Destroy();
    }
    Dot dot = Instantiate(dotPrefab, transform);
    dot.transform.localPosition = new Vector3(dot.Width * x, dot.Height * (y + height));
    int randomType = Random.Range(0, dotTypes.Count);
    dot.Init(x, y, dotTypes[randomType], this);
    dot.OnDotDestroyed += onDotDestroyed;
    dots[x,y] = dot;
    return dot;
  }

  public void StackBoard()
  {
    for (var x = 0; x < width; x++)
    {
      for (var y = 0; y < height; y++)
      {

        if (dots[x, y] != null) continue;

        Dot dot = getUp(x, y);
        if (dot == null)
        {
          continue;
        }
        dots[dot.X, dot.Y] = null;

        dot.SetPosition(x, y);

        dots[x, y] = dot;
      }
    }
  }

  public void RefilBoard()
  {
    for (var x = 0; x < width; x++)
    {
      for (var y = 0; y < height; y++)
      {
        if (dots[x,y] != null) continue;

        Dot dot = CreateDot(x, y);
      }
    }
  }

  public Dot GetDot(int x, int y)
  {
    if (x < width && y < height)
    {
      return dots[x, y];
    }
    return null;
  }

  public Dot GetDot(float xPos,float yPos)
  {
    int x = Mathf.FloorToInt(xPos / dotPrefab.Width);
    int y = Mathf.FloorToInt(yPos / dotPrefab.Height);
    return (GetDot(x, y));
  }

  private Dot getUp(int x, int y)
  {
    Dot result = null;
    for (var i = y; i < height; i++)
    {
      result = dots[x,i];
      if (result == null)
      {
        continue;
      }
      else
      {
        return result;
      }
    }
    return null;
  }

  private int dotColumnCount(int x)
  {
    int count = 0;
    for (int i = 0; i < height; i++)
    {
      if(dots[x, i] != null)
      {
        count++;
      }
    }
    return count;
  }

  private void onDotDestroyed(Dot dot)
  {
    dots[dot.X, dot.Y] = null;
    StartCoroutine(stackAndRefil());
  }

  private IEnumerator stackAndRefil()
  {
    yield return null;
    StackBoard();
    RefilBoard();
  }
}
