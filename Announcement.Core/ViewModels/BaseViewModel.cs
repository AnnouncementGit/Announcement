using System;
using System.Threading.Tasks;

namespace Announcement.Core
{
    public abstract class BaseViewModel
    {
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

        public virtual InitializationStatus Initialize()
        {
            var status = new InitializationStatus();

            status.IsSuccess = true;

            return status;
        }

        public virtual void Erase()
        {
            
        }
    }
}

