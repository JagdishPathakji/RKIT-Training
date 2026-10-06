using System;

// ABSTRACT BASE CLASS
abstract class Payment {
    // Protected field
    // Accessible inside Payment and derived classes

    protected double amount;


    // Property
    // Public get, private set
    // Outside can read it but cannot change it

    public string TransactionId
    {
        get;
        private set;
    }


    // Abstract property
    // Every payment type MUST provide its own PaymentType

    public abstract string PaymentType
    {
        get;
    }


    // Constructor

    protected Payment(
        string transactionId,
        double amount)
    {
        TransactionId = transactionId;

        this.amount = amount;
    }

    // --------------------------------------------------------
    // NORMAL METHOD
    // Common implementation for every payment

    public void DisplayTransaction()
    {
        Console.WriteLine(
            $"Transaction ID : {TransactionId}"
        );

        Console.WriteLine(
            $"Payment Type   : {PaymentType}"
        );

        Console.WriteLine(
            $"Amount         : {amount}"
        );
    }


    // ABSTRACT METHOD
    // Every derived class MUST implement ProcessPayment()

    public abstract void ProcessPayment();


    // ABSTRACT METHOD
    // Every derived class MUST implement Refund()

    public abstract void Refund();
}


// CREDIT CARD PAYMENT

class CreditCardPayment : Payment
{
    private string cardNumber;


    // Constructor
    public CreditCardPayment(
        string transactionId,
        double amount,
        string cardNumber)
        : base(transactionId, amount)
    {
        this.cardNumber = cardNumber;
    }


    // IMPLEMENT ABSTRACT PROPERTY
    public override string PaymentType
    {
        get
        {
            return "Credit Card";
        }
    }


    // IMPLEMENT ABSTRACT METHOD
    public override void ProcessPayment()
    {
        Console.WriteLine(
            $"Processing ₹{amount} using Credit Card."
        );

        Console.WriteLine(
            $"Card: {cardNumber}"
        );
    }


    // IMPLEMENT ABSTRACT METHOD
    public override void Refund()
    {
        Console.WriteLine(
            $"Refunding ₹{amount} to Credit Card."
        );
    }
}


// UPI PAYMENT
class UpiPayment : Payment
{
    private string upiId;


    // Constructor
    public UpiPayment(
        string transactionId,
        double amount,
        string upiId)
        : base(transactionId, amount)
    {
        this.upiId = upiId;
    }


    // IMPLEMENT ABSTRACT PROPERTY
    public override string PaymentType
    {
        get
        {
            return "UPI";
        }
    }


    // IMPLEMENT ABSTRACT METHOD
    public override void ProcessPayment()
    {
        Console.WriteLine(
            $"Processing ₹{amount} using UPI."
        );

        Console.WriteLine(
            $"UPI ID: {upiId}"
        );
    }


    // IMPLEMENT ABSTRACT METHOD
    public override void Refund()
    {
        Console.WriteLine(
            $"Refunding ₹{amount} through UPI."
        );
    }
}


// CASH PAYMENT
class CashPayment : Payment
{
    // Constructor
    public CashPayment(
        string transactionId,
        double amount)
        : base(transactionId, amount)
    {
    }


    // IMPLEMENT ABSTRACT PROPERTY
    public override string PaymentType
    {
        get
        {
            return "Cash";
        }
    }


    // IMPLEMENT ABSTRACT METHOD
    public override void ProcessPayment()
    {
        Console.WriteLine(
            $"Processing ₹{amount} using Cash."
        );
    }


    // IMPLEMENT ABSTRACT METHOD
    public override void Refund()
    {
        Console.WriteLine(
            $"Refunding ₹{amount} in Cash."
        );
    }
}


// PROGRAM

class Program {
    static void Main(string[] args) {
        // CANNOT DO THIS

        // Payment payment = new Payment();
        // ERROR:
        // Cannot create an instance of an abstract class


        // CREATE CONCRETE OBJECTS

        CreditCardPayment creditCard =
            new CreditCardPayment(
                "TXN1001",
                5000,
                "XXXX-XXXX-1234"
            );


        UpiPayment upi =
            new UpiPayment(
                "TXN1002",
                2500,
                "jagdish@upi"
            );


        CashPayment cash =
            new CashPayment(
                "TXN1003",
                1000
            );


        // COMMON METHOD
        // Inherited from Payment

        creditCard.DisplayTransaction();
        Console.WriteLine();


        upi.DisplayTransaction();
        Console.WriteLine();


        cash.DisplayTransaction();
        Console.WriteLine();


        // DIFFERENT IMPLEMENTATIONS

        creditCard.ProcessPayment();
        Console.WriteLine();

        upi.ProcessPayment();
        Console.WriteLine();

        cash.ProcessPayment();
        Console.WriteLine();


        // REFUND

        creditCard.Refund();
        Console.WriteLine();

        upi.Refund();
        Console.WriteLine();

        cash.Refund();
        Console.WriteLine();
    }
}