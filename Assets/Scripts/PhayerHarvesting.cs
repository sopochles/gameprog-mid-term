using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerHarvesting : MonoBehaviour
{
    public static int collectedCrops;
    public Tilemap tm;
    public Grid grid;

    // Start is called before the first frame update
    void Start()
    {
        collectedCrops = 0; 
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Harvestable") && tm != null && other.gameObject == tm.gameObject)
        {
            Vector3 contactPoint = other.ClosestPoint(transform.position);
            
            HandleHarvesting(contactPoint); 
        }
    }

    void HandleHarvesting(Vector3 contactPoint)
    {
        if (grid == null || tm == null) return;
        
        Vector3Int initialCell = grid.WorldToCell(contactPoint);

        TileBase tile = tm.GetTile(initialCell);

        if (tile != null)
        {
            tm.SetTile(initialCell, null);

            collectedCrops++;
            Debug.Log("Crop harvested: " + collectedCrops);
            
            return;
        }
    }
}