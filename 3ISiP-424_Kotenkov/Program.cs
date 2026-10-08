using System;
using System.Collections.Generic;
using System.Linq;

class Program
{

    enum Gender
    {
        Мужской,
        Женский
    }


    abstract class Person
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Phone { get; private set; }
        public Gender Gender { get; private set; }

        protected Person(int id, string name, int age, string phone, Gender gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Phone = phone;
            Gender = gender;
        }

        public abstract void PrintInfo();
    }

    class Student : Person
    {
        private List<Course> courses = new List<Course>();

        public Student(
            int id,
            string name,
            int age,
            string phone,
            Gender gender)
            : base(id, name, age, phone, gender)
        {
        }

        public List<Course> GetCourses()
        {
            return courses;
        }

        public void AddCourse(Course course)
        {
            if (!courses.Contains(course))
            {
                courses.Add(course);
            }
        }

        public override void PrintInfo()
        {
            Console.WriteLine( $"ID: {Id} | Студент: {Name} | Возраст: {Age} | " + $"Телефон: {Phone} | Пол: {Gender}");
        }
    }



    class Teacher : Person
    {
        private List<Course> courses = new List<Course>();

        public Teacher(
            int id,
            string name,
            int age,
            string phone,
            Gender gender)
            : base(id, name, age, phone, gender)
        {
        }

        public List<Course> GetCourses()
        {
            return courses;
        }

        public void AddCourse(Course course)
        {
            if (!courses.Contains(course))
            {
                courses.Add(course);
            }
        }

        public override void PrintInfo()
        {
            Console.WriteLine($"ID: {Id} | Преподаватель: {Name} | Возраст: {Age} | " + $"Телефон: {Phone} | Пол: {Gender}");
        }
    }



    class Course
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int Hours { get; private set; }

        public Teacher Teacher { get; private set; }

        private List<Student> students = new List<Student>();

        public Course(
            int id,
            string name,
            int hours,
            Teacher teacher)
        {
            Id = id;
            Name = name;
            Hours = hours;
            Teacher = teacher;
        }

        public List<Student> GetStudents()
        {
            return students;
        }

        public void AddStudent(Student student)
        {
            if (!students.Contains(student))
            {
                students.Add(student);
                student.AddCourse(this);
            }
        }

        public void PrintInfo()
        {
            Console.WriteLine($"ID курса: {Id}");
            Console.WriteLine($"Название: {Name}");
            Console.WriteLine($"Количество часов: {Hours}");
            Console.WriteLine($"Преподаватель: {Teacher.Name}");
            Console.WriteLine($"Количество студентов: {students.Count}");
        }
    }


    static List<Student> students = new List<Student>();
    static List<Teacher> teachers = new List<Teacher>();
    static List<Course> courses = new List<Course>();



    static int nextStudentId = 1;
    static int nextTeacherId = 1;
    static int nextCourseId = 1;


    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        AddTestData();

        while (true)
        {
            Console.Clear();

            Console.WriteLine("======================================");
            Console.WriteLine("       СИСТЕМА УПРАВЛЕНИЯ ВУЗОМ");
            Console.WriteLine("======================================");

            Console.WriteLine("1. Студенты");
            Console.WriteLine("2. Преподаватели");
            Console.WriteLine("3. Курсы");
            Console.WriteLine("4. Записать студента на курс");
            Console.WriteLine("5. Показать все данные");
            Console.WriteLine("0. Выход");

            Console.Write("\nВыберите действие: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    StudentMenu();
                    break;

                case "2":
                    TeacherMenu();
                    break;

                case "3":
                    CourseMenu();
                    break;

                case "4":
                    EnrollStudent();
                    break;

                case "5":
                    ShowAll();
                    break;

                case "0":
                    Console.WriteLine("Программа завершена.");
                    return;

                default:
                    Console.WriteLine("Ошибка! Такого пункта нет.");
                    Pause();
                    break;
            }
        }
    }


    static void StudentMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("========== СТУДЕНТЫ ==========");
            Console.WriteLine("1. Добавить студента");
            Console.WriteLine("2. Показать всех студентов");
            Console.WriteLine("3. Найти студента");
            Console.WriteLine("4. Посмотреть курсы студента");
            Console.WriteLine("0. Назад");

            Console.Write("\nВыберите действие: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddStudent();
                    break;

                case "2":
                    ShowStudents();
                    break;

                case "3":
                    FindStudent();
                    break;

                case "4":
                    ShowStudentCourses();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Ошибка! Такого пункта нет.");
                    Pause();
                    break;
            }
        }
    }



    static void AddStudent()
    {
        Console.Clear();
        Console.WriteLine("========== ДОБАВЛЕНИЕ СТУДЕНТА ==========\n");

        string name = ReadName("Введите имя и фамилию: ");
        int age = ReadInt("Введите возраст: ", 16, 100);
        string phone = ReadPhone();

        Gender gender = ReadGender();

        Student student = new Student(
            nextStudentId++,
            name,
            age,
            phone,
            gender);

        students.Add(student);

        Console.WriteLine("\nСтудент успешно добавлен!");
        Pause();
    }


    static void ShowStudents()
    {
        Console.Clear();

        Console.WriteLine("========== ВСЕ СТУДЕНТЫ ==========\n");

        if (students.Count == 0)
        {
            Console.WriteLine("Студентов пока нет.");
        }
        else
        {
            List<Person> people = students
                .Cast<Person>()
                .ToList();

            foreach (Person person in people)
            {
                person.PrintInfo();
            }
        }

        Pause();
    }



    static void FindStudent()
    {
        Console.Clear();

        Console.WriteLine("========== ПОИСК СТУДЕНТА ==========\n");

        string name = ReadName("Введите имя для поиска: ");

        List<Student> result = students.Where(s => s.Name.ToLower().Contains(name.ToLower())).ToList();

        if (result.Count == 0)
        {
            Console.WriteLine("Студент не найден.");
        }
        else
        {
            Console.WriteLine("\nНайденные студенты:");

            foreach (Student student in result)
            {
                student.PrintInfo();
            }
        }

        Pause();
    }



    static void ShowStudentCourses()
    {
        Console.Clear();

        Console.WriteLine("========== КУРСЫ СТУДЕНТА ==========\n");

        if (students.Count == 0)
        {
            Console.WriteLine("Студентов нет.");
            Pause();
            return;
        }

        ShowStudentShortList();

        int id = ReadInt("\nВведите ID студента: ", 1, int.MaxValue);

        Student student = students
            .FirstOrDefault(s => s.Id == id);

        if (student == null)
        {
            Console.WriteLine("Студент с таким ID не найден.");
            Pause();
            return;
        }

        List<Course> studentCourses = student.GetCourses();

        Console.WriteLine($"\nКурсы студента {student.Name}:");

        if (studentCourses.Count == 0)
        {
            Console.WriteLine("Студент пока не записан ни на один курс.");
        }
        else
        {
            foreach (Course course in studentCourses)
            {
                Console.WriteLine(
                    $"ID: {course.Id} | {course.Name} | " +
                    $"Преподаватель: {course.Teacher.Name}");
            }
        }

        Pause();
    }



    static void TeacherMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("======= ПРЕПОДАВАТЕЛИ =======");
            Console.WriteLine("1. Добавить преподавателя");
            Console.WriteLine("2. Показать всех преподавателей");
            Console.WriteLine("3. Найти преподавателя");
            Console.WriteLine("0. Назад");

            Console.Write("\nВыберите действие: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddTeacher();
                    break;

                case "2":
                    ShowTeachers();
                    break;

                case "3":
                    FindTeacher();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Ошибка! Такого пункта нет.");
                    Pause();
                    break;
            }
        }
    }



    static void AddTeacher()
    {
        Console.Clear();

        Console.WriteLine("======= ДОБАВЛЕНИЕ ПРЕПОДАВАТЕЛЯ =======\n");

        string name = ReadName("Введите имя и фамилию: ");
        int age = ReadInt("Введите возраст: ", 18, 100);
        string phone = ReadPhone();

        Gender gender = ReadGender();

        Teacher teacher = new Teacher(
            nextTeacherId++,
            name,
            age,
            phone,
            gender);

        teachers.Add(teacher);

        Console.WriteLine("\nПреподаватель успешно добавлен!");
        Pause();
    }



    static void ShowTeachers()
    {
        Console.Clear();

        Console.WriteLine("======= ВСЕ ПРЕПОДАВАТЕЛИ =======\n");

        if (teachers.Count == 0)
        {
            Console.WriteLine("Преподавателей пока нет.");
        }
        else
        {
            List<Person> people = teachers
                .Cast<Person>()
                .ToList();

            foreach (Person person in people)
            {
                person.PrintInfo();
            }
        }

        Pause();
    }



    static void FindTeacher()
    {
        Console.Clear();

        Console.WriteLine("======= ПОИСК ПРЕПОДАВАТЕЛЯ =======\n");

        string name = ReadName("Введите имя для поиска: ");

        List<Teacher> result = teachers.Where(t => t.Name.ToLower().Contains(name.ToLower())).ToList();

        if (result.Count == 0)
        {
            Console.WriteLine("Преподаватель не найден.");
        }
        else
        {
            foreach (Teacher teacher in result)
            {
                teacher.PrintInfo();
            }
        }

        Pause();
    }


    static void CourseMenu()
    {
        while (true)
        {
            Console.Clear();

            Console.WriteLine("============ КУРСЫ ============");
            Console.WriteLine("1. Создать курс");
            Console.WriteLine("2. Показать все курсы");
            Console.WriteLine("3. Подробная информация о курсе");
            Console.WriteLine("4. Показать студентов курса");
            Console.WriteLine("0. Назад");

            Console.Write("\nВыберите действие: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddCourse();
                    break;

                case "2":
                    ShowCourses();
                    break;

                case "3":
                    ShowCourseDetails();
                    break;

                case "4":
                    ShowCourseStudents();
                    break;

                case "0":
                    return;

                default:
                    Console.WriteLine("Ошибка! Такого пункта нет.");
                    Pause();
                    break;
            }
        }
    }



    static void AddCourse()
    {
        Console.Clear();

        Console.WriteLine("============ СОЗДАНИЕ КУРСА ============\n");

        if (teachers.Count == 0)
        {
            Console.WriteLine(
                "Сначала необходимо добавить хотя бы одного преподавателя.");
            Pause();
            return;
        }

        string name = ReadName("Введите название курса: ");
        int hours = ReadInt("Введите количество часов: ", 1, 1000);

        Console.WriteLine("\nДоступные преподаватели:");

        foreach (Teacher teacher in teachers)
        {
            Console.WriteLine(
                $"ID: {teacher.Id} | {teacher.Name}");
        }

        int teacherId = ReadInt(
            "\nВведите ID преподавателя: ",
            1,
            int.MaxValue);

        Teacher selectedTeacher = teachers
            .FirstOrDefault(t => t.Id == teacherId);

        if (selectedTeacher == null)
        {
            Console.WriteLine("Преподаватель не найден.");
            Pause();
            return;
        }

        Course course = new Course(
            nextCourseId++,
            name,
            hours,
            selectedTeacher);

        courses.Add(course);

        selectedTeacher.AddCourse(course);

        Console.WriteLine("\nКурс успешно создан!");
        Pause();
    }



    static void ShowCourses()
    {
        Console.Clear();

        Console.WriteLine("============ ВСЕ КУРСЫ ============\n");

        if (courses.Count == 0)
        {
            Console.WriteLine("Курсов пока нет.");
        }
        else
        {
            foreach (Course course in courses)
            {
                Console.WriteLine(
                    $"ID: {course.Id} | {course.Name} | " +
                    $"Часов: {course.Hours} | " +
                    $"Преподаватель: {course.Teacher.Name}");
            }
        }

        Pause();
    }


    static void ShowCourseDetails()
    {
        Console.Clear();

        Console.WriteLine("======== ИНФОРМАЦИЯ О КУРСЕ ========\n");

        if (courses.Count == 0)
        {
            Console.WriteLine("Курсов пока нет.");
            Pause();
            return;
        }

        ShowCourseShortList();

        int id = ReadInt(
            "\nВведите ID курса: ",
            1,
            int.MaxValue);

        Course course = courses
            .FirstOrDefault(c => c.Id == id);

        if (course == null)
        {
            Console.WriteLine("Курс не найден.");
        }
        else
        {
            Console.WriteLine();
            course.PrintInfo();
        }

        Pause();
    }



    static void ShowCourseStudents()
    {
        Console.Clear();

        Console.WriteLine("======= СТУДЕНТЫ КУРСА =======\n");

        if (courses.Count == 0)
        {
            Console.WriteLine("Курсов пока нет.");
            Pause();
            return;
        }

        ShowCourseShortList();

        int id = ReadInt(
            "\nВведите ID курса: ",
            1,
            int.MaxValue);

        Course course = courses
            .FirstOrDefault(c => c.Id == id);

        if (course == null)
        {
            Console.WriteLine("Курс не найден.");
            Pause();
            return;
        }

        List<Student> courseStudents = course.GetStudents();

        Console.WriteLine(
            $"\nСтуденты курса \"{course.Name}\":");

        if (courseStudents.Count == 0)
        {
            Console.WriteLine("На курс пока никто не записан.");
        }
        else
        {
            foreach (Student student in courseStudents)
            {
                Console.WriteLine(
                    $"ID: {student.Id} | {student.Name}");
            }
        }

        Pause();
    }

    static void EnrollStudent()
    {
        Console.Clear();

        Console.WriteLine("======= ЗАПИСЬ СТУДЕНТА НА КУРС =======\n");

        if (students.Count == 0)
        {
            Console.WriteLine("Сначала добавьте студентов.");
            Pause();
            return;
        }

        if (courses.Count == 0)
        {
            Console.WriteLine("Сначала создайте курсы.");
            Pause();
            return;
        }

        Console.WriteLine("СТУДЕНТЫ:");

        ShowStudentShortList();

        int studentId = ReadInt(
            "\nВведите ID студента: ",
            1,
            int.MaxValue);

        Student student = students
            .FirstOrDefault(s => s.Id == studentId);

        if (student == null)
        {
            Console.WriteLine("Студент не найден.");
            Pause();
            return;
        }

        Console.WriteLine("\nКУРСЫ:");

        ShowCourseShortList();

        int courseId = ReadInt(
            "\nВведите ID курса: ",
            1,
            int.MaxValue);

        Course course = courses
            .FirstOrDefault(c => c.Id == courseId);

        if (course == null)
        {
            Console.WriteLine("Курс не найден.");
            Pause();
            return;
        }

        if (course.GetStudents().Contains(student))
        {
            Console.WriteLine(
                "Ошибка! Студент уже записан на этот курс.");
            Pause();
            return;
        }

        course.AddStudent(student);

        Console.WriteLine($"\nСтудент {student.Name} успешно записан " + $"на курс \"{course.Name}\"!");

        Pause();
    }


    static void ShowStudentShortList()
    {
        foreach (Student student in students)
        {
            Console.WriteLine(
                $"ID: {student.Id} | {student.Name}");
        }
    }



    static void ShowCourseShortList()
    {
        foreach (Course course in courses)
        {
            Console.WriteLine(
                $"ID: {course.Id} | {course.Name} | " +
                $"Преподаватель: {course.Teacher.Name}");
        }
    }




    static void ShowAll()
    {
        Console.Clear();

        Console.WriteLine("======================================");
        Console.WriteLine("          ВСЕ ДАННЫЕ СИСТЕМЫ");
        Console.WriteLine("======================================\n");

        Console.WriteLine("========== СТУДЕНТЫ ==========");

        foreach (Student student in students)
        {
            student.PrintInfo();
        }

        Console.WriteLine("\n======= ПРЕПОДАВАТЕЛИ =======");

        foreach (Teacher teacher in teachers)
        {
            teacher.PrintInfo();
        }

        Console.WriteLine("\n============ КУРСЫ ============");

        foreach (Course course in courses)
        {
            Console.WriteLine(
                $"ID: {course.Id} | {course.Name} | " +
                $"Преподаватель: {course.Teacher.Name} | " +
                $"Студентов: {course.GetStudents().Count}");
        }

        Pause();
    }


    static void AddTestData()
    {
       
        Teacher teacher1 = new Teacher(
            nextTeacherId++,
            "Иванов Иван Иванович",
            45,
            "+79991112233",
            Gender.Мужской);

        Teacher teacher2 = new Teacher(
            nextTeacherId++,
            "Петрова Анна Сергеевна",
            38,
            "+79994445566",
            Gender.Женский);

        teachers.Add(teacher1);
        teachers.Add(teacher2);


        Student student1 = new Student(
            nextStudentId++,
            "Котенков Александр",
            18,
            "+79991234567",
            Gender.Мужской);

        Student student2 = new Student(
            nextStudentId++,
            "Смирнов Артём",
            19,
            "+79997654321",
            Gender.Мужской);

        Student student3 = new Student(
            nextStudentId++,
            "Иванова Мария",
            18,
            "+79998887766",
            Gender.Женский);

        students.Add(student1);
        students.Add(student2);
        students.Add(student3);



        Course course1 = new Course(
            nextCourseId++,
            "Программирование на C#",
            120,
            teacher1);

        Course course2 = new Course(
            nextCourseId++,
            "Базы данных",
            90,
            teacher2);

        Course course3 = new Course(
            nextCourseId++,
            "Объектно-ориентированное программирование",
            100,
            teacher1);

        courses.Add(course1);
        courses.Add(course2);
        courses.Add(course3);


      
        teacher1.AddCourse(course1);
        teacher1.AddCourse(course3);
        teacher2.AddCourse(course2);


      
        course1.AddStudent(student1);
        course1.AddStudent(student2);

        course2.AddStudent(student1);
        course2.AddStudent(student3);

        course3.AddStudent(student2);
    }




    static string ReadName(string message)
    {
        while (true)
        {
            Console.Write(message);
            string input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine(
                    "Ошибка! Поле не может быть пустым.");
                continue;
            }

            input = input.Trim();

            if (input.Length < 2)
            {
                Console.WriteLine(
                    "Ошибка! Имя должно содержать минимум 2 символа.");
                continue;
            }

            if (input.Any(char.IsDigit))
            {
                Console.WriteLine(
                    "Ошибка! Имя не должно содержать цифры.");
                continue;
            }

            return input;
        }
    }




    static int ReadInt(string message, int min, int max)
    {
        while (true)
        {
            Console.Write(message);

            string input = Console.ReadLine();

            if (!int.TryParse(input, out int value))
            {
                Console.WriteLine(
                    "Ошибка! Введите целое число.");
                continue;
            }

            if (value < min || value > max)
            {
                Console.WriteLine(
                    $"Ошибка! Значение должно быть от {min} до {max}.");
                continue;
            }

            return value;
        }
    }


    static string ReadPhone()
    {
        while (true)
        {
            Console.Write("Введите номер телефона: ");

            string phone = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(phone))
            {
                Console.WriteLine(
                    "Ошибка! Телефон не может быть пустым.");
                continue;
            }

            phone = phone.Trim();

            string digits = new string(
                phone.Where(char.IsDigit).ToArray());

            if (digits.Length < 10 || digits.Length > 15)
            {
                Console.WriteLine(
                    "Ошибка! Некорректный номер телефона.");
                continue;
            }

            return phone;
        }
    }



    static Gender ReadGender()
    {
        while (true)
        {
            Console.WriteLine("\nВыберите пол:");
            Console.WriteLine("1. Мужской");
            Console.WriteLine("2. Женский");

            Console.Write("Ваш выбор: ");

            string input = Console.ReadLine();

            if (input == "1")
            {
                return Gender.Мужской;
            }

            if (input == "2")
            {
                return Gender.Женский;
            }

            Console.WriteLine(
                "Ошибка! Выберите 1 или 2.");
        }
    }




    static void Pause()
    {
        Console.WriteLine("\nНажмите Enter для продолжения...");
        Console.ReadLine();
    }
}

