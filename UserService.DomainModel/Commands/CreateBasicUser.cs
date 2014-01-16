using System;

namespace UserService.DomainModel.Commands
{
    public class CreateBasicUser
    {
        public Guid GlobalPersonId { get; set; }
        public string EmailAddress { get; set; }
        public int MetroId { get; set; }
    }
}
