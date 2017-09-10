using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGridChunk : MonoBehaviour
{
    HexCell[] cells;
    public HexMesh terrain;

    //HexMesh hexMesh;
    Canvas gridCanvas;

    void Awake()
    {
        gridCanvas = GetComponentInChildren<Canvas>();
        //hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[HexMetrics.chunkSizeX * HexMetrics.chunkSizeZ];
        ShowUI(false);
    }

    void LateUpdate()
    {
        Triangulate();
        enabled = false;
    }

    public void Triangulate()
    {
        terrain.Clear();
        for (int i = 0; i < cells.Length; i++)
        {
            Triangulate(cells[i]);
        }
        terrain.Apply();
    }

    void Triangulate(HexCell cell)
    {
        for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++)
        {
            Triangulate(d, cell);
        }
    }

    void Triangulate(HexDirection direction, HexCell cell)
    {
        Vector3 center = cell.transform.localPosition;
        terrain.AddTriangle(
            center,
            center + HexMetrics.GetFirstCorner(direction),
            center + HexMetrics.GetSecondCorner(direction)
        );
        HexCell prevNeighbor = cell.GetNeighbor(direction.Previous()) ?? cell;
        HexCell neighbor = cell.GetNeighbor(direction) ?? cell;
        HexCell nextNeighbor = cell.GetNeighbor(direction.Next()) ?? cell;
        terrain.AddTriangleColor(cell.Color);
    }

    public void AddCell(int index, HexCell cell)
    {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }

    public void Refresh()
    {
        enabled = true;
    }

    public void ShowUI(bool visible)
    {
        gridCanvas.gameObject.SetActive(visible);
    }
}
