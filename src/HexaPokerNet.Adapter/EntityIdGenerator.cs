using HexaPokerNet.Domain;

namespace HexaPokerNet.Adapter;

public class EntityIdGenerator: IEntityIdGenerator
{
    public string NewId()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}