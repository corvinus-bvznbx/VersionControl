using MicroSim.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MicroSim
{
    public partial class Form1 : Form
    {
        List<Person> Population = new List<Person>();
        List<Birthprobability> BirthProbabilities = new List<Birthprobability>();
        List<Deathprobability> DeathProbabilities = new List<Deathprobability>();
        Random rng = new Random(1234);
        public Form1()
        {
            InitializeComponent();
            Population = GetPopulation(@"C:\Users\Lenovo\Downloads\nép-teszt.csv");
            BirthProbabilities = GetBirthProbabilities(@"C:\Users\Lenovo\Downloads\születés.csv");
            DeathProbabilities = GetDeathProbabilities(@"C:\Users\Lenovo\Downloads\halál.csv");


        }
        public List<Person> GetPopulation(string csvpath)
        {
            List<Person> population = new List<Person>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    population.Add(new Person()
                    {
                        BirthYear = int.Parse(line[0]),
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[1]),
                        NbrOfChildren = int.Parse(line[2])
                    });
                }
            }

            return population;
        }
        public List<Birthprobability> GetBirthProbabilities(string csvpath)
        {
            List<Birthprobability> birthprob = new List<Birthprobability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    birthprob.Add(new Birthprobability()
                    {
                        Age = int.Parse(line[0]),
                        NbrOfChildren = int.Parse(line[1]),
                        P = Convert.ToDouble(line[2])
                    });
                }
            }

            return birthprob;
        }

        public List<Deathprobability> GetDeathProbabilities(string csvpath)
        {
            List<Deathprobability> deathprob = new List<Deathprobability>();

            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    deathprob.Add(new Deathprobability()
                    {
                        Gender = (Gender)Enum.Parse(typeof(Gender), line[0]),
                        Age = int.Parse(line[1]),
                        P = Convert.ToDouble(line[2])
                    });
                }
            }

            return deathprob;
        }
    }
}
