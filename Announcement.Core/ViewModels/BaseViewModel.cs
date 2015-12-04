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

        public virtual Result Initialize()
        {
            Erase();

            return new Result() { Type = ResultType.Success };
        }

        public virtual void Erase()
        {
            
        }
    }
}

