using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class HexMapEditor : MonoBehaviour {

    public HexGrid hexGrid;
    public HexUnit unitPrefab;
    HexCell searchFromCell, searchToCell;
    bool editMode;

    int activeTerrainTypeIndex;

    void Update () {
		//if (
		//	Input.GetMouseButton(0) &&
		//	!EventSystem.current.IsPointerOverGameObject()
		//) {
		//	HandleInput();
		//}
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0))
            {
                HandleInput();
                return;
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    DestroyUnit();
                }
                else
                {
                    CreateUnit();
                }
                return;
            }
        }
    }

	void HandleInput () {
        //Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;
        //if (Physics.Raycast(inputRay, out hit)) {
        HexCell currentCell = GetCellUnderCursor();
        if (currentCell)
        {
            if (editMode)
            {
                //EditCell(hexGrid.GetCell(hit.point));
                EditCell(currentCell);
            }
        //    HexCell currentCell = hexGrid.GetCell(hit.point);
            if (Input.GetKey(KeyCode.LeftShift) && searchToCell != currentCell)
            {
                if (searchFromCell != currentCell)
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
            }
            if (searchFromCell && searchFromCell != currentCell)
            {
                if (searchToCell != currentCell)
                {
                    searchToCell = currentCell;
                    hexGrid.FindPath(searchFromCell, searchToCell, 24);
                }
            }
        }
    }

    HexCell GetCellUnderCursor()
    {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit))
        {
            return hexGrid.GetCell(hit.point);
        }
        return null;
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

    void CreateUnit()
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && !cell.Unit)
        {
            HexUnit unit = Instantiate(unitPrefab);
            unit.transform.SetParent(hexGrid.transform, false);
            unit.Location = cell;
        }
    }

    void DestroyUnit()
    {
        HexCell cell = GetCellUnderCursor();
        if (cell && cell.Unit)
        {
            cell.Unit.Die();
        }
    }

    public void SetEditMode(bool toggle)
    {
        editMode = toggle;
        hexGrid.ShowUI(!toggle);
    }

    public void ShowUI(bool visible)
    {
        hexGrid.ShowUI(visible);
    }

    public void SetTerrainTypeIndex(int index)
    {
        activeTerrainTypeIndex = index;
    }

    public void Save()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(0);
            hexGrid.Save(writer);
        }     
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int header = reader.ReadInt32();
            if (header == 0)
            {
                hexGrid.Load(reader);
            }
            else
            {
                Debug.LogWarning("Unknown map format " + header);
            }
        }
    }
}