﻿using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

    public HexGrid hexGrid;
    HexCell searchFromCell, searchToCell;

    int activeTerrainTypeIndex;

    void Update () {
		if (
			Input.GetMouseButton(0) &&
			!EventSystem.current.IsPointerOverGameObject()
		) {
			HandleInput();
		}
	}

	void HandleInput () {
		Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(inputRay, out hit)) {
            EditCell(hexGrid.GetCell(hit.point));
            HexCell currentCell = hexGrid.GetCell(hit.point);
            if (Input.GetKey(KeyCode.LeftShift) && searchToCell != currentCell)
            {
                if (searchFromCell)
                {
                    searchFromCell.DisableHighlight();
                }
                searchFromCell = currentCell;
                searchFromCell.EnableHighlight(Color.blue);
                if (searchToCell)
                {
                    hexGrid.FindPath(searchFromCell, searchToCell, 24);
                }
            }
            if (searchFromCell && searchFromCell != currentCell)
            {
                searchToCell = currentCell;
                hexGrid.FindPath(searchFromCell, searchToCell, 24);
            }
        }
    }

    void EditCell(HexCell cell)
    {
        if (cell)
        {
            if (activeTerrainTypeIndex >= 0)
            {
                cell.TerrainTypeIndex = activeTerrainTypeIndex;
            }
        }

    }

    public void ShowUI(bool visible)
    {
        hexGrid.ShowUI(visible);
    }

    public void SetTerrainTypeIndex(int index)
    {
        activeTerrainTypeIndex = index;
    }
}