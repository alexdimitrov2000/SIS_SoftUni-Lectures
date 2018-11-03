using MishMashWebApplication.Models;
using MishMashWebApplication.Models.Enums;
using MishMashWebApplication.ViewModels.Channels;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Linq;

namespace MishMashWebApplication.Controllers
{
    public class ChannelsController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var channel = this.Context.Channels
                .Any(c => c.Id == id);

            if (!channel)
            {
                return this.BadRequestErrorWithView("Invalid channel id.");
            }

            var channelViewModel = this.Context.Channels
                .Where(c => c.Id == id)
                .Select(c => new ChannelDetailsViewModel
                {
                    Name = c.Name,
                    Description = c.Description,
                    Tags = c.Tags,
                    Type = c.Type.ToString(),
                    FollowersCount = c.Followers.Count
                })
                .First();

            return this.View(channelViewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(ChannelCreateInputModel model)
        {
            var name = model.Name;
            var description = model.Description;
            var tags = model.Tags;

            if (!Enum.TryParse<ChannelType>(model.Type, out var type))
            {
                return this.BadRequestErrorWithView("Invalid channel type");
            }

            var channel = new Channel
            {
                Name = name,
                Description = description,
                Tags = tags,
                Type = type
            };

            this.Context.Channels.Add(channel);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestErrorWithView(e.Message);
            }

            return this.Redirect("/channels/details?id=" + channel.Id);
        }

        [Authorize]
        public IHttpResponse Followed()
        {
            var userUsername = this.User.Username;

            var user = this.Context.Users.FirstOrDefault(u => u.Username == userUsername);

            var followedChannels = user.FollowedChannels
                .Select(c => new FollowedChannelViewModel
                {
                    Id = c.ChannelId,
                    Name = c.Channel.Name,
                    Type = c.Channel.Type.ToString(),
                    FollowersCount = c.Channel.Followers.Count
                })
                .ToList();

            var followedChannelsCollection = new FollowedChannelCollectionViewModel
            {
                Channels = followedChannels
            };

            return this.View(followedChannelsCollection);
        }

        [Authorize]
        public IHttpResponse Follow(int id)
        {
            var channel = this.Context.Channels.FirstOrDefault(c => c.Id == id);

            if (channel == null)
            {
                return this.BadRequestError("Invalid channel id.");
            }

            var userUsername = this.User.Username;
            var user = this.Context.Users.First(u => u.Username == userUsername);

            var userChannel = new UserChannel
            {
                Channel = channel,
                User = user
            };

            this.Context.UsersChannels.Add(userChannel);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestErrorWithView(e.Message);
            }

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Unfollow(int id)
        {
            var channel = this.Context.Channels.FirstOrDefault(c => c.Id == id);

            if (channel == null)
            {
                return this.BadRequestError("Invalid channel id.");
            }

            var userUsername = this.User.Username;
            var user = this.Context.Users.First(u => u.Username == userUsername);
            var userChannel = this.Context.UsersChannels.FirstOrDefault(uc => uc.Channel == channel && uc.User == user);

            if (!user.FollowedChannels.Any(fc => fc.ChannelId == id) || userChannel == null)
            {
                return this.BadRequestErrorWithView("You are not following that channel.");
            }

            this.Context.UsersChannels.Remove(userChannel);

            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestErrorWithView(e.Message);
            }

            return this.Redirect("/channels/followed");
        }
    }
}
