using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base item scriptable object
/// Contains data that every item should have
/// </summary>
[CreateAssetMenu(menuName = "Items/New Item", fileName = "New Item")]
public class ItemBase : ScriptableObject
{
    [SerializeField]
    public Sprite itemSprite;
    [SerializeField]
    public List<Vector2Int> occupiesSpots;
    public int startAmount = 1;
    [SerializeField]
    public string itemTitle;
    [SerializeField]
    [TextArea]
    public string itemDescription;

}
