using System.Collections.Generic;

namespace MishMashWebApplication.ViewModels.Home
{
    public class IndexChannelCollectionViewModel
    {
        public List<IndexChannelViewModel> FollowedChannels { get; set; }

        public List<IndexChannelViewModel> SuggestedChannels { get; set; }

        public List<IndexChannelViewModel> SeeOthersChannels { get; set; }
    }
}
