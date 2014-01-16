using System;

namespace UserService.DomainModel.Events
{
    public static class UserEvents
    {
        public static BasicUserCreated Created(string emailAddress, UserId gpid, int metroId, bool disabled)
        {
            return new BasicUserCreated(emailAddress, gpid, metroId, disabled);
        }

        public static UserDisabled Disabled(UserId id)
        {
            return new UserDisabled(id);
        }

        public static UserHasNewFriend NewFriend(UserId gpid, UserId friend, string fName, string lName, 
            int birthdayDay, int birthdayMonth, int birthdayYear)
        {
            return new UserHasNewFriend(gpid, friend, fName, lName, birthdayDay, birthdayMonth, birthdayYear);
        }

        public static UserHasNewFriend NewFriend(UserId gpid, UserId friend, string fName, string lName)
        {
            return new UserHasNewFriend(gpid, friend, fName, lName);
        }


        public class BasicUserCreated
        {
            public readonly string EMailAddress;
            public readonly Guid Gpid;
            public readonly int MetroId;
            public readonly bool Disabled;

            public BasicUserCreated(string eMailAddress, Guid gpid, int metroId, bool disabled)
            {
                EMailAddress = eMailAddress;
                Gpid = gpid;
                MetroId = metroId;
                Disabled = disabled;
            }
        }

        public class UserDisabled
        {
            public readonly Guid Gpid;

            public UserDisabled(Guid gpid)
            {
                Gpid = gpid;
            }
        }

        public class UserHasNewFriend
        {
            public readonly Guid Gpid;
            public readonly Guid FriendsGpid;
            public readonly string FName;
            public readonly string LName;
            public readonly int? BirthdayDay;
            public readonly int? BirthdayMonth;
            public readonly int? BirthdayYear;

            public UserHasNewFriend(Guid gpid, Guid friendsGpid, string fName, string lName, int? birthdayDay, 
                int? birthdayMonth, int? birthdayYear)
            {
                Gpid = gpid;
                FriendsGpid = friendsGpid;
                FName = fName;
                LName = lName;
                BirthdayDay = birthdayDay;
                BirthdayMonth = birthdayMonth;
                BirthdayYear = birthdayYear;
            }

            public UserHasNewFriend(Guid gpid, Guid friendsGpid, string fName, string lName)
            {
                Gpid = gpid;
                FriendsGpid = friendsGpid;
                FName = fName;
                LName = lName;
            }
        }
    }
}
