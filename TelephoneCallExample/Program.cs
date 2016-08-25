using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amp.Logging;
using Stateless;

namespace TelephoneCallExample
{
    class Program
    {
        static StateMachine<State, Trigger> driverStateMachine;

        enum Trigger
        {
            ToDriving,
            ToInnerDriving1,
            ToInnerDriving2,
            ToSleeperBerth,
            ToOffDuty,
            ToOnDutyNotDriving,
        }

        enum State
        {
            Driving,
            DrivingInner1,
            DrivingInner2,
            SleeperBerth,
            OffDuty,
            OnDutyNotDriving
        }

        static void Main(string[] args)
        {
            driverStateMachine  = new StateMachine<State, Trigger>(State.OffDuty, LoggerFactory.GetLogger<StateMachine<State, Trigger>>(), "DRIVER");

            driverStateMachine.TriggerNotValidRaised += DriverStateMachine_TriggerNotValidRaised;

            var toDriving = driverStateMachine.SetTriggerParameters<object>(Trigger.ToDriving);
            var toDrivingInner1 = driverStateMachine.SetTriggerParameters<object>(Trigger.ToInnerDriving1);
            var toDrivingInner2 = driverStateMachine.SetTriggerParameters<object>(Trigger.ToInnerDriving2);
            var toSleeperBerth = driverStateMachine.SetTriggerParameters<object>(Trigger.ToSleeperBerth);
            var toOnDutyNotDriving = driverStateMachine.SetTriggerParameters<object, object>(Trigger.ToOnDutyNotDriving);
            var toOffDuty = driverStateMachine.SetTriggerParameters<object>(Trigger.ToOffDuty);

            driverStateMachine.Configure(State.OffDuty)
                .OnEntryFrom(toOffDuty, OffDutyOnEntry)
                .OnExit(OffDutyOnExit)
                .PermitDynamic(toDriving, arg0 => State.Driving)
                .PermitDynamic(toDrivingInner1, arg0 => State.DrivingInner1)
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
                .PermitDynamic(toDrivingInner1, _ => State.DrivingInner1)
                .PermitDynamic(toDrivingInner2, _ => State.DrivingInner2)
                .PermitDynamic(toOffDuty, _ => State.OffDuty)
                .PermitDynamic(toSleeperBerth, _ => State.SleeperBerth)
                .PermitDynamic(toOnDutyNotDriving, (x, y) => State.OnDutyNotDriving);

            driverStateMachine.Configure(State.DrivingInner1)
                .OnEntryFrom(toDrivingInner1, DrivingInner1OnEntry, "DrivingInner1OnEntry")
                .OnExit(DrivingInner1OnExit)
                .SubstateOf(State.Driving)
                .PermitDynamic(toOffDuty, _ => State.OffDuty)
                .PermitDynamic(toSleeperBerth, _ => State.SleeperBerth)
                .PermitDynamic(toOnDutyNotDriving, (x, y) => State.OnDutyNotDriving);

            driverStateMachine.Configure(State.DrivingInner2)
                .OnEntryFrom(toDrivingInner2, DrivingInner2OnEntry, "DrivingInner2OnEntry")
                .OnExit(DrivingInner2OnExit)
                .SubstateOf(State.Driving)
                .PermitDynamic(toOffDuty, _ => State.OffDuty)
                .PermitDynamic(toSleeperBerth, _ => State.SleeperBerth)
                .PermitDynamic(toOnDutyNotDriving, (x, y) => State.OnDutyNotDriving);


            driverStateMachine.Start(TaskScheduler.Default);

            var result = FireWithResult(driverStateMachine, Trigger.ToDriving).Result;
            result = FireWithResult(driverStateMachine, Trigger.ToInnerDriving1).Result;
            Fire(driverStateMachine, Trigger.ToInnerDriving2);

            Console.WriteLine("done main");

            var t1 = Task.Factory.StartNew(() =>
            {
                Fire(driverStateMachine, Trigger.ToDriving);
                FireWithResult(driverStateMachine, Trigger.ToInnerDriving1);
                Fire(driverStateMachine, Trigger.ToInnerDriving2);
                Fire(driverStateMachine, Trigger.ToOffDuty, 2);
                Fire(driverStateMachine, Trigger.ToSleeperBerth, 3);
                Fire(driverStateMachine, Trigger.ToOnDutyNotDriving, 4, 5);
                Fire(driverStateMachine, Trigger.ToOnDutyNotDriving, 5, 7);
                Fire(driverStateMachine, Trigger.ToDriving, 6);
            }, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current);

            var t2 = Task.Run(() =>
             {
                 Fire(driverStateMachine, Trigger.ToOffDuty, 7);
                 Fire(driverStateMachine, Trigger.ToSleeperBerth, 8);
                 Fire(driverStateMachine, Trigger.ToSleeperBerth, 9);
                 Fire(driverStateMachine, Trigger.ToOnDutyNotDriving, 10, 8);
             });

            Task.WhenAll(t1,t2).Wait();


            Console.WriteLine("Press any key...");
            Console.ReadKey(true);
            driverStateMachine.Stop();
        }

