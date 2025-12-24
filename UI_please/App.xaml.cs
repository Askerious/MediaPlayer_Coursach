using Data.Interfaces;
using Data.SqlServer;
using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
        public IPaymentRepository _paymentRepository = null!;
        public User CurrentUser = null!;
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
            _paymentRepository = new PaymentRepository(db);

            if (!_dbContext.Users.Any(u => u.Username == "askerious"))
                _userRepository.Add(new User("askerious", "123"));

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
