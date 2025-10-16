using System;
using System.Collections.Generic;

void addStudent(string studentName, List<string> studentnames )
{
    studentnames.Add(studentName);
}

var students = new List<string>();
addStudent("Ionut", students);
addStudent("Maria", students);
addStudent("Ana", students);
addStudent("Gigel", students);
addStudent("Gelu", students);
addStudent("Ion", students);


foreach (var student in students)
{
    Console.WriteLine(student);
}