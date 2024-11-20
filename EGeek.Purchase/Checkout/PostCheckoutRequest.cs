namespace EGeek.Purchase.Checkout;

internal record PostCheckoutRequest(
    string ZipCode,
    string CardNumber,
    string CardholderName,
    DateTime ExpirationDate,
    string Cvv
);