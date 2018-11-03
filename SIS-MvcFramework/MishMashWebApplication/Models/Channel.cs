using MishMashWebApplication.Models.Enums;
using System.Collections.Generic;

namespace MishMashWebApplication.Models
{
    public class Channel
    {
        public Channel()
        {
            this.Followers = new HashSet<UserChannel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ChannelType Type { get; set; }

        public string Tags { get; set; }

        public virtual ICollection<UserChannel> Followers { get; set; }
    }
}
