namespace EGeek.Purchase.ShoppingCarts;

internal interface IShoppingCartRepository
{
    public Task Save(ShoppingCart shoppingCart);
    public ShoppingCart? GetBy(string email, Status status);
    public Task Commit();
}