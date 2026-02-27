#region Part 01

/*
Q1:
Static binding happens at compile time.
Dynamic binding happens at runtime with override (polymorphism).
*/

/*
Q2:
Overloading: same method name with different parameters.
Overriding: same method in base and derived class using virtual and override.
*/

/*
Q3:
virtual: allows a method to be overridden.
override: provides new implementation in derived class.
abstract: method without body that must be overridden.
sealed: prevents further overriding.
*/

#endregion

#region Part 02 - Practical (Continue From Assignment 03 + Add Polymorphism)

#region Base Class: Ticket
public class Ticket
{
    private static int counter = 0;

    public int TicketId { get; }
    public string MovieName { get; set; }

    private decimal price;

    public decimal Price
    {
        get => price;
        private set
        {
            if (value <= 0) throw new ArgumentException("Price must be greater than 0");
            price = value;
        }
    }

    public decimal PriceAfterTax => Price * 1.14m;

    public Ticket(string movieName, decimal price)
    {
        MovieName = movieName;
        TicketId = ++counter;
        SetPrice(price);
    }

    // Overloading
    public void SetPrice(decimal price)
    {
        Console.WriteLine($"Setting price directly: {price}");
        Price = price;
    }

    public void SetPrice(decimal basePrice, decimal multiplier)
    {
        decimal result = basePrice * multiplier;
        Console.WriteLine($"Setting price with multiplier: {basePrice} x {multiplier} = {result}");
        Price = result;
    }

    // Polymorphism
    public virtual void PrintTicket()
    {
        Console.WriteLine($"Ticket #{TicketId} | {MovieName} | Price: {Price} EGP | After Tax: {PriceAfterTax:F2} EGP");
    }

    public static int GetTotalTickets() => counter;
}
#endregion

#region Child Classes
public class StandardTicket : Ticket
{
    public string SeatNumber { get; set; }

    public StandardTicket(string movieName, decimal price, string seatNumber)
        : base(movieName, price)
    {
        SeatNumber = seatNumber;
    }

    public override void PrintTicket()
    {
        base.PrintTicket();
        Console.WriteLine($"  Seat: {SeatNumber}");
    }
}

public class VIPTicket : Ticket
{
    public bool LoungeAccess { get; set; }
    public decimal ServiceFee { get; } = 50m;

    public VIPTicket(string movieName, decimal price, bool loungeAccess)
        : base(movieName, price)
    {
        LoungeAccess = loungeAccess;
    }

    public override void PrintTicket()
    {
        base.PrintTicket();
        Console.WriteLine($"  Lounge: {(LoungeAccess ? "Yes" : "No")} | Service Fee: {ServiceFee} EGP");
    }
}

public class IMAXTicket : Ticket
{
    public bool Is3D { get; set; }

    public IMAXTicket(string movieName, decimal price, bool is3D)
        : base(movieName, is3D ? price + 30m : price) 
    {
        Is3D = is3D;
    }

    public override void PrintTicket()
    {
        base.PrintTicket();
        Console.WriteLine($"  IMAX 3D: {(Is3D ? "Yes" : "No")}");
    }
}
#endregion

#region Composition Classes
public sealed class Projector
{
    public void Start() => Console.WriteLine("Projector started.");
    public void Stop() => Console.WriteLine("Projector stopped.");
}

public class Cinema
{
    public string CinemaName { get; set; }
    private readonly Projector projector;
    private readonly Ticket[] tickets = new Ticket[20];

    public Cinema(string name)
    {
        CinemaName = name;
        projector = new Projector();
    }

    public void OpenCinema()
    {
        Console.WriteLine("========== Cinema Opened ==========");
        projector.Start();
    }

    public void CloseCinema()
    {
        Console.WriteLine("\n========== Cinema Closed ==========");
        projector.Stop();
    }

    public void AddTicket(Ticket t)
    {
        for (int i = 0; i < tickets.Length; i++)
        {
            if (tickets[i] == null)
            {
                tickets[i] = t;
                return;
            }
        }
        Console.WriteLine("Cinema is full!");
    }

  
    public void PrintAllTickets()
    {
        Console.WriteLine("\n========== All Tickets ==========");
        foreach (var t in tickets)
        {
            if (t != null) t.PrintTicket();
        }
    }

   
    public static void ProcessTicket(Ticket t)
    {
        Console.WriteLine("\n========== Process Single Ticket ==========");
        t.PrintTicket();
    }
}
#endregion

#endregion

#region Main (Assignment 04 Flow)
class Program
{
    static void Main()
    {
        Cinema cinema = new Cinema("Nile Cinema");
        cinema.OpenCinema();

        Console.WriteLine("\n========== SetPrice Test ==========");
        StandardTicket t1 = new StandardTicket("Inception", 120m, "A-5");
        t1.SetPrice(150m);
        t1.SetPrice(100m, 1.5m);

        VIPTicket t2 = new VIPTicket("Avengers", 200m, true);
        IMAXTicket t3 = new IMAXTicket("Dune", 180m, false);

        cinema.AddTicket(t1);
        cinema.AddTicket(t2);
        cinema.AddTicket(t3);

        cinema.PrintAllTickets();
        Cinema.ProcessTicket(t2);

        cinema.CloseCinema();
    }
}
#endregion
