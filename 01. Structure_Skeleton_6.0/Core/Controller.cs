using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityCompetition.Core.Contracts;
using UniversityCompetition.Models;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories;
using UniversityCompetition.Repositories.Contracts;
using UniversityCompetition.Utilities.Messages;

namespace UniversityCompetition.Core
{
    public class Controller : IController
    {
        private IRepository<IStudent> students;
        private IRepository<IUniversity> universities;
        private IRepository<ISubject> subjects;

        public Controller()
        {
            this.students = new StudentRepository();
            this.universities = new UniversityRepository();
            this.subjects = new SubjectRepository();
        }

        public string AddStudent(string firstName, string lastName)
        {

            if (students.FindByName($"{firstName} {lastName}") != null)
            {
                return string.Format(OutputMessages.AlreadyAddedStudent, firstName, lastName);
            }
            IStudent student = new Student(students.Models.Count + 1, firstName, lastName);
            students.AddModel(student);
            return string.Format(OutputMessages.StudentAddedSuccessfully, firstName, lastName, nameof(StudentRepository));
        }

        public string AddSubject(string subjectName, string subjectType)
        {
            if (subjectType != nameof(EconomicalSubject)
                && subjectType != nameof(HumanitySubject)
                && subjectType != nameof(TechnicalSubject))
            {
                return string.Format(OutputMessages.SubjectTypeNotSupported, subjectType);
            }
            ISubject subject;
            if (this.subjects.FindByName(subjectName) != null)
            {
                return string.Format(OutputMessages.AlreadyAddedSubject, subjectName);
            }
            if (subjectType == nameof(EconomicalSubject))
            {
                subject = new EconomicalSubject(subjects.Models.Count + 1, subjectName);
            }
            else if (subjectType == nameof(HumanitySubject))
            {
                subject = new HumanitySubject(subjects.Models.Count + 1, subjectName);
            }
            else
            {
                subject = new TechnicalSubject(subjects.Models.Count + 1, subjectName);
            }
            this.subjects.AddModel(subject);
            return string.Format(OutputMessages.SubjectAddedSuccessfully, subjectType, subjectName, subjects.GetType().Name);

        }

        public string AddUniversity(string universityName, string category, int capacity, List<string> requiredSubjects)
        {
            List<int> universitySubjects = new List<int>();
            if (universities.FindByName(universityName) != null)
            {
                return string.Format(OutputMessages.AlreadyAddedUniversity, universityName);
            }
            foreach (var subj in requiredSubjects)
            {
                ISubject subject = subjects.FindByName(subj);
                universitySubjects.Add(subject.Id);
            }

            IUniversity university
                = new University(this.universities.Models.Count + 1, universityName, category, capacity, universitySubjects);
            this.universities.AddModel(university);
            return string.Format(OutputMessages.UniversityAddedSuccessfully, universityName, universities.GetType().Name);
        }

        public string ApplyToUniversity(string studentName, string universityName)
        {
            string studentFirstName = studentName.Split(" ")[0];
            string studentLastName = studentName.Split(" ")[1];
            IStudent student = this.students.FindByName(studentName);
            IUniversity university = this.universities.FindByName(universityName);
            if (student == null)
            {
                return string.Format(OutputMessages.StudentNotRegitered, studentFirstName, studentLastName);
            }
            else if (university == null)
            {
                return string.Format(OutputMessages.UniversityNotRegitered, universityName);
            }
            else if (!university.RequiredSubjects.All(x => student.CoveredExams.Any(e => e == x)))
            {
                return string.Format(OutputMessages.StudentHasToCoverExams, studentName, university.Name);
            }
            else if (student.University != null && student.University.Name == universityName)
            {
                return string.Format(OutputMessages.StudentAlreadyJoined, student.FirstName, student.LastName, student.University.Name);
            }
            else
            {
                student.JoinUniversity(university);
                return string.Format(OutputMessages.StudentSuccessfullyJoined, student.FirstName, student.LastName, university.Name);
            }
        }
        public string TakeExam(int studentId, int subjectId)
        {
            if (students.FindById(studentId) == null)
            {
                return string.Format(OutputMessages.InvalidStudentId);
            }
            if (subjects.FindById(subjectId) == null)
            {
                return string.Format(OutputMessages.InvalidSubjectId);
            }
            IStudent student = this.students.FindById(studentId);
            ISubject subject = this.subjects.FindById(subjectId);
            if (student.CoveredExams.Contains(subjectId))
            {
                return string.Format(OutputMessages.StudentAlreadyCoveredThatExam, student.FirstName, student.LastName, subject.Name);
            }
            student.CoverExam(subject);
            return string.Format(OutputMessages.StudentSuccessfullyCoveredExam, student.FirstName, student.LastName, subject.Name);
        }

        public string UniversityReport(int universityId)
        {
            IUniversity university = universities.FindById(universityId);
            int studentsCount = this.students.Models.Where(s => s.University == university).Count();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"*** {university.Name} ***");
            sb.AppendLine($"Profile: {university.Category}");
            sb.AppendLine($"Students admitted: {studentsCount}");
            sb.AppendLine($"University vacancy: {university.Capacity - studentsCount}");
            return sb.ToString().TrimEnd();
        }
    }
}
