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
                        Age = byte.Parse(line[0]),
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
                        Age = byte.Parse(line[1]),
                        P = Convert.ToDouble(line[2])
                    });
                }
            }

            return deathprob;
        }
        private void SimStep(int year, Person person)
        {
            //Ha halott akkor kihagyjuk, ugrunk a ciklus következő lépésére
            if (!person.IsAlive) return;

            // Letároljuk az életkort, hogy ne kelljen mindenhol újraszámolni
            byte age = (byte)(year - person.BirthYear);

            // Halál kezelése
            // Halálozási valószínűség kikeresése
            double pDeath = (from x in DeathProbabilities
                             where x.Gender == person.Gender && x.Age == age
                             select x.P).FirstOrDefault();
            // Meghal a személy?
            if (rng.NextDouble() <= pDeath)
                person.IsAlive = false;

            //Születés kezelése - csak az élő nők szülnek
            if (person.IsAlive && person.Gender == Gender.Female)
            {
                //Szülési valószínűség kikeresése
                double pBirth = (from x in BirthProbabilities
                                 where x.Age == age
                                 select x.P).FirstOrDefault();
                //Születik gyermek?
                if (rng.NextDouble() <= pBirth)
                {
                    Person újszülött = new Person();
                    újszülött.BirthYear = year;
                    újszülött.NbrOfChildren = 0;
                    újszülött.Gender = (Gender)(rng.Next(1, 3));
                    Population.Add(újszülött);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            var filePath = string.Empty;

            ofd.InitialDirectory = "c:\\";
            ofd.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                textBox1.Text = filePath;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            MalePop.Clear();
            FemalePop.Clear();
            Population = GetPopulation(textBox1.Text);
            Simulation(Convert.ToInt32(numericUpDown1.Value));
        }
        private void Simulation(int endyear)
        {
            for (int year = 2005; year <= endyear; year++)
            {
                for (int i = 0; i < Population.Count; i++)
                {
                    SimStep(year, Population[i]);
                }

                int nbrOfMales = (from x in Population
                                  where x.Gender == Gender.Male && x.IsAlive
                                  select x).Count();
                int nbrOfFemales = (from x in Population
                                    where x.Gender == Gender.Female && x.IsAlive
                                    select x).Count();
                Console.WriteLine(
                    string.Format("Év:{0} Fiúk:{1} Lányok:{2}", year, nbrOfMales, nbrOfFemales));

                MalePop.Add(nbrOfMales);
                FemalePop.Add(nbrOfFemales);
            }

            DisplayResults(endyear);
        }

        private void DisplayResults(int endyear)
        {
            string output = string.Empty;

            for (int year = 2005; year < endyear + 1; year++)
            {
                output += "Szimulációs év:" + year + "\n \tFiúk: " + MalePop[year - 2005] + "\n\tLányok: " + FemalePop[year - 2005] + "\n\n";
            }

            richTextBox1.Text = output;
        }
    }
}
