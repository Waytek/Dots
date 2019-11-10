using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dot : MonoBehaviour
{
  public int X { get; private set; }
  public int Y { get; private set; }
  public DotTypes Type { get; private set; }
  public float Width => image.rectTransform.sizeDelta.x;
  public float Height => image.rectTransform.sizeDelta.y;
  public bool IsReady = true;

  public Action<Dot> OnDotDestroyed;
 
  [SerializeField]
  private float dropTime = 1f;
  [SerializeField]
  private AnimationCurve dropCurve;
  [SerializeField]
  private Image image;

  private Board board;

  public void Init(int x, int y, DotTypes type, Board board)
  {
    this.board = board;
    SetPosition(x, y);
    SetType(type);
  }

  public void SetPosition(int x, int y)
  {
    X = x;
    Y = y;
    StartCoroutine(move(CalculatedPosition(X,Y), dropTime));
  }

  public void SetType(DotTypes type)
  {
    Type = type;
    image.color = type.color;
  }

  public Vector3 CalculatedPosition(int x, int y)
  {
    return new Vector3(x * Width, y * Height, 0);
  }

  private IEnumerator move(Vector3 position, float dropTime)
  {
    IsReady = false;
    float time = 0f;
    Vector3 startPos = transform.localPosition;
    while (time <= dropTime)
    {
      transform.localPosition = Vector3.Lerp(startPos, position, dropCurve.Evaluate(time));
      time += Time.deltaTime;
      yield return null;
    }
    transform.localPosition = Vector3.Lerp(startPos, position, dropCurve.Evaluate(1));
    IsReady = true;
  }

  public void Destroy()
  {
    OnDotDestroyed?.Invoke(this);
    Destroy(gameObject);
  }
}
