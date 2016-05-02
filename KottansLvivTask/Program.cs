using System;

namespace KottansLvivTask
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Enter card number: ");
                string cardNumber = Console.ReadLine();
                string vendor = CreditCardAnalizer.GetCreditCardVendor(cardNumber);
                Console.WriteLine("Vendor of card " + cardNumber + " is " + vendor);
                Console.WriteLine("Next card number is " + CreditCardAnalizer.GenerateNextCreditCardNumber(cardNumber));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
