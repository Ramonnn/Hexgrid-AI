using UnityEngine;
using UnityEngine.UI;

public class HexCell : MonoBehaviour {

    public HexCell PathFrom { get; set; }
    public int SearchHeuristic { get; set; }
    public HexCell NextWithSamePriority { get; set; }

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
            //UpdateDistanceLabel();
        }
    }
    int distance;

    public Color Color
    {
        get
        {
            return HexMetrics.colors[terrainTypeIndex];
        }
    }

    public HexGridChunk chunk;

    int terrainTypeIndex;

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
    //void UpdateDistanceLabel()
    //{
    //  Text label = uiRect.GetComponent<Text>();
    //label.text = distance == int.MaxValue ? "" : distance.ToString();
    //}
    public void SetLabel(string text)
    {
        UnityEngine.UI.Text label = uiRect.GetComponent<Text>();
        label.text = text;
    }

    public void DisableHighlight()
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.enabled = false;
    }

    public void EnableHighlight(Color color)
    {
        Image highlight = uiRect.GetChild(0).GetComponent<Image>();
        highlight.color = color;
        highlight.enabled = true;
    }

    public int TerrainTypeIndex
    {
        get
        {
            return terrainTypeIndex;
        }
        set
        {
            if (terrainTypeIndex != value)
            {
                terrainTypeIndex = value;
                Refresh();
            }
        }
    }

    public int SearchPriority
    {
        get
        {
            return distance + SearchHeuristic;
        }
    }
}