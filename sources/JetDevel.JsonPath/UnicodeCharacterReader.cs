namespace JetDevel.JsonPath;
public abstract class UnicodeCharacterReader
{
    protected private UnicodeCharacterReader() { }
    public abstract bool TryReadNext(out UnicodeCharacter character);
}