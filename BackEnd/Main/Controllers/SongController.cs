using BusinessLogicLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.AspNetCore.Cors;

//using System.Web.Abstractions;



namespace WhatsThatSong.Controllers
{
    [ApiController]
    [Route("Song")]
    public class SongController : ControllerBase
    {
        //private HostingEnvironment _env;
        private readonly BusinessLogicClass _businessLogicClass;
        private readonly ILogger<SongController> _logger;


        public SongController(BusinessLogicClass businessLogicClass, ILogger<SongController> logger)//, HostingEnvironment env)
        {
            _businessLogicClass = businessLogicClass;
            _logger = logger;
            //_env = env;
        }

        /// <summary>
        /// gets a song based on the id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getSong")]
        [EnableCors("AllowOrigin")]
        public async Task<Song> GetSongByIdAsync(int id)
        {
            return await _businessLogicClass.GetSongById(id);
        }

        /// <summary>
        /// adds a song to favorites. takes in the song id
        /// </summary>
        /// <param name="songid"></param>
        /// <returns></returns>
        [HttpGet]   //Should probably be post, but don't have time to figure it out
        [Route("addSongToFavorites")]
        [EnableCors("AllowOrigin")]
        public async Task addSongToFavorites(int songid, int userId)
        {
            await _businessLogicClass.AddSongToFavorites(songid, userId);
        }

        

        //[HttpPost]
        //[Route("uploadSongWithFile")]
        //public async void UploadSongWithFile(IFormFile file, int userid, string artist,
        //    string genre, string title, string lyrics, string urlPath, bool isOriginal)
        //{
        //    //var ctx = HttpContext.Current;
        //    //var root = ctx.Server.MapPath();

        //    Song NewSong = new Song(artist, genre, title, lyrics, urlPath, isOriginal);
        //    byte[] songByte = _businessLogicClass.ConvertIformFileToByteArray(file);
        //    NewSong.ByteArrayImage = songByte;
        //    await _businessLogicClass.ConvertFileToBitArray(NewSong);

            //string path = @"\wwwroot\Songs\" + title;
            //if (file != null)
            //{
            //    var dir = _env.ContentRootPath;
            //    using (var fileStream = new FileStream(Path.Combine(dir, path), FileMode.Create, FileAccess.Write))
            //    {
            //        //string path = Path.Combine(dir, file.FileName);
            //        await file.CopyToAsync(fileStream);
            //        Song song = new Song(artist, genre, title, lyrics, path, isOriginal);
            //        await _businessLogicClass.sendSongToRepCLass(song);
            //    }

            //    file.CopyToAsync(path + file.FileName);

            //}
        //}

        [HttpPost]
        [Route("uploadSong")]
        [EnableCors("AllowOrigin")]
        public async Task UploadSong(Song song)
        {
            Song s = new Song(song.ArtistName, song.Genre, song.Title, song.Lyrics, song.UrlPath, song.isOriginal, song.albumUrl);
            await _businessLogicClass.SaveSong(s);

        }

        /// <summary>
        /// return a list of songs by a certain user.
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAllSongsByACertainUser")]
        [EnableCors("AllowOrigin")]
        public async Task<List<Song>> GetAllSongsByACertainUser(int id)
        {
            List<Song> allUserSongs = await _businessLogicClass.GetallSongsByAUser(id);
            return allUserSongs;

        }

        /// <summary>
        /// return a list of original songs searched by genre
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getOriginalsongSearch")]
        [EnableCors("AllowOrigin")]
        public async Task<List<Song>> GetOriginalSongSearchByGenre(string genre)
        {
            List<Song> originalSongSearch = await _businessLogicClass.GetSongsBySearhGenre(genre);
            return originalSongSearch;
        }

        /// <summary>
        /// gets a list of original song serached by lyric phrase
        /// </summary>
        /// <param name="genre"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getOriginalsongsByLyrics")]
        [EnableCors("AllowOrigin")]
        public async Task<List<Song>> GetOriginalsongsByLyrics(string phrase)
        {
            List<Song> originalSongSearch = await _businessLogicClass.GetOriginalsongsByLyrics(phrase);
            return originalSongSearch;

        }

        /// <summary>
        /// return the top 5 songs basedon the number of plays
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getTop5Originals")]
        [EnableCors("AllowOrigin")]
        public async Task<List<Song>> GetTop5Originals()
        {
            List<Song> songs = await _businessLogicClass.GetTop5Originals();
            return songs;
        }

        /// <summary>
        /// increments the song number of plays property
        /// </summary>
        [HttpGet] //Should probably be put, but don't have time to figure it out
        [Route("incrementNumPlays")]
        [EnableCors("AllowOrigin")]
        public async Task IncrementNumPlays(int songId)
        {
            await _businessLogicClass.IncrementNUmPlays(songId);
        }

        /// <summary>
        /// gets all of a users favorite songs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetUsersFavoriteSongs")]
        [EnableCors("AllowOrigin")]
        public async Task<List<Song>> GetUsersFavoriteSongs(int Id)
        {
            List<Song> songs = await _businessLogicClass.GetUsersFavoriteSongs(Id);
            return songs;
        }

        /// <summary>
        /// gets a users forst 5 favorite songs
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Get5FavoriteSongsForUser")]
        [EnableCors("AllowOrigin")]
        public async Task<List<Song>> Get5FavoriteSongsForUser(int Id)
        {
            List<Song> songs = await _businessLogicClass.Get5FavoriteSongsForUser(Id);
            return songs;
        }

        /// <summary>
        /// returns a boolean for show weather a song is already on a users favorites list. 
        /// </summary>
        /// <param name="songId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("isSongAlreadyAFavorite")]
        [EnableCors("AllowOrigin")]
        public async Task<bool> isSongAlreadyAFavorite(int songId, int userId)
        {
            bool isFavorite = await _businessLogicClass.IsFavorite(songId, userId);
            return isFavorite;
        }
       
        /// <summary>
        /// delets a favoriteslist object form a users favoritesList
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="songId"></param>
        /// <returns></returns>
        //[HttpGet]
        [HttpDelete]
        [Route("DeleteSongFromFavorites")]
        [EnableCors("AllowOrigin")]
        public async Task DeleteSongFromFavorites(int userId, int songId)
        {
            await _businessLogicClass.DeleteFavoritesListForUser(userId, songId);
        }

        /// <summary>
        /// deletes a song from the songs list of uploaded songs
        /// </summary>
        /// <param name="songId"></param>
        /// <returns></returns>
        //[HttpGet]
        [HttpDelete]
        [Route("DeleteUploadedSong")]
        [EnableCors("AllowOrigin")]
        public async Task DeleteUploadedSong(int songId)
        {
            await _businessLogicClass.DeleteUploadedSong(songId);
        }

        /// <summary>
        /// returns a bool to see in a song is in the database
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("IsSongInDb")]
        [EnableCors("AllowOrigin")]
        public async Task<Song> IsSongInDb(string artistName, string title)
        {
            Song isInDataBase = await _businessLogicClass.IsInDataBase(artistName, title);
            return isInDataBase;
        }
    }
}
