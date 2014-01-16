using System;

namespace UserService.DomainModel
{
    public class RestaurantId : IEquatable<RestaurantId>
    {
        private readonly Guid _id;

        public Guid Gpid
        {
            get { return _id; }
        }

        public RestaurantId(Guid gpid)
        {
            _id = gpid;
        }

        public bool Equals(RestaurantId other)
        {
            return _id.Equals(other._id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RestaurantId && Equals((RestaurantId)obj);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }

        public static implicit operator Guid(RestaurantId id)
        {
            return id._id;
        }
    }
}
