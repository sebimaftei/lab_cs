using System.Reflection.PortableExecutable;

public record Course(string Title, int Credits);
public record Student(int Id, string Name, int Age, List<Course> Courses);

public class Instructor(string name, string department, string email)
{
    public string Name {get; init;} = name;
    public string Department {get; init;} = department;
    public string Email {get; init;} = email;
}

public static class MainClass
{
    public static void FindType(object o)
    {
        switch (o)
        {
            case Student student:
                Console.WriteLine($"{student.Name}, {student.Courses.Count}");
                break;
            case Course course:
                Console.WriteLine($"{course.Title}, {course.Credits}");
                break;
            default:
                Console.WriteLine("Tip necunoscut");
                break;
        }
    }
    
    public static Func<IEnumerable<Course>, IEnumerable<Course>> FilterCourses = courses => courses.Where(course => course.Credits > 3);
    
    public static void Main()
    {
        Course course1 = new Course("C#", 5);
        Student s1 = new Student(1, "John", 20, new List<Course>());
        Student s2 = s1 with
        {
            Courses = [course1]
        };
        Instructor i = new Instructor("Ion", "CS", "ion@gmail.com");
        Console.WriteLine(i.Name + ", " + i.Department + ", " + i.Email);
        List<Course> courses = new List<Course>
        {
            new Course("Math", 5),
            new Course("History", 2),
            new Course("Science", 4),
            new Course("Art", 1)
        };
        
        var filteredCourses = FilterCourses(courses);
        
        foreach (var course in filteredCourses)
        {
            Console.WriteLine($"Course: {course.Title}, Points: {course.Credits}");
        }
    }
}