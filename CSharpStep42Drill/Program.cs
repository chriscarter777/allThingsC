using System;
using System.Collections.Generic;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpStep42Drill
{
    public delegate void MyDelegate(int arg);
    public delegate void FeedingDelegate();


    class Program
    {
        static void Main(string[] args)
        {
            //derived classes
            Dog Sparky = new Dog("Sparky");
            Sparky.Coat = CoatColor.yellow;
            Human Bob = new Human();
            Bob.Name = "Bob";


            //use of overloading and overriding
            Sparky.MakesNoise();
            Bob.MakesNoise();
            Bob.MakesNoise("Here, Sparky!");
            (Sparky as Mammal).MakesNoise();


            //use of a structure
            int r = 10;
            Console.WriteLine("\n\n\nA circle of radius " + r + " has circumference " + BitOMath.Circumference(r) + " and area " + BitOMath.CircleArea(r) + ".");


            //value and reference types, generic Enumerable
            int valueInt = 99;
            List<int> refInt = new List<int>();
            refInt.Add(99);
            Console.WriteLine("\n\n\nvalueInt is " + valueInt + " and refInt is " + refInt[0]);
            int newValueInt = valueInt;
            newValueInt++;
            int newRefInt = refInt[0];
            newRefInt++;
            Console.WriteLine("value \t newValue \t reference \t newReference");
            Console.WriteLine(valueInt + "\t" + newValueInt + "\t" + refInt + "\t" + newRefInt);


            //delegate and multicast delegate, Version One
            Console.WriteLine("\n\n\nThe counter will run from 0 to 100:");
            Counter myCounter = new Counter(100);
            for (int i=0; i < 11; i++)
            {
                myCounter.Increment();
            }


            //delegate and multicast delegate, Version Two
            Console.WriteLine( "\n\n\nA day in the Johnson household..." );
            FeedingDelegate meal;
            for (int h = 0; h == 23; h++)
            {
                Console.WriteLine("It is " + h + "o\'clock");
                meal = null;
                if (h % 3 == 0)
                {
                    meal += Bob.Feed;
                }
                if (h % 12 == 0)
                {
                    meal += Sparky.Feed;
                }
                meal();
            }


            //Enumerable
            Console.WriteLine("\n\n\nMaking the band:");
            List < string > band= new List<string> { "Crosby", "Stills", "Nash" };
            foreach(string musician in band)
            {
                Console.WriteLine(musician + ", ");
            }
            band.Add("and Young");
            foreach (string musician in band)
            {
                Console.WriteLine(musician + ", ");
            }


            //Nullable
            Console.WriteLine("\n\n\nWatch as the phantom disappears.");
            int? phantom = 999;
            Console.WriteLine("Phantom value is: " + phantom.Value);
            phantom = 0;
            Console.WriteLine("Phantom value is: " + phantom.Value);
            phantom = null;
            Console.WriteLine("Phantom value is: " + phantom.Value);


            //try-catch, exception handling, logging and serialization
            string logPath = @"c:\Users\Owner\Desktop\log.txt";
            string sourcePath = @"c:\Users\Owner\Desktop\Hoff.jpg";
            string serialPath = @"c:\Users\Owner\Desktop\Hoff_serialization.txt";
            string destinationPath = @"c:\Users\Owner\Desktop\Copy_of_Hoff.jpg";

            Serialize(sourcePath, serialPath, logPath);
            Deserialize(serialPath, destinationPath, logPath);

            Console.WriteLine("That\'s all folks!");

        //end of Main Method
    }

        public static void Serialize(string srcPath, string destPath, string logPath)
        {
            try
            {
                using (StreamWriter log = new StreamWriter(logPath))
                {
                    byte[] imgBytes = File.ReadAllBytes(srcPath);
                    log.WriteLine("Data read from Source file.");

                    string imgStr = Convert.ToBase64String(imgBytes);
                    log.WriteLine("Data converted from binary to string");

                    File.Create(destPath);
                    log.WriteLine("Serialization file created.");

                    File.WriteAllText(destPath, imgStr);
                    log.WriteLine("Data saved in Serialization file.");
                }

            }
            catch (Exception ex)
            {
                using (StreamWriter log = new StreamWriter(logPath))
                {
                    log.WriteLine("An exception occurred during File serialization: " + ex.Message);
                }
            }

        }

        public static void Deserialize(string srcPath, string destPath, string logPath)
        {
            try
            {
                using (StreamWriter log = new StreamWriter(logPath))
                {
                    string imgStr = File.ReadAllText(srcPath);
                    log.WriteLine("Data read from Serialization file.");

                    byte[] imgBytes = Convert.FromBase64String(imgStr);
                    log.WriteLine("Data converted from string to binary.");

                    File.Create(destPath);
                    log.WriteLine("Destination file created.");

                    File.WriteAllBytes(destPath, imgBytes);
                    log.WriteLine("Data saved in Destination file.");
                }

            }
            catch (Exception ex)
            {
                using (StreamWriter log = new StreamWriter(logPath))
                {
                    log.WriteLine("An exception occurred during File deserialization: " + ex.Message);
                }
            }
        }

        //end of Program Class
    }


    enum CoatColor
    {
        white,
        yellow,
        brown,
        black,
        red,
        spotted
    };

    struct BitOMath
    {
        public const double Pi = 3.1415927;
        public const double E = 2.71828;
        public static double Circumference(double radius)
        {
            return 2 * radius * Pi;
        }

        public static double CircleArea(double radius)
        {
            return radius * radius * Pi;
        }
    }

    interface ICreature
    {
        int BreathingRate { get; set; }
        double Lifespan { get; set; }
    }

    interface IAnimal
    {
        int LegNumber { get; set; }
        bool Fur { get; set; }
        void Feed();
    }

    abstract class Mammal : ICreature, IAnimal
    {
        public int BreathingRate { get; set; }
        public double Lifespan { get; set; }
        public int LegNumber { get; set; }
        public bool Fur { get; set; }

        public virtual void MakesNoise()
        {
            Console.WriteLine("The mammal makes its sound.");
        }

        public virtual void Feed()
        {
            Console.WriteLine("The mammal gets its food and is happy.");
        }
    }

    //derived class
    class Dog : Mammal, ICreature, IAnimal
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //only dogs can properly smell dogs: protected.
        private string smell;
        protected string Smell
        {
            get { return name; }
            set { name = value; }
        }

        //and we only tell other dogs in our own assembly if we have a bone.  We covet it. :protected internal
        private bool bone;
        protected internal bool Bone
        {
            get { return bone; }
            set { bone = value; }
        }
        public CoatColor Coat { get; set; }

        public Dog()
        {
            BreathingRate = 20;
            Lifespan = 14.5;
            LegNumber = 4;
            Fur = true;
        }

        //overloaded constructor
        public Dog(string n)
        {
            BreathingRate = 20;
            Lifespan = 14.5;
            LegNumber = 4;
            Fur = true;
            Name = n;
        }

        //overridden method
        public override void MakesNoise()
        {
            Console.WriteLine("The dog (" + this.Name + ") barks.");
        }

        //overridden method
        public override void Feed()
        {
            Console.WriteLine("The dog (" + this.Name + ") gets some dogfood and is happy.");
        }

        //but to facilitate multicast delegates, we're going to use another method instead:
        public void FeedDog()
        {
            Console.WriteLine("The dog (" + this.Name + ") is fed some dogfood. It curls up and goes to sleep.");
        }
    }

    //derived class
    //sealed: while we may want to speciate dogs in future, we don't want to subdivide humans
    sealed class Human : Mammal, ICreature, IAnimal
    {
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        //internal property: we only give out our SSN to people in our own assembly
        private string ssn;
        internal string SSN
        {
            get { return name; }
            set { name = value; }
        }

        public Human()
        {
            BreathingRate = 12;
            Lifespan = 76;
            LegNumber = 2;
            Fur = false;
        }

        //overloaded constructor
        public Human(string n)
        {
            BreathingRate = 12;
            Lifespan = 76;
            LegNumber = 2;
            Fur = false;
            Name = n;
        }

        //overridden method
        public override void MakesNoise()
        {
            Console.WriteLine("The human talks.");
        }
        public void MakesNoise(string words)
        {
            Console.WriteLine("The human (" + this.Name + ") says: " + words + ".");
        }

        //overridden method 
        public override void Feed()
        {
            Console.WriteLine("The human (" + this.Name + ") gets a steak and is happy.");
        }

        //but to facilitate multicast delegates, we're going to use another method instead:
        public void FeedHuman()
        {
            Console.WriteLine("The human (" + this.Name + ") is fed a delicious meal. It goes back to coding.");
        }


    }

    class Counter
    {
        private MyDelegate Feedback;
        private int currentCount;
        public int CurrentCount { get; set; }
        public int Limit { get; set; }

        public Counter(int lim)
        {
            CurrentCount = 0;
            Limit = lim;
            this.Feedback += Information;
        }

        public void Increment()
        {
            this.CurrentCount += 10;
            if (this.CurrentCount == 70)
            {
                this.Feedback += Notification;
            }
            if (this.CurrentCount == 90)
            {
                this.Feedback += Warning;
            }
           Feedback(this.CurrentCount);
        }

        private void Information(int n)
        {
            Console.WriteLine("Currently at " + n);
        }

        private void Notification(int n)
        {
            Console.WriteLine("In the upper half of range. " + (100 - n) + " to go.");
        }

        private void Warning(int n)
        {
            Console.WriteLine("Approaching upper limit of 100!");
        }
    }
}
