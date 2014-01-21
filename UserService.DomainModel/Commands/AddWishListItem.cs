using System;

namespace UserService.DomainModel.Commands
{
    public class AddWishListItem
    {
        public readonly Guid Gpid;
        public readonly Guid WishListItemId;
        public readonly Guid RestaurantId;
        public readonly string Notes;

        public AddWishListItem(Guid gpid, Guid wishListItemId, Guid restaurantId, string notes)
        {
            Gpid = gpid;
            RestaurantId = restaurantId;
            Notes = notes;
            WishListItemId = wishListItemId;
        }
    }
}
