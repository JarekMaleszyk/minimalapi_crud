using Microsoft.EntityFrameworkCore;

namespace minimalapi_crud
{
    class NoteDb : DbContext
    {
        public NoteDb(DbContextOptions<NoteDb> options) : base(options)
        {

        }
        public DbSet<Note> Notes { get; set; }
    }
}
