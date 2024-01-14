using System.Security.Claims;
using StockTicker.Core.Common.Contracts;

namespace StockTicker.WebApi.Services;

internal class CurrentUserService : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId => TryGetUserId();
    public string UserName => TryGetUserName();

    public bool IsTemporary => IsPasswordTemporaryClaim();

    private Guid TryGetUserId()
    {
        if (!Guid.TryParse(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId))
            userId = Guid.Empty;

        return userId;
    }

    private string TryGetUserName()
    {
        string? userName = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return userName ?? "";
    }

    private bool IsPasswordTemporaryClaim()
    {
        Claim? temporaryClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);

        if (temporaryClaim == null)
            return false;

        if (!bool.TryParse(temporaryClaim.Value, out var isTemporary))
            isTemporary = true;
        return isTemporary;
    }
}

