
using UnityEngine;
using UnityEngine.InputSystem;

public class Placement : MonoBehaviour
{
    public GameObject Tower1;
    public LayerMask Groundlayer;

    private GameObject towerPreview;

    void Update()
    {
        // Als je een tower aan het plaatsen bent
        if (towerPreview != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(
                Mouse.current.position.ReadValue()
            );

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, Groundlayer))
            {
                // Tower volgt de muis
                towerPreview.transform.position = hit.point;

                // Plaats tower met linkermuisknop
                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    towerPreview = null;
                }
            }
        }
    }

    // Wordt aangeroepen door de button
    public void StartPlacement()
    {
        towerPreview = Instantiate(Tower1);
    }
}
