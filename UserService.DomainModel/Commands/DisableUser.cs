using System;

namespace UserService.DomainModel.Commands
{
    public class DisableUser
    {
        public readonly Guid Gpid;

        public DisableUser(Guid gpid)
        {
            Gpid = gpid;
        }
    }
}
