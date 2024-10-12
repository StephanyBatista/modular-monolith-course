namespace EGeek.Purchase.Checkout;

internal interface IPayment
{
    public Task<(bool, string)> Process(
        decimal amount,
        string cardNumber,
        string cardHolderName,
        DateTime expirationDate,
        string cvv
    );
}