using Data.Interfaces;
using Data.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TagLib;
using UI_please;

namespace UI
{
    public partial class App : Application
    {
        public IAudioTrackRepository _trackRepository = null!;
        public IPlaylistRepository _playlistRepository = null!;
        public IUserRepository _userRepository = null!;
        public MediaDbContext _dbContext = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.database.json")
                .Build();

            var factory = new DbContextFactory();
            var db = factory.CreateDbContext(configuration);
            _dbContext = db;
            db.Database.Migrate();

            _trackRepository = new AudioTrackRepository(db);
            _playlistRepository = new PlaylistRepository(db);
            _userRepository = new UserRepository(db);

            var login = new LoginWindow();
            login.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _dbContext?.Dispose();
            base.OnExit(e);
        }
    }
}
