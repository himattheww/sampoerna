using LeaderboardAPI.Entities;
using LeaderboardAPI.Repositories.Interface;

namespace LeaderboardAPI.Repositories.Data
{
    public class WholesellerMappingRepository : IWholesellerMapping
    {
        private readonly LeaderboardContext _context;
        public WholesellerMappingRepository(LeaderboardContext context)
        {
            _context = context;
        }

        public IEnumerable<WholesellerMapping> Get()
        {
            return _context.WholesellerMappings.ToList();
        }

        public int UploadMapping(IEnumerable<WholesellerMapping> wholesellers)
        {
            foreach (var wholesellerMapping in wholesellers)
            {
                _context.WholesellerMappings.Add(wholesellerMapping);
            }
            return _context.SaveChanges();
        }
    }
}
