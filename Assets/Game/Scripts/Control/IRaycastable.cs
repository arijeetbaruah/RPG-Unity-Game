namespace RPG.Control
{
    public enum CursorType
    {
        None,
        Movement,
        Combat,
        UI,
        PickUp
    }

    interface IRaycastable
    {
        bool HandleRaycast(PlayerController controller);
        CursorType GetCursorType();
    }
}
