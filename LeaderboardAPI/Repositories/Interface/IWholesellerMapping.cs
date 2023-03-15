using LeaderboardAPI.Entities;

namespace LeaderboardAPI.Repositories.Interface
{
    public interface IWholesellerMapping
    {
        IEnumerable<WholesellerMapping> Get();
        int UploadMapping(IEnumerable<WholesellerMapping> wholesellers);
    }
}
