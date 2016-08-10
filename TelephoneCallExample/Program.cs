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
            var toOnDutyNotDriving = driverStateMachine.SetTriggerParameters<object, object>(Trigger.ToOnDutyNotDriving);
            var toOffDuty = driverStateMachine.SetTriggerParameters<object>(Trigger.ToOffDuty);

            driverStateMachine.Configure(State.OffDuty)
                .OnEntryFrom(toOffDuty, OffDutyOnEntry)
                .OnExit(OffDutyOnExit)
                .PermitDynamic(toDriving, arg0 => State.Driving)
                .PermitDynamic(toSleeperBerth, _ => State.SleeperBerth)
                .PermitDynamic(toOnDutyNotDriving, (x, y) => State.OnDutyNotDriving);

            driverStateMachine.Configure(State.SleeperBerth)
                .OnEntryFrom(toSleeperBerth, SleeperBerthgOnEntry)
                .OnExit(SleeperBerthgOnExit)
                .PermitDynamic(toDriving, _ => State.Driving)
                .PermitDynamic(toOffDuty, _ => State.OffDuty)
                .PermitDynamic(toOnDutyNotDriving, (x, y) => State.OnDutyNotDriving);

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
                .PermitDynamic(toOnDutyNotDriving, (x, y) => State.OnDutyNotDriving);


            var t1 = Task.Run(() =>
            {
                Fire(driverStateMachine, Trigger.ToDriving);
                Fire(driverStateMachine, Trigger.ToOffDuty, 2);
                Fire(driverStateMachine, Trigger.ToSleeperBerth, 3);
                Fire(driverStateMachine, Trigger.ToOnDutyNotDriving, 4, 5);
                Fire(driverStateMachine, Trigger.ToOnDutyNotDriving, 5, 7);
                Fire(driverStateMachine, Trigger.ToDriving, 6);
            });

            var t2 =Task.Run(() =>
            {
                Fire(driverStateMachine, Trigger.ToOffDuty, 7);
                Fire(driverStateMachine, Trigger.ToSleeperBerth, 8);
                Fire(driverStateMachine, Trigger.ToSleeperBerth, 9);
                Fire(driverStateMachine, Trigger.ToOnDutyNotDriving, 10, 8);
            });

            Task.WhenAll(t1, t2).Wait();

            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
        }

        static void DrivingOnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingOnEntry {value}");
        }

        static void DrivingOnExit(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingOnExit  {value}");
        }

        static void OnDutyNotDrivingOnEntry(object value, object value2)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} OnDutyNotDrivingOnEntry {value} {value2}");
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

        static void Fire(StateMachine<State, Trigger> stateMachine, Trigger trigger)
        {

            try
            {
                stateMachine.Fire(trigger);
            }
            catch (InvalidOperationException ex)
            {
                
       
            }
            catch (Exception ex)
            {


            }

        }

        static void Fire<TArg0>(StateMachine<State, Trigger> stateMachine, Trigger trigger, TArg0 arg0)
        {

            try
            {
                stateMachine.Fire(trigger, arg0);
            }
            catch (Exception)
            {


            }
        }

        static void Fire<TArg0, TArg1>(StateMachine<State, Trigger> stateMachine, Trigger trigger, TArg0 arg0, TArg1 arg1)
        {

            try
            {
                stateMachine.Fire(trigger, arg0, arg1);
            }
            catch (Exception ex)
            {


            }
        }

        static void Fire<TArg0, TArg1, TArg2>(StateMachine<State, Trigger> stateMachine, Trigger trigger, TArg0 arg0, TArg1 arg1, TArg2 arg2)
        {

            try
            {
                stateMachine.Fire(trigger, arg0, arg1, arg2);
            }
            catch (Exception)
            {


            }
        }

        static void Print(StateMachine<State, Trigger> stateMachine)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} Status: {stateMachine}");
        }
    }
}
