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
        
        public ModeratorsViewModel()
        {
            
        }

        public override InitializationStatus Initialize()
        {
            var status = new InitializationStatus() { IsSuccess = true };

            Erase();

            var pullModeratorsResult = SourceManager.PullModerators();

            if (pullModeratorsResult.HasError)
            {
                status.IsSuccess = false;

                status.Message = pullModeratorsResult.Message;

                return status;
            }

            return status;
        }

        public override void Erase()
        {
                
        }

        public void DeleteModerator(int id, Action<bool> callback)
        {
            
        }


            
        private static ModeratorsViewModel instance;
    }
}

