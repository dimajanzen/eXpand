﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using Quartz.Impl.Calendar;
using Xpand.ExpressApp.PropertyEditors;
using Xpand.Persistent.Base.General.CustomAttributes;
using Xpand.Persistent.Base.JobScheduler.Calendars;
using Xpand.Utils.Helpers;
using Xpand.Xpo.Converters.ValueConverters;

namespace Xpand.Persistent.BaseImpl.JobScheduler.Calendars {
    [Tooltip(@"Summary:
This implementation of the Calendar stores a list of holidays (full days that are excluded from scheduling). 
Remarks:
The implementation DOES take the year into consideration, so if you want to exclude July 4th for the next 10 years, you need to add 10 entries to the exclude list. ")]
    public class XpandHolidayCalendar : XpandTriggerCalendar, IHolidayCalendar {

        public XpandHolidayCalendar(Session session)
            : base(session) {
        }
        Type ITriggerCalendar.CalendarType {
            get { return typeof(HolidayCalendar); }
        }
        public override void AfterConstruction() {
            base.AfterConstruction();
            _datesExcluded = new List<DateTime>();
        }
        [Persistent("DatesExcluded")]
        [Size(SizeAttribute.Unlimited)]
        [ValueConverter(typeof(SerializableObjectConverter))]
        private List<DateTime> _datesExcluded;

        [PropertyEditor(typeof(IChooseFromListCollectionEditor))]
        [DataSourceProperty("AllDates")]
        [DisplayFormat("{0:dd/MM}")]
        public List<DateTime> DatesExcluded {
            get { return _datesExcluded; }
        }
        [Browsable(false)]
        public List<DateTime> AllDates {
            get {
                return DateTimeUtils.GetDates().ToList();
            }
        }

    }
}