namespace ConsoleApp3
{
    class User
    {
        public int UID { get; set; }
        public int ProcTime { get; set; } = 0;
        public int UserTime { get; set; } = 0;
        public double k { get; set; } = 0;
        public bool work { get; set; } = true;
        public List<Proc> Procs { get; set; } = new List<Proc>();
        public User()
        {
            Random random = new Random();
            UID = random.Next(1000000);
            Procs.Add(new Proc());
            Procs.Add(new Proc());
            Procs.Add(new Proc());
        }
        public User(int countProc)
        {
            Random random = new Random();
            UID = random.Next(1000000);
            for (int i = 0; i < countProc; i++)
                Procs.Add(new Proc());
        }

        public User(int uID, int procTime, int userTime, double k, bool work, List<Proc> procs)
        {
            UID = uID;
            ProcTime = procTime;
            UserTime = userTime;
            this.k = k;
            this.work = work;
            Procs = procs;
        }

        public override string? ToString()
        {
            return "UID: " + UID + "\tProcTime: " + ProcTime + "\tUserTime: " + UserTime + "\tk: " + k;
        }
    }
    class Proc
    {
        public int PID { get; set; }
        public int CpuBurst { get; set; }
        public int CpuCreate { get; set; }
        public bool work { get; set; } = true;
        public Proc()
        {
            Random random = new Random();
            PID = random.Next(1000000);
            CpuBurst = random.Next(10);
            CpuCreate = random.Next(5);
        }

        public Proc(int pID, int cpuBurst, int cpuCreate, bool work)
        {
            PID = pID;
            CpuBurst = cpuBurst;
            CpuCreate = cpuCreate;
            this.work = work;
        }
        public override string? ToString()
        {
            return "PID: " + PID + "\tCpuBurst: " + CpuBurst + "\tCpuCreate: " + CpuCreate;
        }
    }
    public class Program
    {
        private static void Main(string[] _)
        {
            List<User> users = new List<User>();
            users.Add(new User());
            users.Add(new User());
            users.Add(new User());


            int tact = 0;

            //users.ForEach(x => x.UserTime = x.Procs.Min(y => y.CpuCreate));
            while (true)
            {
                users = users.OrderBy(x => x.UID).ToList();
                users = users.OrderBy(x => x.work).Reverse().ToList();
                users = users.OrderBy(x => x.k).ToList();

                for (int i = 0; i < users.Count; i++)
                {
                    for (int j = 0; j < users[i].Procs.Count; j++)
                    {
                        if (users[i].Procs[j].CpuBurst <= 0) users[i].Procs[j].work = false;
                    }
                    if (users[i].Procs.All(x => x.work == false)) users[i].work = false;

                    users[i].Procs = users[i].Procs.OrderBy(x => x.PID).ToList();
                    users[i].Procs = users[i].Procs.OrderBy(x => x.CpuCreate).ToList();
                    users[i].Procs = users[i].Procs.OrderBy(x => x.work).Reverse().ToList();

                    //users[i].Procs.OrderBy(x => x.work).OrderBy(x => x.CpuBurst).OrderBy(x => x.PID).ToList();
                }
                users.ForEach(x => x.UserTime++);

                if (users.All(x => x.work == false))
                {
                    break;
                }

                User user = users.FirstOrDefault(x => x.work);
                Proc proc = user.Procs.FirstOrDefault(x => x.work);
                proc.CpuBurst--;
                user.ProcTime++;

                users.ForEach(x => x.k = Math.Round(((double)users.Count * (double)x.ProcTime) / (double)x.UserTime, 2));

                Console.WriteLine("USER: " + user.ToString() + "\t|\tPROC: " + proc.ToString());

                tact++;
            }
        }
    }
}