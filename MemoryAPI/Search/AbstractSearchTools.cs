namespace MemoryAPI
{
    public interface ISearchTools
    {
        int PageCount { get; }
        int TotalCount { get; }

        Job MainJob(short index);
        byte MainLvl(short index);
        string Name(short index);
        Job SubJob(short index);
        byte SubLvl(short index);
        Zone Zone(short index);
    }
}