using UnityEngine;

public class TowerButton : MonoBehaviour
{
    public Placement placement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SelectTower()
    {
        placement.enabled = true;
    }
}
