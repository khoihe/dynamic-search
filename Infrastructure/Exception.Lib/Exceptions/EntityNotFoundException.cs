namespace Exception.Lib;

public class EntityNotFoundException : BaseException
{
    public EntityNotFoundException(string message = null)
        : base(ErrorCodes.ENTITY_NOT_FOUND, message)
    {
    }
}