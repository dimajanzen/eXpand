﻿using System;
using System.Linq;
using DevExpress.XtraScheduler.Native;
using Quartz;
using Quartz.Impl.Calendar;
using Xpand.ExpressApp.JobScheduler.Qaurtz;
using Xpand.Persistent.Base.JobScheduler.Calendars;
using Xpand.Persistent.Base.JobScheduler.Triggers;

namespace Xpand.ExpressApp.JobScheduler {
    internal static class CalendarBuilder {
        public static void Build(IJobTrigger trigger, IXpandScheduler scheduler) {
            if (trigger.Calendar != null) {
                var calendar = (ICalendar) Activator.CreateInstance(trigger.Calendar.CalendarType);
                Initialize(calendar,trigger);
                scheduler.AddCalendar(trigger.Calendar.Name, calendar, true, true);
            }
        }

        static void Initialize(ICalendar calendar, IJobTrigger trigger) {
            if (calendar is AnnualCalendar) {
                InitializeAnnual(calendar as AnnualCalendar,trigger.Calendar as IAnnualCalendar);
            }
            else if (calendar is HolidayCalendar) {
                InitializeHoliday(calendar as HolidayCalendar,trigger.Calendar as IHolidayCalendar);
            }
            else if (calendar is WeeklyCalendar) {
                InitializeWeekly(calendar as WeeklyCalendar, trigger.Calendar as IWeeklyCalendar);
            }
            else if (calendar is MonthlyCalendar) {
                InitializeMonthly(calendar as MonthlyCalendar, trigger.Calendar as IMonthlyCalendar);
            }
            else if (calendar is DailyCalendar) {
                InitializeDaily(calendar as DailyCalendar, trigger.Calendar as IDailyCalendar);
            }
            else if (calendar is CronCalendar) {
                InitializeCron(calendar as CronCalendar, trigger.Calendar as ICronCalendar);
            }
        }

        static void InitializeCron(CronCalendar cronCalendar, ICronCalendar  calendar) {
            cronCalendar.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(RegistryTimeZoneProvider.GetRegistryKeyNameByTimeZoneId(calendar.TimeZone));
            cronCalendar.CronExpression = new CronExpression(calendar.CronExpression);
        }

        static void InitializeDaily(DailyCalendar dailyCalendar, IDailyCalendar calendar) {
            dailyCalendar.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(RegistryTimeZoneProvider.GetRegistryKeyNameByTimeZoneId(calendar.TimeZone));
            calendar.DateRanges.ToList().ForEach(range => dailyCalendar.SetTimeRange((DateTime) range.StartPoint, range.EndPoint));
        }

        static void InitializeMonthly(MonthlyCalendar monthlyCalendar, IMonthlyCalendar calendar) {
            monthlyCalendar.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(RegistryTimeZoneProvider.GetRegistryKeyNameByTimeZoneId(calendar.TimeZone));
            calendar.DaysExcluded.ForEach(i => monthlyCalendar.SetDayExcluded(i,true));
            calendar.DaysIncluded.ForEach(i => monthlyCalendar.SetDayExcluded(i,false));
        }

        static void InitializeWeekly(WeeklyCalendar weeklyCalendar, IWeeklyCalendar calendar) {
            weeklyCalendar.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(RegistryTimeZoneProvider.GetRegistryKeyNameByTimeZoneId(calendar.TimeZone));
            calendar.DaysOfWeekExcluded.ForEach(week => weeklyCalendar.SetDayExcluded(week, true));
            calendar.DaysOfWeekIncluded.ForEach(week => weeklyCalendar.SetDayExcluded(week, false));
        }

        static void InitializeHoliday(HolidayCalendar holidayCalendar, IHolidayCalendar calendar) {
            holidayCalendar.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(RegistryTimeZoneProvider.GetRegistryKeyNameByTimeZoneId(calendar.TimeZone));
            calendar.DatesExcluded.ForEach(holidayCalendar.AddExcludedDate);
        }

        static void InitializeAnnual(AnnualCalendar annualCalendar, IAnnualCalendar calendar) {
            annualCalendar.TimeZone = TimeZoneInfo.FindSystemTimeZoneById(RegistryTimeZoneProvider.GetRegistryKeyNameByTimeZoneId(calendar.TimeZone));
            calendar.DatesExcluded.ForEach(time => annualCalendar.SetDayExcluded(time, true));
            calendar.DatesIncluded.ForEach(time => annualCalendar.SetDayExcluded(time, false));
        }
    }
}