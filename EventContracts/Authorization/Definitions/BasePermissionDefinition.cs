namespace EventContracts.Authorization.Definitions
{
    public class BasePermissionDefinition
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public int SortOrder { get; set; }
    }
}
