using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseHover : MonoBehaviour
{
    // Reference to the Text to display tile information
    public TextMeshProUGUI positionText;

    void Update()
    {
        // Create a ray from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Perform the raycast and check if it hits a gameobject
        if (Physics.Raycast(ray, out hit))
        {
            // Get the TileInfo component of the hit gameobject
            TileInfo tileInfo = hit.transform.GetComponent<TileInfo>();

            // If the gameobject has a TileInfo component, update the Text
            if (tileInfo != null)
            {
                positionText.text = $"Tile Position: ({tileInfo.gridX}, {tileInfo.gridY})";
            }
        }
    }
}
