
namespace RuneSlinger.Base.ValueObjects
{
	public class RuneSlot
	{
        public uint Id { get; private set; }
        public RuneType Type { get; private set; }
        public uint RuneTypeIndex { get; private set; }

        public RuneSlot(uint id, RuneType type, uint runeTypeIndex)
        {
            Id = id;
            Type = type;
            RuneTypeIndex = runeTypeIndex;
        }


        
    }
}
