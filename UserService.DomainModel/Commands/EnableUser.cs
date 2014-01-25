using System;

namespace UserService.DomainModel.Commands
{
    public class EnableUser
    {
        public readonly Guid Gpid;

        public EnableUser(Guid gpid)
        {
            Gpid = gpid;
        }
    }
}
