namespace Assets.Code.ValueObjects
{
	public class Rune
	{
        public uint Id { get; private set; }
        public RuneType Type { get; private set; }
        public uint RuneTypeIndex { get; private set; }

        public Rune(uint id, RuneType type, uint runeTypeIndex)
        {
            Id = id;
            Type = type;
            RuneTypeIndex = runeTypeIndex;
        }

        
    }
}
