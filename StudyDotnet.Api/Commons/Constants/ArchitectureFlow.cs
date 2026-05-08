namespace StudyDotnet.Commons.Constants;

public static class ArchitectureFlow
{
    public static readonly string[] RequestFlow =
    {
        "Request",
        "Controller",
        "Service",
        "UnitOfWork",
        "Repository",
        "DbContext",
        "PostgreSQL"
    };

    public static readonly string[] EntityRelationshipFlow =
    {
        "User",
        "UserAccessLevel",
        "AccessLevel",
        "DeviceAccessLevel",
        "Device"
    };
}
