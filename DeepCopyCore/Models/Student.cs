﻿namespace DeepCopyCore.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class StudentDto {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
