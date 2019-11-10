using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputLineMatcher : MonoBehaviour
{
  [SerializeField]
  private Board board;
  [SerializeField]
  private LineRenderer lineRenderer;
  [SerializeField]
  private LineRenderer mouseLineRenderer;

  private Camera mainCamera;
  private List<Dot> selectableDots = new List<Dot>();

  public void Start()
  {
    mainCamera = Camera.main;
  }

  void Update()
  {
    if (!board.IsReady)
    {
      return;
    }
    if (Input.GetKey(KeyCode.Mouse0))
    {
      foreach (var res in getEventSystemRaycastResults())
      {
        if (res.gameObject.tag == "dot")
        {
          var dot = board.GetDot(res.gameObject.transform.localPosition.x, res.gameObject.transform.localPosition.y);
          if (canMatch(dot))
          {
            selectableDots.Add(dot);
            updateLine();
          }
          if (selectableDots.Count > 1)
          {
            if (dot == selectableDots[selectableDots.Count - 2])
            {
              selectableDots.Remove(selectableDots[selectableDots.Count - 1]);
              updateLine();
            }
          }
        }
      }
      updateMouseLineRenderer();
    }
    if (Input.GetKeyUp(KeyCode.Mouse0))
    {
      destroySelectedDots();
    }
  }

  private List<RaycastResult> getEventSystemRaycastResults()
  {
    PointerEventData eventData = new PointerEventData(EventSystem.current);
    eventData.position = Input.mousePosition;
    List<RaycastResult> raycastResults = new List<RaycastResult>();
    EventSystem.current.RaycastAll(eventData, raycastResults);
    return raycastResults;
  }

  private void updateLine()
  {
    if(selectableDots.Count > 0)
    {
      lineRenderer.startColor = selectableDots[0].Type.color;
      lineRenderer.endColor =  selectableDots[0].Type.color;
    }
    if (selectableDots.Count > lineRenderer.positionCount-1)
    {
      for (int i = 0; i < selectableDots.Count; i++)
      {
        if(i >= lineRenderer.positionCount)
        {
          lineRenderer.positionCount++;
          lineRenderer.SetPosition(i, selectableDots[i].transform.position);
        }
      }
    }
    else
    {
      lineRenderer.positionCount = selectableDots.Count;
    }
  }

  private void updateMouseLineRenderer()
  {
    if (lineRenderer.positionCount > 0)
    {
      mouseLineRenderer.startColor = selectableDots[0].Type.color;
      mouseLineRenderer.endColor = selectableDots[0].Type.color;
      mouseLineRenderer.positionCount = 2;
      mouseLineRenderer.SetPosition(0, selectableDots[selectableDots.Count - 1].transform.position);
      Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
      mouseWorldPosition.z = 90;
      mouseLineRenderer.SetPosition(1, mouseWorldPosition);
    }
    else
    {
      mouseLineRenderer.positionCount = 0;
    }
  }

  private bool canMatch(Dot matchDot)
  {
    if (selectableDots.Contains(matchDot))
    {
      return false;
    }
    if (selectableDots.Count > 0)
    {
      bool canMatch = true;
      Dot lastDot = selectableDots[selectableDots.Count - 1];
      canMatch &= Mathf.Abs(lastDot.Y - matchDot.Y) == 1 && lastDot.X - matchDot.X == 0
        || lastDot.Y - matchDot.Y == 0 && Mathf.Abs(lastDot.X - matchDot.X) == 1;
      canMatch &= lastDot.Type == matchDot.Type;
      return canMatch;
    }
    else
    {
      return true;
    }
  }

  private void destroySelectedDots()
  {
    if (selectableDots.Count > 1)
    {

      foreach (var dot in selectableDots)
      {
        dot.Destroy();
      }      
    }
    selectableDots.Clear();
    updateLine();
    updateMouseLineRenderer();
  }
}
