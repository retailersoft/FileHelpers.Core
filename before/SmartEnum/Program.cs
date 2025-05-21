using SmartEnum;

var creditCard = CreditCard.Platinum;

var discount = creditCard switch
{
    CreditCard.Standard => 0.01,
    CreditCard.Premium => 0.05,
    CreditCard.Platinum => 0.1
};

Console.WriteLine($"Discount for {creditCard} is {discount:P}");

Console.ReadKey();
