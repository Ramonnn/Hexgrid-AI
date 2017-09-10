using UnityEngine;
using UnityEngine.EventSystems;

public class HexMapEditor : MonoBehaviour {

	public Color[] colors;

    public HexGrid hexGrid;
    HexCell searchFromCell;


	private Color activeColor;
    bool applyColor;

    void Awake () {
		SelectColor(0);
	}

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
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (searchFromCell)
                {
                    searchFromCell.DisableHighlight();
                }
                searchFromCell = currentCell;
                searchFromCell.EnableHighlight(Color.blue);
            }
            if (searchFromCell && searchFromCell != currentCell)
            {
                hexGrid.FindPath(searchFromCell, currentCell);
            }
        }
    }

    void EditCell(HexCell cell)
    {
        if (applyColor)
        {
            cell.Color = activeColor;
        }
    }

    public void SelectColor (int index) {
        applyColor = index >= 0;
        if (applyColor)
        {
            activeColor = colors[index];
        }
	}

    public void ShowUI(bool visible)
    {
        hexGrid.ShowUI(visible);
    }
}