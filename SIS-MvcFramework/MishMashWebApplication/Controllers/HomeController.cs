using MishMashWebApplication.ViewModels.Home;
using SIS.HTTP.Responses;
using System.Collections.Generic;
using System.Linq;

namespace MishMashWebApplication.Controllers
{
    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (this.User.IsLoggedIn)
            {
                var collectionOfChannels = new IndexChannelCollectionViewModel();

                var userUsername = this.User.Username;
                var user = this.Context.Users.First(u => u.Username == userUsername);

                var userFollowedChannels = this.Context.UsersChannels
                    .Where(uc => uc.UserId == user.Id)
                    .Select(uc => uc.Channel)
                    .ToList();

                var userUnfollowedChannels = this.Context.Channels
                    .Where(c => !userFollowedChannels.Contains(c))
                    .ToList();

                var allFollowedTags = new List<string>();

                var followedChannelsTags = userFollowedChannels
                    .Select(c => c.Tags)
                    .ToList();

                followedChannelsTags.ForEach(t => allFollowedTags.AddRange(t.Split(", ")));


                var followedChannels = userFollowedChannels
                    .Select(c => new IndexChannelViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = c.Type.ToString(),
                        FollowersCount = c.Followers.Count
                    })
                    .ToList();
                
                var suggestedChannels = userUnfollowedChannels
                    .Where(c => allFollowedTags.Any(t => c.Tags.Contains(t)))
                    .Select(c => new IndexChannelViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = c.Type.ToString(),
                        FollowersCount = c.Followers.Count
                    })
                    .ToList();

                var seeOthersChannels = userUnfollowedChannels
                    .Select(c => new IndexChannelViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        Type = c.Type.ToString(),
                        FollowersCount = c.Followers.Count
                    })
                    .ToList();

                collectionOfChannels.FollowedChannels = followedChannels;
                collectionOfChannels.SuggestedChannels = suggestedChannels;
                collectionOfChannels.SeeOthersChannels = seeOthersChannels;

                return this.View("Home/LoggedIndex", collectionOfChannels);
            }

            return this.View();
        }
    }
}
