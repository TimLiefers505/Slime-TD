using UnityEngine;

public class Placement : MonoBehaviour
{
    public GameObject Tower1;
    public LayerMask Groundlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 1000f, Groundlayer))
            {
                Instantiate(Tower1, hit.point, Quaternion.identity);
            }
        }
    }
}
