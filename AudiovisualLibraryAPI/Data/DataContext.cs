using AudiovisualLibraryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AudiovisualLibraryAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<AudioFile> AudioFiles{ get; set; }
        public DbSet<VideoFile> VideoFiles { get; set; }
        public DbSet<MusicAlbum> MusicAlbums { get; set;}
    }
}
