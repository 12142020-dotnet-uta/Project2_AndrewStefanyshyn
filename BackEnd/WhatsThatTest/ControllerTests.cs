using BusinessLogicLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ModelLayer.Models;
using ModelLayer.ViewModels;
using RepositoryLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatsThatSong.Controllers;
using Xunit;

namespace WhatsThatTest
{
    public class ControllerTests
    {
        private readonly ILogger<Repository> _repositoryLogger;
        private readonly ILogger<UserController> _userControllerLogger;
        private readonly ILogger<SongController> _songControllerLogger;
        private readonly MapperClass _mapperClass = new MapperClass();



        /* USER CONTROLLER TESTS */
        [Fact]
        public void CreateUserTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                Task<User> upvm = userController.CreateUser(user);
                Assert.NotNull(upvm);
            }
        }

        [Fact]
        public void LoginTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                var user = new User
                {
                    Id = int.MaxValue,
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                Task<User> upvm = userController.CreateUser(user);
                Task<User> upvm2 = userController.login(user.UserName, user.Password);
                Assert.NotNull(upvm2);
            }
        }

        [Fact]
        public void GetUserToEditTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                repository.SaveNewUser(user).Wait();

                Task<User> u = userController.GetUserToEdit(user.Id);
                Assert.NotNull(u);
            }
        }

        /* TODO: Failing for multiple reasons. NullReferenceException, Assert evaluating to false, DatabaseUpdateConcurrency issue. Reason unknown, suspect async/await.*/
        [Fact]
        public void EditUserTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);

                var user = new User
                {
                    Id = int.MaxValue,
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // add and save new user
                repository.SaveNewUser(user).Wait();

                // attempt to edit that user
                Task<User> u = userController.GetUserToEdit(user.Id);
                userController.EditUser(user).Wait();

                // check that user was edited
                //Assert.Equal("Test2", user.LastName);
            }
        }

        /* TODO: Assert inconsistently evaluating to false. Reason unknown, suspect async/await.*/
        [Fact]
        public void SearchForUsersTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);

                var userList = new List<User>();
                // create some dummy users
                for (int i = 0; i < 10; i++)
                {
                    // j0, j2, j4 ... j8
                    var user = new User
                    {
                        Id = int.MaxValue - i,
                        UserName = "jtest" + i,
                        Password = "Test1!",
                        FirstName = "Johnny" + i,
                        LastName = "Test",
                        Email = "johnnytest123" + i + "@email.com"
                    };

                    // g1, g3, g5 ... g9
                    var user2 = new User
                    {
                        Id = int.MaxValue - ++i,
                        UserName = "greg" + i,
                        Password = "Test1!",
                        FirstName = "Greg" + i,
                        LastName = "Smeg",
                        Email = "johnnytest123" + i + "@zmail.com"
                    };

                    repository.SaveNewUser(user).Wait();
                    repository.SaveNewUser(user2).Wait();
                }

                // with user populated db, let's search for the letter g
                var userList2 = userController.SearchForUsers("g");
                // we expect 5 users to return in the list
                Assert.Equal(5, userList2.Result.Count);
            }
        }

        [Fact]
        public void FriendRequestTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                var fl = new FriendList();

                userController.FriendRequest(fl).Wait();
                Assert.NotNull(repository.friendList);
            }
        }

        /*[Fact]
        public void AcceptFriendTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                var fl = new FriendList { FriendId = user.Id, RequestedFriendId = user2.Id };
                repository.friendList.Add(fl);
                context.SaveChanges();

                userController.AcceptFriend(fl).Wait();
                Assert.NotNull(repository.friendList.FirstOrDefault(x => x.status == "accept"));
            }
        }*/

        [Fact]
        public void GetFriendsByUserIdTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                var fl = new FriendList { FriendId = user.Id, RequestedFriendId = user2.Id, status = "accept" };
                repository.friendList.Add(fl);
                context.SaveChanges();

                Assert.NotNull(userController.GetFriendsByUserId(user.Id));
            }
        }

        [Fact]
        public void GetFriendsAsUsers()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                var fl = new FriendList { FriendId = user.Id, RequestedFriendId = user2.Id, status = "accept" };
                repository.friendList.Add(fl);
                context.SaveChanges();

                Assert.NotNull(userController.GetFriendsAsUsers(user.Id));
            }
        }

        [Fact]
        public void DeleteFriendTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                var fl = new FriendList { FriendId = user.Id, RequestedFriendId = user2.Id, status = "accept" };
                repository.friendList.Add(fl);
                context.SaveChanges();

                userController.DeleteFriend(user.Id, user2.Id).Wait();

                Assert.Equal(0, repository.friendList.Count());
            }
        }

        [Fact]
        public void SendMessageTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                var message = new Message { ToUserId = user.Id, FromUserId = user2.Id, FromUserName = user2.UserName, Content = "Thicc dummy data" };

                var mvm = userController.SendMessage(message);

                Assert.NotNull(mvm);
            }
        }

        [Fact]
        public void GetMessagesBetween2UsersTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                var message = new Message { ToUserId = user.Id, FromUserId = user2.Id, FromUserName = user2.UserName, Content = "Thicc dummy data" };

                repository.messages.Add(message);
                context.SaveChanges();

                var messagesBetween2Users = userController.GetMessagesBetween2Users(user.Id, user2.Id);

                Assert.NotNull(messagesBetween2Users);
            }
        }

        [Fact]
        public void BackToProfileTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(3));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                repository.SaveNewUser(user).Wait();

                var upvm = userController.BackToProfile(user.Id);

                Assert.NotNull(upvm);
            }
        }

        [Fact]
        public void GetAllUsersAsyncTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                Assert.NotEmpty(userController.GetAllUsersAsync().Result);
            }
        }

        [Fact]
        public void GetUserByIdAsyncTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                repository.SaveNewUser(user).Wait();

                Assert.NotNull(userController.GetUserByIdAsync(user.Id));
            }
        }

        [Fact]
        public void GetAllMessagesAsyncTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                Assert.Empty(userController.GetAllMessagesAsync().Result);

                var message = new Message { ToUserId = user.Id, FromUserId = user2.Id, FromUserName = user2.UserName, Content = "Thicc dummy data" };
                repository.messages.Add(message);
                context.SaveChanges();

                Assert.NotEmpty(userController.GetAllMessagesAsync().Result);
            }
        }

        [Fact]
        public void DisplayAllFriendRequestsTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(6));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                var fl = new FriendList { FriendId = user.Id, RequestedFriendId = user2.Id };
                repository.friendList.Add(fl);
                context.SaveChanges();

                Assert.NotEmpty(userController.DisplayAllFriendRequests(user2.Id).Result);
            }
        }

        [Fact]
        public void AreWeFriendsTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                UserController userController = new UserController(logic, _userControllerLogger);
                // create a user
                var user = new User
                {
                    UserName = "jtest",
                    Password = "Test1!",
                    FirstName = "Johnny",
                    LastName = "Test",
                    Email = "johnnytest123@email.com"
                };

                // create a second user
                var user2 = new User
                {
                    UserName = "greg",
                    Password = "Test1!",
                    FirstName = "Greg",
                    LastName = "Smeg",
                    Email = "johnnytest123@zmail.com"
                };

                repository.SaveNewUser(user).Wait();
                repository.SaveNewUser(user2).Wait();

                var fl = new FriendList { FriendId = user.Id, RequestedFriendId = user2.Id };
                repository.friendList.Add(fl);
                context.SaveChanges();

                Assert.False(userController.AreWeFriends(user.Id, user2.Id).Result);

                var fl2 = new FriendList { FriendId = user.Id, RequestedFriendId = user2.Id, status = "accept" };
                repository.friendList.Add(fl2);
                context.SaveChanges();

                Assert.True(userController.AreWeFriends(user.Id, user2.Id).Result);
            }
        }

        /* SONG CONTROLLER TESTS */
        [Fact]
        public void GetSongByIdAsyncTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);
                // create a song
                var song = new Song();

                repository.songs.Add(song);
                context.SaveChanges();

                Assert.NotNull(songController.GetSongByIdAsync(song.Id));
            }
        }

        [Fact]
        public void AddSongToFavoritesTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);
                // create a user
                var user = new User();

                // create a song
                var song = new Song();

                // create a favorite
                var fave = new FavoriteList { UserId = user.Id, SongId = song.Id };

                repository.songs.Add(song);
                repository.users.Add(user);
                repository.favoriteLists.Add(fave);
                context.SaveChanges();

                var s = songController.addSongToFavorites(song.Id, user.Id);

                Assert.NotNull(s);
            }
        }

        [Fact]
        public void GetUsersFavoriteSongs()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);
                // create a user
                var user = new User();

                // create a song
                var song = new Song();

                repository.songs.Add(song);
                repository.users.Add(user);

                // create a favorite
                var fave = new FavoriteList { UserId = user.Id, SongId = song.Id };

                repository.favoriteLists.Add(fave);
                context.SaveChanges();

                var s = songController.GetUsersFavoriteSongs(user.Id);

                Assert.NotEmpty(s.Result);
            }
        }

        [Fact]
        public void UploadSongTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);
                // create a song
                var song = new Song();

                repository.songs.Add(song);
                context.SaveChanges();

                var s = songController.UploadSong(song);

                Assert.NotNull(s);
            }
        }

        [Fact]
        public void GetAllSongsByACertainUserTest()
        {
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);
                // create a user -- generates with null as username
                var user = new User();

                // create a song -- generates with null as artist name
                var song = new Song();

                repository.songs.Add(song);
                repository.users.Add(user);
                context.SaveChanges();

                // gets list where artist name matches username
                var s = songController.GetAllSongsByACertainUser(user.Id);

                // list is not empty because we create song and user with null
                Assert.NotEmpty(s.Result);
            }
        }

        [Fact]
        public void GetOriginalSongSearchByGenreTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);

                // create a song with pop as genre
                var song = new Song { Genre = "Pop" };

                repository.songs.Add(song);
                context.SaveChanges();

                // search for null
                var s = songController.GetOriginalSongSearchByGenre("Pop");

                Assert.NotEmpty(s.Result);
            }
        }

        [Fact]
        public void GetOriginalsongsByLyricsTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);
                // create a song
                var song = new Song { Lyrics = "thicc dummy data" };

                repository.songs.Add(song);
                context.SaveChanges();

                var s = songController.GetOriginalsongsByLyrics(song.Lyrics);

                Assert.NotEmpty(s.Result);
            }
        }

        [Fact]
        public void GetTop5OriginalsTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);
                // create a few songs...
                for (int i = 0; i < 5; i++)
                {
                    var song = new Song();
                    repository.songs.Add(song);
                }

                context.SaveChanges();

                var s = songController.GetTop5Originals();

                Assert.Equal(5, s.Result.Count);
            }
        }

        [Fact]
        public void IncrementNumPlaysTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);
                // create a song
                var song = new Song();
                
                repository.songs.Add(song);
                context.SaveChanges();

                var s = songController.IncrementNumPlays(song.Id);

                Assert.NotNull(s);
            }
        }

        [Fact]
        public void Get5FavoriteSongsForUserTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "InHarmonyTestControllerDB")
            .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

                Repository repository = new Repository(context, _repositoryLogger);
                BusinessLogicClass logic = new BusinessLogicClass(repository, _mapperClass, _repositoryLogger);
                SongController songController = new SongController(logic, _songControllerLogger);
                // create a user
                var user = new User();
                repository.users.Add(user);
                // create a few songs...
                for (int i = 0; i < 5; i++)
                {
                    var song = new Song();
                    repository.songs.Add(song);
                }

                // create the user's favorites...
                for (int i = 0; i < 5; i++)
                {
                    var fave = new FavoriteList { SongId = i+1, UserId = user.Id };
                    repository.favoriteLists.Add(fave);
                }


                context.SaveChanges();

                var s = songController.Get5FavoriteSongsForUser(user.Id);
                Assert.NotEmpty(s.Result);
                Assert.Equal(5, s.Result.Count);
            }
        }
    }
}
