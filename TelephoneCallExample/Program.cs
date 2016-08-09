using System;
using System.Threading.Tasks;
using Stateless;

namespace TelephoneCallExample
{
    class Program
    {
        static StateMachine<State, Trigger> driverStateMachine = new StateMachine<State, Trigger>(State.OffDuty);

        enum Trigger
        {
            ToDriving,
            ToSleeperBerth,
            ToOffDuty,
            ToOnDutyNotDriving,
        }

        enum State
        {
            Driving,
            SleeperBerth,
            OffDuty,
            OnDutyNotDriving
        }

        static void Main(string[] args)
        {
            var toDriving = driverStateMachine.SetTriggerParameters<object>(Trigger.ToDriving);
            var toSleeperBerth = driverStateMachine.SetTriggerParameters<object>(Trigger.ToSleeperBerth);
            var toOnDutyNotDriving = driverStateMachine.SetTriggerParameters<object>(Trigger.ToOnDutyNotDriving);
            var toOffDuty = driverStateMachine.SetTriggerParameters<object>(Trigger.ToOffDuty);

            driverStateMachine.Configure(State.OffDuty)
                .OnEntryFrom(toOffDuty, OffDutyOnEntry)
                .OnExit(OffDutyOnExit)
                .PermitDynamic(toDriving, _ => State.Driving)
                .PermitDynamic(toSleeperBerth, _ => State.SleeperBerth)
                .PermitDynamic(toOnDutyNotDriving, _ => State.OnDutyNotDriving);

            driverStateMachine.Configure(State.SleeperBerth)
                .OnEntryFrom(toSleeperBerth, SleeperBerthgOnEntry)
                .OnExit(SleeperBerthgOnExit)
                .PermitDynamic(toDriving, _ => State.Driving)
                .PermitDynamic(toOffDuty, _ => State.OffDuty)
                .PermitDynamic(toOnDutyNotDriving, _ => State.OnDutyNotDriving);

            driverStateMachine.Configure(State.OnDutyNotDriving)
                .OnEntryFrom(toOnDutyNotDriving, OnDutyNotDrivingOnEntry)
                .OnExit(OnDutyNotDrivingOnExit)
                .PermitDynamic(toOffDuty, _ => State.OffDuty)
                .PermitDynamic(toSleeperBerth, _ => State.SleeperBerth)
                .PermitDynamic(toDriving, _ => State.Driving);

            driverStateMachine.Configure(State.Driving)
                .OnEntryFrom(toDriving, DrivingOnEntry, "DrivingOnEntry")
                .OnExit(DrivingOnExit)
                .PermitDynamic(toOffDuty, _ => State.OffDuty)
                .PermitDynamic(toSleeperBerth, _ => State.SleeperBerth)
                .PermitDynamic(toOnDutyNotDriving, _ => State.OnDutyNotDriving);


            Task.Run(() =>
            {
                Fire(driverStateMachine, Trigger.ToDriving, 1);
                Fire(driverStateMachine, Trigger.ToOffDuty, 2);
                Fire(driverStateMachine, Trigger.ToSleeperBerth, 3);
                Fire(driverStateMachine, Trigger.ToOnDutyNotDriving, 4);
                Fire(driverStateMachine, Trigger.ToOnDutyNotDriving, 5);
                Fire(driverStateMachine, Trigger.ToDriving, 6);
            });

            Task.Run(() =>
            {
                Fire(driverStateMachine, Trigger.ToOffDuty, 7);
                Fire(driverStateMachine, Trigger.ToSleeperBerth, 8);
                Fire(driverStateMachine, Trigger.ToSleeperBerth, 9);
                Fire(driverStateMachine, Trigger.ToOnDutyNotDriving, 10);
            });

            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }

        static void DrivingOnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingOnEntry {value}");
        }

        static void DrivingOnExit()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingOnExit");
        }

        static void OnDutyNotDrivingOnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} OnDutyNotDrivingOnEntry {value}");
        }

        static void OnDutyNotDrivingOnExit()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} OnDutyNotDrivingOnExit");
        }

        static void SleeperBerthgOnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} SleeperBerthOnEntry {value}");
        }

        static void SleeperBerthgOnExit()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} SleeperBerthOnExit");
        }

        static void OffDutyOnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} OffDutyOnEntry {value}");
        }

        static void OffDutyOnExit()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} OffDutyOnExit");
        }

        static void Fire<T>(StateMachine<State, Trigger> stateMachine, Trigger trigger, T value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} Firing: {stateMachine.State} - {trigger} {value}");
            try
            {
                if (stateMachine.CanFire(trigger))
                {
                    stateMachine.Fire(trigger, value);
                }
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} {ex.Message}");
            }

        }

        static void Print(StateMachine<State, Trigger> stateMachine)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} Status: {stateMachine}");
        }
    }
}
