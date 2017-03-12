using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "年龄"), Range(1, 200)]
        public byte Age { get; set; }
        [Display(Name = "身高")]
        public byte Height { get; set; }
        [Display(Name = "体重")]
        public byte Weight { get; set; }
        [Range(1, 100)]
        public int Strength { get; set; }
        [Range(1, 100)]
        public int Stamina { get; set; }
        [Range(1, 100)]
        public int Intelligence { get; set; }

    }
    public class BodyMeasure
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<byte> Weight { get; set; }
        public Nullable<byte> Breast { get; set; }
        public Nullable<byte> Waist { get; set; }
        public Nullable<byte> Butt { get; set; }
    }
    public class Consume
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public int amount { get; set; }
        public System.DateTime consumedate { get; set; }
        public string description { get; set; }
        public byte[] image { get; set; }

    }
    public class Injury
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public System.DateTime injurydate { get; set; }
        public string Injury1 { get; set; }
    }
}
