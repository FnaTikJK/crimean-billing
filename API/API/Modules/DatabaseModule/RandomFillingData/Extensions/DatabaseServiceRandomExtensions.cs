namespace API.Modules.DatabaseModule.RandomFillingData.Extensions;

public class DatabaseServiceRandomExtensions
{
   public string GenerateEmail()
    {
        var random = new Random();
        var domains = new[] { "gmail.com", "yahoo.com", "outlook.com", "mail.ru" };
        return $"user{random.Next(1, 10000)}@{domains[random.Next(domains.Length)]}";
    }

   public string GenerateFullName()
    {
        var random = new Random();
        var firstNames = new[] { "Иван", "Анна", "Сергей", "Мария", "Алексей", "Елена", "Дмитрий", "Ольга", "Павел", "Светлана" };
        var lastNames = new[] { "Иванов", "Петров", "Сидоров", "Кузнецов", "Смирнов", "Попов", "Васильев", "Михайлов", "Новиков", "Федоров" };
        var patronymics = new[] { "Иванович", "Петрович", "Сергеевич", "Алексеевич", "Дмитриевич", "Александрович", "Васильевич", "Михайлович", "Николаевич", "Павлович" };

        return $"{lastNames[random.Next(lastNames.Length)]} {firstNames[random.Next(firstNames.Length)]} {patronymics[random.Next(patronymics.Length)]}";
    }

   public string GeneratePhoneNumber()
    {
        var random = new Random();
        return $"8{random.Next(900, 999)}{random.Next(1000000, 9999999)}";
    }

   public string GenerateNumber()
    {
        var random = new Random();
        return random.Next(1, 998 + 1).ToString();
    }

   public T GetRandomEnumValue<T>() where T : Enum
   {
       var values = Enum.GetValues(typeof(T));
           var random = new Random();
           return (T)values.GetValue(random.Next(values.Length));
   }
}