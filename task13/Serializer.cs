using System.Text.Json;

namespace task13
{
    public class Serializer
    {
        private readonly JsonSerializerOptions options;
        public Serializer()
        {
            options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true

            };
            options.Converters.Add(new DateConverter());
        }
        public string Serialize(Student student)
        {
            return JsonSerializer.Serialize(student, options);
        }
        public Student Deserialize(string json)
        {
            Student? student = JsonSerializer.Deserialize<Student>(json, options);
            if (student == null)
            {
                throw new JsonException("Ошибка десериализации");
            }
            Validate(student);
            return student;
        }
        private void Validate(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.FirstName))
            {
                throw new JsonException("Ошибка! Имя не может быть пустым");
            }
            if (string.IsNullOrWhiteSpace(student.LastName))
            {
                throw new JsonException("Ошибка! Фамилия не может быть пустой");
            }
            if (student.Grades == null)
            {
                return;
            }
            foreach (var subject in student.Grades)
            {
                if (string.IsNullOrWhiteSpace(subject.Name))
                {
                    throw new JsonException("Ошибка! Предмет не может быть пустым");
                }
                if (subject.Grade < 2 || subject.Grade > 5)
                {
                    throw new JsonException($"Ошибка! Неверный формат оценок");
                }
            }
        }
        public void Save(Student student, string filePath)
        {
            string json = Serialize(student);
            File.WriteAllText(filePath, json);
        }
        public Student Load(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Ошибка! Файл не найден");
            }
            string json = File.ReadAllText(filePath);
            return Deserialize(json);
        }
    }
}

