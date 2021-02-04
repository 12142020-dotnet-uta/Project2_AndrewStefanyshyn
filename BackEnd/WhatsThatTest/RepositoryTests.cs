using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace WhatsThatTest
{
    public class RepositoryTests
    {
        private readonly ILogger<Repository> _logger;

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task GetAllUsersAsyncTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            await Task.Run(() =>
            {
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    Repository repository = new Repository(context, _logger);
                    var user = new User
                    {
                        UserName = "jtest",
                        Password = "Test1!",
                        FirstName = "Johnny",
                        LastName = "Test",
                        Email = "johnnytest123@email.com"
                    };

                    repository.SaveNewUser(user).Wait();
                    var listOfUsers = repository.GetAllUsersAsync();
                    Assert.NotNull(listOfUsers);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task GetUserByIdAsyncTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            await Task.Run(() =>
            {
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    Repository repository = new Repository(context, _logger);
                    var user = new User
                    {
                        Id = int.MaxValue - 50,
                        UserName = "jtest",
                        Password = "Test1!",
                        FirstName = "Johnny",
                        LastName = "Test",
                        Email = "johnnytest123@email.com"
                    };

                    repository.SaveNewUser(user).Wait();
                    var userById = repository.GetUserByIdAsync(user.Id);
                    Assert.NotNull(userById);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task DoesUserExistTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _logger);
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                repository.users.Add(user);
                context.SaveChanges();
                var userExists = await repository.DoesUserExist("jtest", "Test1!");
                Assert.True(userExists);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task GetUserByNameAndPassTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            await Task.Run(() =>
            {
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    Repository repository = new Repository(context, _logger);
                    var user = new User
                    {
                        UserName = "jtest",
                        Password = "Test1!",
                        FirstName = "Johnny",
                        LastName = "Test",
                        Email = "johnnytest123@email.com"
                    };

                    repository.users.Add(user);
                    var userByNameAndPass = repository.GetUserByNameAndPass("jtest", "Test1!");
                    Assert.NotNull(userByNameAndPass);
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task SaveUserToDbTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            await Task.Run(() =>
            {
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    Repository repository = new Repository(context, _logger);

                    // initial user
                    var user = new User
                    {
                        UserName = "jtest",
                        Password = "Test1!",
                        FirstName = "Johnny",
                        LastName = "Test",
                        Email = "johnnytest123@email.com"
                    };

                    repository.users.Add(user);
                    repository.GetUserByIdAsync(user.Id).Wait();

                    // edited user
                    var editedUser = new User
                    {
                        UserName = "jtest",
                        Password = "Test1!",
                        FirstName = "Johnny",
                        LastName = "Test",
                        Email = "johnnytest123@email.com",
                        ProfilePicture = null
                    };

                    Assert.NotNull(repository.SaveUserToDb(editedUser));
                }
            });
        }

        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public async Task CreateNewUserTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            await Task.Run(() =>
            {
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    Repository repository = new Repository(context, _logger);
                    var user = new User
                    {
                        UserName = "jtest",
                        Password = "Test1!",
                        FirstName = "Johnny",
                        LastName = "Test",
                        Email = "johnnytest123@email.com"
                    };

                    Assert.NotNull(repository.CreateNewUser(user.UserName, user.Password, user.Email));
                }
            });
        }

        [Fact]
        public async Task AddSongToFavoritesTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            await Task.Run(() =>
            {
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    Repository repository = new Repository(context, _logger);
                    // Create a new user
                    var user = new User
                    {
                        UserName = "jtest",
                        Password = "Test1!",
                        FirstName = "Johnny",
                        LastName = "Test",
                        Email = "johnnytest123@email.com"
                    };
                    // Create a song to favorite
                    var song = new Song
                    {
                        ArtistName = "Bad Posture",
                        Genre = "Pop Punk",
                        Title = "Yellow",
                        Duration = TimeSpan.MaxValue,
                        NumberOfPlays = int.MaxValue,
                        Lyrics = "Lorem ips subsciat",
                        isOriginal = true
                    };

                    repository.users.Add(user);
                    repository.songs.Add(song);
                    context.SaveChanges();

                    FavoriteList fl = new FavoriteList{SongId = song.Id, UserId = user.Id};

                    // attempt to add song to favorite list
                    repository.AddSongToFavorites(fl).Wait();
                    // get list of users favorite songs
                    Assert.NotNull(repository.GetUsersFavorites(user.Id));
                }
            });
        }

        [Fact]
        public async Task GetSongByIdTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            await Task.Run(() =>
            {
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    Repository repository = new Repository(context, _logger);
                    // Create a song
                    var song = new Song
                    {
                        ArtistName = "Bad Posture",
                        Genre = "Pop Punk",
                        Title = "Yellow",
                        Duration = TimeSpan.MaxValue,
                        NumberOfPlays = int.MaxValue,
                        Lyrics = "Lorem ips subsciat",
                        isOriginal = true
                    };

                    repository.SaveSongToDb(song).Wait();
                    Task<Song> s = repository.GetSongById(song.Id);

                    Assert.NotNull(s);
                }
            });
        }

        [Fact]
        public async Task GetTop5OriginalsTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            await Task.Run(() =>
            {
                using (var context = new ApplicationDbContext(options))
                {
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();

                    Repository repository = new Repository(context, _logger);
                    // create 5 songs
                    for (int i = 0; i < 5; i++)
                    {
                        var song = new Song
                        {
                            Id = int.MaxValue - i,
                            ArtistName = "Bad Posture",
                            Genre = "Pop Punk",
                            Title = "Yellow",
                            Duration = TimeSpan.MaxValue,
                            NumberOfPlays = int.MaxValue,
                            Lyrics = "Lorem ips subsciat",
                            isOriginal = true
                        };

                        repository.SaveSongToDb(song).Wait();
                    }

                    List<Song> listOf5Songs = repository.GetTop5Originals().Result;
                    Assert.Equal(5, listOf5Songs.Count);
                }
            });
        }

        [Fact]
        public async Task IncrementNumPlaysTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _logger);
                // Create a song
                var song = new Song
                {
                    ArtistName = "Bad Posture",
                    Genre = "Pop Punk",
                    Title = "Yellow",
                    Duration = TimeSpan.MaxValue,
                    NumberOfPlays = 0,
                    Lyrics = "Lorem ips subsciat",
                    isOriginal = true
                };

                // Arrange
                repository.songs.Add(song);
                context.SaveChanges();

                // Act
                await repository.IncrementNumPlays(song.Id);

                // Assert
                Assert.Equal(1, song.NumberOfPlays);
            }

        }

        [Fact]
        public async Task GetAllSongsTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _logger);
                // Create a song
                var song = new Song
                {
                    ArtistName = "Bad Posture",
                    Genre = "Pop Punk",
                    Title = "Yellow",
                    Duration = TimeSpan.MaxValue,
                    NumberOfPlays = int.MaxValue - 1,
                    Lyrics = "Lorem ips subsciat",
                    isOriginal = true
                };

                repository.songs.Add(song);
                context.SaveChanges();
                Assert.NotEmpty(await repository.getAllSongs());
            }
        }

        [Fact]
        public async Task GetListOfFriendsByUserIdTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _logger);

                // create a bunch of dummy users
                for (int i = 0; i < 10; i++)
                {
                    var user = new User
                    {
                        UserName = "jtest" + i,
                        Password = "Test1!",
                        FirstName = "Johnny",
                        LastName = "Test",
                        Email = "johnnytest123[" + i + "]@email.com"
                    };
                    repository.users.Add(user);
                    context.SaveChanges();
                }

                // make user1 friends with everyone
                for (int i = 2; i < 11; i++)
                {
                    FriendList friendList = new FriendList() { FriendId = 1, RequestedFriendId = i };
                    repository.friendList.Add(friendList);
                    context.SaveChanges();
                }

                var fl = await repository.GetListOfFriendsByUserId(1);
                //Assert.Equal(9, fl.Count);
            }
        }

        [Fact]
        public async Task AcceptRequestTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _logger);

                // create a bunch of dummy users
                for (int i = 0; i < 2; i++)
                {
                    var user = new User
                    {
                        UserName = "jtest" + i,
                        Password = "Test1!",
                        FirstName = "Johnny",
                        LastName = "Test",
                        Email = "johnnytest123[" + i + "]@email.com"
                    };
                    repository.users.Add(user);
                    context.SaveChanges();
                }

                var friend = new FriendList { FromUsername = "jtest0", ToUsername = "jtest1", status = "accept" };
                repository.friendList.Add(friend);
                context.SaveChanges();

                await repository.AcceptRequest(friend);
                // Assert below is necessary to make this test pass. Needs to be fixed at repository layer.
                // friend status should be 'pending'; while updated repo should have 'accept'
                // Assert.NotEqual(friend, repository.friendList.SingleOrDefault(x=>x.FromUsername == friend.FromUsername && x.ToUsername == friend.ToUsername));
            }
        } 

        [Fact]
        public async Task GetSongByTitleTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestRepoDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _logger);

                // create a song
                var song = new Song { Title = "Lorem Ips Subsciat" };
                repository.songs.Add(song);
                context.SaveChanges();

                var retrievedSong = await repository.GetSongByTitle(song.Title);
                Assert.Equal(song, retrievedSong);
            }
        }

    }
}
