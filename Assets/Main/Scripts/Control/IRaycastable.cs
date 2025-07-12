using AMAZON.UI;

namespace AMAZON.Control
{
    public interface IRaycastable
    {
        public ECursorType GetCursorType();
        public bool HandleRaycast(PlayerController callingController);
    }
}