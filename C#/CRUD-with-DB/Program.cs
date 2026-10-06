using MySql.Data.MySqlClient;

class Program {

    public static void Main() {

        string connectionString = "Server=localhost;Port=3306;Database=knowledge_base;User Id=root;Password=root";


        // using keyword automatically disposes resources occupied by connection 
        using MySqlConnection connection = new MySqlConnection(connectionString);

        connection.Open();

        Console.WriteLine("Connected to MySQL!");
    }
}