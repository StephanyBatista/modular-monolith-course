namespace EGeek.Purchase.Checkout;

internal interface IPayment
{
    public Task<(bool, string)> Process(
        decimal ammount,
        string cardNumber,
        string cardHolderName,
        DateTime expirationDate,
        string cvv
    );
}