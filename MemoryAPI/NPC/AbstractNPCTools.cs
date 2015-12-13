namespace MemoryAPI
{
    public abstract class AbstractNPCTools : INPCTools
    {
        public abstract int ClaimedID(int id);
        public abstract double Distance(int id);
        public abstract IPosition GetPosition(int id);
        public abstract short HPPCurrent(int id);
        public abstract bool IsActive(int id);
        public abstract bool IsClaimed(int id);
        public abstract bool IsRendered(int id);
        public abstract string Name(int id);
        public abstract NPCType NPCType(int id);
        public abstract float PosX(int id);
        public abstract float PosY(int id);
        public abstract float PosZ(int id);
        public abstract Status Status(int id);
    }
}