namespace MemoryAPI
{
    public abstract class AbstractSearchTools : ISearchTools
    {
        public virtual int PageCount { get; }
        public virtual int TotalCount { get; }
        
        public abstract Job MainJob(short index);
        public abstract byte MainLvl(short index);
        public abstract string Name(short index);
        public abstract Job SubJob(short index);
        public abstract byte SubLvl(short index);
        public abstract Zone Zone(short index);
    }
}