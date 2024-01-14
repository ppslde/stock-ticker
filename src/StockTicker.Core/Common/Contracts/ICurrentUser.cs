namespace StockTicker.Core.Common.Contracts;

public interface ICurrentUser
{
    Guid UserId { get; }
    string UserName { get; }
    bool IsTemporary { get; }
}
