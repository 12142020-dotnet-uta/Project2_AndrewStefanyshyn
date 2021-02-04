using ModelLayer.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace WhatsThatTest
{
    public class ModelTests
    {
        /// <summary>
        /// Accepts an object model, attempts to build and validate that model. If validation encounters
        /// problems, returns a list of errors in the form of ValidationResults. An empty list is returned
        /// when there are no problems validating the model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private static IList<ValidationResult> ValidateModel(object model)
        {
            // create list to return
            var result = new List<ValidationResult>();
            // create new validation context
            var validationContext = new ValidationContext(model);
            // true means validate all properties
            Validator.TryValidateObject(model, validationContext, result, true);

            if (model is IValidatableObject) (model as IValidatableObject).Validate(validationContext);

            return result;
        }

        /// <summary>
        /// Attempts to validate model. All attributes of user should pass.
        /// </summary>
        [Fact]
        public void UserIsValid()
        {
            // Create a new user with good data.
            var user = new User
            {
                Id = int.MaxValue,
                UserName = "TestSubject-0143",
                Email = "testSub_0143@email.com",
                Password = "Test1!",
                FirstName = "Johnny",
                LastName = "Test"
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(user).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        /// <summary>
        /// Attempts to invalidate model. Email attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void UserIsInvalid()
        {
            // Create a new user with bad data.
            var user = new User
            {
                Id = int.MaxValue,
                UserName = "TestSubject-0143",
                Email = "testSub_0143",
                Password = "Test1!",
                FirstName = "Johnny",
                LastName = "Test"
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(user).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of artist should pass.
        /// </summary>
        [Fact]
        public void ArtistIsValid()
        {
            // Create a new artist with good data.
            var artist = new Artist
            {
                Id = int.MaxValue,
                Name = "JohnnyTest"
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(artist).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        /// <summary>
        /// Attempts to invalidate model. Name attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void ArtistIsInvalid()
        {
            // Create a new artist with bad data.
            var artist = new Artist
            {
                Id = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(artist).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of favorite list should pass.
        /// </summary>
        [Fact]
        public void FavoriteListIsValid()
        {
            // Create a new favorite list with good data.
            var favoriteList = new FavoriteList
            {
                Id = int.MaxValue,
                FavoriteListLink = int.MaxValue,
                SongId = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(favoriteList).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        // TODO: Make this test fail! Maybe require ints to be positive via data annotation restraint and assign negative int to an attribute?
        /// <summary>
        /// Attempts to invalidate model. Song id attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void FavoriteListIsInvalid()
        {
            // Create a new favorite list with bad data.
            var favoriteList = new FavoriteList
            {
                Id = int.MaxValue,
                FavoriteListLink = int.MaxValue,
                //SongId = null -- can't assign null to int
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(favoriteList).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of favorite list link should pass.
        /// </summary>
        [Fact]
        public void FavoriteListLinkIsValid()
        {
            // Create a new favorite list link with good data.
            var favoriteListLink = new FavoriteListLink
            {
                Id = int.MaxValue,
                UserId = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(favoriteListLink).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        // TODO: Make this test fail! Maybe require ints to be positive via data annotation restraint and assign negative int to an attribute?
        /// <summary>
        /// Attempts to invalidate model. User id attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void FavoriteListLinkIsInvalid()
        {
            // Create a new favorite list link with bad data.
            var favoriteListLink = new FavoriteListLink
            {
                Id = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(favoriteListLink).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of friend list should pass.
        /// </summary>
        [Fact]
        public void FriendListIsValid()
        {
            // Create a new friend list with good data.
            var friendList = new FriendList
            {
                Id = int.MaxValue,
                FriendId = int.MaxValue,
                FriendListLink = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(friendList).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        // TODO: Make this test fail! Maybe require ints to be positive via data annotation restraint and assign negative int to an attribute?
        /// <summary>
        /// Attempts to invalidate model. Friend list link attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void FriendListIsInvalid()
        {
            // Create a new friend list with bad data.
            var friendList = new FriendList
            {
                Id = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(friendList).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of friend list link should pass.
        /// </summary>
        [Fact]
        public void FriendListLinkIsValid()
        {
            // Create a new friend list link with good data.
            var friendListLink = new FriendListLink
            {
                Id = int.MaxValue,
                UserId = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(friendListLink).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        // TODO: Make this test fail! Maybe require ints to be positive via data annotation restraint and assign negative int to an attribute?
        /// <summary>
        /// Attempts to invalidate model. User id attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void FriendListLinkIsInvalid()
        {
            // Create a new friend list link with bad data.
            var friendListLink = new FriendListLink
            {
                Id = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(friendListLink).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of genre should pass.
        /// </summary>
        [Fact]
        public void GenreIsValid()
        {
            // Create a new genre with good data.
            var genre = new Genre
            {
                Id = int.MaxValue,
                Name = "Rock"
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(genre).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        /// <summary>
        /// Attempts to invalidate model. Name attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void GenreIsInvalid()
        {
            // Create a new genre with bad data.
            var genre = new Genre
            {
                Id = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(genre).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of message should pass.
        /// </summary>
        [Fact]
        public void MessageIsValid()
        {
            // Create a new message with good data.
            var message = new Message
            {
                Id = int.MaxValue,
                FromUserId = int.MaxValue,
                ToUserId = 1,
                Content = "Dulce et decorum est pro patria mori"
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(message).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        /// <summary>
        /// Attempts to invalidate model. Content attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void MessageIsInvalid()
        {
            // Create a new message with bad data.
            var message = new Message
            {
                Id = int.MaxValue,
                FromUserId = int.MaxValue,
                ToUserId = int.MinValue,
                Content = ""
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(message).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of playlist link should pass.
        /// </summary>
        [Fact]
        public void PlaylistLinkIsValid()
        {
            // Create a new playlist link with good data.
            var playlistLink = new PlaylistLink
            {
                Id = int.MaxValue,
                UserId = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(playlistLink).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        // TODO: Make this test fail! Maybe require ints to be positive via data annotation restraint and assign negative int to an attribute?
        /// <summary>
        /// Attempts to invalidate model. User id attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void PlaylistLinkIsInvalid()
        {
            // Create a new playlist link with bad data.
            var playlistLink = new PlaylistLink
            {
                Id = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(playlistLink).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of playlist song should pass.
        /// </summary>
        [Fact]
        public void PlaylistSongIsValid()
        {
            // Create a new playlist song with good data.
            var playlistSong = new PlaylistSong
            {
                Id = int.MaxValue,
                PlaylistLinkId = int.MaxValue,
                SongId = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(playlistSong).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        // TODO: Make this test fail! Maybe require ints to be positive via data annotation restraint and assign negative int to an attribute?
        /// <summary>
        /// Attempts to invalidate model. Song id attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void PlaylistSongIsInvalid()
        {
            // Create a new playlist song with bad data.
            var playlistSong = new PlaylistSong
            {
                Id = int.MaxValue,
                PlaylistLinkId = int.MaxValue
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(playlistSong).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        /// <summary>
        /// Attempts to validate model. All attributes of song should pass.
        /// </summary>
        [Fact]
        public void SongIsValid()
        {
            // Create a new song with good data.
            var song = new Song
            {
                Id = int.MaxValue,
                ArtistName = "z",
                Duration = TimeSpan.MaxValue,
                Genre = "Rock",
                Title = "Du Hast",
                NumberOfPlays = 0
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(song).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);

            // overload song
            var song2 = new Song("z", "Rock", "Du Hast", "", "", false, "");
            errorcount = ValidateModel(song2).Count;
            Assert.Equal(0, errorcount);

            // empty song constructor
            var song3 = new Song();
            errorcount = ValidateModel(song3).Count;
            // 3 values are automatically assigned null, so we should expect that
            Assert.Equal(3, errorcount);
        }

        /// <summary>
        /// Attempts to invalidate model. Title attribute should fail annotation restraint.
        /// </summary>
        [Fact]
        public void SongIsInvalid()
        {
            // Create a new song with bad data.
            var song = new Song
            {
                Id = int.MaxValue,
                ArtistName = "Bad Posture",
                Duration = TimeSpan.MaxValue,
                Genre = "Pop Punk",
                Title = ""
            };

            // Get the number of errors in the model. Expecting 1 error.
            var errorcount = ValidateModel(song).Count;
            // There should be an error. If there's not, something is wrong with validation.
            Assert.NotEqual(0, errorcount);
        }

        [Fact]
        public void CreateUserWithOnlyRequiredParameters()
        {
            // Create a user with only 3 paramaters
            var user = new User
            {
                UserName = "TestSubject-0143",
                Email = "testSub_0143@email.com",
                Password = "Test1!"
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(user).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);
        }

        /// <summary>
        /// Checks that salted and hashed passwords don't return the same value.
        /// </summary>
        [Fact]
        public void HashPasswordTest()
        {
            // Create const string to test against
            const string password = "Test1!";

            // Create a user with plain text password
            var user = new User
            {
                Id = int.MaxValue,
                UserName = "TestSubject-0143",
                Email = "testSub_0143@email.com",
                Password = "Test1!",
                FirstName = "Johnny",
                LastName = "Test"
            };

            // Get the number of errors in the model. Expecting 0 errors.
            var errorcount = ValidateModel(user).Count;
            // No errors indicates that the model built without errors, and we like that.
            Assert.Equal(0, errorcount);

            // Password salting and hashing gives different passwords.
            Assert.NotEqual(user.HashPassword(user.Password), password);
        }
    }
}
