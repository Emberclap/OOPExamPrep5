﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityCompetition.Models.Contracts;
using UniversityCompetition.Repositories.Contracts;

namespace UniversityCompetition.Repositories
{
    public class StudentRepository : IRepository<IStudent>
    {
        private List<IStudent> students;
        public StudentRepository()
        {
            this.students = new List<IStudent>();
        }
        public IReadOnlyCollection<IStudent> Models
            => students;

        public void AddModel(IStudent model) => this.students.Add(model);
        
        public IStudent FindById(int id) => this.students.FirstOrDefault(x => x.Id == id);
        public IStudent FindByName(string name)
        {
            string firstName = name.Split(" ")[0];
            string lastName = name.Split(" ")[1];
            return this.students.FirstOrDefault(x=>x.FirstName == firstName && x.LastName == lastName);
        }
            
       
    }
}
