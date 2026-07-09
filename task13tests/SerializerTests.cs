using System.Text.Json;
using Xunit;
using task13;

namespace task13tests
{
    public class SerializerTests
    {
        private readonly Serializer serializer = new Serializer();
        private Student TestStudent()
        {
            return new Student
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                BirthDate = new DateTime(2001, 1, 1),
                Grades = new List<Subject>
                {
                    new Subject { Name = "Programming", Grade = 5},
                    new Subject { Name = "Mathematics", Grade = 4}
                }
            };
        }

        [Fact]
        public void Serialize_ReturnsValidJson()
        {
            var student = TestStudent();
            string json = serializer.Serialize(student);
            Assert.Contains("Ivan", json);
            Assert.Contains("Ivanov", json);
            Assert.Contains("2001.01.01", json);
        }

        [Fact]
        public void Serialize_NullGrades()
        {
            var student = new Student
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                BirthDate = new DateTime(2001, 1, 1),
                Grades = null!
            };
            string json = serializer.Serialize(student);
            Assert.DoesNotContain("Grades", json);
        }

        [Fact]
        public void Deserialize_ReturnsStudent()
        {
            var student = TestStudent();
            string json = serializer.Serialize(student);
            var result = serializer.Deserialize(json);
            Assert.Equal("Ivan", result.FirstName);
            Assert.Equal("Ivanov", result.LastName);
            Assert.Equal(2, result.Grades.Count);
        }

        [Fact]
        public void Deserialize_EmptyFirstName()
        {
            string json = """
            {
                "FirstName": "",
                "LastName": "Ivanov",
                "BirthDate": "2001.01.01",
                "Grades": []
            }
            """;
            Assert.Throws<JsonException>(() => serializer.Deserialize(json));
        }

        [Fact]
        public void SaveAndLoad()
        {
            var student = TestStudent();
            string path = Path.GetTempFileName();
            serializer.Save(student, path);
            var loaded = serializer.Load(path);
            Assert.Equal(student.FirstName, loaded.FirstName);
            Assert.Equal(student.LastName, loaded.LastName);
            Assert.Equal(student.Grades.Count, loaded.Grades.Count);
            File.Delete(path);
        }

        [Fact]
        public void Load_NonExistentFile()
        {
            Assert.Throws<FileNotFoundException>(() => serializer.Load("qwerty123.json"));
        }
    }
}

