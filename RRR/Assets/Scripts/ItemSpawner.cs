using TMPro;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnLocation;
    public int quantity;
    public TMP_Text label;
    public GameObject spawnedItem;

    private void Start()
    {
        spawnedItem = Instantiate(prefab);
        spawnedItem.transform.position = spawnLocation.position;
        label.text = $"{prefab.name} x{quantity}";
    }

    private void Update()
    {
        if (quantity <= 0)
        {
            return;
        }
        if (Vector2.Distance(spawnedItem.transform.position, spawnLocation.position) > 0.1f)
        {
            Taken();
        }
    }

    public void Taken()
    {
        quantity--;

        if (quantity > 0)
        {
            spawnedItem = Instantiate(prefab);
            spawnedItem.transform.position = spawnLocation.position;
        }


        label.text = $"{prefab.name} x{quantity}";
    }
}
