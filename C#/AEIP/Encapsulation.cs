// Keep an object's internal state protected and control how outside code interacts with that state.

using System;

// class BankAccount
// {
//     private double balance;

//     public BankAccount(double initialBalance)
//     {
//         if (initialBalance >= 0)
//         {
//             balance = initialBalance;
//         }
//         else
//         {
//             balance = 0;
//         }
//     }

//     public void Deposit(double amount)
//     {
//         if (amount <= 0)
//         {
//             Console.WriteLine("Deposit amount must be positive.");
//             return;
//         }

//         balance += amount;
//     }

//     public void Withdraw(double amount)
//     {
//         if (amount <= 0)
//         {
//             Console.WriteLine("Withdrawal amount must be positive.");
//             return;
//         }

//         if (amount > balance)
//         {
//             Console.WriteLine("Insufficient balance.");
//             return;
//         }

//         balance -= amount;
//     }

//     public double GetBalance()
//     {
//         return balance;
//     }
// }

// class Program
// {
//     static void Main(string[] args)
//     {
//         BankAccount account = new BankAccount(5000);

//         account.Deposit(2000);

//         account.Withdraw(1500);

//         Console.WriteLine(
//             $"Balance: {account.GetBalance()}"
//         );

//         // account.balance = -100000;
//         // ERROR: balance is private
//     }
// }






// Property and its Variations
class BankAccount {

    private double balance;
    // getter and setter for balance
    public double Balance {
        get {
            return balance;
        }

        set {
            balance = value;
        }
    }


    private int age;
    // getter and validation based setter for age
    public int Age {
        get {
            return age;
        }

        set {
            if(value >= 0) {
                age = value;
            }
        }
    }


    private string accountNumber;
    // read only 
    public string AccountNumber {
        get {
            return accountNumber;
        }
    }


    private string pin;
    // write only
    public string Pin {
        set {
            pin = value;
        }
    }


    // auto property (get + set)
    public string OwnerName {
        get;
        set;
    }

    
    // auto property (get + private set) [can set only inside class]
    public double InterestRate {
        get;
        private set;
    }


    // read-only auto property
    public int BranchId {
        get;
    }


    // auto property (get + private set)
    public double CreditLimit {
        get;
        private set;
    }

    
    // auto property with default values
    public string Currency {
        get;
        set;
    } = "INR";

    
    // read-only with default value
    public string BankName {
        get;
    } = "ABC Bank";


    public BankAccount(string accountNumber, int branchId, double interestRate) {
        // read-only can be assigned during construction.
        this.accountNumber = accountNumber;
        BranchId = branchId;
        InterestRate = interestRate;
        CreditLimit = 100000;
    }


    public void ChangeInterestRate(double newRate) {
        // class itself can modify using private set 
        InterestRate = newRate;
    }


    public void ChangeCreditLimit(double newLimit) {
        CreditLimit = newLimit;
    }


    public void Display()
    {
        Console.WriteLine($"Balance      : {Balance}");
        Console.WriteLine($"Age          : {Age}");
        Console.WriteLine($"Account No   : {AccountNumber}");
        Console.WriteLine($"Owner        : {OwnerName}");
        Console.WriteLine($"Interest     : {InterestRate}");
        Console.WriteLine($"Branch ID    : {BranchId}");
        Console.WriteLine($"Credit Limit : {CreditLimit}");
        Console.WriteLine($"Currency     : {Currency}");
        Console.WriteLine($"Bank         : {BankName}");
    }
}


class Program {

    static void Main(string[] args) {

        BankAccount account = new BankAccount("ACC1001",101,5.5);
        account.Balance = 5000;
        Console.WriteLine(account.Balance);
    
    
        account.Age = 21;
        Console.WriteLine(account.Age);

        // account.Age = -50; // rejected by setter

        Console.WriteLine(account.AccountNumber);
        // account.AccountNumber = "ACC00009"; // not allowed

        account.Pin = "1234";
        // not allowed
        // Console.WriteLine(account.Pin);

        account.OwnerName = "Jagdish";
        Console.WriteLine(account.OwnerName);

        Console.WriteLine(account.InterestRate);

        // NOT allowed outside BankAccount:
        // account.InterestRate = 10;
        // But BankAccount itself can change it:
        account.ChangeInterestRate(7.5);


        Console.WriteLine(account.BranchId);
        // not allowed
        // account.BranchId = 999;


        // default one's
        Console.WriteLine(account.Currency);
        Console.WriteLine(account.BankName);

    }
}