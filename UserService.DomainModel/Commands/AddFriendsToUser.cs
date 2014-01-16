using System;

namespace UserService.DomainModel.Commands
{
    public class AddFriendToUser
    {
        public readonly Guid Gpid;
        public readonly Guid FriendsGpid;
        public readonly string FName;
        public readonly string LName;
        public readonly int? BirthdayDay;
        public readonly int? BirthdayMonth;
        public readonly int? BirthdayYear;

        public AddFriendToUser(Guid gpid, Guid friendsGpid, string fName, string lName, int? birthdayDay, int? birthdayMonth, int? birthdayYear)
        {
            Gpid = gpid;
            FriendsGpid = friendsGpid;
            FName = fName;
            LName = lName;
            BirthdayDay = birthdayDay;
            BirthdayMonth = birthdayMonth;
            BirthdayYear = birthdayYear;
        }
    }
}
