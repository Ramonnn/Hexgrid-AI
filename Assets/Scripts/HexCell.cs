using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour {

	public HexCoordinates coordinates;
    public RectTransform uiRect;
    public int Distance
    {
        get
        {
            return distance;
        }
        set
        {
            distance = value;
            UpdateDistanceLabel();
        }
    }
    int distance;

    public Color Color
    {
        get
        {
            return color;
        }
        set
        {
            if (color == value)
            {
                return;
            }
            color = value;
            Refresh();
        }
    }

    public HexGridChunk chunk;

    Color color;

    [SerializeField]
	HexCell[] neighbors;

	public HexCell GetNeighbor (HexDirection direction) {
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}

    void Refresh()
    {
        if (chunk)
        {
            chunk.Refresh();
        }
    }
    void UpdateDistanceLabel()
    {
        Text label = uiRect.GetComponent<Text>();
        label.text = distance.ToString();
    }
}