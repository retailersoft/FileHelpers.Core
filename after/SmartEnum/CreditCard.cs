namespace SmartEnum;

public abstract class CreditCard : Enumeration<CreditCard>
{
    public static readonly CreditCard Standard = new StandardCreditCard();
    public static readonly CreditCard Premium = new PremiumCreditCard();
    public static readonly CreditCard Platinum = new PlatinumCreditCard();

    protected CreditCard(int value, string name)
        : base(value, name)
    {
    }

    public abstract double Discount { get; }

    private sealed class StandardCreditCard : CreditCard
    {
        public StandardCreditCard()
            : base(1, "Standard")
        {
        }

        public override double Discount => 0.01;
    }

    private sealed class PremiumCreditCard : CreditCard
    {
        public PremiumCreditCard()
            : base(2, "Premium")
        {
        }

        public override double Discount => 0.05;
    }

    private sealed class PlatinumCreditCard : CreditCard
    {
        public PlatinumCreditCard()
            : base(3, "Platinum")
        {
        }

        public override double Discount => 0.1;
    }
}

public  abstract class PointOfSale : Enumeration<PointOfSale>
{
    public static readonly PointOfSale Standard = new StandardPointOfSale();
    public static readonly PointOfSale Premium = new PremiumPointOfSale();
    public static readonly PointOfSale Platinum = new PlatinumPointOfSale();
    protected PointOfSale(int value, string name)
        : base(value, name)
    {
    }
    public abstract double Discount { get; }
    private sealed class StandardPointOfSale : PointOfSale
    {
        public StandardPointOfSale()
            : base(1, "Standard")
        {
        }
        public override double Discount => CreditCard.FromValue(1) == CreditCard.Platinum ? 1 : 0;
    }
    private sealed class PremiumPointOfSale : PointOfSale
    {
        public PremiumPointOfSale()
            : base(2, "Premium")
        {
        }
        public override double Discount => 0.05;
    }
    private sealed class PlatinumPointOfSale : PointOfSale
    {
        public PlatinumPointOfSale()
            : base(3, "Platinum")
        {
        }
        public override double Discount => 0.1;
    }
}



