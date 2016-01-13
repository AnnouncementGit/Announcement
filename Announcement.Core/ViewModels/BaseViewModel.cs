using System;
using System.Threading.Tasks;

namespace Announcement.Core
{
    public abstract class BaseViewModel
    {
        public static UserCredentials UserInfo { get; set; }
        
        public SourceManager SourceManager
        {
            get
            {
                return SourceManager.Instance;
            }
        }

        public BaseViewModel ()
        {

        }

        public virtual Result Initialize()
        {
            Erase();

            return new Result();
        }

        public virtual void Erase()
        {
            
        }
    }
}

