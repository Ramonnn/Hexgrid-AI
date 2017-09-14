using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class HexUnit : MonoBehaviour {

    public static HexUnit unitPrefab;
    public HexCell Location
    {
        get
        {
            return location;
        }
        set
        {
            location = value;
            value.Unit = this;
            transform.localPosition = value.Position;
        }
    }
    
    HexCell location;

    public void Die()
    {
        location.Unit = null;
        Destroy(gameObject);
    }

    public void Save(BinaryWriter writer)
    {
        location.coordinates.Save(writer);
    }

    public static void Load(BinaryReader reader, HexGrid grid)
    {
        HexCoordinates coordinates = HexCoordinates.Load(reader);
        float orientation = reader.ReadSingle();
        grid.AddUnit(Instantiate(unitPrefab), grid.GetCell(coordinates));
    }
}
