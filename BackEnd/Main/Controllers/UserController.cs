using BusinessLogicLayer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using ModelLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhatsThatSong.Controllers
{
    
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly BusinessLogicClass _businessLogicClass;
        private readonly ILogger<UserController> _logger;


        public UserController(BusinessLogicClass businessLogicClass, ILogger<UserController> logger)
        {
            _businessLogicClass = businessLogicClass;
            _logger = logger;
        }
		/*
        [HttpPost]
        [Route("CreateUser")]
        [EnableCors("AllowOrigin")]
        public async Task<User> CreateUser(User u)
        {
<<<<<<< HEAD

            return await _businessLogicClass.CreatNewBC(u.UserName, u.Password, u.Email);
=======
			
            User newUser = await _businessLogicClass.CreatNewBC(u.UserName, u.Password, u.Email);
            if (newUser != null)
            {
                UserProfileViewModel UPVM = await _businessLogicClass.GetUserProfileViewModel(newUser.Id);
                return UPVM;
            }
            else
            {
                return null;
            }
>>>>>>> 9c760d41f61baa04aade1b7ae4d43c35989df5c5
        }
		*/
		[HttpPost]
        [Route("CreateUser")]
        [EnableCors("AllowOrigin")]
        public async Task<User> CreateUser(User u)
        {
			
            return await _businessLogicClass.CreatNewBC(u.UserName, u.Password, u.Email);
        }

        [HttpGet]
        [Route("login")]
        [EnableCors("AllowOrigin")]
        //public async Task<UserProfileViewModel> login(string userName, string password)
        public async Task<User> login(string userName, string password)
        {
            return await _businessLogicClass.LoginUser(userName, password);
		}

        
        /// <summary>
        /// Gets the user to edit
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("EditButton")]
        [EnableCors("AllowOrigin")]
        public async Task<User> GetUserToEdit(int id)
        {
            User user = await _businessLogicClass.GetUserByIdAsync(id);
            return user;
        }

        [HttpPut]
        [Route("SaveEdit")]
        [EnableCors("AllowOrigin")]
        //public async Task<UserProfileViewModel> EditUser(int userId, string userName, string password, string email, string firstName, string lastName)
        public async Task<UserProfileViewModel> EditUser(User u)
        {
            User userToEdit = await _businessLogicClass.GetUserByIdAsync(u.Id);
            userToEdit.UserName = u.UserName; userToEdit.Password = u.Password; userToEdit.Email = u.Email; userToEdit.FirstName = u.FirstName; userToEdit.LastName = u.LastName;userToEdit.Description = u.Description;
            await _businessLogicClass.SaveUserToDb(userToEdit);
            UserProfileViewModel UPVM = await _businessLogicClass.GetUserProfileViewModel(userToEdit.Id);
            return UPVM;
        }

        [HttpGet]
        [Route("SearchForUsers")]
        [EnableCors("AllowOrigin")]
        public async Task<List<User>> SearchForUsers(string searchString)
        {
            List<User> listOfUsers = await _businessLogicClass.SearchForUsersByPartialN(searchString);

            return listOfUsers;
        }

        [HttpPost]
        [Route("RequestFriend")]
        [EnableCors("AllowOrigin")]
        public async Task FriendRequest(FriendList fl)
        {
            FriendList newf = new FriendList(fl.FriendId, fl.RequestedFriendId, fl.ToUsername, fl.FromUsername);
            await _businessLogicClass.RequestFriend(newf);
        }


        [HttpPut]
        [Route("EditFriendStatus")]
        [EnableCors("AllowOrigin")]
        public async Task AcceptFriend(FriendList friend)
        {
            await _businessLogicClass.AcceptFriend(friend);
        }

        /// <summary>
        /// sends a list of friends that have been accepted
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetFriends")]
        [EnableCors("AllowOrigin")]
        public async Task<List<FriendList>> GetFriendsByUserId(int id)
        {
            List<FriendList> friendList = await _businessLogicClass.GetListOfFriendsByUserId(id);
            return friendList;
        }
        [HttpGet]
        [Route("GetFriendsAsUsers")]
        [EnableCors("AllowOrigin")]
        public async Task<List<User>> GetFriendsAsUsers(int id)
        {
            List<User> userFriends = await _businessLogicClass.GetFriendsToUserList(id);
            return userFriends;
        }
        /// <summary>
        /// deletes a friend from the friend list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        //[HttpGet]
        [HttpDelete]
        [Route("DeleteFriend")]
        [EnableCors("AllowOrigin")]
        public async Task<List<FriendList>> DeleteFriend(int LoggedInUserId, int friendToDeleteId)
        {
            await _businessLogicClass.DeleteFriend(LoggedInUserId, friendToDeleteId);
            List<FriendList> friendList = await _businessLogicClass.GetListOfFriendsByUserId(LoggedInUserId);
            return friendList;
        }

        [HttpPost]
        [Route("sendMessage")]
        [EnableCors("AllowOrigin")]
        //public async Task<MessagingViewModel> SendMessage(string FromUserName, int LoggedInUserIdint,int UserToMessageId, string content)
        public async Task<MessagingViewModel> SendMessage(Message m)
        {
            MessagingViewModel viewModel = await _businessLogicClass.sendMessage(m.FromUserName, m.FromUserId, m.ToUserId, m.Content);
            //MessagingViewModel viewModel = await _businessLogicClass.GetMessagesViewModel(UserToMessageId);
            return viewModel;
        }

        /// <summary>
        /// RETURNS A MESSAGEVIEWMODEL WITH ALL OF THE MESSAGES BETWEEN 2 USERS based in both user id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GoToChat")]
        [EnableCors("AllowOrigin")]
        public async Task<MessagingViewModel> GetMessagesBetween2Users(int loggedInUser, int UserToMessageId)
        {
            MessagingViewModel viewModel = await _businessLogicClass.GetMessagesViewModel(loggedInUser, UserToMessageId);
            return viewModel;
        }



        [HttpGet]
        [Route("BakToProfile")]
        [EnableCors("AllowOrigin")]
        public async Task<UserProfileViewModel> BackToProfile(int LoggedInUserid)
        {
            User user = await _businessLogicClass.GetUserByIdAsync(LoggedInUserid);
            UserProfileViewModel viewModel = await _businessLogicClass.GetUserProfileViewModel(LoggedInUserid);
            return viewModel;
        }

        
        [HttpGet]
        [Route("getAllUsers")]
        [EnableCors("AllowOrigin")]
        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _businessLogicClass.GetAllUsersAsync();
        }

        [HttpGet]
        [Route("getUserByIdaAync")]
        [EnableCors("AllowOrigin")]
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _businessLogicClass.GetUserByIdAsync(id);
        }

        [HttpGet]
        [Route("GetAllMessagesAsync")]
        [EnableCors("AllowOrigin")]
        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            return await _businessLogicClass.GetAllMessagesAsync();
        }

        [HttpGet]
        [Route("DisplayAllFriendRequests")]
        [EnableCors("AllowOrigin")]
        public async Task<List<FriendList>> DisplayAllFriendRequests(int UserId)
        {
            List<FriendList> list = await _businessLogicClass.GetAllFriendRequestsOUserId(UserId);
            return list;
        }

        [HttpGet]
        [Route("AreWeFriends")]
        [EnableCors("AllowOrigin")]
        public async Task<bool> AreWeFriends(int userId, int FriendId)
        {
            bool AreFriends = await _businessLogicClass.AreWeFriends(userId, FriendId);
            return AreFriends;
        }
    }
}