        private static void DriverStateMachine_TriggerNotValidRaised(object sender, TriggerNotValidEventArgs<Trigger, State> e)
        {
            Console.WriteLine($"CALLBACK [{e.CurrentState}] [{e.Trigger}]");
        }

        static object DrivingOnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingOnEntry {value}");

            Thread.Sleep(2000);
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DOOOOOOOONE");
            return 45;

        }

        static void DrivingOnExit(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingOnExit");
        }

        static object DrivingInner1OnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingInner1OnEntry {value}");
            return new List<string>() {"teset", "encore"};
        }

        static void DrivingInner2OnExit(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingInner2OnExit  {value}");
        }

        static object DrivingInner2OnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingInner2OnEntry {value}");
            return null;
        }

        static void DrivingInner1OnExit(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} DrivingInner1OnExit  {value}");
        }

        static object OnDutyNotDrivingOnEntry(object value, object value2)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} OnDutyNotDrivingOnEntry {value} {value2}");
            return null;
        }

        static void OnDutyNotDrivingOnExit()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} OnDutyNotDrivingOnExit");
        }

        static object SleeperBerthgOnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} SleeperBerthOnEntry {value}");
            return null;
        }

        static void SleeperBerthgOnExit()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} SleeperBerthOnExit");
        }

        static object OffDutyOnEntry(object value)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} OffDutyOnEntry {value}");
            return null;
        }

        static void OffDutyOnExit()
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} OffDutyOnExit");
        }

        static async Task<object> FireWithResult(StateMachine<State, Trigger> stateMachine, Trigger trigger)
        {
            return await Task.Run(() =>
            {
                var mre = new ManualResetEvent(false);
                stateMachine.Fire(trigger, mre);
                mre.WaitOne();
                var result = stateMachine.ResultFromFire;
                stateMachine.SetManualResetEventAfterGetResult();

                return result;
            }).ConfigureAwait(false);
        }

        static async Task<object> FireWithResult<TArg0>(StateMachine<State, Trigger> stateMachine, Trigger trigger, TArg0 arg0)
        {
            return await Task.Run(() =>
            {
                var mre = new ManualResetEvent(false);
                stateMachine.Fire(trigger, mre, arg0);
                mre.WaitOne();
                var result = stateMachine.ResultFromFire;
                stateMachine.SetManualResetEventAfterGetResult();

                return result;
            }).ConfigureAwait(false);
        }

        static async Task<object> FireWithResult<TArg0, TArg1>(StateMachine<State, Trigger> stateMachine, Trigger trigger, TArg0 arg0, TArg1 arg1)
        {
            return await Task.Run(() =>
            {
                var mre = new ManualResetEvent(false);
                stateMachine.Fire(trigger, mre, arg0, arg1);
                mre.WaitOne();
                var result = stateMachine.ResultFromFire;
                stateMachine.SetManualResetEventAfterGetResult();

                return result;
            }).ConfigureAwait(false);
        }

        static void Fire(StateMachine<State, Trigger> stateMachine, Trigger trigger)
        {
            stateMachine.Fire(trigger, null);
        }

        static void Fire<TArg0>(StateMachine<State, Trigger> stateMachine, Trigger trigger, TArg0 arg0)
        {
            stateMachine.Fire(trigger, null, arg0);
        }

        static void Fire<TArg0, TArg1>(StateMachine<State, Trigger> stateMachine, Trigger trigger, TArg0 arg0, TArg1 arg1)
        {
            stateMachine.Fire(trigger, null, arg0, arg1);
        }

        static void Fire<TArg0, TArg1, TArg2>(StateMachine<State, Trigger> stateMachine, Trigger trigger, TArg0 arg0, TArg1 arg1, TArg2 arg2)
        {
            stateMachine.Fire(trigger, null, arg0, arg1, arg2);
        }

        static void Print(StateMachine<State, Trigger> stateMachine)
        {
            Console.WriteLine($"{DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss:fff")} Status: {stateMachine}");
        }
    }
}
