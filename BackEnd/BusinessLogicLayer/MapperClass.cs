using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ModelLayer.Models;
using ModelLayer.ViewModels;
using RepositoryLayer;

namespace BusinessLogicLayer
{
    public class MapperClass
    {

        public UserProfileViewModel BuildUserProfileViewModel(int Id, int numOfFriend, string pending, string username)
        {
            UserProfileViewModel model = new UserProfileViewModel();
            model.userId = Id;
            model.userName = username;
            model.numberOfFriends = numOfFriend;
            model.FirendStatus = pending;
            
            return model;
        }

        public  MessagingViewModel GetMessagingViewModel(int loggedInUserId, int usertomessageId, List<Message> Messages, string LoggedInUserName, string userToMessageUserName)
        {
            MessagingViewModel viewModel = new MessagingViewModel();
            viewModel.CurrentUserId = loggedInUserId;
            viewModel.currentUserName = LoggedInUserName;
            viewModel.friendToMessageUserId = usertomessageId;
            viewModel.friendToMessageUserName = userToMessageUserName;
            viewModel.messages = Messages;

            return viewModel;
        }
    }
}
