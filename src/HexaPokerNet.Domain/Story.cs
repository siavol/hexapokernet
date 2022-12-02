namespace HexaPokerNet.Domain;

public class Story: Entity
{
    public string Title { get; }

    public Story(string title) =>
        Title = title ?? throw new ArgumentNullException(nameof(title));
    
    public Story(string id, string title): base(id) =>
        Title = title ?? throw new ArgumentNullException(nameof(title));
}