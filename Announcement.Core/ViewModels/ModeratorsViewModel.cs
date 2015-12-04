using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Announcement.Core
{
    public class ModeratorsViewModel : BaseViewModel
    {
        public static ModeratorsViewModel Instance
        {
            get
            {
                return instance ?? (instance = new ModeratorsViewModel());
            }
        }

        public List<Moderator> Moderators { get; set; }

        public override Result Initialize()
        {
            base.Initialize();

            var result = SourceManager.PullModerators();

            return result;
        }
  
        private static ModeratorsViewModel instance;
    }
}

