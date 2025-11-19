namespace Exception.Lib;

public class EntityDuplicationException : BaseException
{
    public EntityDuplicationException(string message = null)
        : base(ErrorCodes.ERROR_ENTITY_DUPLICATION, message)
    {
    }
}