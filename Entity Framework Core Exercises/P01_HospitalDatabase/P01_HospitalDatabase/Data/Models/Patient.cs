﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_HospitalDatabase.Data.Models
{
    public class Patient
    {
        public int PatientId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        [Column("Email", TypeName = "varchar(80)")]
        public string Email { get; set; }

        public bool HasInsurance { get; set; }

        public ICollection<Visitation> Visitations { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }


        public ICollection<PatientMedicament> Prescriptions { get; set; }
    }
}