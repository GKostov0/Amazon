using AMAZON.UI;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCursors", menuName = "Cursors")]
public class CursorsSO : ScriptableObject
{
    [field: SerializeField] public CursorMapping[] CursorMappings { get; private set; } = null;

    private CursorMapping GetCursorMapping(ECursorType type)
    {
        return CursorMappings.FirstOrDefault(x => x.type.Equals(type));
    }

    public void SetCursor(ECursorType type)
    {
        CursorMapping mappiong = GetCursorMapping(type);
        Cursor.SetCursor(mappiong.texture, mappiong.hotspot, CursorMode.Auto);
    }
}