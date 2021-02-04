using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RepositoryLayer
{
    public class Repository
    {
        private ApplicationDbContext _applicationDbContext;
        readonly ILogger _logger;

        public DbSet<User> users;
        public DbSet<Song> songs;
        public DbSet<Message> messages;
        public DbSet<Artist> artists;
        public DbSet<Genre> genres;
        public DbSet<FriendList> friendList;
        public DbSet<FavoriteList> favoriteLists;
       

        public Repository(ApplicationDbContext applicationDbContext, ILogger<Repository> logger)
        {
            _applicationDbContext = applicationDbContext;
            _logger = logger;
            this.users = _applicationDbContext.Users;
            this.songs = _applicationDbContext.Songs;
            this.messages = _applicationDbContext.Messages;
            this.artists = _applicationDbContext.Artists;
            this.genres = _applicationDbContext.Genres;
            this.friendList = _applicationDbContext.FriendList;
            this.favoriteLists = _applicationDbContext.FavoriteLists;
        }


        /// <summary>
        /// adds a song to the favorites of a user if it isnt already a favorite of that user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task AddSongToFavorites(FavoriteList favoriteSong)
        {
                FavoriteList SongToAdd = favoriteSong;
                // Add to database -- this will auto assign the SongToAdd.Id
                await favoriteLists.AddAsync(SongToAdd);
                // Finally, save the changes to our database
                await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<List<FavoriteList>> GetUsersFavorites(int userId)
        {
            return await favoriteLists.Where(item => item.UserId == userId).ToListAsync();
        }

        public async Task<Song> GetSongById(int id)
        {
            return await songs.FirstOrDefaultAsync(item => item.Id == id);
        }

        public async Task SaveSongToDb(Song song)
        {
            Song s = song;
            await songs.AddAsync(s);
            await _applicationDbContext.SaveChangesAsync();
        }

        

        public async Task<FriendList> HasPendingFrinedRequest(int id)
        {
            return await friendList.FirstOrDefaultAsync(x => x.status == "pending" && x.RequestedFriendId == id);
            
        }

        public async Task<List<FriendList>> GetFriendListFriendByUserId(int UserId)
        {
            return await friendList.Where(x => x.Id == UserId || x.FriendId == UserId).ToListAsync();
        }

        public async Task<FriendList> GetFriendListFriend(int UserId, int pendingFriendId)
        {
            FriendList fl = friendList.FirstOrDefault(x => x.RequestedFriendId == pendingFriendId && x.FriendId == UserId);// && x.status == "pending");
            if(fl != null)  
                return fl;
            else
                return friendList.FirstOrDefault(x => x.RequestedFriendId == UserId && x.FriendId == pendingFriendId);
        }
        public async Task AcceptRequest(FriendList pendingFriendId)
        {
            FriendList fl = pendingFriendId;
            _applicationDbContext.Update(fl);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<FriendList> RequestFriend(FriendList fl)
        {
            FriendList request = fl;
            await friendList.AddAsync(request);
            await _applicationDbContext.SaveChangesAsync();
            return request;
        }

        public async Task DeleteFriendByFreindListId(int friendListId)
        {
                FriendList f1 = friendList.FirstOrDefault(x => x.Id == friendListId);
                friendList.Remove(f1);
                await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<List<Song>> GetOriginalSongsByGenre(string genre)
        {
            List<Song> OriginalSongs = await songs.Where(x => x.Genre == genre).ToListAsync();
            return OriginalSongs;
        }

        public async Task<Song> GetSongByTitle(string title)
        {
            Song s = await songs.FirstOrDefaultAsync(x => x.Title == title);
            return s;
        }

        public async Task<int> GetNumOfFriendsByUserId(int id)
        {
            int numOfFriends = 0;
            await foreach(var item in friendList)
            {
                if((item.FriendId == id || item.RequestedFriendId == id)&& (item.status == "accept"))
                {
                    numOfFriends += 1;
                }
            }
            return numOfFriends;
        }

        public async Task<List<Song>> GetAllSongsByUserName(string userName)
        {
            return await songs.Where(x => x.ArtistName == userName).ToListAsync();
        }



        /// <summary>
        /// returns the top 5 songs based on the number of plays
        /// </summary>
        /// <returns></returns>
        public async Task<List<Song>> GetTop5Originals()
        {
			IOrderedQueryable<Song> topSongs = songs.OrderByDescending(x => x.NumberOfPlays);
            List<Song> songtoSend = new List<Song>();

            int max = 5, count = 0;
            foreach(Song x in topSongs)
            {
                if(count >= max)    break;
           
                songtoSend.Add(x);
                count++;
            }
            
            return songtoSend; 
           
        }

        public async Task IncrementNumPlays(int songId)
        {
            Song songToIncrement = await GetSongById(songId);
            songToIncrement.NumberOfPlays += 1;
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<List<Song>> getAllSongs()
        {
            List<Song> x = new List<Song>();
            await foreach(var item in songs)
            {
                x.Add(item);
            }
            return x;
        }
        public async Task<bool> DoesUserExist(string username, string passw)
        {
            User user = await users.FirstOrDefaultAsync(x => x.UserName == username);// && x.Password == passw);
            if (user==null){
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<List<Song>> GetOriginalSongByLyrics(string phrase)
        {
            string lowerCasePhrase = phrase.ToLower();
            List<Song> songlist = new List<Song>();
            await foreach (var item in songs)
            {
                if(item.Title != null && item.Title.ToLower().Contains(lowerCasePhrase))
                {
                    songlist.Add(item);
                }
                else if(item.Lyrics != null && item.Lyrics.ToLower().Contains(lowerCasePhrase))
                {
                    songlist.Add(item);
                }
            }
            return songlist;
        }


        /// <summary>
        /// gets a favoriteslist item buy the userid and songId
        /// </summary>
        /// <param name="songId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<FavoriteList> GetFavoriteBySongIdUserId(int songId, int userId)
        {
            return await favoriteLists.FirstOrDefaultAsync(x => x.SongId == songId && x.UserId == userId);
        }

        public async Task DeleteFavoriteListitem(int userId, int songId)
        {
            FavoriteList fToDelete = await favoriteLists.FirstOrDefaultAsync(x => x.UserId == userId && x.SongId == songId);
            favoriteLists.Remove(fToDelete);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserByNameAndPass(string userName)
        {
            return await users.FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task DeleteUploadedSong(Song song)
        {
            songs.Remove(song);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<User> GetUserByNameAndPass(string username, string passw)
        {
            return await users.FirstOrDefaultAsync(x => x.UserName == username && x.Password == passw);
        }

        public async Task<List<FriendList>> GetAllFRiendRequestsByUserId(int userId)
        {
            //return await friendList.Where(x => x.RequestedFriendId == userId && x.status == "pending").ToListAsync();
            return await friendList.Where(x => x.RequestedFriendId == userId).ToListAsync();
        }

        public async Task<FriendList> GetFriendByBothIds(int id1, int id2)
        {
            return await friendList.FirstOrDefaultAsync(x => x.FriendId == id1 && x.RequestedFriendId == id2 && x.status == "accept");
        }

        public async Task<User> SaveUserToDb(User userToEdit)
        {
            User UserInDb = userToEdit;
            await _applicationDbContext.SaveChangesAsync();
            return UserInDb;
        }

        public async Task<Song> GetSongByArtistNameAndTitle(string artistName, string title)
        {
            return await songs.FirstOrDefaultAsync(x => x.ArtistName == artistName && x.Title == title);
        }



        /// <summary>
        /// creates a new user
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<User> CreateNewUser(string userName, string password, string email)
        {
            User newUser = new User(userName, password, email);
            await users.AddAsync(newUser);
            await _applicationDbContext.SaveChangesAsync();
            //return await users.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
            return newUser;
        }

        public async Task<List<FriendList>> GetListOfFriendsByUserId(int id)
        {
            return await friendList.Where(item => (item.FriendId == id || item.RequestedFriendId == id) && item.status == "accept").ToListAsync();
        }

        /// <summary>
        /// Saves a new user to the database. takes in a user as a perameter
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task SaveNewUser(User user)
        {
             users.Add(user);
             await _applicationDbContext.SaveChangesAsync();
        }

       


        /// <summary>
        /// Returns all messages for a user.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            return await messages.ToListAsync();
        }

        

        public async Task SaveMessage(string FromUserName, int userToMessageId, int loggedInId, string content)
        {

            Message message = new Message(FromUserName, userToMessageId, loggedInId, content);
            await messages.AddAsync(message);
            await _applicationDbContext.SaveChangesAsync();
        }

        public async Task<List<Message>> GetMessages2users(int id, int userToMessage)
        {
            return await messages.Where(item => item.FromUserId == id && item.ToUserId == userToMessage ||
            item.ToUserId == id && item.FromUserId == userToMessage).ToListAsync();
        }

        public async Task<List<User>> GetUsersByPartialN(string searchString)
        {
            List<User> list = new List<User>();
            List<User> usersToSearch = await users.ToListAsync();
            foreach(var x in usersToSearch)
            {
                if (x.UserName.Contains(searchString))
                {
                    list.Add(x);
                }
            }
            return list;
        }

        /// <summary>
        /// Returns all users to a list.
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await users.ToListAsync();
        }

        /// <summary>
        /// Returns a User specified by their id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await users.FirstOrDefaultAsync(x=>x.Id == id);
        }
    }
}
