namespace StockTicker.Core.Common.Security;

// <summary>
/// Specifies the class this attribute is applied to requires authorization.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
internal class AuthorizeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthorizeAttribute"/> class. 
    /// </summary>
    public AuthorizeAttribute() { }

    /// <summary>
    /// Gets or sets a comma delimited list of roles that are allowed to access the resource.
    /// </summary>
    //public UserRole Roles { get; set; } = UserRole.None;

    ///// <summary>
    ///// Gets or sets the policy name that determines access to the resource.
    ///// </summary>
    //public UserPermission Permission { get; set; } = UserPermission.None;

    /// <summary>
    /// Gets or sets a value indicating whether temporary users are allowed to access the resource.
    /// </summary>
    public bool AllowTemporaryAccess { get; set; } = false;
}