
using System;
using System.Threading.Tasks;

namespace Server
{
    [Serializable]
    public class Target
    {
        private bool condition;
        private bool done;
        private Regularity _regularity;
        private DateTime CheckTime;
        private DateTime DoneTime;
        private DateTime _planedTime;
        private int _periodTime;
        public string KeyTask { get { return _task; } }

        public bool Done { get => done; }
        public Regularity regularity { get => _regularity; }

        public Target(string KeyTask, Regularity Regularity, DateTime PlanedTime = default(DateTime), int PeriodTime = 0)
        { _task = KeyTask; _regularity = Regularity; _planedTime = PlanedTime; _periodTime = PeriodTime; condition = false; done = false; }
        public enum Regularity
        {
            once,
            by_time,
            after_time,
            by_time_once
        }
        private string _task;
        public bool TargetCheck()
        {
            CheckTime = DateTime.Now;
            checking_conditions();
            if (condition)
            {
                Task.Factory.StartNew(Server.TargetDictionary.Dictionarys[_task]);
                DoneTime = DateTime.Now;
                condition = false;
                return true;
            }
            return false;
        }

        private void checking_conditions()
        {
            switch (_regularity)
            {
                case Regularity.once:

                    if (_planedTime <= CheckTime)
                    {
                        if (!Done)
                        {
                            condition = true;
                            done = true;
                        }
                    }
                    break;
                case Regularity.by_time:
                    if (DateTime.Now.Day > DoneTime.Day)
                    {
                        if (_planedTime.TimeOfDay <= CheckTime.TimeOfDay)
                        {

                            condition = true;
                        }
                        else
                        {
                            condition = false;
                        }
                    }
                    else
                    {
                        condition = false;
                    }
                    break;
                case Regularity.after_time:

                    if (_planedTime == default(DateTime))
                    {
                        _planedTime = CheckTime.AddSeconds(_periodTime);
                    }
                    else
                    {
                        if (_planedTime <= CheckTime)
                        {
                            condition = true;
                            _planedTime = default(DateTime);
                        }
                        else
                        {
                            condition = false;
                        }
                    }

                    break;
                case Regularity.by_time_once:


                    if (DateTime.Now.Day > DoneTime.Day)
                    {
                        if (_planedTime.TimeOfDay <= CheckTime.TimeOfDay)
                        {
                                if (!Done)
                                {
                                    condition = true;
                                    done = true;
                                }
                        }
                        else
                        {
                            condition = false;
                        }
                    }
                    else
                    {
                        condition = false;
                    }

                    break;
                default:
                    break;
            }

        }
    }
}

