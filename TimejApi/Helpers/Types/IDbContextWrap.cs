using Microsoft.EntityFrameworkCore;

namespace TimejApi.Helpers.Types
{
    public interface IDbContextWrap
    {
        public DbContext DbContext { get; }
    }

    public interface IDbContextWrap<TContext> : IDbContextWrap
    {
        public new TContext DbContext { get; }
    }
}
